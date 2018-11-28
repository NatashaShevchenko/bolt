using System;
using Kompas6API5;
using Kompas6Constants3D;
using Kompas6Constants;
using NutScrew.Model.Point;
using NutScrew.Model.FigureParam;
using NutScrew.Model.Entity;
using NutScrew.Error;
using NutScrew.Validator;

namespace NutScrew.Manager
{
	class NutManager : IManagable
	{
		/// <summary>
		/// Last error code getter
		/// </summary>
		public ErrorCodes LastErrorCode
		{
			get;
			private set;
		}

		/// <summary>
		/// Kompas application specimen
		/// </summary>
		private KompasApplication _kompasApp;

		/// <summary>
		/// Constructor of nut manager
		/// </summary>
		/// <param name="kompas">Kompas object</param>
		public NutManager(KompasApplication kompasApp)
		{
			if (kompasApp == null)
			{
				LastErrorCode = ErrorCodes.ArgumentNull;
				return;
			}

			_kompasApp = kompasApp;
		}

		/// <summary>
		/// Create nut
		/// </summary>
		/// <returns>true if operation successful, false in case of error</returns>
		public bool CreateDetail()
		{
			// Base point of nut in three-dimensional coordinate system
			var nutBasePoint = new KompasPoint3D(0.0, Math.Abs(_kompasApp.Parameters[0]), Math.Abs(_kompasApp.Parameters[0])); // / nut placement /

			// Depth of cylinder which sets base plane
			// Nut base point is (0;0) in YOZ after nut base plane creation
			var regPolyParam = new RegularPolygonParameter(_kompasApp, 6, _kompasApp.Parameters[0] / 2.0, new KompasPoint2D(0.0, 0.0));
			if (regPolyParam.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = regPolyParam.LastErrorCode;
				return false;
			}

			// Base plane cylinder depth is W1 + W2 + H + 0.1*W1*W2
			var basePlaneCylinderDepth = _kompasApp.Parameters[2] + _kompasApp.Parameters[3] + _kompasApp.Parameters[4] + (_kompasApp.Parameters[2] + _kompasApp.Parameters[3]) * 0.1;
			if (!DoubleValidator.Validate(basePlaneCylinderDepth))
			{
				LastErrorCode = ErrorCodes.DoubleValueValidationError;
				return false;
			}

			// 1. Create nut base plane cylinder (base plane is plane in three-dimensional coordinate system, not an entity!)
			var nutBasePlane = CreateNutBasePlaneCylinder(nutBasePoint, basePlaneCylinderDepth);
			if (nutBasePlane == null)
			{
				LastErrorCode = ErrorCodes.ArgumentNull;
				return false;
			}

			// Nut base point is (0;0) in YOZ after nut base plane creation
			nutBasePoint.Y = 0.0;
			nutBasePoint.Z = 0.0;

			// 2. Create nut base
			var nutBaseEntities = CreateNutBase(nutBasePlane, nutBasePoint, regPolyParam);
			if (nutBaseEntities == null 
				|| nutBaseEntities[0] == null
				|| nutBaseEntities[1] == null
			) {
				return false;
			}

			// 3. Delete nut base plane cylinder
			if (!DeleteNutBasePlaneCylinder(nutBasePlane, basePlaneCylinderDepth))
			{
				return false;
			}

			// 4. Create nut chamfer entities
			var chamferEntities = CreateNutChamferEntities(nutBasePoint, regPolyParam, nutBaseEntities);
			if (chamferEntities == null
				|| chamferEntities[0] == null
				|| chamferEntities[1] == null
			) {
				return false;
			}

			// 5. Create nut base cut
			if (!CreateBaseCut(chamferEntities, nutBasePoint))
			{
				return false;
			}

			// 6. Create nut thread
			if (!CreateNutThread(chamferEntities, nutBasePoint, basePlaneCylinderDepth)) return false;

			return true;
		}

