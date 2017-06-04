using Assets.Scripts.Enums;
using Assets.Scripts.Factories.Helpers;
using Assets.Scripts.Factories.Unity;
using OSMtoSharp.Enums.Keys;
using OSMtoSharp.Enums.Values;
using OSMtoSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Factories.Osm
{
    public class RailWaysFactory
    {
        private static class RailwaysConstants
        {
            public const float defaultWidth = 0.2f;
            public static Color defaultColor = new Color(16 / 255f, 17 / 255f, 17 / 255f, 1);
        }

        private static float GetHighwayWidth(RailwayTypeEnum type)
        {
            return RailwaysConstants.defaultWidth * Assets.Scripts.Constants.Constants.Scale;
        }

        private static Color GetHighwayColor(RailwayTypeEnum type)
        {
            return RailwaysConstants.defaultColor;
        }
        public static void CreateRailway(OsmWay railway, OsmBounds bounds, Transform parent)
        {
            RailwayTypeEnum type = OSMtoSharp.Enums.Helpers.EnumExtensions.
                                                       GetTagKeyEnum<RailwayTypeEnum>
                                                       (railway.Tags[TagKeyEnum.Railway]);

            Vector3[] linePoints = new Vector3[railway.Nodes.Count];

            for (int i = 0; i < railway.Nodes.Count; i++)
            {
                linePoints[i] = OsmToUnityConverter.GetPointFromUnityPointVec3(railway.Nodes[i].Point, bounds);
            }
            float width = GetHighwayWidth(type);
            Color color = GetHighwayColor(type);

            GameObject result = LineFactory.CreateLine(linePoints, width, color, new Material(Shader.Find("Sprites/Default")));
            result.name = "<railway>";

            result.transform.parent = parent;

        }
    }
}
