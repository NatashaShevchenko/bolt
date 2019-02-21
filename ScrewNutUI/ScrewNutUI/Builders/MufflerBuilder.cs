using System;
using Kompas6API5;
using Kompas6Constants3D;
using ScrewNutUI.Managers;
using ScrewNutUI.Parameters;

namespace ScrewNutUI.Builders
{
    internal class MufflerBuilder
    {
        /// <summary>
        ///     Kompas object
        /// </summary>
        private readonly KompasApplication _kompasApp;

        /// <summary>
        ///     Muffler manager constructor
        /// </summary>
        /// <param name="kompasApp">Kompas application specimen</param>
        /// <param name="basePlane">Base plane of muffler, by default is null</param>
        public MufflerBuilder(KompasApplication kompasApp, Obj3dType basePlaneAxis, Direction_Type direction,
            Point2D basePlanePoint, ksEntity basePlane = null)
        {
            if (!(direction == Direction_Type.dtNormal
                  || direction == Direction_Type.dtReverse))
                throw new ArgumentException("Ошибка при создании вспомогательного элемента.");

            _kompasApp = kompasApp;
            Document3DPart = kompasApp.ScrewPart;
            BasePlaneAxis = basePlaneAxis;
            Direction = direction;
            BasePlanePoint = basePlanePoint;

            Extrusion = CreateMuffler(basePlane) ?? throw new
                            InvalidOperationException(
                                "Ошибка при создании вспомогательного элемента.");
        }

        /// <summary>
        ///     Part with detail in 3D document
        /// </summary>
        public ksPart Document3DPart { get; }

        /// <summary>
        ///     Axis of base plane of muffler
        /// </summary>
        public Obj3dType BasePlaneAxis { get; }

        /// <summary>
        ///     Direction type of muffler
        /// </summary>
        public Direction_Type Direction { get; }

        /// <summary>
        ///     Point of base plane of muffler
        /// </summary>
        public Point2D BasePlanePoint { get; }

        /// <summary>
        ///     Muffler extrusion getter
        /// </summary>
        public ExtrusionManager Extrusion { get; }

        /// <summary>
        ///     Create muffler in detail in base plane axis
        /// </summary>
        /// <param name="figureParameters">Parameters of muffler</param>
        /// <param name="basePlane">Base plane of muffler, by default is null</param>
        /// <returns>Muffler extrusion or null if extrusion returns error</returns>
        private ExtrusionManager CreateMuffler(ksEntity basePlane)
        {
            var muffler = new KompasSketchManager(Document3DPart, BasePlaneAxis);

            if (basePlane != null) muffler = new KompasSketchManager(Document3DPart, basePlane);

            var mufflerSketchEdit = muffler.BeginEntityEdit();
            if (mufflerSketchEdit == null) return null;

            var mufflerRectangleParam = new RectangleParameter(_kompasApp, _kompasApp.Parameters[0],
                _kompasApp.Parameters[0], BasePlanePoint);
            if (mufflerSketchEdit.ksRectangle(mufflerRectangleParam.FigureParam) == 0) return null;

            muffler.EndEntityEdit();

            var extrusionParameters = new KompasExtrusionParameters(Document3DPart,
                Obj3dType.o3d_baseExtrusion, muffler.Entity,
                Direction, 13);
            var mufflerExtrusion = new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity);

            return mufflerExtrusion;
        }

        /// <summary>
        ///     Delete muffler from document 3D part
        /// </summary>
        public bool DeleteDetail()
        {
            if (Extrusion == null) return false;

            Extrusion.BaseFaceAreaState = KompasFacesManager.BaseFaceAreaState.BaseFaceAreaLower;
            var extruded = Extrusion.ExtrudedEntity;
            if (extruded == null) return false;

            var extrusionParameters = new KompasExtrusionParameters(Document3DPart,
                Obj3dType.o3d_cutExtrusion, extruded, Direction,
                13);
            var mufflerDeletion = new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity);

            return true;
        }
    }
}