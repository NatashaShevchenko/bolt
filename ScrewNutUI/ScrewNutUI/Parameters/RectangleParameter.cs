using System;
using Kompas6API5;
using Kompas6Constants;

namespace ScrewNutUI.Parameters
{
    /// <summary>
    ///     Rectangle parameter.
    ///     Represents parameters of rectangle of 2D document.
    /// </summary>
    public class RectangleParameter
    {
        /// <summary>
        ///     Set rectangle param
        /// </summary>
        /// <param name="kompasApp">KompasObject</param>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        /// <param name="point2D">2D point of rectangle position on sketch</param>
        public RectangleParameter(KompasApplication kompasApp, double width, double height, Point2D point2D)
        {
            if (kompasApp == null)
                throw new ArgumentNullException(nameof(kompasApp));

            if (width <= 0.0
                || height <= 0.0)
            {
                throw new ArgumentException("Ошибка при создании прямоугольника. " +
                                            "Введите корректные параметры и попробуйте снова");
            }

            ksRectangleParam rectangleParam =
                kompasApp.KompasObject.GetParamStruct((short) StructType2DEnum.ko_RectangleParam);
            rectangleParam.width = width;
            rectangleParam.height = height;
            rectangleParam.ang = 0;
            rectangleParam.style = 1;
            rectangleParam.x = point2D.X;
            rectangleParam.y = point2D.Y;

            FigureParam = rectangleParam;
        }

        /// <summary>
        ///     Get rectangle parameter
        /// </summary>
        public ksRectangleParam FigureParam { get; }
    }
}