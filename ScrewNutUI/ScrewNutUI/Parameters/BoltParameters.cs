namespace ScrewNutUI.Parameters
{
    /// <summary>
    /// Параметры болта
    /// </summary>
    public class BoltParameters
    {
        /// <summary>
        /// Диаметр описывающей окружности шляпки
        /// </summary>
        public double DiameterOut { get; set; }

        /// <summary>
        /// Высота шляпки
        /// </summary>
        public double HatHeight { get; set; }

        /// <summary>
        /// Угол фаски
        /// </summary>
        public int ChamferAngle { get; set; }

        /// <summary>
        /// Диаметр шпильки
        /// </summary>
        public double ShaftDiameter { get; set; }

        /// <summary>
        /// Длина шпильки
        /// </summary>
        public double ShaftLength { get; set; }
    }
}