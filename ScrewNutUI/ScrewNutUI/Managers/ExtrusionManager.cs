using System;
using Kompas6API5;
using Kompas6Constants3D;
using ScrewNutUI.Builders;
using ScrewNutUI.Parameters;

namespace ScrewNutUI.Managers
{
    /// <summary>
    ///     Type of extrusion: by entity (depth and direction)
    ///     or by collecton of sketches
    /// </summary>
    public enum ExtrusionType
    {
        ByEntity,
        BySketchesCollection
    }

    /// <summary>
    ///     Extrusion class.
    ///     Represents extrusion of detail of 3D document.
    /// </summary>
    public class ExtrusionManager
    {
        /// <summary>
        ///     Part with detail in document
        /// </summary>
        private readonly ksPart _doc3DPart;

        /// <summary>
        ///     Extruded entity
        /// </summary>
        private ksEntity _extrudedEntity;

        /// <summary>
        ///     Faces count after extrusion
        /// </summary>
        private readonly int _facesCountAfterExtrusion;

        /// <summary>
        ///     Faces count before extrusion
        /// </summary>
        private readonly int _facesCountBeforeExtrusion;

        /// <summary>
        ///     Extrusion by direction, depth and extrudable entity
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="extrusionType">Extrusion type</param>
        public ExtrusionManager(KompasExtrusionParameters parameters, ExtrusionType extrusionType)
        {
            // Extrusion entity
            var entity = (ksEntity) parameters.Document3DPart
                .NewEntity((short) parameters.ExtrusionType);

            if (parameters.ExtrusionType != Obj3dType.o3d_baseLoft 
                && parameters.ExtrudableEntity == null)
            {
                GenerateException();
            }

            // Get direction of extrusion
            var normalDirection = true;

            if (extrusionType == ExtrusionType.ByEntity)
            {
                if (parameters.Direction == Direction_Type.dtReverse)
                {
                    normalDirection = false;
                }
                else if (parameters.Direction == Direction_Type.dtNormal)
                {
                    normalDirection = true;
                }
                else
                {
                    GenerateException();
                }

                // Depth must not be equal to zero
                if (Math.Abs(parameters.Depth) < 0.001)
                {
                    GenerateException();
                }
            }

            // Entity faces count before extrusion
            var faceCollection =
                (ksEntityCollection) parameters.Document3DPart.EntityCollection((short) Obj3dType.o3d_face);
            _facesCountBeforeExtrusion = faceCollection.GetCount();

            switch (extrusionType)
            {
                case ExtrusionType.ByEntity:
                    if (!CreateExtrusionByDirection(entity, parameters, normalDirection))
                    {
                        GenerateException();
                    }
                    break;
                case ExtrusionType.BySketchesCollection:
                    if (!CreateExtrusionBySketchCollection(entity, parameters))
                    {
                        GenerateException();
                    }
                    break;
            }

            _doc3DPart = parameters.Document3DPart;

            // Get faces count after extrusion
            faceCollection.refresh();
            _facesCountAfterExtrusion = faceCollection.GetCount();
        }

        /// <summary>
        ///     Main base face area state. Necessary for correctly define _main_ base plane and _parallel_ base plane.
        /// </summary>
        public KompasFacesManager.BaseFaceAreaState BaseFaceAreaState { get; set; }

        /// <summary>
        ///     Extruded entity
        /// </summary>
        public ksEntity ExtrudedEntity
        {
            get
            {
                // If extruded entity isn't finded -- find where is it after extrusion operation
                if (_extrudedEntity == null)
                    return _extrudedEntity = GetExtrudedEntity();
                // Else just return already finded entity
                return _extrudedEntity;
            }
        }

