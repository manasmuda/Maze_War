using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDirectionScript : MonoBehaviour
{

    public float radius = 0.6f;
    public MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] verticies = new Vector3[200];

        float angle = 70.2f;
        float angleDiff = 0.2f;

        int[] trianglies = new int[594];

        verticies[0] = new Vector3(0, 0, 0);
        verticies[100] = new Vector3(0, 0, 0.6f);

        for (int i = 1; i < 100; i++)
        {
            verticies[i] = new Vector3(-radius * Mathf.Cos(2 * Mathf.PI * angle / 360), 0, radius * Mathf.Sin(2 * Mathf.PI * angle / 360));
            verticies[100 + i] = new Vector3(radius * Mathf.Cos(2 * Mathf.PI * angle / 360), 0, radius * Mathf.Sin(2 * Mathf.PI * angle / 360));
            angle = angle + angleDiff;
        }

        List<int> trianglesList = new List<int> { };
        for (int i = 0; i < 198; i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i + 1);
            trianglesList.Add(i + 2);
        }
        int[] triangles = trianglesList.ToArray();

        mesh.vertices = verticies;

        mesh.triangles = triangles;

        meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = mesh;
    }


}
