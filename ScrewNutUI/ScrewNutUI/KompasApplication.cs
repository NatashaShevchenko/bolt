using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Kompas6API5;
using Kompas6Constants3D;

namespace ScrewNutUI
{
    /// <summary>
    /// Screwdriver type
    /// </summary>
    public enum ScrewdriverHoleType
    {
        [Description("Без отверстия")]
        None,
        [Description("Шестигранное")]
        Hexagon,
        [Description("Четырёхранное")]
        Tetrahedral,
        [Description("Плоское")]
        Flat
    }

    /// <summary>
    ///     Kompas 3D manager
    /// </summary>
    public class KompasApplication
    {
        private List<double> _parameters;

        /// <summary>
        ///     Object constructor
        /// </summary>
        public KompasApplication(bool visibleApp = true)
        {
            if (!GetActiveApp())
                if (!CreateNewApp(visibleApp))
                {
                    throw new InvalidOperationException("При подключению к Компасу произошла ошибка");
                }
        }

        /// <summary>
        ///     Active Kompas 3D window
        /// </summary>
        public KompasObject KompasObject { get; private set; }

        /// <summary>
        ///     Kompas 3D document
        /// </summary>
        public ksDocument3D Document3D { get; private set; }

        /// <summary>
        ///     Screw detail
        /// </summary>
        public ksPart ScrewPart { get; private set; }

        /// <summary>
        ///     Model parameters
        /// </summary>
        public List<double> Parameters
        {
            get => _parameters;
            set
            {
                if (value.All(IsCorrect) && value.Count == 5)
                {
                    _parameters = value;
                }
                else
                {
                    throw new InvalidOperationException("Один или больше параметров некорректны");
                }
            }
        }

        public ScrewdriverHoleType ScrewdriverHoleType { get; set; }

        /// <summary>
        ///     Create 3D document
        /// </summary>
        public void CreateDocument3D()
        {
            Document3D = (ksDocument3D) KompasObject.Document3D();

            // Create build
            if (!Document3D.Create(false /*visible*/, false /*build*/))
                throw new ArgumentException("Ошибка создания документа");

            // Create screw detail on 3D document
            ScrewPart = (ksPart) Document3D.GetPart((short) Part_Type.pTop_Part);
        }

        /// <summary>
        ///     Destruct Kompas 3D application
        /// </summary>
        public void DestructApp()
        {
            Document3D?.close();
            KompasObject.Quit();
            KompasObject = null;

            Document3D = null;
            ScrewPart = null;

            foreach (var process in Process.GetProcessesByName("KOMPAS"))
            {
                process.Kill();
            }
        }

        /// <summary>
        ///     Get open session Kompas 3D
        /// </summary>
        /// <returns>true, if succesfull, false, if have errors</returns>
        private bool GetActiveApp()
        {
            if (KompasObject == null)
            {
                try
                {
                    KompasObject = (KompasObject)Marshal.GetActiveObject("KOMPAS.Application.5");
                }
                catch
                {
                    return false;
                }
            }

            if (KompasObject == null)
                return false;

            KompasObject.Visible = true;
            KompasObject.ActivateControllerAPI();

            return true;
        }

        /// <summary>
        ///     Create new session Kompas 3D
        /// </summary>
        /// <returns>true, if succesfull, false, if have errors</returns>
        private bool CreateNewApp(bool visible)
        {
            var t = Type.GetTypeFromProgID("KOMPAS.Application.5");
            KompasObject = (KompasObject) Activator.CreateInstance(t);

            if (KompasObject == null) return false;

            KompasObject.Visible = visible;
            KompasObject.ActivateControllerAPI();

            return true;
        }

        /// <summary>
        ///     Check on infinity and NaN
        /// </summary>
        /// <param name="value">verifiable value</param>
        /// <returns>is correct or not</returns>
        private bool IsCorrect(double value)
        {
            return !double.IsInfinity(value) && !double.IsNaN(value) && value >= 0;
        }
    }
}