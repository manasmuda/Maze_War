using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeInitiator : MonoBehaviour
{
    private int[,] maze;
    public GameObject prefab;
    public GameObject prefab1;
    public GameObject tile;

    void Start()
    {
       
        maze = new int[30, 30];

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                maze[i, j] = 0;
            }
        }

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                bool check = true;
                int temp = (int)Random.Range(-1, 2);
                if (j == 29)
                {
                    if (temp == -1)
                    {
                        if (maze[i, j - 1] != 0)
                        {
                            check = false;
                        }
                    }
                }
                else if (i == 29)
                {
                    if (temp == 1)
                    {
                        if (maze[i - 1, j] != 0)
                        {
                            check = false;
                        }
                    }
                }
                if (check == true)
                {
                    maze[i, j] = temp;
                    //Debug.Log(maze[i, j].ToString());
                }
                else
                {
                    maze[i, j] = 0;
                }
            }
        }
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                Vector3 position = new Vector3(0, 0, 0);
                Debug.Log(maze[i, j]);
                Instantiate(tile, new Vector3(6*j-87,0.5f,87-6*i), Quaternion.identity);
                if (maze[i, j] == -1)
                {
                    Debug.Log("--1-");
                    position = new Vector3(6 * j - 87, 1, 84 - 6* i);
                    Debug.Log("--2-");
                    Instantiate(prefab, position, Quaternion.Euler(0, 90, 0));
                }
                else if (maze[i, j] == 1)
                {
                    Debug.Log("-1-");
                    position = new Vector3(6 * j - 84, 1, 87 - 6 * i);
                    Debug.Log("-2-");
                    Instantiate(prefab, position, Quaternion.identity);
                }

            }
        }
        PathFindingScript.maze = maze;
    }
}