		/// <summary>
		/// Create base plane cylinder extrusion for nut
		/// </summary>
		/// <param name="basePlaneCylinderDepth">Depth of cylinder of base plane (see <seealso cref="CreateDetail"></seealso> for more info)</param>
		/// <param name="nutBasePoint">Nut base point</param>
		/// <returns>Nut base cylinder extruded entity or null in case of error during creation</returns>
		private ksEntity CreateNutBasePlaneCylinder(KompasPoint3D nutBasePoint, double basePlaneCylinderDepth)
		{
			// 1. Create plane in global coordinates.
			// We are using small cylinder with diameter 0.1*W3 in base plane YOZ
			// and extrude him in length: W1 + W2 + H + 0.1*W1*W2.
			// Extruded plane is plane in global coordinates.
			// After this we can simply delete the cylinder.

			// 1.1 Cylinder sketch
			var basePlaneCylinderSketch = new KompasSketch(_kompasApp.NutPart, Obj3dType.o3d_planeYOZ);
			if (basePlaneCylinderSketch.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = basePlaneCylinderSketch.LastErrorCode;
				return null;
			}

			var cylinderSketchEdit = basePlaneCylinderSketch.BeginEntityEdit();

			if (cylinderSketchEdit.ksCircle(nutBasePoint.Y, nutBasePoint.Z, _kompasApp.Parameters[0] * 0.1, 1) == 0)
			{
				LastErrorCode = ErrorCodes.Document2DCircleCreatingError;
				return null;
			}

			basePlaneCylinderSketch.EndEntityEdit();


			// 1.2 Cylinder extrusion
			var extrusionParameters = new KompasExtrusionParameters(_kompasApp.NutPart, Obj3dType.o3d_baseExtrusion, basePlaneCylinderSketch.Entity, Direction_Type.dtReverse, basePlaneCylinderDepth);
			var cylinderExtrusion = new KompasExtrusion(extrusionParameters, ExtrusionType.ByEntity);
			if (cylinderExtrusion.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = cylinderExtrusion.LastErrorCode;
				return null;
			}

			// 0.1 Muffler creation
			var mufflerParameters = new MufflerParameters();
			mufflerParameters.Document3DPart = _kompasApp.NutPart;
			mufflerParameters.Direction = Direction_Type.dtNormal;
			mufflerParameters.BasePlaneAxis = Obj3dType.o3d_planeYOZ;
			mufflerParameters.BasePlanePoint = new KompasPoint2D(nutBasePoint.Y, nutBasePoint.Z);

			var mufflerManager = new Muffler(_kompasApp, mufflerParameters);

			if (mufflerManager.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = mufflerManager.LastErrorCode;
				return null;
			}

			var mufflerExtrusion = mufflerManager.Extrusion;
			if (mufflerExtrusion == null)
			{
				LastErrorCode = ErrorCodes.ArgumentNull;
				return null;
			}

			// 1.3 Get nut base plane
			cylinderExtrusion.BaseFaceAreaState = KompasFaces.BaseFaceAreaState.BaseFaceAreaLower;
			var nutBasePlane = cylinderExtrusion.ExtrudedEntity;
			if (nutBasePlane == null)
			{
				LastErrorCode = cylinderExtrusion.LastErrorCode;
				return null;
			}

			// 0.2 Muffler deletion
			if (!mufflerManager.DeleteDetail())
			{
				return null;
			}

			return nutBasePlane;
		}

