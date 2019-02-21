using System;
using System.Collections.Generic;
using System.Linq;
using Kompas6API5;
using Kompas6Constants3D;

namespace ScrewNutUI.Managers
{
    public static class KompasFacesManager
    {
        /// <summary>
        ///     Base face area state:
        ///     higher than parralel base face
        ///     or lower than the latter
        /// </summary>
        public enum BaseFaceAreaState
        {
            BaseFaceAreaHigher,
            BaseFaceAreaLower
        }

        /// <summary>
        ///     Get cylinder base plane indexes by indexes of faces of cylinder inside detail faces collection
        /// </summary>
        /// ksFaceDefinition.IsCylinder() defines is face cylindric or not,
        /// it seems to be unlogical, but in any case: base planes are NOT cylindric,
        /// they are just plane circles
        /// <param name="doc3DPart">Document 3D part, represents detail</param>
        /// <param name="startIndex">Start index of faces in faces collection</param>
        /// <param name="endIndex">End index of faces in faces collection</param>
        /// <param name="outFirstIndex">First base plane index</param>
        /// <param name="outSecondIndex">Second base plane index</param>
        public static void GetCylinderBasePlaneIndexes(ksPart doc3DPart, int startIndex, int endIndex,
            out int outFirstIndex, out int outSecondIndex)
        {
            var faceCollection = (ksEntityCollection) doc3DPart.EntityCollection((short) Obj3dType.o3d_face);

            if (faceCollection == null)
            {
                outFirstIndex = outSecondIndex = -1;
                return;
            }

            var isFirstIndexSet = false;

            var firstIndex = -1;
            var secondIndex = -1;

            for (var i = startIndex - 1; i < endIndex; i++)
            {
                uint ST_MIX_SM = 0x0; // area in santimeters
                var entity = (ksEntity) faceCollection.GetByIndex(i);
                var def = (ksFaceDefinition) entity.GetDefinition();

                Math.Round(def.GetArea(ST_MIX_SM), 10);

                // If face isn't cylindric and if it is not base face (see xml-comment to this function)
                if (!def.IsCylinder() && isFirstIndexSet == false)
                {
                    isFirstIndexSet = true;
                    firstIndex = i;
                }
                else if (!def.IsCylinder() && isFirstIndexSet)
                {
                    secondIndex = i;
                }
            }

            outFirstIndex = firstIndex;
            outSecondIndex = secondIndex;
        }

        /// <summary>
        ///     Return face which is parallel to base face
        /// </summary>
        /// This algorithm uses areas of faces collection 
        /// in diapason from start index to end index.
        /// The essence of the algorithm is that any extruded figure
        /// has parralel top and bottom planes and sides planes,
        /// and areas of these planes are equal as side planes!
        /// But all these planes are sorted randomly.
        /// If we add any figure to bottom plane
        /// (i.e. create sketch and extrude him on bottom plane)
        /// then area of bottom plane decreases
        /// and we can exactly say which index belongs to each parralel plane!
        /// <param name="doc3DPart">Kompas part of 3D document</param>
        /// <param name="startIndex">Face collection start index</param>
        /// <param name="endIndex">Face collection end index</param>
        /// <param name="outFirstIndex">First base plane index in faces collection</param>
        /// <param name="outSecondIndex">Second base plane index in faces collection</param>
        public static void GetRegPolyBasePlanesIndexes(ksPart doc3DPart, int startIndex, int endIndex,
            out int outFirstIndex, out int outSecondIndex)
        {
            // Collection of entities in all figure
            var faceCollection = (ksEntityCollection) doc3DPart.EntityCollection((short) Obj3dType.o3d_face);

            var initList = new List<double>();
            var unroundedList = new List<double>();
            var uniqList = new List<double>();
            var notUniqList = new List<double>();

            if (faceCollection == null)
            {
                outFirstIndex = outSecondIndex = -1;
                return;
            }

            var firstIndex = startIndex - 1;
            var secondIndex = startIndex - 1;

            // Set figure faces areas list with all areas
            for (var i = startIndex - 1; i < endIndex; i++)
            {
                uint ST_MIX_SM = 0x0; // this is similar to "area in santimeters" definition in API
                var entity = (ksEntity) faceCollection.GetByIndex(i);

                var def = (ksFaceDefinition) entity.GetDefinition();

                // Get unrounded area of plane
                var area = def.GetArea(ST_MIX_SM);

                unroundedList.Add(area);
            }

            // Get minimal epsilon for all unrounded areas (see comment to this function)
            var minimalEpsilon = GetMinimalEspilonOfAreas(unroundedList);

            // Set init list with rounded values
            for (int i = 0, length = unroundedList.Count; i < length; i++)
                initList.Add(Math.Round(unroundedList[i], minimalEpsilon));

            // Get unique areas in this list
            for (int i = 0, length = initList.Count; i < length; i++)
            {
                // If value is not set neither in unique nor in non-unique list --
                // -- then set him to unique list
                if (!uniqList.Contains(initList[i]))
                {
                    if (!notUniqList.Contains(initList[i])) uniqList.Add(initList[i]);
                }
                else
                {
                    // Else if he already set in unique list -- 
                    // -- delete from this list and set him to not-unique list, but only once
                    uniqList.Remove(initList[i]);
                    notUniqList.Add(initList[i]);
                }
            }

            // Check this 2 indexes for adjacency with base face of figure
            if (uniqList.Count == 2)
            {
                firstIndex += initList.IndexOf(uniqList[0]);
                secondIndex += initList.IndexOf(uniqList[1]);
            }
            // Else if main base face area is higher than parallel base face area
            // -- then return only first index
            else if (uniqList.Count == 1)
            {
                firstIndex += initList.IndexOf(uniqList[0]);
                secondIndex = -1;
            }
            // Else if parallel faces doesn't have the same area --
            // -- then get value which repeat twice in array
            else if (uniqList.Count == 0)
            {
                var getFirstElement = false;

                for (int i = 0, count = initList.Count; i < count; i++)
                {
                    var isTwoElements = GetElementCountInList(initList, initList[i]) == 2;

                    if (isTwoElements && getFirstElement == false)
                    {
                        firstIndex += i;
                        getFirstElement = true;
                    }
                    else if (isTwoElements)
                    {
                        secondIndex += i;
                        break;
                    }
                }
            }
            // Else try to do facepalm and exit immediately
            else
            {
                firstIndex = secondIndex = -1;
            }

            outFirstIndex = firstIndex;
            outSecondIndex = secondIndex;
        }

