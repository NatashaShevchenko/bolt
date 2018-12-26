using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using ScrewNutUI.Parameters;

namespace ScrewNutUI.Builders
{
    /// <summary>
    /// Построитель болта
    /// </summary>
	class BoltBuilder
	{
	    private const short O3DSketch = (short)Obj3dType.o3d_sketch;
	    private const short EtBlind = (short)ksEndTypeEnum.etBlind;
	    private const short O3DBaseExtrusion = (short)Obj3dType.o3d_baseExtrusion;

        /// <summary>
        /// Экземпляр компаса
        /// </summary>
        private readonly KompasApplication _kompas;

        /// <summary>
        /// Экземпляр детали
        /// </summary>
        private ksPart _part;

        /// <summary>
        /// Экземпляр эскиза
        /// </summary>
        private ksEntity _entityDraw;

        /// <summary>
        /// Экземпляр документа
        /// </summary>
        private ksDocument3D _doc3D;

        /// <summary>
        /// Параметры болта
        /// </summary>
	    private BoltParameters _screwParameters;


	    /// <summary>
        /// Конструктор класса BoltBuilder
        /// </summary>
        /// <param name="kompasApp">Экземпляр менеджера компаса</param>
        public BoltBuilder(KompasApplication kompasApp)
        {
            _kompas = kompasApp;
        }

        /// <summary>
        /// Метод построения болта
        /// </summary>
        /// <param name="screwParameters">Параметры болта</param>
        public void BuildDetail(BoltParameters screwParameters)
        {

            _doc3D = _kompas.Document3D;

            _part = _kompas.ScrewPart;

            _screwParameters = screwParameters;

            BuildModel(screwParameters.DiameterOut, screwParameters.HatHeight);
            BuildChamfer(screwParameters.DiameterOut, screwParameters.ChamferAngle);
            BuildHat(screwParameters.DiameterOut, screwParameters.HatHeight);
            BuildBoltShaft(screwParameters.ShaftDiameter, screwParameters.ShaftLength);

            
        }

	    /// <summary>
	    /// Построение начального вида шляпки болта в виде цилиндра
	    /// </summary>
	    /// <param name="diameterOut">Внешний диаметр шляпки</param>
	    /// <param name="height">Высота шляпки</param>
	    private void BuildModel(double diameterOut, double height)
        {
            //Получаем интерфейс объекта "Эскиз"
            _entityDraw = _part.NewEntity(O3DSketch);
            //Получаем интерфейс параметров эскиза
            ksSketchDefinition sketchDefinition = _entityDraw.GetDefinition();
            //Получаем интерфейс объекта "плоскость XOY"
            ksEntity entityPlane = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            //Устанавливаем плоскость XOY базовой для эскиза
            sketchDefinition.SetPlane(entityPlane);
            //Создаем эскиз
            _entityDraw.Create();

            //Входим в режим редактирования эскиза
            ksDocument2D document2D = sketchDefinition.BeginEdit();
            //Строим окружность (Указывается радиус, поэтому диаметр делим пополам)
            document2D.ksCircle(0, 0, diameterOut / 2, 1);
            //Выходим из режима редактирования эскиза
            sketchDefinition.EndEdit();

            BuildExtrusion(height, true);
        }

	    /// <summary>
        /// Метод построения шпильки болта
        /// </summary>
        /// <param name="diameter">Диаметр шпильки</param>
        /// <param name="length">Длина шпильки</param>
	    private void BuildBoltShaft(double diameter, double length)
	    {
	        //Получаем интерфейс объекта "Эскиз"
	        _entityDraw = _part.NewEntity(O3DSketch);
	        //Получаем интерфейс параметров эскиза
	        ksSketchDefinition sketchDefinition = _entityDraw.GetDefinition();


	        //Получаем интерфейс объекта "плоскость XOY"
	        ksEntityCollection entityCollection = _part.EntityCollection((short)Obj3dType.o3d_face);
	        entityCollection.SelectByPoint(0, 0, 0);
	        ksEntity face = entityCollection.First();

	        //Устанавливаем плоскость XOY базовой для эскиза
	        sketchDefinition.SetPlane(face);
	        //Создаем эскиз
	        _entityDraw.Create();
	        //Входим в режим редактирования эскиза
	        ksDocument2D document2D = sketchDefinition.BeginEdit();
	        //Строим окружность (Указывается радиус, поэтому диаметр делим попалам)
	        document2D.ksCircle(0, 0, diameter / 2, 1);
	        //Выходим из режима редактирования эскиза
	        sketchDefinition.EndEdit();

	        //Получаем интерфейс объекта "операция выдавливание"
	        ksEntity entityExtrusion = _part.NewEntity((short)Obj3dType.o3d_bossExtrusion);

	        ksBossExtrusionDefinition definition = entityExtrusion.GetDefinition();

            //Установим параметры выдавливания
	        definition.SetSketch(_entityDraw);
	        ExtrusionParam parameters = definition.ExtrusionParam();
	        parameters.direction = (short)Direction_Type.dtNormal;
	        parameters.depthNormal = length;
	        parameters.depthReverse = 0;

	        entityExtrusion.Create();
	    }