		/// <summary>
		/// Create nut base extrusion and sketch entities
		/// </summary>
		/// <param name="nutBasePlane">Base plane (see <seealso cref="CreateDetail"></seealso> for more info)</param>
		/// <param name="nutBasePoint">Nut base point</param>
		/// <param name="regPolyParam">Regular polygon parameters</param>
		/// <returns>
		/// An array of nut base sketch and extrusion entities 
		/// or null in case of error during creation
		/// </returns>
		private ksEntity[] CreateNutBase(ksEntity nutBasePlane, KompasPoint3D nutBasePoint, RegularPolygonParameter regPolyParam)
		{
			// 1.1 Nut base sketch
			var nutBaseSketch = new KompasSketch(_kompasApp.NutPart, nutBasePlane);
			if (nutBaseSketch.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = nutBaseSketch.LastErrorCode;
				return null;
			}

			var nutBaseEdit = nutBaseSketch.BeginEntityEdit();
			if (nutBaseEdit == null)
			{
				LastErrorCode = nutBaseSketch.LastErrorCode;
				return null;
			}
			
			// Nut width is equal to W3 (screw hat width)
			if (nutBaseEdit.ksRegularPolygon(regPolyParam.FigureParam, 1) == 0)
			{
				LastErrorCode = ErrorCodes.Document2DRegPolyCreatingError;
				return null;
			}

			nutBaseSketch.EndEntityEdit();

			// 1.2 Nut base extrusion
			var extrusionParameters = new KompasExtrusionParameters(_kompasApp.NutPart, Obj3dType.o3d_baseExtrusion, nutBaseSketch.Entity, Direction_Type.dtReverse, _kompasApp.Parameters[4] / 2.0);
			var nutBaseExtrusion = new KompasExtrusion(extrusionParameters, ExtrusionType.ByEntity);  // / H /

			if (nutBaseExtrusion.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = nutBaseExtrusion.LastErrorCode;
				return null;
			}

			// 0.1 Create muffler
			var mufflerParameters = new MufflerParameters();
			mufflerParameters.Document3DPart = _kompasApp.NutPart;
			mufflerParameters.Direction = Direction_Type.dtNormal;
			mufflerParameters.BasePlaneAxis = Obj3dType.o3d_planeXOZ;
			mufflerParameters.BasePlanePoint = new KompasPoint2D(nutBasePoint.Y, nutBasePoint.Z);

			var muffler = new Muffler(_kompasApp, mufflerParameters, nutBaseSketch.Entity);
			if (muffler.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = muffler.LastErrorCode;
				return null;
			}

			// 1.3 Get base plane
			nutBaseExtrusion.BaseFaceAreaState = KompasFaces.BaseFaceAreaState.BaseFaceAreaLower;
			var nutBaseExtrudedEntity = nutBaseExtrusion.ExtrudedEntity;
			if (nutBaseExtrudedEntity == null)
			{
				LastErrorCode = nutBaseExtrusion.LastErrorCode;
				return null;
			}

			// 0.2 Delete muffler
			if (!muffler.DeleteDetail())
			{
				LastErrorCode = muffler.LastErrorCode;
				return null;
			}

			return new ksEntity[2] { nutBaseSketch.Entity, nutBaseExtrudedEntity };
		}

		/// <summary>
		/// Delete cylinder which sets base plane of nut
		/// </summary>
		/// <param name="nutBasePlane">Base plane (see <seealso cref="CreateDetail"></seealso> for more info)</param>
		/// <param name="basePlaneCylinderDepth">Depth of cylinder of base plane (see <seealso cref="CreateDetail"></seealso> for more info)</param>
		/// <returns>true if operation successful, false in case of error</returns>
		private bool DeleteNutBasePlaneCylinder(ksEntity nutBasePlane, double basePlaneCylinderDepth)
		{
			// 1.2 Delete cylinder
			// В случае чего поменять Reverse на Normal
			var extrusionParameters = new KompasExtrusionParameters(_kompasApp.NutPart, Obj3dType.o3d_cutExtrusion, nutBasePlane, Direction_Type.dtNormal, basePlaneCylinderDepth);
			var cylinderDeletion = new KompasExtrusion(extrusionParameters, ExtrusionType.ByEntity);
			if (cylinderDeletion.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = cylinderDeletion.LastErrorCode;
				return false;
			}

			return true;
		}

