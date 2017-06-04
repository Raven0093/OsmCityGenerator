using Assets.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Factories.Unity
{
    public class MeshFactory
    {
        public static Mesh CreateMesh(List<Vector3> verts)
        {
            var tris = new Triangulator(verts.Select(x => new Vector2(x.x, x.z)).ToArray());
            var mesh = new Mesh();

            var vertices = verts.Select(x => new Vector3(x.x, 0, x.z)).ToList();
            var indices = tris.Triangulate().ToList();
            //var uv = new List<Vector2>();

            var n = vertices.Count;
            for (int index = 0; index < n; index++)
            {
                var v = vertices[index];
                vertices.Add(new Vector3(v.x, 0, v.z));
            }

            for (int i = 0; i < n - 1; i++)
            {
                indices.Add(i);
                indices.Add(i + n);
                indices.Add(i + n + 1);
                indices.Add(i);
                indices.Add(i + n + 1);
                indices.Add(i + 1);
            }

            indices.Add(n - 1);
            indices.Add(n);
            indices.Add(0);

            indices.Add(n - 1);
            indices.Add(n + n - 1);
            indices.Add(n);



            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
