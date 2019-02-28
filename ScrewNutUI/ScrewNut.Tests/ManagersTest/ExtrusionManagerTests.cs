using System;
using Kompas6API5;
using Kompas6Constants3D;
using NUnit.Framework;
using ScrewNutUI;
using ScrewNutUI.Managers;
using ScrewNutUI.Parameters;

namespace ScrewNut.Tests.ManagersTest
{
    [TestFixture]
    public class ExtrusionManagerTests
    {
        private KompasApplication _kompasApplication;

        [SetUp]
        public void InitializeKompasInstance()
        {
            _kompasApplication = TestHelper.GetKompasInstance();
        }

        [OneTimeTearDown]
        public void DestructKompasInstance()
        {
            _kompasApplication.DestructApp();
        }

        /// <summary>
        /// Test of creating base extrusion by sketch
        /// </summary>
        /// <param name="isError">positive or negative tests</param>
        /// <param name="extrusionType">Extrusion type, an instance of Obj3dType</param>
        /// <param name="directionType">Direction type, an instance of Direction_Type</param>
        [TestCase(false, Obj3dType.o3d_baseExtrusion, Direction_Type.dtNormal, 
		    TestName = "Вызов конструктора с передачей корректных параметров " +
		               "ExtrusionType = o3d_baseExtrusion, DirectionType = dtNormal")]
        [TestCase(false, Obj3dType.o3d_baseExtrusion, Direction_Type.dtReverse, 
            TestName = "Вызов конструктора с передачей корректных параметров " +
                       "ExtrusionType = o3d_baseExtrusion, DirectionType = dtReverse")]
        [TestCase(true, Obj3dType.o3d_baseExtrusion, Direction_Type.dtMiddlePlane, 
            TestName = "Вызов конструктора с передачей некорректных параметров " +
                       "ExtrusionType = o3d_baseExtrusion, DirectionType = dtMiddlePlane")]
        [TestCase(true, Obj3dType.o3d_baseExtrusion, Direction_Type.dtBoth, 
            TestName = "Вызов конструктора с передачей некорректных параметров " +
                       "ExtrusionType = o3d_baseExtrusion, DirectionType = dtBoth")]
        [TestCase(true, Obj3dType.o3d_axisOZ, Direction_Type.dtReverse, 
            TestName = "Вызов конструктора с передачей некорректных параметров " +
                       "ExtrusionType = o3d_axisOZ, DirectionType = dtReverse")]
        public void SketchConstructorTests(bool isError, Obj3dType extrusionType, Direction_Type directionType)
        {
            var sketch = CreateCirle();
            var extrusionParameters = new KompasExtrusionParameters(
                _kompasApplication.ScrewPart, extrusionType, sketch, directionType, 10);
            ExtrusionManager extrusionManager;

            if (isError)
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    extrusionManager = new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity);
                });
            }
            else
            {
                extrusionManager = new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity);
                Assert.IsTrue(extrusionManager.ExtrudedEntity.IsCreated());
            }
        }

        /// <summary>
        /// Test of creating cut extrusion by sketch
        /// </summary>
        /// <param name="directionType">Direction type, an instance of Direction_Type</param>
        [TestCase(Direction_Type.dtNormal, 
            TestName = "Построение выдавливания с прямым направлением")]
        [TestCase(Direction_Type.dtReverse, 
            TestName = "Построение выдавливания с обратным направлением")]
        public void ExtrusionEntityPositiveTests(Direction_Type directionType)
        {
            var sketch = CreateCirle();

            var extrusionParameters = new KompasExtrusionParameters(_kompasApplication.ScrewPart, 
                Obj3dType.o3d_baseExtrusion, sketch, directionType, 10);
            var extrusion = new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity);
            Assert.IsTrue(extrusion.ExtrudedEntity.IsCreated());
        }

        /// <summary>
        /// Test of creating cut extrusion by sketch with not supported direction
        /// </summary>
        /// <param name="extrusionType">Extrusion type, an instance of Obj3dType</param>
        /// <param name="directionType">Direction type, an instance of Direction_Type</param>
        [TestCase(Obj3dType.o3d_cutExtrusion, Direction_Type.dtMiddlePlane, 
            TestName = "Попытка построение выреза с параметром типа выреза DirectionType = dtMiddlePlane")]
        [TestCase(Obj3dType.o3d_cutExtrusion, Direction_Type.dtBoth, 
            TestName = "Попытка построение выреза с параметром типа выреза DirectionType = dtBoth")]
        public void CutxtrusionEntityNegativeTests(Obj3dType extrusionType, Direction_Type directionType)
        {
            var sketch = CreateCirle();

            var extrusionParameters = new KompasExtrusionParameters
                (_kompasApplication.ScrewPart, Obj3dType.o3d_baseExtrusion, sketch, directionType, 10);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var extrusion = new ExtrusionManager(extrusionParameters, ExtrusionType.ByEntity);
            });
        }

        /// <summary>
        /// Create kompas sketch with circle inside
        /// </summary>
        /// <returns>Kompas sketch with circle</returns>
        public ksEntity CreateCirle()
        {
            var sketch = new KompasSketchManager(_kompasApplication.ScrewPart, Obj3dType.o3d_planeXOY);
            var sketchEdit = sketch.BeginEntityEdit();

            sketchEdit.ksCircle(0.0, 0.0, 10, 1);
            sketch.EndEntityEdit();

            return sketch.Entity;
        }
    }
}