using System;
using Kompas6API5;
using Kompas6Constants3D;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ScrewNutUI;
using ScrewNutUI.Managers;

namespace ScrewNut.Tests.ManagersTest
{
    [TestFixture]
    public class KompasSketchManagerTests
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
		/// Test of creating sketch on base plane axis
		/// </summary>
		/// <param name="isError">positive or negative tests</param>
		/// <param name="basePlaneAxis">base plane axis, an instance of Obj3dType</param>
		[TestCase(false, Obj3dType.o3d_planeXOY, 
		    TestName = "Вызов конструктора с передачей корректного параметра BasePlaneAxis = Obj3dType.o3d_planeXOY")]
        [TestCase(false, Obj3dType.o3d_planeXOZ, 
            TestName = "Вызов конструктора с передачей корректного параметра BasePlaneAxis = Obj3dType.o3d_planeXOZ")]
        [TestCase(false, Obj3dType.o3d_planeYOZ, 
            TestName = "Вызов конструктора с передачей корректного параметра BasePlaneAxis = Obj3dType.o3d_planeYOZ")]
        [TestCase(true, Obj3dType.o3d_axisOX, 
            TestName = "Вызов конструктора с передачей некорректного параметра BasePlaneAxis = Obj3dType.o3d_axisOX")]
        [TestCase(true, Obj3dType.o3d_axisOY, 
            TestName = "Вызов конструктора с передачей некорректного параметра BasePlaneAxis = Obj3dType.o3d_axisOY")]
        [TestCase(true, Obj3dType.o3d_axisOZ, 
            TestName = "Вызов конструктора с передачей некорректного параметра BasePlaneAxis = Obj3dType.o3d_axisOZ")]
        [TestCase(true, Obj3dType.o3d_unknown, 
            TestName = "Вызов конструктора с передачей некорректного параметра BasePlaneAxis = Obj3dType.o3d_unknown")]
        [TestCase(true, Obj3dType.o3d_face, 
            TestName = "Вызов конструктора с передачей некорректного параметра BasePlaneAxis = Obj3dType.o3d_face")]
        [TestCase(true, Obj3dType.o3d_edge, 
            TestName = "Вызов конструктора с передачей некорректного параметра BasePlaneAxis = Obj3dType.o3d_edge")]
        public void SketchConstructorTests(bool isError, Obj3dType basePlaneAxis)
        {
            KompasSketchManager sketch;

            if (isError)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    sketch = new KompasSketchManager(_kompasApplication.ScrewPart, basePlaneAxis);
                });
            }
            else
            {
                sketch = new KompasSketchManager(_kompasApplication.ScrewPart, basePlaneAxis);
                Assert.IsTrue(sketch.Entity.IsCreated());
            }
        }

        /// <summary>
        /// Test of creating sketch on base plane = null
        /// </summary>
        [TestCase(TestName = "Вызов конструктора с передачей параметра Entity = null")]
        public void SketchNegativeConstructorTestsWithNullEntity()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var sketch = new KompasSketchManager(_kompasApplication.ScrewPart, null);
            });
        }
    }
}