		/// <summary>
		/// Create rounded chamfers inside nut
		/// </summary>
		/// <param name="nutBasePoint">Nut base point</param>
		/// <param name="regPolyParam">Regular polygon parameters</param>
		/// <param name="nutBaseEntities">Nut base entities: sketch and extrusion</param>
		/// <returns>Chamfers entities from left and right sides of nut</returns>
		private ksEntity[] CreateNutChamferEntities(KompasPoint3D nutBasePoint, RegularPolygonParameter regPolyParam, ksEntity[] nutBaseEntities)
		{
			// 1.3 Nut rounded chamfers creation:
			// 1.3.1 Right rounded chamfer
			var rightChamferPoint = new KompasPoint2D(Math.Abs(nutBasePoint.Y), Math.Abs(nutBasePoint.Z));

			// Nut width is equal to W3 (screw hat width)
			var rightChamferRegPoly = new RegularPolygonParameter(_kompasApp, 6, _kompasApp.Parameters[0] / 2.0, rightChamferPoint);
			if (rightChamferRegPoly == null)
			{
				LastErrorCode = rightChamferRegPoly.LastErrorCode;
				return null;
			}

			var rightChamferSketch = new KompasSketch(_kompasApp.NutPart, nutBaseEntities[1]);
			if (rightChamferSketch.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = rightChamferSketch.LastErrorCode;
				return null;
			}

			var rightChamferParameters = new RoundedChamferParameters();
			rightChamferParameters.Document3DPart = _kompasApp.NutPart;
			rightChamferParameters.RegularPolygonSketch = rightChamferSketch.Entity;
			rightChamferParameters.RegularPolygonParameters = rightChamferRegPoly;
			rightChamferParameters.BasePlanePoint = rightChamferPoint;
			rightChamferParameters.Direction = Direction_Type.dtNormal;

			var rightChamferManager = new RoundedChamfer(_kompasApp, rightChamferParameters);
			if (!rightChamferManager.CreateDetail())
			{
				LastErrorCode = rightChamferManager.LastErrorCode;
				return null;
			}

			if (rightChamferManager.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = rightChamferManager.LastErrorCode;
				return null;
			}

			var rightChamferEntity = rightChamferManager.Entity;
			if (rightChamferEntity == null)
			{
				LastErrorCode = rightChamferManager.LastErrorCode;
				return null;
			}

			// 1.3.2 Left rounded chamfer
			var leftChamferParameters = new RoundedChamferParameters();
			leftChamferParameters.Document3DPart = _kompasApp.NutPart;
			leftChamferParameters.RegularPolygonSketch = nutBaseEntities[0];
			leftChamferParameters.RegularPolygonParameters = regPolyParam;
			leftChamferParameters.BasePlanePoint = new KompasPoint2D(nutBasePoint.Y, nutBasePoint.Z);
			leftChamferParameters.Direction = Direction_Type.dtNormal;

			var leftChamferManager = new RoundedChamfer(_kompasApp, leftChamferParameters);
			if (!leftChamferManager.CreateDetail())
			{
				LastErrorCode = leftChamferManager.LastErrorCode;
				return null;
			}

			if (leftChamferManager.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = leftChamferManager.LastErrorCode;
				return null;
			}

			var leftChamferEntity = leftChamferManager.Entity;
			if (leftChamferEntity == null)
			{
				LastErrorCode = leftChamferManager.LastErrorCode;
				return null;
			}

			return new ksEntity[2] { leftChamferEntity, rightChamferEntity };
		}

		/// <summary>
		/// Create nut base cut for thread of screw
		/// </summary>
		/// <param name="chamferEntities">Chamfer entities from left and right sides of nut</param>
		/// <param name="nutBasePoint">Nut base point</param>
		/// <returns>true if operation successful; false in case of error</returns>
		private bool CreateBaseCut(ksEntity[] chamferEntities, KompasPoint3D nutBasePoint)
		{
			// 1.4 Nut base cut
			var nutBaseCut = new KompasSketch(_kompasApp.NutPart, chamferEntities[0]);
			if (nutBaseCut.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = ErrorCodes.EntityCreateError;
				return false;
			}

			var nutBaseCutEdit = nutBaseCut.BeginEntityEdit();
			if (nutBaseCutEdit == null)
			{
				LastErrorCode = ErrorCodes.ArgumentNull;
				return false;
			}
			if (nutBaseCutEdit.ksCircle(nutBasePoint.Y, nutBasePoint.Z, 0.75 * _kompasApp.Parameters[5] / 2.0, 1) == 0)   // / D /
			{
				LastErrorCode = ErrorCodes.Document2DCircleCreatingError;
				return false;
			}

			nutBaseCut.EndEntityEdit();

			// 1.5 Nut base cut extrusion
			var extrusionParameters = new KompasExtrusionParameters(_kompasApp.NutPart, Obj3dType.o3d_cutExtrusion, nutBaseCut.Entity, Direction_Type.dtNormal, _kompasApp.Parameters[4]);
			var nutBaseCutExtrusion = new KompasExtrusion(extrusionParameters, ExtrusionType.ByEntity);    // / H /
			if (nutBaseCutExtrusion.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = nutBaseCutExtrusion.LastErrorCode;
				return false;
			}

			return true;
		}

