using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Kompas6API5;
using Kompas6Constants3D;


namespace ScrewNutUI
{
    /// <summary>
    /// Менеджер Компаса
    /// </summary>
    public class KompasApplication
    {
        /// <summary>
        /// Экземпляр запущенной программы Компас
        /// </summary>
        public KompasObject KompasObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Документ 3D моделей
        /// </summary>
        public ksDocument3D Document3D
        {
            get;
            private set;
        }

        /// <summary>
        /// Деталь Болт
        /// </summary>
        public ksPart ScrewPart
        {
            get;
            private set;
        }

        /// <summary>
        /// Деталь Гайка
        /// </summary>
        public ksPart NutPart
        {
            get;
            private set;
        }

        /// <summary>
        /// Шаг резьбы 
        /// </summary>
        public double ThreadStep
        {
            get;
            set;
        }

        /// <summary>
        /// Параметры моделей
        /// </summary>
        public List<double> Parameters
        {
            get;
            set;
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public KompasApplication()
        {
            if (!GetActiveApp())
            {
                if (!CreateNewApp())
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Create 3D document
        /// </summary>
        public void CreateDocument3D()
        {
            Document3D = (ksDocument3D)KompasObject.Document3D();

            // Create build
            if (!Document3D.Create(false/*visible*/, false/*build*/))
            {
                throw new ArgumentException("Ошибка создания документа");
            }

            // Create screw detail on 3D document
            ScrewPart = (ksPart)Document3D.GetPart((short)Part_Type.pTop_Part);

            // Create nut detail on 3D document
            NutPart = (ksPart)Document3D.GetPart((short)Part_Type.pTop_Part);

            if (ScrewPart == null
                || NutPart == null
            )
            {
                throw new ArgumentException("Ошибка создания деталей");
            }
         
        }

        /// <summary>
        /// Получить открытую сессию Компаса
        /// </summary>
        /// <returns>true, если проведено успешно, false, если произошли ошибки</returns>
        private bool GetActiveApp()
        {
            // Попробовать получить открытую сессию
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

            // Если не загрузилось - выйти из метода
            if (KompasObject == null)
            {
                return false;
            }

            KompasObject.Visible = true;        // Сделать окно видимым
            KompasObject.ActivateControllerAPI(); // Активировать контроллер API

            return true;
        }

        /// <summary>
        /// Создает новую сессию Компас
        /// </summary>
        /// <returns>true, если проведено успешно, false, если произошли ошибки</returns>
        private bool CreateNewApp()
        {
            Type t = Type.GetTypeFromProgID("KOMPAS.Application.5");
            KompasObject = (KompasObject)Activator.CreateInstance(t);

            if (KompasObject == null)
            {
                return false;
            }

            KompasObject.Visible = true;
            KompasObject.ActivateControllerAPI();

            return true;
        }

        /// <summary>
        /// Деструктор объекта Компас
        /// </summary>
        public void DestructApp()
        {
            KompasObject.Quit();
            KompasObject = null;

            Document3D = null;
            ScrewPart = null;
            NutPart = null;
        }
    }
}
