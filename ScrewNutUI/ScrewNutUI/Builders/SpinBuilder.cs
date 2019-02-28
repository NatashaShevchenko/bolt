using System;
using Kompas6API5;
using Kompas6Constants3D;
using ScrewNutUI.Parameters;

namespace ScrewNutUI.Builders
{
	/// <summary>
	/// Spin class.
	/// Presents spin parameters of 2D document.
	/// </summary>
	internal class SpinBuilder
	{
	    /// <summary>
	    /// Part of document with detail
	    /// </summary>
	    public ksPart Document3DPart { get; }

        /// <summary>
        /// Begin face of spin
        /// </summary>
        public ksEntity BeginSpinFace { get; }

        /// <summary>
        /// End face of spin
        /// </summary>
        public ksEntity EndSpinFace { get; }

        /// <summary>
        /// Spin location point
        /// </summary>
        public Point2D SpinLocationPoint { get; }

        /// <summary>
        /// Size of diameter of spin
        /// </summary>
        public double DiameterSize { get; }

        /// <summary>
        /// Figure param
        /// </summary>
        public ksEntity Entity
		{
			get;
			private set;
		}

		/// <summary>
		/// Spin step
		/// </summary>
		public double SpinStep { get; private set; }

	    /// <summary>
		/// Spin parameter by spin faces (begin and end), spin location point, diameter size and by spin step
		/// </summary>
		public SpinBuilder(ksPart document3DPart, ksEntity beginSpinFace, ksEntity endSpinFace, Point2D spinLocationPoint, double diameterSize, double spinStep)
		{

		    if (beginSpinFace == null
				|| endSpinFace == null
				|| Math.Abs(diameterSize - default(double)) < 0.01
				|| Math.Abs(spinStep - default(double)) < 0.01)
			{
			    throw new ArgumentException("Ошибка при создании резьбы.");
            }

		    BeginSpinFace = beginSpinFace;
		    EndSpinFace = endSpinFace;
		    SpinLocationPoint = spinLocationPoint;
		    DiameterSize = diameterSize;
		    Document3DPart = document3DPart;
		    SpinStep = spinStep;

            if (!CreateSpin())
			{
			    throw new InvalidOperationException("Ошибка при создании резьбы.");
            }
		}

		/// <summary>
		/// Create spin definition by begin and end entity, diameter and spin height
		/// </summary>
		private bool CreateSpin()
		{
			var spin = (ksEntity)Document3DPart.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
			if (spin == null)
			{
				return false;
			}

			var spinDefinition = (ksCylindricSpiralDefinition)spin.GetDefinition();
			if (spinDefinition == null)
			{
				return false;
			}

			spinDefinition.SetPlane(BeginSpinFace);

			spinDefinition.buildDir = true;
			spinDefinition.buildMode = 1;
			spinDefinition.diamType = 0;
			spinDefinition.diam = DiameterSize;
			spinDefinition.heightType = 1;
            spinDefinition.SetHeightObject(EndSpinFace);
			spinDefinition.turnDir = true;
			spinDefinition.step = SpinStep;
			
			if (!spinDefinition.SetLocation(SpinLocationPoint.X, SpinLocationPoint.Y) 
				|| !spin.Create())
			{
				return false;
			}

			Entity = spin;
			SpinStep = spinDefinition.step;

			return true;
		}
	}
}
