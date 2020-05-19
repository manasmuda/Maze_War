using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingScript : MonoBehaviour
{

    public GameObject tilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                Instantiate(tilePrefab, new Vector3(6 * j - 87, 0.5f, 87 - 6 * i), Quaternion.identity);
            }
        }
    }
            
    void Update()
    {
        
    }
}
