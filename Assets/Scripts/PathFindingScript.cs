using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingScript 
{
    public static int[,] maze = new int[30, 30];
    
    public static List<Vector2> findPath(Vector2 source,Vector2 dest)
    {
        Debug.Log("PathFinding started");
        bool shortPathAchieved = false;
        List<List<Vector2>> routePaths = new List<List<Vector2>> { };
        
        int i = (int)source.x;
        int j = (int)source.y;

        int curPath = 0;
        routePaths.Add(new List<Vector2> { });
        routePaths[0].Add(source);
        
        while (!shortPathAchieved)
        {
            bool up = false;
            bool right = false;
            bool down = false;
            bool left = false;
            int countR = 0;

            if (i!=0)
            {
                if (maze[i - 1, j] != -1)
                {
                    up = true;
                    countR++;
                }
            }
            if (j!=29)
            {
                if (maze[i, j] != 1)
                {
                    right = true;
                    countR++;
                }
            }
            if (i!=29)
            {
                if (maze[i, j] != -1)
                {
                    down=true;
                    countR++;
                }
            }
            if (j!=0)
            {
                if (maze[i, j - 1] != 1)
                {
                    left = true;
                    countR++;
                }
            }

            List<Vector2> tempCurVectors = new List<Vector2> { };

            for(int k = 0; k < routePaths[curPath].Count; k++)
            {
                tempCurVectors.Add(routePaths[curPath][k]);
            }

            bool curSet = true;

            if (up && curSet)
            {
                if (!routePaths[curPath].Contains(new Vector2(i-1, j)))
                {
                    routePaths[curPath].Add(new Vector2(i-1,j));
                    curSet = false;
                }
                countR--;
                up = false;
            }
            if (right && curSet)
            {
                if (!routePaths[curPath].Contains(new Vector2(i, j+1)))
                {
                    routePaths[curPath].Add(new Vector2(i, j+1));
                    curSet = false;
                }
                countR--;
                right = false;
            }
            if (down && curSet)
            {
                if (!routePaths[curPath].Contains(new Vector2(i + 1, j)))
                {
                    routePaths[curPath].Add(new Vector2(i + 1, j));
                    curSet = false;
                }
                countR--;
                down = false;
            }
            if (left && curSet)
            {
                if (!routePaths[curPath].Contains(new Vector2(i, j-1)))
                {
                    routePaths[curPath].Add(new Vector2(i, j-1));
                    curSet = false;
                }
                countR--;
                left = false;
            }
          
                for (int k = 0; k < countR; k++)
                {
                    routePaths.Add(new List<Vector2> { });
                    for(int k1 = 0; k1 < tempCurVectors.Count; k1++)
                    {
                        routePaths[routePaths.Count - 1].Add(tempCurVectors[k1]);
                    }
                    if (up)
                    {
                        if (!routePaths[routePaths.Count - 1].Contains(new Vector2(i - 1, j)))
                        {
                            routePaths[routePaths.Count - 1].Add(new Vector2(i - 1, j));
                        }
                        else
                        {
                            routePaths.RemoveAt(routePaths.Count - 1);
                        }
                        up = false;
                        
                    }
                    else if (right)
                    {
                        if (!routePaths[routePaths.Count - 1].Contains(new Vector2(i, j+1)))
                        {
                            routePaths[routePaths.Count - 1].Add(new Vector2(i, j + 1));
                        }
                        else
                        {
                            routePaths.RemoveAt(routePaths.Count - 1);
                        }
                        right = false;
                        
                    }
                    else if (down)
                    {
                        if (!routePaths[routePaths.Count - 1].Contains(new Vector2(i+1, j)))
                        {
                            routePaths[routePaths.Count - 1].Add(new Vector2(i+1, j));
                        }
                        else
                        {
                            routePaths.RemoveAt(routePaths.Count - 1);
                        }
                        down = false;
                        
                    }
                    else if (left)
                    {
                        if (!routePaths[routePaths.Count - 1].Contains(new Vector2(i, j - 1)))
                        {
                            routePaths[routePaths.Count - 1].Add(new Vector2(i, j - 1));
                        }
                        else
                        {
                            routePaths.RemoveAt(routePaths.Count - 1);
                        }
                        left = false;
                    }

                }
            List<bool> delList = new List<bool> { };
            for(int k = 0; k < routePaths.Count; k++)
            {
                delList.Add(false);
            }
            for(int k1 = 0; k1 < routePaths.Count - 1; k1++)
            {
                for(int k2 = k1 + 1; k2 < routePaths.Count; k2++)
                {
                    if((int)routePaths[k1][routePaths[k1].Count-1].x==(int)routePaths[k2][routePaths[k2].Count - 1].x && (int)routePaths[k1][routePaths[k1].Count - 1].y == (int)routePaths[k2][routePaths[k2].Count - 1].y)
                    {
                        if (routePaths[k1].Count < routePaths[k2].Count)
                        {
                            delList[k2] = true;
                        }
                        else
                        {
                            delList[k1] = true;
                        }
                    }
                }
            }

            for(int k = routePaths.Count - 1; k > -1; k--)
            {
                if (delList[k])
                {
                    routePaths.RemoveAt(k);
                }
            }

            int min = 100;
            int minPos = 0;
            for(int k = 0; k < routePaths.Count; k++)
            {
                int sd = (int)(Mathf.Abs(routePaths[k][routePaths[k].Count - 1].x - source.x) + Mathf.Abs(routePaths[k][routePaths[k].Count - 1].y - source.y));
                int dd = (int)(Mathf.Abs(routePaths[k][routePaths[k].Count - 1].x - dest.x) + Mathf.Abs(routePaths[k][routePaths[k].Count - 1].y - dest.y));
                int tempv = sd + dd;
                if (tempv<min)
                {
                    min = tempv;
                    minPos = k;
                }
            }
            curPath = minPos;
            i = (int)routePaths[curPath][routePaths[curPath].Count - 1].x;
            j = (int)routePaths[curPath][routePaths[curPath].Count - 1].y;
            if (i==(int)dest.x && j ==(int)dest.y)
            {
                shortPathAchieved = true;
            }
            /*for(int mx = 0; mx < routePaths.Count; mx++)
            {
                for(int nx = 0; nx < routePaths[mx].Count; nx++)
                {
                    Debug.Log("Route " + mx.ToString() + ": " + routePaths[mx][nx].x.ToString() + "," + routePaths[mx][nx].y.ToString());
                }
                Debug.Log("route " + mx.ToString() + " end");
            }*/
        }

        return routePaths[curPath];
    } 

}