        /// <summary>
        ///     Create extrusion based on extrudable sketch
        /// </summary>
        /// <param name="entity">Entity of extrusion</param>
        /// <param name="normalDirection">Normal direction of extrusion (true if normal or false if reversed)</param>
        /// <returns>true if operation is successful; false in case of error</returns>
        private bool CreateExtrusionByDirection(ksEntity entity, KompasExtrusionParameters parameters,
            bool normalDirection)
        {
            switch (parameters.ExtrusionType)
            {
                case Obj3dType.o3d_baseExtrusion:
                    if (!SetBaseExtrusionDefinition(entity, parameters, normalDirection)) return false;
                    break;
                case Obj3dType.o3d_cutExtrusion:
                    if (!SetCutExtrusionDefinition(entity, parameters, normalDirection)) return false;
                    break;
                default:
                    return false;
            }

            if (entity.Create() != true) return false;

            return true;
        }

        /// <summary>
        ///     Create extrusion based on extrudable sketch
        /// </summary>
        /// <param name="entity">Entity of extrusion</param>
        /// <returns>true if operation is successful; false in case of error</returns>
        private bool CreateExtrusionBySketchCollection(ksEntity entity, KompasExtrusionParameters parameters)
        {
            switch (parameters.ExtrusionType)
            {
                case Obj3dType.o3d_baseEvolution:
                    if (!SetBaseEvolutionDefinition(entity, parameters)) return false;
                    break;
                case Obj3dType.o3d_cutEvolution:
                    if (!SetCutEvolutionDefinition(entity, parameters)) return false;
                    break;
                case Obj3dType.o3d_baseLoft:
                    if (!SetBaseLoftDefinition(entity, parameters)) return false;
                    break;
                default:
                    return false;
            }

            if (entity.Create() != true) return false;

            return true;
        }

        /// <summary>
        ///     Set base extruson definition
        /// </summary>
        /// <param name="entity">Extrusion entity</param>
        /// <param name="normalDirection">Normal direction of extrusion</param>
        /// <returns>true if operation successful, false in case of error</returns>
        private bool SetBaseExtrusionDefinition(ksEntity entity, KompasExtrusionParameters parameters,
            bool normalDirection)
        {
            var entityDefBase = (ksBaseExtrusionDefinition) entity.GetDefinition();
            if (entityDefBase == null) return false;

            entityDefBase.directionType = (short) parameters.Direction;

            if (entityDefBase.SetSideParam(normalDirection, (short) ksEndTypeEnum.etBlind, parameters.Depth) !=
                true) return false;
            if (entityDefBase.SetSketch(parameters.ExtrudableEntity) != true) return false;

            return true;
        }

        /// <summary>
        ///     Set cut extrusion
        /// </summary>
        /// <param name="entity">Extrusion entity</param>
        /// <returns>true if operation successful, false in case of error</returns>
        private bool SetCutExtrusionDefinition(ksEntity entity, KompasExtrusionParameters parameters,
            bool normalDirection)
        {
            var entityDefCut = (ksCutExtrusionDefinition) entity.GetDefinition();
            if (entityDefCut == null) return false;

            entityDefCut.directionType = (short) parameters.Direction;

            if (entityDefCut.SetSideParam(normalDirection, (short) ksEndTypeEnum.etBlind, parameters.Depth) !=
                true) return false;
            if (entityDefCut.SetSketch(parameters.ExtrudableEntity) != true) return false;

            return true;
        }

        /// <summary>
        ///     Set base loft definition
        /// </summary>
        /// <param name="entity">Extruson entity</param>
        /// <returns>true if operation successful, false in case of error</returns>
        private bool SetBaseLoftDefinition(ksEntity entity, KompasExtrusionParameters parameters)
        {
            var entityDefBaseLoft = (ksBaseLoftDefinition) entity.GetDefinition();

            if (entityDefBaseLoft == null) return false;
            if (parameters.SketchesCollection == null) return false;
            if (parameters.SketchesCollection.GetCount() == 0) return false;

            // Set loft sketches
            for (int i = 0, count = parameters.SketchesCollection.GetCount(); i < count; i++)
                entityDefBaseLoft.Sketchs()
                    .Add(i == 0 ? parameters.SketchesCollection.First() : parameters.SketchesCollection.Next());

            if (entityDefBaseLoft.SetLoftParam(
                    true /*directory is closed*/,
                    true /*parameter reserved by API >_< */,
                    true /*autopath*/
                ) != true)
                return false;

            return true;
        }

