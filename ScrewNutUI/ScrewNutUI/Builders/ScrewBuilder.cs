using System;
using Kompas6API5;
using Kompas6Constants3D;
using ScrewNutUI.Managers;
using ScrewNutUI.Parameters;

namespace ScrewNutUI.Builders
{
    public class ScrewBuilder
    {
        /// <summary>
        ///     Kompas application
        /// </summary>
        private readonly KompasApplication _kompasApp;

        /// <summary>
        ///     Constructor of screw manager
        /// </summary>
        public ScrewBuilder(KompasApplication kompasApp)
        {
            _kompasApp = kompasApp;
        }

        /// <summary>
        ///     Step of thread of screw
        /// </summary>
        public double ThreadStep { get; private set; }

        /// <summary>
        ///     Create detail
        /// </summary>
        /// <returns>true if operation successful; false in case of error</returns>
        public bool CreateDetail()
        {
            var hatEntities = CreateHat();
            var basePlaneOfHat = hatEntities[0];
            if (basePlaneOfHat == null) return false;

            var carvingEntities = CreateBase(basePlaneOfHat);
            if (carvingEntities?[0] == null || carvingEntities[1] == null) return false;

            if (!CreateThread(carvingEntities)) return false;
            return CreateScrewdriverHole(hatEntities[1]);
        }

        private bool CreateScrewdriverHole(ksEntity entity)
        {
            if (_kompasApp.ScrewdriverHoleType == ScrewdriverHoleType.None)
                return true;

            var hatDiameter = _kompasApp.Parameters[0];
            var screwdriverHoleSketchManager = new KompasSketchManager(_kompasApp.ScrewPart, entity);
            var screwdriverHoleSketch = screwdriverHoleSketchManager.BeginEntityEdit();

            switch (_kompasApp.ScrewdriverHoleType)
            {
                case ScrewdriverHoleType.Flat:
                    var width = hatDiameter * 0.6;
                    var height = hatDiameter * 0.1;
                    screwdriverHoleSketch.ksLineSeg(-width / 2, height / 2, width / 2, height / 2, 1);
                    screwdriverHoleSketch.ksLineSeg(width / 2, height / 2, width / 2, -height / 2, 1);
                    screwdriverHoleSketch.ksLineSeg(width / 2, -height / 2, -width / 2, -height / 2, 1);
                    screwdriverHoleSketch.ksLineSeg(-width / 2, -height / 2, -width / 2, height / 2, 1);
                    break;
                case ScrewdriverHoleType.Tetrahedral:
                    var regPolyParam = new PolygonParameter(_kompasApp, 4,
                        hatDiameter * 0.2, new Point2D(0, 0));
                    screwdriverHoleSketch.ksRegularPolygon(regPolyParam.FigureParam);
                    break;
                case ScrewdriverHoleType.Hexagon:
                    regPolyParam = new PolygonParameter(_kompasApp, 6,
                        hatDiameter * 0.2, new Point2D(0, 0));
                    screwdriverHoleSketch.ksRegularPolygon(regPolyParam.FigureParam);
                    break;
            }

            screwdriverHoleSketchManager.EndEntityEdit();

            var extrusionParameters = new KompasExtrusionParameters(_kompasApp.ScrewPart,
                Obj3dType.o3d_cutExtrusion, screwdriverHoleSketchManager.Entity, Direction_Type.dtReverse,
                25);
            var extrusion = new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity);
            return true;
        }

        /// <summary>
        ///     Create screw hat with extrusion operation
        /// </summary>
        /// <returns>Extruded entity of hat for base part of screw</returns>
        private ksEntity[] CreateHat()
        {
            var basePoint = -(_kompasApp.Parameters[0] / 5.0);

            var mufflerManager = new MufflerBuilder(_kompasApp,
                Obj3dType.o3d_planeYOZ, Direction_Type.dtNormal, new Point2D(basePoint, basePoint));
            var regPolySketch = new KompasSketchManager(_kompasApp.ScrewPart, Obj3dType.o3d_planeYOZ);

            var regPolySketchEdit = regPolySketch.BeginEntityEdit();

            var regPolyPoint = new Point2D(0, 0);
            var regPolyParam = new PolygonParameter(_kompasApp, 6,
                _kompasApp.Parameters[0] / 2.0, regPolyPoint);
            regPolySketchEdit.ksRegularPolygon(regPolyParam.FigureParam);

            regPolySketch.EndEntityEdit();

            var extrusionParameters = new KompasExtrusionParameters(
                _kompasApp.ScrewPart,
                Obj3dType.o3d_baseExtrusion,
                regPolySketch.Entity, Direction_Type.dtReverse,
                _kompasApp.Parameters[1]);
            var regPolyExtrusion = new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity)
            {
                BaseFaceAreaState = KompasFacesManager.BaseFaceAreaState.BaseFaceAreaLower
            };

            var extruded = regPolyExtrusion.ExtrudedEntity;

