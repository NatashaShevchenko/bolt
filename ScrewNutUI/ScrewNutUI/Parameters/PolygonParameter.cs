using System;
using Kompas6API5;
using Kompas6Constants;

namespace ScrewNutUI.Parameters
{
    /// <summary>
    ///     Regular polygon parameter.
    ///     Represents parameters of regular polygon of 2D document.
    /// </summary>
    public class PolygonParameter
    {
        /// <summary>
        ///     Get polygon parameters
        /// </summary>
        /// <param name="kompasApp">Kompas object</param>
        /// <param name="anglesCount">Angles count</param>
        /// <param name="inscribedCircleRadius">Inscribed circle radius</param>
        /// <param name="point2D">Two-dimensional point of figure base</param>
        public PolygonParameter(KompasApplication kompasApp, int anglesCount, 
            double inscribedCircleRadius, Point2D point2D)
        {
            if (kompasApp == null)
                throw new ArgumentNullException(nameof(kompasApp));

            if (anglesCount <= 2
                || anglesCount >= 13
                || inscribedCircleRadius <= 0.0)
            {
                throw new ArgumentException("Ошибка при создании полигона. " +
                                            "Введите корректные параметры и попробуйте снова");
            }

            ksRegularPolygonParam polyParam =
                kompasApp.KompasObject.GetParamStruct((short) StructType2DEnum.ko_RegularPolygonParam);

            if (polyParam == null)
            {
                throw new ArgumentException("Ошибка при создании полигона. " +
                                            "Введите корректные параметры и попробуйте снова");
            }

            polyParam.count = anglesCount;
            polyParam.ang = 0;
            polyParam.describe = true;
            polyParam.radius = inscribedCircleRadius;
            polyParam.style = 1;
            polyParam.xc = point2D.X;
            polyParam.yc = point2D.Y;

            FigureParam = polyParam;
        }

        /// <summary>
        ///     Object with parameters of regular polygon
        /// </summary>
        public ksRegularPolygonParam FigureParam { get; }
    }
}