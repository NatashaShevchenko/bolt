using System;

namespace ScrewNutUI.Parameters
{
    /// <summary>
    ///     Class KompasPoint2D.
    ///     Presents two-dimensional point with X and Y coordinates.
    /// </summary>
    public class Point2D
    {
        /// <summary>
        ///     X coordinate of point
        /// </summary>
        public double X { get; set; }

        /// <summary>
        ///     Y coordinate of point
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        ///     Kompas 2D Point
        /// </summary>
        /// <param name="xc">X coordinate</param>
        /// <param name="yc">Y coordinate</param>
        public Point2D(double xc, double yc)
        {
            if (!IsCorrect(xc) || !IsCorrect(yc))
            {
                throw new ArgumentException($"Координата {xc} и/или {yc} некорректна.");
            }

            X = xc;
            Y = yc;
        }

        /// <summary>
        ///     Check on infinity and NaN
        /// </summary>
        /// <param name="value">verifiable value</param>
        /// <returns>is correct or not</returns>
        private bool IsCorrect(double value)
        {
            return !double.IsInfinity(value) && !double.IsNaN(value) && Math.Abs(value) > 0.001;
        }
    }
}