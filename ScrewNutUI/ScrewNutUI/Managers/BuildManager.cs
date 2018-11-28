using System;
using Kompas6API5;
using Kompas6Constants3D;
using Kompas6Constants;
using NutScrew.Model;
using NutScrew.Model.Point;
using NutScrew.Model.FigureParam;
using NutScrew.Model.Entity;
using NutScrew.Error;
using NutScrew.Validator;

namespace NutScrew.Manager
{
	/// <summary>
	/// Build manager.
	/// Manages creation of build with screw and nut.
	/// </summary>
	public class BuildManager : IManagable
    {
		/// <summary>
		/// Kompas application specimen
		/// </summary>
		private KompasApplication _kompasApp;

		/// <summary>
		/// Type of screwdriver.
		/// </summary>
		public Screwdriver ScrewdriverType = Screwdriver.WithoutHole;

		/// <summary>
		/// Last error code
		/// </summary>
		public ErrorCodes LastErrorCode
		{
			get;
			private set;
		}

		/// <summary>
		/// Build Manager constructor
		/// </summary>
		public BuildManager(KompasApplication kompasApp)
		{
			if (kompasApp == null)
			{
				LastErrorCode = ErrorCodes.ArgumentNull;
				return;
			}

			_kompasApp = kompasApp;
		}

		/// <summary>
		/// Create test figure
		/// </summary>
		/// <returns>true if operation successful, false in case of error</returns>
		public bool CreateDetail()
		{
			if (!CreateScrew()) return false;

			if (!CreateNut()) return false;

			return true;
		}

		/// <summary>
		/// Create screw with hat and base
		/// </summary>
		/// <returns>true if operation successful; false in case of error</returns>
		private bool CreateScrew()
		{
			var screwManager = new ScrewManager(_kompasApp);
			if (screwManager.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = screwManager.LastErrorCode;
				return false;
			}

			screwManager.ScrewdriverType = ScrewdriverType;

			if (!screwManager.CreateDetail())
			{
				LastErrorCode = screwManager.LastErrorCode;
				return false;
			}

			_kompasApp.ThreadStep = screwManager.ThreadStep;

			return true;
		}

		/// <summary>
		/// Create nut in build
		/// </summary>
		/// <returns>true if operation successful; false in case of error</returns>
		private bool CreateNut()
		{
			var nutManager = new NutManager(_kompasApp);

			if (!nutManager.CreateDetail())
			{
				LastErrorCode = nutManager.LastErrorCode;
				return false;
			}

			return true;
		}
	}
}
