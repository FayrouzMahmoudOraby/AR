using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PieSlice : MonoBehaviour
{
    public float startAngle;
    public float endAngle;
    public float radius = 0.1f;
    public int segments = 30;

    public void CreateSlice()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = Vector3.zero;
        float angleStep = (endAngle - startAngle) / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + i * angleStep;
            float rad = Mathf.Deg2Rad * angle;
            vertices[i + 1] = new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0f);
        }

        int[] triangles = new int[segments * 3];
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / radius + 0.5f, vertices[i].y / radius + 0.5f);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
