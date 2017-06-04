using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;
using OSMtoSharp;
using OSMtoSharp.Model;
using OSMtoSharp.Enums.Values;
using OSMtoSharp.Enums.Keys;
using System.Globalization;
using Assets.Scripts.Factories.Helpers;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Factories.Osm
{
    public class BuildingFactory : MonoBehaviour
    {
        private static class BuildingsConstants
        {
            public const float wallHeight = 2f;
            public const float wallWidth = 0.0001f;
            public const int firstIndexRandomRoof = 0;
            public const int endIndexRandomRoof = 4;

            public const int firstIndexRandomShop = 0;
            public const int endIndexRandomShop = 0;

            public const int firstIndexRandomBuilding = 0;
            public const int endIndexRandomBuilding = 3;

        }

        public static float GetWallHeight(BuildingsTypeEnum type, int level)
        {
            return BuildingsConstants.wallHeight * Assets.Scripts.Constants.Constants.Scale * level;
        }

        public static bool SetColor(string colorString, Material material)
        {
            try
            {
                if (OsmToUnityConverter.OnlyHexInString(colorString))
                {
                    var color = OsmToUnityConverter.HexToColor(colorString);
                    material.color = color;
                    return true;
                }
                else
                {
                    System.Drawing.Color systemColor = System.Drawing.Color.FromName(colorString);
                    if (systemColor.IsKnownColor)
                    {
                        var color = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                        material.color = color;
                        return true;
                    }
                }
            }
            catch (Exception)
            {


            }


            return false;
        }
        public static void CreateRoof(OsmWay buildingData, OsmBounds bounds, float height, Transform parent)
        {
            GameObject roof = OsmMeshFactory.CreateMesh(buildingData, bounds);
            roof.transform.localPosition = new Vector3(roof.transform.localPosition.x, height, roof.transform.localPosition.z);
            roof.name = "roof";

            if (ConfigManager.Instance.Textures)
            {

                bool colorSet = false;
                if (buildingData.Tags.ContainsKey(TagKeyEnum.RoofColour))
                {
                    var colorString = buildingData.Tags[TagKeyEnum.RoofColour].Replace("#", "");
                    colorSet = SetColor(colorString, roof.GetComponent<Renderer>().material);
                    colorSet = true;

                }
                if (!colorSet)
                {
                    string materiaName = string.Format("{0}/{1}_{2}",
                       Constants.Constants.MaterialsFolderName,
                       "roof", new System.Random().Next(BuildingsConstants.firstIndexRandomRoof, BuildingsConstants.endIndexRandomRoof));

                    Material material = Resources.Load(materiaName, typeof(Material)) as Material;
                    roof.GetComponent<Renderer>().material = material;
                }

            }


            roof.transform.parent = parent;
        }

        public static GameObject CreateWall(Vector3 pointA, Vector3 pointB, float width, float height, float minHeight)
        {
            GameObject newWall = GameObject.CreatePrimitive(PrimitiveType.Cube);

            newWall.name = "Wall";

            Vector3 between = pointB - pointA;
            float distance = between.magnitude;

            newWall.transform.localScale = new Vector3(width, height - minHeight, distance);
            newWall.transform.position = pointA + (between / 2.0f);
            newWall.transform.LookAt(pointB);
            newWall.transform.position = new Vector3(newWall.transform.position.x, minHeight + ((height - minHeight) / 2), newWall.transform.position.z);

            return newWall;
        }

        public static void CreateWalls(OsmWay buildingData, OsmBounds bounds, float width, float levels, float height, float minHeight, Transform parent)
        {
            BuildingsTypeEnum type = OSMtoSharp.Enums.Helpers.EnumExtensions.
                                                       GetTagKeyEnum<BuildingsTypeEnum>
                                                       (buildingData.Tags[TagKeyEnum.Building]);

            string materiaName;
            Material material = null;

            if (ConfigManager.Instance.Textures)
            {
                if (!buildingData.Tags.ContainsKey(TagKeyEnum.BuildingColor))
                {
                    if (buildingData.Tags.ContainsKey(TagKeyEnum.Shop) || buildingData.Tags.ContainsKey(TagKeyEnum.Shop1) || buildingData.Tags.ContainsKey(TagKeyEnum.Shop2))
                    {
                        materiaName = string.Format("{0}/{1}_{2}",
                          Constants.Constants.MaterialsFolderName,
                          "shop", new System.Random().Next(BuildingsConstants.firstIndexRandomShop, BuildingsConstants.endIndexRandomShop));

                    }

                    else if (type == BuildingsTypeEnum.House ||
                        type == BuildingsTypeEnum.Apartments ||
                        type == BuildingsTypeEnum.Bungalow ||
                        type == BuildingsTypeEnum.Detached ||
                        type == BuildingsTypeEnum.Greenhouse ||
                        type == BuildingsTypeEnum.Residential
                        )
                    {
                        materiaName = string.Format("{0}/{1}_{2}",
                          Constants.Constants.MaterialsFolderName,
                          OSMtoSharp.Enums.Helpers.EnumExtensions.GetEnumAttributeValue(TagKeyEnum.Building),
                          OSMtoSharp.Enums.Helpers.EnumExtensions.GetEnumAttributeValue(BuildingsTypeEnum.House));
                    }

                    else if (type == BuildingsTypeEnum.Hotel ||
                        type == BuildingsTypeEnum.Commercial
                        )
                    {
                        materiaName = string.Format("{0}/{1}_{2}",
                          Constants.Constants.MaterialsFolderName,
                          OSMtoSharp.Enums.Helpers.EnumExtensions.GetEnumAttributeValue(TagKeyEnum.Building),
                          OSMtoSharp.Enums.Helpers.EnumExtensions.GetEnumAttributeValue(BuildingsTypeEnum.Hotel));

                    }
                    else
                    {
                        materiaName = string.Format("{0}/{1}_{2}",
                          Constants.Constants.MaterialsFolderName,
                          OSMtoSharp.Enums.Helpers.EnumExtensions.GetEnumAttributeValue(TagKeyEnum.Building),
                          new System.Random().Next(BuildingsConstants.firstIndexRandomBuilding, BuildingsConstants.endIndexRandomBuilding));


                    }
                    material = Resources.Load(materiaName, typeof(Material)) as Material;
                }
            }





            for (int i = 1; i < buildingData.Nodes.Count; i++)
            {
                Vector2 pointA = OsmToUnityConverter.GetPointFromUnityPointVec2(buildingData.Nodes[i].Point, bounds);
                Vector2 pointB = OsmToUnityConverter.GetPointFromUnityPointVec2(buildingData.Nodes[i - 1].Point, bounds);

                GameObject wall = CreateWall(new Vector3(pointA.x, minHeight, pointA.y),
                                               new Vector3(pointB.x, minHeight, pointB.y),
                                               width, height, minHeight);


                if (buildingData.Tags.ContainsKey(TagKeyEnum.BuildingColor))
                {
                    var color = buildingData.Tags[TagKeyEnum.BuildingColor].Replace("#", "");
                    SetColor(color, wall.GetComponent<Renderer>().material);
                }
                else
                {
                    if (material != null)
                    {
                        Material mat = new Material(material);
                        int tilingX = (int)wall.transform.localScale.z;
                        if (tilingX == 0)
                        {
                            tilingX = 1;
                        }
                        mat.mainTextureScale = new Vector2(tilingX, levels);

                        wall.GetComponent<Renderer>().material = mat;
                    }
                }
                wall.transform.SetParent(parent);
            }

        }

        public static void CreateBuilding(OsmWay buildingData, OsmBounds bounds, Transform parent)
        {
            if (buildingData.Nodes.Count < 3)
            {
                return;
            }
            GameObject result = new GameObject();

            float levels = 0;
            float height = 0;
            float minHeight = 0;
            if (buildingData.Tags.ContainsKey(TagKeyEnum.Name))
            {
                result.name = buildingData.Tags[TagKeyEnum.Name];
            }
            else
            {
                result.name = "<building>";
            }

            if (buildingData.Tags.ContainsKey(TagKeyEnum.BuildingLevels))
            {
                levels += float.Parse(buildingData.Tags[TagKeyEnum.BuildingLevels], CultureInfo.InvariantCulture);
                height = levels * BuildingsConstants.wallHeight;
            }
            if (buildingData.Tags.ContainsKey(TagKeyEnum.BuildingMinLevel))
            {
                float minLevel = float.Parse(buildingData.Tags[TagKeyEnum.BuildingMinLevel], CultureInfo.InvariantCulture);
                levels -= minLevel;
                minHeight = BuildingsConstants.wallHeight * minLevel;
            }

            if (buildingData.Tags.ContainsKey(TagKeyEnum.BuildingHeight))
            {
                height = float.Parse(buildingData.Tags[TagKeyEnum.BuildingHeight].Replace(" ", "").Replace("m", ""), CultureInfo.InvariantCulture);
            }

            if (buildingData.Tags.ContainsKey(TagKeyEnum.BuildingMinHeight))
            {
                minHeight = float.Parse(buildingData.Tags[TagKeyEnum.BuildingMinHeight], CultureInfo.InvariantCulture);
            }
            if (levels == 0)
            {
                levels = 1;

            }
            if (height == 0)
            {
                height = BuildingsConstants.wallHeight;

            }



            CreateWalls(buildingData, bounds, BuildingsConstants.wallWidth, levels, height, minHeight, result.transform);
            CreateRoof(buildingData, bounds, height, result.transform);

            result.transform.parent = parent;
        }
    }
}
