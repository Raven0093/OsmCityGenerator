using Assets.Scripts.Factories.Helpers;
using OSMtoSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Factories.Osm
{
    public class MeshFactory
    {
        public static GameObject CreateMesh(OsmWay areaData, OsmBounds bounds)
        {
            var buildingCorners = new List<Vector3>();


            for (int i = 0; i < areaData.Nodes.Count - 1; i++)
            {
                buildingCorners.Add(OsmToUnityConverter.GetPointFromUnityPointVec3(areaData.Nodes[i].Point, bounds));
            }

            GameObject result = new GameObject();

            result.AddComponent<MeshRenderer>();
            MeshFilter mf = result.AddComponent<MeshFilter>();

            if (buildingCorners.Count > 2)
            {
                Mesh mesh = Assets.Scripts.Factories.Unity.MeshFactory.CreateMesh(buildingCorners);
                mf.mesh = mesh;
            }

            return result;
        }

        public static void CreateMesh(OsmWay areaData, OsmBounds bounds, Transform parent)
        {
            CreateMesh(areaData, bounds).transform.parent = parent;
        }
    }
}