        /// <summary>
        ///     Get minimal epsilon in unrounded areas list
        /// </summary>
        /// <param name="areas">Unrounded areas list</param>
        /// <returns>Minimal epsilon or -1 in case of error</returns>
        private static int GetMinimalEspilonOfAreas(List<double> areas)
        {
            var epsilons = new List<int>();

            for (int i = 0, length = areas.Count; i < length; i++)
                epsilons.Add(GetEpsilonOfArea(areas[i]));

            var epsilon = epsilons.Max();
            // Maximum available epsilon is 15
            if (epsilon > 15) epsilon = 15;

            return epsilon;
        }

        /// <summary>
        ///     Get epsilon to adequate round area
        /// </summary>
        /// <param name="area">Area of plane</param>
        /// <returns></returns>
        private static int GetEpsilonOfArea(double area)
        {
            // This is optimal epsilon by my countings,
            // works for all areas without any division remainders on mantissa.
            var epsilon = 13;

            // If area is higher then 1 --
            // -- then just round logarithm of area
            if (area > 1)
                return epsilon - (int) Math.Floor(Math.Log10(area)) - 1;
            // Else count amount of zeros after comma

            if (area < 1)
            {
                var decimals = 0;

                while (area < 1)
                {
                    area *= 10;
                    decimals++;
                }

                return epsilon + decimals - 1;
            }

            return -1;
        }

        /// <summary>
        ///     Get base plane which is parallel to _main_ base plane.
        ///     The difference between this planes is the _main_ base plane is extrudABLE entity,
        ///     while the _parallel_ base plane is extrudED entity.
        /// </summary>
        /// <example>
        ///     This example explains how program correctly gets index of face by
        ///     <paramref cref="baseFaceAreaState">
        ///         information about base face area
        ///     </paramref>
        ///     .
        ///     <code language="cs">
        ///  case BaseFaceAreaState.BaseFaceAreaHigher:
        /// 		if (faceDefinition1.GetArea(SM) > faceDefinition2.GetArea(SM)) 
        /// 		{
        /// 			face = faceCollection.GetByIndex(faceIndex2);
        /// 		}
        /// 		else 
        /// 		{ 
        /// 			face = faceCollection.GetByIndex(faceIndex1);
        /// 		}
        /// 		break;
        ///  </code>
        /// </example>
        /// <param name="doc3DPart">Kompas part of 3D document</param>
        /// <param name="faceIndex1">Base plane index 1</param>
        /// <param name="faceIndex2">Base plane index 2</param>
        /// <param name="baseFaceAreaState">
        ///     Base plane area state, using for correct
        ///     definition of parallel base plane and main base plane indexes.
        /// </param>
        /// <returns>Parallel base plane by base plane area state and indexes of faces in detail faces collection</returns>
        public static ksEntity GetParallelBasePlane(ksPart doc3DPart, int faceIndex1,
            int faceIndex2, BaseFaceAreaState baseFaceAreaState)
        {
            if (faceIndex1 == -1) return null;

            var faceCollection = (ksEntityCollection) doc3DPart.EntityCollection((short) Obj3dType.o3d_face);

            var face1 = (ksEntity) faceCollection.GetByIndex(faceIndex1);
            var faceDefinition1 = (ksFaceDefinition) face1.GetDefinition();

            // If second base face index isn't defined --
            // -- then get first base face
            if (faceIndex2 == -1) return face1;

            var face2 = (ksEntity) faceCollection.GetByIndex(faceIndex2);
            var faceDefinition2 = (ksFaceDefinition) face2.GetDefinition();

            var face = (ksEntity) faceCollection.GetByIndex(0);

            uint SM = 0x0; // this is similar to "area in santimeters" in API

            switch (baseFaceAreaState)
            {
                case BaseFaceAreaState.BaseFaceAreaHigher:
                    face = faceDefinition1.GetArea(SM) > faceDefinition2.GetArea(SM) ? face2 : face1;
                    break;
                case BaseFaceAreaState.BaseFaceAreaLower:
                    face = faceDefinition1.GetArea(SM) < faceDefinition2.GetArea(SM) ? face2 : face1;
                    break;
            }

            return face;
        }

        /// <summary>
        ///     Get elements count in list
        /// </summary>
        /// <param name="list">List of doubles</param>
        /// <param name="findElement">Element to find</param>
        /// <returns>Elements count in list</returns>
        private static int GetElementCountInList(List<double> list, double findElement)
        {
            var count = 0;

            foreach (var item in list)
                if (item == findElement)
                    count++;

            return count;
        }
    }
}