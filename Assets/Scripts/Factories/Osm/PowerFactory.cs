using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using OSMtoSharp.Enums.Keys;
using OSMtoSharp.Enums.Values;
using OSMtoSharp.Model;
using Assets.Scripts.Factories.Helpers;
using Assets.Scripts.Factories.Unity;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Factories.Osm
{
    public class PowerFactory
    {
        private static class PowerLinesConstants
        {
            public const float defaultWidth = 0.1f;
            public static Color defaultColor = new Color(1 / 255f, 1 / 255f, 1 / 255f, 1);
            public const float defaultHeight = 4f;
        }

        private static class PowerTowersConstants
        {
            public const float defaultWidth = 0.1f;
            public static Color defaultColor = new Color(1 / 255f, 1 / 255f, 1 / 255f, 1);
            public const float defaultHeight = 4f;
        }

        private static float GetPowerLineWidth(PowerTypeEnum type)
        {
            return PowerLinesConstants.defaultWidth * Assets.Scripts.Constants.Constants.Scale;
        }

        private static float GetPowerLineHeight(PowerTypeEnum type)
        {
            return PowerLinesConstants.defaultHeight * Assets.Scripts.Constants.Constants.Scale;
        }

        private static Color GetPowerLineColor(PowerTypeEnum type)
        {
            return PowerLinesConstants.defaultColor;
        }

        private static float GetPowerTowerWidth()
        {
            return PowerTowersConstants.defaultWidth * Assets.Scripts.Constants.Constants.Scale;
        }

        private static float GetPowerTowerHeight()
        {
            return PowerTowersConstants.defaultHeight * Assets.Scripts.Constants.Constants.Scale;
        }

        private static Color GetPowerTowerColor()
        {
            return PowerTowersConstants.defaultColor;
        }

        public static void CreatePower(OsmWay lineData, OsmBounds bounds, Transform parent)
        {
            GameObject result = new GameObject();
            result.name = "<powerline>";

            PowerTypeEnum type = OSMtoSharp.Enums.Helpers.EnumExtensions.
                                           GetTagKeyEnum<PowerTypeEnum>
                                           (lineData.Tags[TagKeyEnum.Power]);

            Vector3[] linePoints = new Vector3[lineData.Nodes.Count];

            for (int i = 0; i < lineData.Nodes.Count; i++)
            {
                linePoints[i] = OsmToUnityConverter.GetPointFromUnityPointVec3(lineData.Nodes[i].Point, bounds);
                linePoints[i].y = GetPowerLineHeight(type);
            }

            LineRenderer lineRender = result.AddComponent<LineRenderer>();
            lineRender.positionCount = lineData.Nodes.Count;

            lineRender.material = new Material(Shader.Find("Sprites/Default"));

            float width = GetPowerLineWidth(type);
            Color color = GetPowerLineColor(type);

            lineRender.startWidth = width;
            lineRender.endWidth = width;

            lineRender.startColor = color;
            lineRender.endColor = color;

            lineRender.SetPositions(linePoints);

            result.transform.parent = parent;
        }

        public static void CreatePower(OsmNode towerData, OsmBounds bounds, Transform parent)
        {
            Vector3[] linePoints = new Vector3[2];

            linePoints[0] = OsmToUnityConverter.GetPointFromUnityPointVec3(towerData.Point, bounds);
            linePoints[1] = linePoints[0];
            linePoints[1].y = GetPowerTowerHeight();

            float width = GetPowerTowerWidth();
            Color color = GetPowerTowerColor();

            GameObject result = LineFactory.CreateLine(linePoints, width, color, new Material(Shader.Find("Sprites/Default")));
            result.name = "<powertower>";

            result.transform.parent = parent;
        }
    }
}
