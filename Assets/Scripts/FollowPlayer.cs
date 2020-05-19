using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Transform playerPos;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (Playerprefs.playerPos == 1)
        {
            offset = new Vector3(0, 8, -5.5f);
        }
        else
        {
            offset = new Vector3(0, 8, 5.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPos.position + offset;
    }
}
