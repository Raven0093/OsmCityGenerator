using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Factories.Unity
{
    public class LineFactory
    {
        public static GameObject CreateLine(Vector3[] linePoints)
        {
            GameObject result = new GameObject();

            LineRenderer lineRender = result.AddComponent<LineRenderer>();
            lineRender.positionCount = linePoints.Length;
            lineRender.SetPositions(linePoints);

            return result;

        }

        public static GameObject CreateLine(Vector3[] linePoints, float width, Color color, Material material)
        {
            GameObject result = CreateLine(linePoints);

            LineRenderer lineRender = result.GetComponent<LineRenderer>();

            lineRender.material = new Material(Shader.Find("Sprites/Default"));

            lineRender.startWidth = width;
            lineRender.endWidth = width;

            lineRender.startColor = color;
            lineRender.endColor = color;

            return result;

        }
    }
}
