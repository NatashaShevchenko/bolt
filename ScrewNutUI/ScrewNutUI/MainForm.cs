using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ScrewNutUI
{
    public partial class MainForm : Form
    {
        #region Private fields

        /// <summary>
		/// Менеджер сессии Компас
		/// </summary>
		private KompasApplication _kompasApp;

        /// <summary>
        /// Параметры фигуры
        /// </summary>
        private List<double> _figureParameters;

        #endregion

        #region Constructor

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            CloseButton.Enabled = false;
            BuildButton.Enabled = false;
            SetAllInputsEnabledState(false);

        }

        #endregion

        #region Private Method

        /// <summary>
        /// Включение контролов ввода
        /// </summary>
        /// <param name="state">Состояние</param>
        private void SetAllInputsEnabledState(bool state)
        {
            NutDiameterNumeric.Enabled = NutHeightNumeric.Enabled = HeadDiameterNumeric.Enabled 
                = HeadLengthNumeric.Enabled = SmoothLengthNumeric.Enabled = ThreadLengthNumeric.Enabled = state;
        }

        #endregion

        #region Event Handler

        /// <summary>
        /// Кнопка запуска Компас
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LaunchButton_Click(object sender, EventArgs e)
        {
            // Создать либо подключиться к Компас
            _kompasApp = new KompasApplication();
            if (_kompasApp == null)
            {
                throw new ArgumentNullException("KompasApp не имеет экземпляра");
            }

            SetAllInputsEnabledState(true);

            BuildButton.Enabled = true;

            LaunchButton.Enabled = false;
            CloseButton.Enabled = true;
        }

        /// <summary>
        /// Кнопка закрытия компаса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            SetAllInputsEnabledState(false);

            BuildButton.Enabled = false;

            LaunchButton.Enabled = true;
            CloseButton.Enabled = false;
            try
            {
                _kompasApp.DestructApp();
            }
            catch (System.Runtime.InteropServices.COMException) // Если Компас закрыт, то просто вернуть состояние при запуске
            {
                _kompasApp = null;
            }
        }

        #endregion


    }
}
