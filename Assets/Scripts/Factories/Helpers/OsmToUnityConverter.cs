using OSMtoSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Factories.Helpers
{
    public class OsmToUnityConverter
    {
        public static Vector2 GetPointFromUnityPointVec2(Point point, OsmBounds bounds)
        {
            float x = (((float)point.Lat - (float)bounds.MinLat) * Assets.Scripts.Constants.Constants.multipler);
            float y = -(((float)point.Lon - (float)bounds.MinLon) * Assets.Scripts.Constants.Constants.multipler);

            return new Vector2(x, y);
        }

        public static Vector3 GetPointFromUnityPointVec3(Point point, OsmBounds bounds)
        {
            float x = (((float)point.Lat - (float)bounds.MinLat) * Assets.Scripts.Constants.Constants.multipler);
            float y = 0f;
            float z = -(((float)point.Lon - (float)bounds.MinLon) * Assets.Scripts.Constants.Constants.multipler);

            return new Vector3(x, y, z);
        }
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }


        public static bool OnlyHexInString(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }
    }
}