            mufflerManager.DeleteDetail();

            var roundedChamferManager = new RoundedChamferBuilder(_kompasApp, regPolySketch.Entity, regPolyParam,
                new Point2D(0d, 0d), Direction_Type.dtNormal, _kompasApp.Parameters[4]);
            
            if (!roundedChamferManager.CreateDetail())
            {
                throw new InvalidOperationException("Ошибка при создании скругления головки болта. ");
            }
            return new[]{extruded, regPolySketch.Entity};
        }

        /// <summary>
        ///     Create screw base with extrusion operation
        /// </summary>
        /// Width of screw base cylinder is 0.7 * W3
        /// <param name="basePlaneOfHat">Base plane of hat of screw</param>
        /// <returns>
        ///     Carving entities: smooth part end and thread part end,
        ///     these ones need for thread operation
        /// </returns>
        private ksEntity[] CreateBase(ksEntity basePlaneOfHat)
        {
            var screwBase = new KompasSketchManager(_kompasApp.ScrewPart, basePlaneOfHat);

            var screwBasePoint = new Point2D(0, 0);

            var screwBaseSketchEdit = screwBase.BeginEntityEdit();
            if (screwBaseSketchEdit.ksCircle(screwBasePoint.X, screwBasePoint.Y,
                    _kompasApp.Parameters[0] * 0.7 / 2.0, 1) == 0)
            {
                throw new ArgumentException("Ошибка при создании основания болта. " +
                                            "Диаметр шляпки введён некорректно");
            }

            screwBase.EndEntityEdit();

            var extrusionParameters = new KompasExtrusionParameters(_kompasApp.ScrewPart, Obj3dType.o3d_baseExtrusion,
                screwBase.Entity, Direction_Type.dtNormal, _kompasApp.Parameters[2] - _kompasApp.Parameters[3]);
            var screwBaseExtrusion = new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity)
            {
                BaseFaceAreaState = KompasFacesManager.BaseFaceAreaState.BaseFaceAreaLower
            };

            var screwCarving = new KompasSketchManager(_kompasApp.ScrewPart, screwBaseExtrusion.ExtrudedEntity);

            var screwCarvingSketchEdit = screwCarving.BeginEntityEdit();
            screwCarvingSketchEdit.ksCircle(0, 0, _kompasApp.Parameters[0] * 0.525 / 2.0, 1);
            screwCarving.EndEntityEdit();

            extrusionParameters = new KompasExtrusionParameters(_kompasApp.ScrewPart, Obj3dType.o3d_baseExtrusion,
                screwCarving.Entity, Direction_Type.dtNormal, _kompasApp.Parameters[3]);
            var screwCarvingExtrusion =
                new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity)
                {
                    BaseFaceAreaState = KompasFacesManager.BaseFaceAreaState.BaseFaceAreaLower
                };

            var extruded = screwCarvingExtrusion.ExtrudedEntity;

            return new[] {screwCarving.Entity, extruded};
        }

        /// <summary>
        ///     Create thread of base of screw
        /// </summary>
        /// <returns>true if operation successful; false in case of error</returns>
        private bool CreateThread(ksEntity[] carvingEntities)
        {
            var screwThreadSpin = new SpinBuilder(_kompasApp.ScrewPart, carvingEntities[1], carvingEntities[0],
                new Point2D(0d, 0d), 85 * 0.7,
                _kompasApp.Parameters[3] * 0.037);
            ThreadStep = screwThreadSpin.SpinStep;
            var screwThreadSketch = new KompasSketchManager(_kompasApp.ScrewPart, Obj3dType.o3d_planeXOZ);
            var screwThreadEdit = screwThreadSketch.BeginEntityEdit();

            var step = screwThreadSpin.SpinStep;
            var endX = _kompasApp.Parameters[2] + 25.5;

            var startY = -(_kompasApp.Parameters[0] * 0.7 / 2d);
            var endY = -(3d / 4d * _kompasApp.Parameters[0] * 0.7) / 2d;

            screwThreadEdit.ksLineSeg(endX - step, endY, endX, endY, 1);
            screwThreadEdit.ksLineSeg(endX, endY, endX - step / 2d, startY, 1);
            screwThreadEdit.ksLineSeg(endX - step / 2d, startY, endX - step, endY, 1);

            screwThreadSketch.EndEntityEdit();

            var spinCollection =
                (ksEntityCollection) _kompasApp.ScrewPart.EntityCollection((short) Obj3dType.o3d_cylindricSpiral);
            spinCollection.Clear();

            spinCollection.Add(screwThreadSpin.Entity);
            spinCollection.refresh();

            var extrusionParameters = new KompasExtrusionParameters(_kompasApp.ScrewPart, Obj3dType.o3d_baseEvolution,
                screwThreadSketch.Entity, spinCollection);
            var kompasExtrusion = new ExtrusionManager(extrusionParameters, ExtrusionType.BySketchesCollection); ;
            
            return true;
        }
    }
}