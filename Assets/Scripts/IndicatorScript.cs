using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Transform playerTransform;

    public float radius = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] verticies = new Vector3[722];

        float angle = 0f;
        float angleDiff = 0.5f;

        int[] trianglies = new int[594];

        verticies[0] = new Vector3(0, 0, 0);
        verticies[1] = new Vector3(0, -0.2f, 0);

        for (int i = 2; i < 722; i++)
        {
            
                verticies[i] = new Vector3(radius * Mathf.Cos(2 * Mathf.PI * angle / 360), 0, radius * Mathf.Sin(2 * Mathf.PI * angle / 360));
            
            angle = angle + angleDiff;
        }

        List<int> trianglesList = new List<int> { };
        for (int i = 2; i < 721; i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i);
            trianglesList.Add(i+1);
        }
        int[] triangles = trianglesList.ToArray();

        mesh.vertices = verticies;

        mesh.triangles = triangles;

        meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = mesh;
    }

    void Update()
    {
        transform.position = playerTransform.position;
    }

}