	    /// <summary>
	    /// Операция выдавливания
	    /// </summary>
	    /// <param name="height">Высота выдавливания</param>
	    /// <param name="direction">Направление выдавливания</param>
	    private void BuildExtrusion(double height, bool direction)
        {
            //Получаем интерфейс объекта "операция выдавливание"
            ksEntity entityExtrusion = _part.NewEntity(O3DBaseExtrusion);
            //Получаем интерфейс параметров операции "выдавливание"
            ksBaseExtrusionDefinition baseExtrusionDefinition = entityExtrusion.GetDefinition();
            //Устанавливаем параметры операции выдавливания
            baseExtrusionDefinition.SetSideParam(direction, EtBlind, height, 0, false);
            //Устанавливаем эскиз операции выдавливания
            baseExtrusionDefinition.SetSketch(_entityDraw);

            //Создаем операцию выдавливания
            entityExtrusion.Create();
            //Включаем отображение каркаса
            _doc3D.shadedWireframe = true;
        }

	    /// <summary>
	    /// Операция "Фаска" для всех граней
	    /// </summary>
	    /// <param name="diameter">Диаметр фаски</param>
	    /// <param name="angle">Угол фаски головки</param>
	    private void BuildChamfer(double diameter, int angle)
        {
            #region Константы для фаски
            //Устанавливаем значение коэфициента для расчета угла фаски через второй катет 
            //TODO: скорее всего ГОСТовское
            double index = angle == 15 ? 3.732 : 1.732;
            #endregion

            //Получаем интерфейс объекта "фаска"
            ksEntity entityChamfer = _part.NewEntity((short)Obj3dType.o3d_chamfer);

            //Получаем интерфейс параметров объекта "скругление"
            ksChamferDefinition chamferDefinition = entityChamfer.GetDefinition();

            //Не продолжать по касательным ребрам
            chamferDefinition.tangent = false;

            //Устанавливаем параметры фаски
            chamferDefinition.SetChamferParam(true, diameter / 10, diameter / 10 / index);

            //Получаем массив поверхностей детали
            ksEntityCollection entityCollectionPart = _part
                .EntityCollection((short)Obj3dType.o3d_face);

            //Получим верхнюю грань шляпки по координатам
            entityCollectionPart.SelectByPoint(0, 0, _screwParameters.HatHeight);

            //Получаем массив поверхностей, на которых будет строиться фаска, и очищаем его
            ksEntityCollection entityCollectionChamfer = chamferDefinition.array();
            entityCollectionChamfer.Clear();

            //Заполняем массив поверхностей, на которых будет строится фаска 
            entityCollectionChamfer.Add(entityCollectionPart.First());

            //Создаем фаску
            entityChamfer.Create();
        }

        /// <summary>
        /// Операция "Вырезание выдавливанием"
        /// </summary>
        /// <param name="diameterOut">Внешний диаметр шестиугольника</param>
        /// <param name="height">Высота шляпки</param>
        private void BuildHat(double diameterOut, double height)
        {
            #region Задание параметров шестиугольника
            //
            ksRegularPolygonParam hexagon = _kompas.KompasObject
                .GetParamStruct((short)StructType2DEnum.ko_RegularPolygonParam);
            // Количество вершин
            hexagon.count = 6;
            // Координаты центра окружности
            hexagon.xc = 0;
            hexagon.yc = 0;
            // Угол радиус-вектора
            hexagon.ang = 0;
            // Построить по описанной окружности
            hexagon.describe = false;
            // Радиус окружности
            hexagon.radius = diameterOut / 2;
            // Стиль линии
            hexagon.style = 1;
            #endregion

            //Получаем интерфейс объекта "Эскиз"
            _entityDraw = _part.NewEntity(O3DSketch);
            //Получаем интерфейс параметров эскиза
            ksSketchDefinition sketchDefinition = _entityDraw.GetDefinition();
            //Получаем интерфейс объекта "плоскость XOY"
            ksEntity entityPlane = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            //Устанавливаем плоскость XOY базовой для эскиза
            sketchDefinition.SetPlane(entityPlane);
            //Создаем эскиз
            _entityDraw.Create();
            //Входим в режим редактирования эскиза
            ksDocument2D document2D = sketchDefinition.BeginEdit();
            //Строим окружность многобольшего диаметра
            document2D.ksCircle(0, 0, diameterOut + 3, 1);
            //Строим шестиугольник даметром равным внешнему диаметру
            document2D.ksRegularPolygon(hexagon, 0);
            //Выходим из режима редактирования эскиза
            sketchDefinition.EndEdit();

            //Получаем интерфейс объекта "операция вырезание выдавливанием"
            ksEntity entityCutExtrusion = _part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            //Получаем интерфейс параметров операции
            ksCutExtrusionDefinition cutExtrusionDefinition = entityCutExtrusion.GetDefinition();
            //Вычитание элементов
            cutExtrusionDefinition.cut = true;
            //Обратное направление
            cutExtrusionDefinition.directionType = (short)Direction_Type.dtReverse;
            //Устанавливаем параметры вырезания
            cutExtrusionDefinition.SetSideParam(true, EtBlind, height + 3);
            //Устанавливаем экиз операции
            cutExtrusionDefinition.SetSketch(sketchDefinition);
            //Создаем операцию вырезания выдавливанием
            entityCutExtrusion.Create();

        }

        
        
    }
}