using System;
using Kompas6API5;
using Kompas6Constants3D;
using ScrewNutUI.Managers;
using ScrewNutUI.Parameters;

namespace ScrewNutUI.Builders
{
    /// <summary>
    ///     Rounded chamfer
    /// </summary>
    internal class RoundedChamferBuilder
    {
        /// <summary>
        ///     Kompas application specimen
        /// </summary>
        private readonly KompasApplication _kompasApp;

        /// <summary>
        ///     Create rounded chamfer in regular polygon sketch by base sketch,
        ///     regular polygon parameters and base plane coordinates in plane
        /// </summary>
        /// <returns>Entity of rounded chamfer or null in case of error</returns>
        public RoundedChamferBuilder(KompasApplication kompasApp, ksEntity regularPolygonSketch,
            PolygonParameter regularPolygonParameters, Point2D basePlanePoint,
            Direction_Type direction, double chamferRadius)
        {
            if (kompasApp == null
                || regularPolygonSketch == null
                || regularPolygonParameters == null
                || Math.Abs(chamferRadius - default(double)) < 0.01)
            {
                throw new ArgumentException("Ошибка при создании скругления головки болта. ");
            }
                

            _kompasApp = kompasApp;
            Document3DPart = kompasApp.ScrewPart;
            RegularPolygonSketch = regularPolygonSketch;
            RegularPolygonParameters = regularPolygonParameters;
            BasePlanePoint = basePlanePoint;
            Direction = direction;
            ChamferRadius = chamferRadius;
        }

        /// <summary>
        ///     Part with detail in 3D document
        /// </summary>
        public ksPart Document3DPart { get; }

        /// <summary>
        ///     Sketch of regular polygon -- base of rounded chamfer
        /// </summary>
        public ksEntity RegularPolygonSketch { get; }

        /// <summary>
        ///     Parameters of regular polygon -- base of rounded chamfer
        /// </summary>
        public PolygonParameter RegularPolygonParameters { get; }

        /// <summary>
        ///     2D point of base plane
        /// </summary>
        public Point2D BasePlanePoint { get; }

        /// <summary>
        ///     Direction of rounded chamfer
        /// </summary>
        public Direction_Type Direction { get; }

        /// <summary>
        ///     Rounded chamfer entity getter
        /// </summary>
        public ksEntity Entity { get; private set; }

        public double ChamferRadius { get; }

        /// <summary>
        ///     Create rounded chamfer
        /// </summary>
        /// <returns>true if operation successful; false in case of error</returns>
        public bool CreateDetail()
        {
            var baseOfChamfer = CreateBase();
            if (baseOfChamfer == null) return false;

            if (!CreateSection(baseOfChamfer)) return false;

            return true;
        }

        /// <summary>
        ///     Create base of rounded chamfer
        /// </summary>
        /// <returns>Extruded entity of base of rounded chamfer</returns>
        private ksEntity CreateBase()
        {
            var innerCircleSketch = new KompasSketchManager(Document3DPart, RegularPolygonSketch);

            var innerCircleEdit = innerCircleSketch.BeginEntityEdit();
            if (innerCircleEdit == null) return null;

            if (innerCircleEdit.ksCircle(BasePlanePoint.X,
                    BasePlanePoint.Y, _kompasApp.Parameters[0] * 0.88 / 2.0, 1) == 0)
                return null;

            innerCircleSketch.EndEntityEdit();

            var extrusionParameters = new KompasExtrusionParameters(Document3DPart,
                Obj3dType.o3d_baseExtrusion, innerCircleSketch.Entity, Direction, 50 * 0.16);
            var innerCircleExtrusion =
                new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity)
                {
                    BaseFaceAreaState = KompasFacesManager.BaseFaceAreaState.BaseFaceAreaLower
                };

            var extruded = innerCircleExtrusion.ExtrudedEntity;

            return extruded;
        }

        /// <summary>
        ///     Create section operation of rounded chamfer
        /// </summary>
        /// <returns>true if operation successful; false in case of error</returns>
        private bool CreateSection(ksEntity baseOfChamfer)
        {
            var extraInnerCircleSketch = new KompasSketchManager(Document3DPart, baseOfChamfer);

            var extraInnerCircleEdit = extraInnerCircleSketch.BeginEntityEdit();
            if (extraInnerCircleEdit == null) return false;

            if (extraInnerCircleEdit.ksCircle(BasePlanePoint.X,
                    BasePlanePoint.Y, (_kompasApp.Parameters[0] - ChamferRadius) * 0.88 / 2.0, 1) == 0)
                return false;

            extraInnerCircleSketch.EndEntityEdit();

            var extraRegPolySketch = new KompasSketchManager(Document3DPart, RegularPolygonSketch);

            var extraRegPolyEdit = extraRegPolySketch.BeginEntityEdit();
            if (extraRegPolyEdit == null) return false;

            if (extraRegPolyEdit.ksRegularPolygon(RegularPolygonParameters.FigureParam, 0) == 0) return false;

            extraRegPolySketch.EndEntityEdit();

            var screwChamferSketches =
                (ksEntityCollection) _kompasApp.Document3D.EntityCollection((short) Obj3dType.o3d_sketch);
            screwChamferSketches.Clear();
            screwChamferSketches.Add(extraInnerCircleSketch.Entity);
            screwChamferSketches.Add(extraRegPolySketch.Entity);
            screwChamferSketches.refresh();

            var extrusionParameters = new KompasExtrusionParameters(Document3DPart,
                Obj3dType.o3d_baseLoft, null, screwChamferSketches);
            var screwChamferExtrusion = new ExtrusionManager(extrusionParameters, ExtrusionType.BySketchesCollection);

            if (extraInnerCircleSketch.Entity == null) return false;

            Entity = extraInnerCircleSketch.Entity;
            return true;
        }
    }
}