        /// <summary>
        ///     Set base evolution extrusion definition
        /// </summary>
        /// <param name="entity">Extruson entity</param>
        /// <returns>true if operation successful, false in case of error</returns>
        private bool SetBaseEvolutionDefinition(ksEntity entity, KompasExtrusionParameters parameters)
        {
            var entityDefEvolution = (ksBaseEvolutionDefinition) entity.GetDefinition();
            if (entityDefEvolution == null)
                return false;
            if (parameters.SketchesCollection == null)
                return false;
            if (parameters.SketchesCollection.GetCount() == 0)
                return false;

            for (int i = 0, count = parameters.SketchesCollection.GetCount(); i < count; i++)
            {
                entityDefEvolution.PathPartArray()
                    .Add(i == 0 ? parameters.SketchesCollection.First() : parameters.SketchesCollection.Next());
            }

            if (entityDefEvolution.SetSketch(parameters.ExtrudableEntity) != true)
                return false;

            return true;
        }

        /// <summary>
        ///     Set cut evolution extrusion definition
        /// </summary>
        /// <param name="entity">Extruson entity</param>
        /// <returns>true if operation successful, false in case of error</returns>
        private bool SetCutEvolutionDefinition(ksEntity entity, KompasExtrusionParameters parameters)
        {
            var entityDefEvolution = (ksCutEvolutionDefinition) entity.GetDefinition();
            if (entityDefEvolution == null) return false;
            if (parameters.SketchesCollection == null) return false;
            if (parameters.SketchesCollection.GetCount() == 0) return false;

            for (int i = 0, count = parameters.SketchesCollection.GetCount(); i < count; i++)
                if (i == 0)
                    entityDefEvolution.PathPartArray().Add(parameters.SketchesCollection.First());
                else
                    entityDefEvolution.PathPartArray().Add(parameters.SketchesCollection.Next());

            if (entityDefEvolution.SetSketch(parameters.ExtrudableEntity) != true) return false;

            return true;
        }

        /// <summary>
        ///     Get extruded entity
        /// </summary>
        /// <returns>Extruded entity</returns>
        private ksEntity GetExtrudedEntity()
        {
            var faceIndex1 = 0;
            var faceIndex2 = 0;
            var facesCount = _facesCountAfterExtrusion - _facesCountBeforeExtrusion;

            if (_doc3DPart == null) return null;

            // If count of faces is 2 or 3 -- then get cylinder base plane indexes
            if (facesCount == 2 || facesCount == 3)
                KompasFacesManager.GetCylinderBasePlaneIndexes(_doc3DPart,
                    _facesCountBeforeExtrusion + 1,
                    _facesCountAfterExtrusion,
                    out faceIndex1, out faceIndex2);
            // Square or just a single face isn't supported
            else if (facesCount == 0 || facesCount == 1 || facesCount == 4)
                return null;
            else
                KompasFacesManager.GetRegPolyBasePlanesIndexes(_doc3DPart,
                    _facesCountBeforeExtrusion + 1,
                    _facesCountAfterExtrusion,
                    out faceIndex1, out faceIndex2);

            var entity = KompasFacesManager.GetParallelBasePlane(_doc3DPart,
                faceIndex1,
                faceIndex2,
                BaseFaceAreaState);

            return entity;
        }

        private void GenerateException()
        {
            throw new InvalidOperationException("Ошибка при создании выдавливания. " +
                                        "Введите корректные параметры и попробуйте снова");
        }
    }
}