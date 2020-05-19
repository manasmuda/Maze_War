using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallAutoMoveScript : MonoBehaviour
{

    public bool onCallMove;
    public List<Vector2> route;
    public int pathIndex = 0;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        onCallMove = false;
        route = null;
        pathIndex = 0;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onCallMove)
        {
            int cx = (int)((90.0f - transform.position.z) / 6);
            int cy = (int)((transform.position.x + 90.0f) / 6);
            int px = (int)route[pathIndex].x;
            int py = (int)route[pathIndex].y;

            if(cx!=px || cy != py)
            {
                pathIndex++;
            }
            else if (pathIndex < route.Count - 1)
            {
                int npx = (int)route[pathIndex + 1].x;
                int npy = (int)route[pathIndex + 1].y;
                Vector3 vec = transform.position;
                vec.z = vec.z+(px-npx)*1.3f*Time.deltaTime;
                vec.x = vec.x+(npy-py)*1.3f*Time.deltaTime;
                rb.MovePosition(vec);
            }
        }
    }

    public void StopMove()
    {
        onCallMove = false;
        route = null;
    }

    public void StartMove(List<Vector2> vector2s)
    {
        onCallMove = true;
        route = vector2s;
        pathIndex = 0;
    }
}
