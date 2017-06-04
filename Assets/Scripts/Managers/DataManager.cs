using OSMtoSharp;
using OSMtoSharp.FileManagers;
using OSMtoSharp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using OSMtoSharp.Enums.Keys;
using System.IO;

public class DataManager : MonoBehaviour
{

    OsmData osmData;

    private object osmDataLock;

    private bool dataManagerFinished;
    private bool dataParserStarted;
    private bool dataParserFinished;

    public Text debugConsoleText;

    private string DebugConsoleText
    {
        get
        {
            return debugConsoleText.text;
        }
        set
        {
            debugConsoleText.text = value;
            Canvas.ForceUpdateCanvases();
        }
    }

    private object dataParserLock;

    void Start()
    {
        osmDataLock = new object();
        dataParserLock = new object();

        dataParserStarted = false;
        dataParserFinished = false;

        dataManagerFinished = false;

        osmData = null;
        if (!ConfigManager.Instance.Load(Assets.Scripts.Constants.Constants.ConfigFile))
        {
            DebugConsolManager.Instance.UpdateUI(debugConsoleText);
            dataManagerFinished = true;
        }
    }

    void Update()
    {
        if (ConfigManager.Instance.DebugConsole && DebugConsolManager.Instance.IsUpdate)
        {
            DebugConsolManager.Instance.UpdateUI(debugConsoleText);
        }
        if (!dataManagerFinished)
        {
            if (!dataParserFinished)
            {
                if (!dataParserStarted)
                {
                    dataParserStarted = true;

                    DebugConsolManager.Instance.AddLineWithTime("Data loading");

                    GetOsmData();
                }
                else
                {
                    lock (dataParserLock)
                    {
                        if (osmData != null)
                        {
                            DebugConsolManager.Instance.AddLineWithTime("Data loaded");

                            osmData.FillWaysNode();
                            osmData.RemoveNodesWithoutTags();
                            osmData.RemoveRelationsWithoutMembers();
                            osmData.RemoveWaysWithoutNodes();

                            dataParserFinished = true;
                        }
                    }
                }
            }
            else
            {
                AddDataToScene();
                dataManagerFinished = true;
            }
        }
    }



    private void AddDataToScene()
    {
        GameObject highways = new GameObject();
        highways.name = "highways";
        highways.transform.parent = transform;
        GameObject powerLines = new GameObject();
        powerLines.name = "powerLines";
        powerLines.transform.parent = transform;
        GameObject railways = new GameObject();
        railways.name = "railways";
        railways.transform.parent = transform;
        GameObject buildings = new GameObject();
        buildings.name = "buildings";
        buildings.transform.parent = transform;
        GameObject powerTowers = new GameObject();
        powerTowers.name = "powerTowers";
        powerTowers.transform.parent = transform;
        GameObject flatArea = new GameObject();
        flatArea.name = "landuses";
        flatArea.transform.parent = transform;


        foreach (var nodeDic in osmData.Nodes)
        {
            if (ConfigManager.Instance.PowerTowers && nodeDic.Value.Tags.ContainsKey(TagKeyEnum.Power))
            {
                Assets.Scripts.Factories.Osm.PowerFactory.CreatePower(nodeDic.Value, osmData.bounds, powerTowers.transform);
                continue;
            }

        }

        foreach (var wayDic in osmData.Ways)
        {
            if (ConfigManager.Instance.PowerLines && wayDic.Value.Tags.ContainsKey(TagKeyEnum.Power))
            {
                Assets.Scripts.Factories.Osm.PowerFactory.CreatePower(wayDic.Value, osmData.bounds, powerLines.transform);
                continue;
            }
            if (ConfigManager.Instance.Buildings && wayDic.Value.Tags.ContainsKey(TagKeyEnum.Building))
            {
                Assets.Scripts.Factories.Osm.BuildingFactory.CreateBuilding(wayDic.Value, osmData.bounds, buildings.transform);
                continue;
            }
            if (ConfigManager.Instance.HighWays && wayDic.Value.Tags.ContainsKey(TagKeyEnum.Highway))
            {
                Assets.Scripts.Factories.Osm.HighwayFactory.CreateHighway(wayDic.Value, osmData.bounds, highways.transform);
                continue;
            }
            if (ConfigManager.Instance.RailWays && wayDic.Value.Tags.ContainsKey(TagKeyEnum.Railway))
            {
                Assets.Scripts.Factories.Osm.RailWaysFactory.CreateRailway(wayDic.Value, osmData.bounds, railways.transform);
                continue;
            }
            if (ConfigManager.Instance.Landuses && (wayDic.Value.Tags.ContainsKey(TagKeyEnum.Landuse) || wayDic.Value.Tags.ContainsKey(TagKeyEnum.Leisure) || wayDic.Value.Tags.ContainsKey(TagKeyEnum.Amenity)))
            {
                Assets.Scripts.Factories.Osm.FlatAreaFactory.CreateArea(wayDic.Value, osmData.bounds, flatArea.transform);
                continue;
            }
        }
    }


    private void GetOsmData()
    {
        new Thread(delegate ()
        {
            OsmData tmpOsmData = OsmParser.Parse(ConfigManager.Instance.FilePath, ConfigManager.Instance.MinLon, ConfigManager.Instance.MinLat, ConfigManager.Instance.MaxLon, ConfigManager.Instance.MaxLat);

            lock (osmDataLock)
            {
                osmData = tmpOsmData;
            }
        }).Start();
    }
}