		/// <summary>
		/// Create nut thread
		/// </summary>
		/// Creating inner thread tooth:
		///				  endZZ
		///			A------=------B	•–→X
		///			|\	   |	 /|	↓
		///			| \	   |	/ |	Z
		///			|  \   |   /  |
		///	startX	|	\  |  /	  | endX
		///			|	 \ | /	  |
		///			|     \|/	  |
		///			|------C------|
		///				 startZ	
		/// 
		///		
		///		Here offsetX is (W1 + W2 + H + 0.1*W1*W2 (i.e. _basePlaneCylinderDepth) - 0.68 * H (i.e. nut height minus heights of right and left chamfers) 
		///		(chamfer is 0.16 of neight of nut)
		///		
		///		threadStartX = offsetX - step;
		///		threadEndX = offsetX;
		///		
		///		threadEndZ = 1/2 * (0.7 * W3)
		///		threadStartZ = 1/2 * (3/4 * 0.7 * W3)
		///
		///		A = (threadStartX; threadEndZ);
		///		B = (threadEndX; threadEndZ);
		///		C = (threadStartX + step/2; threadStartZ);
		/// <param name="chamferEntities">Chamfer entities from left and right sides of nut</param>
		/// <param name="nutBasePoint">Nut base point</param>
		/// <param name="basePlaneCylinderDepth">Depth of cylinder of base plane (see <seealso cref="CreateDetail"></seealso> for more info)</param>
		/// <returns>true if operation successful; false in case of error</returns>
		private bool CreateNutThread(ksEntity[] chamferEntities, KompasPoint3D nutBasePoint, double basePlaneCylinderDepth)
		{
			// 1.6 Nut base thread spin
			var spinParameters = new SpinParameters();
			spinParameters.Document3DPart = _kompasApp.NutPart;
			spinParameters.BeginSpinFace = chamferEntities[1];
            spinParameters.EndSpinFace = chamferEntities[0];
			spinParameters.SpinLocationPoint = new KompasPoint2D(nutBasePoint.Y, nutBasePoint.Z);
			spinParameters.DiameterSize = _kompasApp.Parameters[5];
			spinParameters.SpinStep = _kompasApp.ThreadStep;

			var nutThreadSpin = new Spin(spinParameters); // D
			if (nutThreadSpin.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = nutThreadSpin.LastErrorCode;
				return false;
			}

			// 1.7 Nut base thread sketch
			var nutThreadSketch = new KompasSketch(_kompasApp.NutPart, Obj3dType.o3d_planeXOZ);
			if (nutThreadSketch.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = nutThreadSketch.LastErrorCode;
				return false;
			}

			var nutThreadEdit = nutThreadSketch.BeginEntityEdit();

			// See comment on top of the function
			// 0.18 is 0.16 plus some extra space: 0.02.
			var offsetX = basePlaneCylinderDepth + _kompasApp.Parameters[4] * 0.18;
			var step = _kompasApp.ThreadStep;

			var threadStartX = offsetX - step;
			var threadEndX = offsetX;

			var threadStartZ = 1.0 / 2.0 * (_kompasApp.Parameters[0] * 0.7);                // 1/2 * (0.7 * W3)
			var threadEndZ = 1.0 / 2.0 * (_kompasApp.Parameters[0] * 0.7 * (3.0 / 4.0));	// 1/2 * (3/4 * 0.7 * W3)

			//	A = (threadStartX; threadEndZ);
			//	B = (threadEndX; threadEndZ);
			//	C = (threadStartX + step/2; threadStartZ);
			nutThreadEdit.ksLineSeg(threadStartX, threadEndZ, threadEndX, threadEndZ, 1);						// AB
			nutThreadEdit.ksLineSeg(threadStartX, threadEndZ, threadStartX + step / 2.0, threadStartZ, 1);		// AC
			nutThreadEdit.ksLineSeg(threadStartX + step / 2.0, threadStartZ, threadEndX, threadEndZ, 1);		// CB

			nutThreadSketch.EndEntityEdit();

			// 1.8 Nut base spin creation
			var spinCollection = (ksEntityCollection)_kompasApp.ScrewPart.EntityCollection((short)Obj3dType.o3d_cylindricSpiral);

			spinCollection.Clear();
			spinCollection.Add(nutThreadSpin.Entity);
			spinCollection.refresh();

			var extrusionParameters = new KompasExtrusionParameters(_kompasApp.NutPart, Obj3dType.o3d_cutEvolution, nutThreadSketch.Entity, spinCollection);
			var nutBaseSpin = new KompasExtrusion(extrusionParameters, ExtrusionType.BySketchesCollection);
			if (nutBaseSpin.LastErrorCode != ErrorCodes.OK)
			{
				LastErrorCode = nutBaseSpin.LastErrorCode;
				return false;
			}

			return true;
		}
	}
}
