using System;
using Kompas6API5;
using Kompas6Constants3D;

namespace ScrewNutUI.Managers
{
    /// <summary>
    ///     Sketch class.
    ///     Represents sketch in 2D entity in detail.
    /// </summary>
    public class KompasSketchManager
    {
        /// <summary>
        ///     Base plane fo sketch
        /// </summary>
        private readonly ksEntity _basePlane;

        /// <summary>
        ///     Axis of base plane
        /// </summary>
        private readonly Obj3dType _basePlaneAxis;

        /// <summary>
        ///     Sketch definition
        /// </summary>
        private ksSketchDefinition _sketchDef;

        /// <summary>
        ///     Create sketch by base plane
        /// </summary>
        /// <param name="doc3DPart">Document 3D part</param>
        /// <param name="basePlane">Base plane</param>
        public KompasSketchManager(ksPart doc3DPart, ksEntity basePlane)
        {
            if (doc3DPart == null || basePlane == null) return;

            _basePlane = basePlane;

            Entity = CreateEntity(doc3DPart);
        }

        /// <summary>
        ///     Create sketch by base plane
        /// </summary>
        /// <param name="doc3DPart">Document 3D part</param>
        /// <param name="basePlaneAxis">Base plane axis</param>
        public KompasSketchManager(ksPart doc3DPart, Obj3dType basePlaneAxis)
        {
            if (doc3DPart == null) return;

            if (!(basePlaneAxis == Obj3dType.o3d_planeXOY
                  || basePlaneAxis == Obj3dType.o3d_planeXOZ
                  || basePlaneAxis == Obj3dType.o3d_planeYOZ))
            {
                throw new ArgumentException("Ошибка при построении эскизка.");
            }

            _basePlaneAxis = basePlaneAxis;

            Entity = CreateEntity(doc3DPart);
        }

        /// <summary>
        ///     Sketch entity getter
        /// </summary>
        public ksEntity Entity { get; }

        /// <summary>
        ///     Begin entity edit
        /// </summary>
        /// <returns>Kompas 2D document (editable sketch)</returns>
        public ksDocument2D BeginEntityEdit()
        {
            if (_sketchDef == null) return null;
            return (ksDocument2D) _sketchDef.BeginEdit();
        }

        /// <summary>
        ///     End entity edit
        /// </summary>
        public void EndEntityEdit()
        {
            _sketchDef.EndEdit();
        }

        /// <summary>
        ///     Create entity by base plane
        /// </summary>
        /// <param name="doc3DPart">Part of 3D document (detail in build)</param>
        /// <returns>true if operation successful; false in case of error</returns>
        private ksEntity CreateEntity(ksPart doc3DPart)
        {
            // Sketch
            var sketch = (ksEntity) doc3DPart.NewEntity((short) Obj3dType.o3d_sketch);
            if (sketch == null) return null;

            // Sketch definition
            var sketchDef = (ksSketchDefinition) sketch.GetDefinition();
            if (sketchDef == null) return null;

            // Base plane for sketch
            var basePlane = GetBasePlane(doc3DPart);
            if (basePlane == null) return null;

            sketchDef.SetPlane(basePlane);
            if (sketch.Create() != true) return null;

            _sketchDef = sketchDef;

            return sketch;
        }

        /// <summary>
        ///     Get base plane by axis or get already set base plane
        /// </summary>
        /// <param name="doc3DPart">Part of 3D document (detail in build)</param>
        /// <returns>Already set base plane or base plane by axis</returns>
        private ksEntity GetBasePlane(ksPart doc3DPart)
        {
            ksEntity basePlane;

            if (_basePlane != null)
                basePlane = _basePlane;
            else
                basePlane = (ksEntity) doc3DPart.GetDefaultEntity((short) _basePlaneAxis);

            return basePlane;
        }
    }
}