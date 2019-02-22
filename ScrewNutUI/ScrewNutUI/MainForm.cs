using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ScrewNutUI.Builders;

namespace ScrewNutUI
{
    public partial class MainForm : Form
    {
        #region Private fields

        /// <summary>
        ///     Session manager
        /// </summary>
        private KompasApplication _kompasApp;

        #endregion

        #region Constructor

        /// <summary>
        ///     Object costructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _kompasApp = new KompasApplication
            {
                Parameters = new List<double>()
            };

            SetAllInputsEnabledState(_kompasApp.KompasObject != null);
            LaunchButton.Enabled = _kompasApp.KompasObject == null;

            ScrewdriverHoleTypeComboBox.DisplayMember = "Description";
            ScrewdriverHoleTypeComboBox.ValueMember = "Value";
            ScrewdriverHoleTypeComboBox.DataSource = Enum.GetValues(typeof(ScrewdriverHoleType))
                .Cast<Enum>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()),
                        typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     Включение контролов ввода
        /// </summary>
        /// <param name="state">Состояние</param>
        private void SetAllInputsEnabledState(bool state)
        {
            KernelLengthNumeric.Enabled = ThreadLengthNumeric.Enabled =
                HatLengthNumeric.Enabled = HatDiameterNumeric.Enabled =
                    ChamferRadiusNumeric.Enabled = ScrewdriverHoleTypeComboBox.Enabled 
                        = CloseButton.Enabled = BuildButton.Enabled = state;
        }

        #endregion

        #region Event Handler

        /// <summary>
        ///     Connect to Kompas 3D
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LaunchButton_Click(object sender, EventArgs e)
        {
            _kompasApp = new KompasApplication
            {
                Parameters = new List<double>()
            };
            if (_kompasApp == null)
                throw new ArgumentNullException("KompasApp не имеет экземпляра");

            SetAllInputsEnabledState(true);
            LaunchButton.Enabled = false;
        }

        /// <summary>
        ///     Close Kompas 3D
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            SetAllInputsEnabledState(false);

            LaunchButton.Enabled = true;
            try
            {
                _kompasApp.DestructApp();
            }
            catch (COMException)
            {
                _kompasApp = null;
            }
        }

        /// <summary>
        ///     Build screw hanling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildButton_Click(object sender, EventArgs e)
        {
            try
            {
                _kompasApp.Parameters = GetNumericScrewParameters();
                _kompasApp.ScrewdriverHoleType =
                    (ScrewdriverHoleType)ScrewdriverHoleTypeComboBox.SelectedValue;
                _kompasApp.CreateDocument3D();
                var builder = new ScrewBuilder(_kompasApp);
                builder.CreateDetail();
            }
            catch (Exception exception) when(exception is ArgumentException 
                                             || exception is InvalidOperationException
                                             || exception is NullReferenceException)
            {
                MessageBox.Show(exception.Message, @"Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _kompasApp.Document3D?.close();
            }
        }

        /// <summary>
        /// Get all values from numerics control
        /// </summary>
        /// <returns></returns>
        private List<double> GetNumericScrewParameters()
        {
            if (ThreadLengthNumeric.Value >= KernelLengthNumeric.Value * (decimal) 0.85)
            {
                throw new ArgumentException("Длина резьбы не может составлять " +
                                            "более 85% от длины всего стержня");
            }
            return new List<double>
            {
                (double) HatDiameterNumeric.Value,
                (double) HatLengthNumeric.Value,
                (double) KernelLengthNumeric.Value,
                (double) ThreadLengthNumeric.Value,
                (double) ChamferRadiusNumeric.Value
            };
        }

        #endregion
    }
}