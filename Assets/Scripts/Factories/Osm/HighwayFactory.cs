using Assets.Scripts.Enums;
using Assets.Scripts.Factories.Helpers;
using Assets.Scripts.Factories.Unity;
using OSMtoSharp.Enums.Keys;
using OSMtoSharp.Model;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Factories.Osm
{
    public class HighwayFactory
    {
        private static class HighwaysConstants
        {
            public const float motorwayWidth = 1f;
            public const float trunkWidth = 0.9f;
            public const float primaryWidth = 0.8f;
            public const float secondaryWidth = 0.7f;
            public const float tertiaryWidth = 0.5f;
            public const float residentialWidth = 0.3f;
            public const float serviceWidth = 0.2f;
            public const float proposedWidth = 0.01f;
            public const float underConstruction = 0.01f;
            public const float cycleway = 0.005f;
            public const float defaultWidth = 0.1f;

            public const float roadNameLabelSize = 0.5f;
            public const float roadNameLabelPosY = 0.2f;
            public const float roadNameStringSizeMultipler = 0.35f;
            public static Color roadNameLabelColor = Color.black;


            public static Color motorwayColor = new Color(232 / 255, 146f / 255f, 162f / 255f, 1);
            public static Color trunkColor = new Color(251f / 255f, 177f / 255f, 153f / 255f, 1);
            public static Color primaryColor = new Color(254f / 255f, 214f / 255f, 161f / 255f, 1);
            public static Color secondaryColor = new Color(246f / 255f, 250f / 255f, 186f / 255f, 1);
            public static Color cycleColor = new Color(150f / 255f, 150f / 255f, 150f / 255f, 1);
            public static Color defaultColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1);

        }

        private static float GetHighwayWidth(HighwayTypeEnum type)
        {
            float result = 0;
            switch (type)
            {
                case HighwayTypeEnum.Motorway: result = HighwaysConstants.motorwayWidth; break;
                case HighwayTypeEnum.Trunk: result = HighwaysConstants.trunkWidth; break;
                case HighwayTypeEnum.Primary: result = HighwaysConstants.primaryWidth; break;
                case HighwayTypeEnum.Secondary: result = HighwaysConstants.secondaryWidth; break;
                case HighwayTypeEnum.Tertiary: result = HighwaysConstants.tertiaryWidth; break;
                case HighwayTypeEnum.Residential: result = HighwaysConstants.residentialWidth; break;
                case HighwayTypeEnum.Service: result = HighwaysConstants.serviceWidth; break;
                case HighwayTypeEnum.Construction: result = HighwaysConstants.proposedWidth; break;
                case HighwayTypeEnum.Proposed: result = HighwaysConstants.underConstruction; break;
                case HighwayTypeEnum.Cycleway: result = HighwaysConstants.cycleway; break;
                default: result = HighwaysConstants.defaultWidth; break;
            }
            return result * Assets.Scripts.Constants.Constants.Scale;

        }

        private static Color GetHighwayColor(HighwayTypeEnum type)
        {
            switch (type)
            {
                case HighwayTypeEnum.Motorway: return HighwaysConstants.motorwayColor;
                case HighwayTypeEnum.Trunk: return HighwaysConstants.trunkColor;
                case HighwayTypeEnum.Primary: return HighwaysConstants.primaryColor;
                case HighwayTypeEnum.Secondary: return HighwaysConstants.secondaryColor;
                case HighwayTypeEnum.Cycleway: return HighwaysConstants.cycleColor;
                default: return HighwaysConstants.defaultColor;
            }

        }
        private static Vector3[] FindMaxDistance(Vector3[] linePoints)
        {
            Vector3[] result = new Vector3[2];
            float max = 0;
            for (int i = 1; i < linePoints.Length; i++)
            {
                float dis = Vector3.Distance(linePoints[i - 1], linePoints[i]);
                if (dis > max)
                {
                    max = dis;
                    result[0] = linePoints[i - 1];
                    result[1] = linePoints[i];
                }

            }

            return result;
        }
        private static void CreateRoadNameLabel(Vector3 pointA, Vector3 pointB, string roadName, Transform parent)
        {
            GameObject text = new GameObject();
            text.transform.parent = parent.transform;
            text.name = "Road name";
            TextMesh textMesh = text.AddComponent<TextMesh>();
            textMesh.text = roadName;
            textMesh.transform.Rotate(90, 90, 0);
            textMesh.characterSize = HighwaysConstants.roadNameLabelSize;
            textMesh.color = HighwaysConstants.roadNameLabelColor;

            if (pointA.z < pointB.z)
            {
                text.transform.position = new Vector3(pointB.x, HighwaysConstants.roadNameLabelPosY, pointB.z);
                text.transform.LookAt(pointA);
                text.transform.Rotate(90, text.transform.rotation.y - 90.0f, 0);
            }
            else if (pointA.z > pointB.z)
            {
                text.transform.position = new Vector3(pointA.x, HighwaysConstants.roadNameLabelPosY, pointA.z);
                text.transform.LookAt(pointB);
                text.transform.Rotate(90, text.transform.rotation.y - 90.0f, 0);
            }
            else
            {
                if (pointA.x < pointB.x)
                {
                    text.transform.position = new Vector3(pointA.x, HighwaysConstants.roadNameLabelPosY, pointA.z);
                    text.transform.LookAt(pointB);
                    text.transform.Rotate(90, text.transform.rotation.y - 90.0f, 0);
                }
                else
                {
                    text.transform.position = new Vector3(pointB.x, HighwaysConstants.roadNameLabelPosY, pointB.z);
                    text.transform.LookAt(pointA);
                    text.transform.Rotate(90, text.transform.rotation.y - 90.0f, 0);
                }
            }
        }
        private static void CreateRoadNameLabel(Vector3[] linePoints, string roadName, Transform parent)
        {
            if (linePoints.Length > 1)
            {
                int b = linePoints.Length / 2;
                int a = b - 1;
                Vector3 pointA = new Vector3(linePoints[a].x, linePoints[a].y, linePoints[a].z);
                Vector3 pointB = new Vector3(linePoints[b].x, linePoints[b].y, linePoints[b].z);
                if (Vector3.Distance(pointA, pointB) > roadName.Length * HighwaysConstants.roadNameStringSizeMultipler)
                {
                    CreateRoadNameLabel(pointA, pointB, roadName, parent);
                }
                else
                {
                    Vector3[] maxDis = FindMaxDistance(linePoints);
                    pointA = maxDis[0];
                    pointB = maxDis[1];
                    if (Vector3.Distance(pointA, pointB) > roadName.Length * HighwaysConstants.roadNameStringSizeMultipler)
                    {
                        CreateRoadNameLabel(pointA, pointB, roadName, parent);
                    }
                }
            }
        }
        public static GameObject CreateHighway(OsmWay highwayData, OsmBounds bounds, Transform parent)
        {
            Vector3[] linePoints = new Vector3[highwayData.Nodes.Count];

            for (int i = 0; i < highwayData.Nodes.Count; i++)
            {
                linePoints[i] = OsmToUnityConverter.GetPointFromUnityPointVec3(highwayData.Nodes[i].Point, bounds);
            }

            HighwayTypeEnum type = OSMtoSharp.Enums.Helpers.EnumExtensions.
                                                       GetTagKeyEnum<HighwayTypeEnum>
                                                       (highwayData.Tags[TagKeyEnum.Highway]);

            float width = GetHighwayWidth(type);
            Color color = GetHighwayColor(type);

            GameObject result = LineFactory.CreateLine(linePoints, width, color, new Material(Shader.Find("Sprites/Default")));
            if (highwayData.Tags.ContainsKey(TagKeyEnum.Name))
            {
                result.name = highwayData.Tags[TagKeyEnum.Name];
                CreateRoadNameLabel(linePoints, result.name, result.transform);
            }
            else
            {
                result.name = "<highway>";
            }

            result.transform.parent = parent;

            return result;
        }
    }
}