using Kompas6API5;
using Kompas6Constants3D;

namespace ScrewNutUI.Parameters
{
    /// <summary>
    ///     Parameters for "KompasExtrusion" class.
    /// </summary>
    public class KompasExtrusionParameters
    {
        /// <summary>
        ///     Part with detail in 3d document
        /// </summary>
        public ksPart Document3DPart { get; }

        /// <summary>
        ///     Type of extrusion
        /// </summary>
        public Obj3dType ExtrusionType { get; }

        /// <summary>
        ///     Extrudable entity
        /// </summary>
        public ksEntity ExtrudableEntity { get; }

        /// <summary>
        ///     Type of direction of extrusion
        /// </summary>
        public Direction_Type Direction { get; }

        /// <summary>
        ///     Depth of extrusion
        /// </summary>
        public double Depth { get; }

        /// <summary>
        ///     Collection of extrudable sketches for types
        ///     such as loft, evolution etc.
        /// </summary>
        public ksEntityCollection SketchesCollection { get; }

        /// <summary>
        ///     Kompas extrusion parameters for extrusion by entity
        /// </summary>
        /// <param name="document3DPart">Part with detail in 3d document</param>
        /// <param name="extrusionType">Type of extrusion</param>
        /// <param name="extrudableEntity">Extrudable (выдавливаЕМЫЙ) entity</param>
        /// <param name="direction">Type of direction of extrusion</param>
        /// <param name="depth">Depth of extrusion</param>
        public KompasExtrusionParameters(ksPart document3DPart, Obj3dType extrusionType,
            ksEntity extrudableEntity, Direction_Type direction, double depth)
        {
            Document3DPart = document3DPart;
            ExtrusionType = extrusionType;
            ExtrudableEntity = extrudableEntity;
            Direction = direction;
            Depth = depth;

            SketchesCollection = null;
        }

        /// <summary>
        ///     Kompas extrusion parameters for extrusion by sketches collection
        /// </summary>
        /// <param name="document3DPart">Part with detail in 3d document</param>
        /// <param name="extrusionType">Type of extrusion</param>
        /// <param name="extrudableEntity">Extrudable (выдавливаЕМЫЙ) entity</param>
        /// <param name="sketchesCollection">
        ///     Collection of extrudable sketches for types
        ///     such as loft, evolution etc.
        /// </param>
        public KompasExtrusionParameters(ksPart document3DPart, Obj3dType extrusionType,
            ksEntity extrudableEntity, ksEntityCollection sketchesCollection)
        {
            Document3DPart = document3DPart;
            ExtrusionType = extrusionType;
            ExtrudableEntity = extrudableEntity;
            SketchesCollection = sketchesCollection;

            Direction = Direction_Type.dtBoth;
            Depth = default(double);
        }
    }
}