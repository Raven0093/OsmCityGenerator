using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class ConfigManager
{
    private static ConfigManager _instance;

    public static ConfigManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ConfigManager();
            }
            return _instance;
        }
    }

    public bool DebugConsole { get; private set; }
    public string FilePath { get; private set; }
    public double MinLon { get; private set; }
    public double MinLat { get; private set; }
    public double MaxLon { get; private set; }
    public double MaxLat { get; private set; }

    public bool Textures { get; private set; }
    public bool HighWays { get; private set; }
    public bool RailWays { get; private set; }
    public bool PowerLines { get; private set; }
    public bool Buildings { get; private set; }
    public bool PowerTowers { get; private set; }
    public bool Landuses { get; private set; }

    public bool Load(string fileName)
    {
        bool pathBool = false;
        bool minLonBool = false;
        bool minLatBool = false;
        bool maxLonBool = false;
        bool maxLatBool = false;

        if (!File.Exists(Assets.Scripts.Constants.Constants.ConfigFile))
        {
            DebugConsoleManager.Instance.AddLineWithTime("Config file not exists");
            return false;
        }
        using (StreamReader inputile = new StreamReader(Assets.Scripts.Constants.Constants.ConfigFile))
        {
            try
            {
                double doubleTmp;
                bool boolTmp;
                while (!inputile.EndOfStream)
                {

                    var a = inputile.ReadLine();
                    var b = a.Split('=');
                    VariableName(new { DebugConsole });

                    if (b[0] == VariableName(new { DebugConsole }))
                    {
                        boolTmp = bool.Parse(b[1]);
                        DebugConsole = boolTmp;
                    }
                    if (b[0] == VariableName(new { Textures }))
                    {
                        boolTmp = bool.Parse(b[1]);
                        Textures = boolTmp;
                    }
                    if (b[0] == VariableName(new { HighWays }))
                    {
                        boolTmp = bool.Parse(b[1]);
                        HighWays = boolTmp;
                    }
                    if (b[0] == VariableName(new { RailWays }))
                    {
                        boolTmp = bool.Parse(b[1]);
                        RailWays = boolTmp;
                    }
                    if (b[0] == VariableName(new { PowerLines }))
                    {
                        boolTmp = bool.Parse(b[1]);
                        PowerLines = boolTmp;
                    }
                    if (b[0] == VariableName(new { Buildings }))
                    {
                        boolTmp = bool.Parse(b[1]);
                        Buildings = boolTmp;
                    }
                    if (b[0] == VariableName(new { PowerTowers }))
                    {
                        boolTmp = bool.Parse(b[1]);
                        PowerTowers = boolTmp;
                    }
                    if (b[0] == VariableName(new { Landuses }))
                    {
                        boolTmp = bool.Parse(b[1]);
                        Landuses = boolTmp;
                    }
                    if (b[0] == VariableName(new { FilePath }))
                    {
                        if (!string.IsNullOrEmpty(b[1]))
                        {
                            FilePath = b[1];
                            pathBool = true;
                        }
                    }
                    else if (b[0] == VariableName(new { MinLat }))
                    {
                        doubleTmp = double.Parse(b[1]);
                        MinLat = doubleTmp;
                        minLatBool = true;
                    }
                    else if (b[0] == VariableName(new { MinLon }))
                    {
                        doubleTmp = double.Parse(b[1]);
                        MinLon = doubleTmp;
                        minLonBool = true;
                    }
                    else if (b[0] == VariableName(new { MaxLat }))
                    {
                        doubleTmp = double.Parse(b[1]);
                        MaxLat = doubleTmp;
                        maxLatBool = true;
                    }
                    else if (b[0] == VariableName(new { MaxLon }))
                    {
                        doubleTmp = double.Parse(b[1]);
                        MaxLon = doubleTmp;
                        maxLonBool = true;
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception)
            {
                DebugConsoleManager.Instance.AddLineWithTime("Config parse exeption");
                return false;
            }




            if (pathBool && minLonBool && minLatBool && maxLonBool && maxLatBool)
            {
                DebugConsoleManager.Instance.AddLineWithTime("Load config data");
                DebugConsoleManager.Instance.AddLine("File path", FilePath);
                DebugConsoleManager.Instance.AddLine("MinLat", MinLat);
                DebugConsoleManager.Instance.AddLine("MinLon", MinLon);
                DebugConsoleManager.Instance.AddLine("MaxLat", MaxLat);
                DebugConsoleManager.Instance.AddLine("MaxLon", MaxLon);

                DebugConsoleManager.Instance.AddLine("Console", DebugConsole);
                DebugConsoleManager.Instance.AddLine("Textures", Textures);
                DebugConsoleManager.Instance.AddLine("RailWays", RailWays);
                DebugConsoleManager.Instance.AddLine("PowerLines", PowerLines);
                DebugConsoleManager.Instance.AddLine("Buildings", Buildings);
                DebugConsoleManager.Instance.AddLine("PowerTowers", PowerTowers);
                DebugConsoleManager.Instance.AddLine("Landuses", Landuses);
                return true;
            }
            else
            {
                DebugConsoleManager.Instance.AddLineWithTime("Config file without Osm file Path or lat, lon coords");
                return false;
            }

        }
    }

    public static string VariableName<T>(T myInput) where T : class
    {
        if (myInput == null)
            return string.Empty;

        return typeof(T).GetProperties()[0].Name;
    }

}