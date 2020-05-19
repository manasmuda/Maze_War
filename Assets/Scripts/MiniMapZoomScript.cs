using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapZoomScript : MonoBehaviour
{
    private int zoomState;
    private int zoomValue;

    public Button zoomInButton;
    public Button zoomOutButton;

    public Button rightButton;
    public Button leftButton;
    public Button upButton;
    public Button downButton;

    public RectTransform map;
    public Camera topCamera;

    private int cameraPosX;
    private int cameraPosY;

    // Start is called before the first frame update
    void Start()
    {
        zoomState = 0;
        zoomValue = 90;
        cameraPosX = 0;
        cameraPosY = 0;
        zoomInButton.onClick.AddListener(zoomIn);
        zoomOutButton.onClick.AddListener(zoomOut);
        rightButton.onClick.AddListener(moveRight);
        leftButton.onClick.AddListener(moveLeft);
        upButton.onClick.AddListener(moveUp);
        downButton.onClick.AddListener(moveDown);
    }

    void zoomIn()
    {
        if (zoomState < 2)
        {
            zoomState++;
            zoomValue = zoomValue - 30;
            topCamera.orthographicSize = zoomValue;
            topCamera.transform.position = new Vector3(zoomState*30*cameraPosX,200, zoomState * 30 * cameraPosY);
        }
    }

    void zoomOut()
    {
        if (zoomState > 0)
        {
            zoomState--;
            zoomValue = zoomValue + 30;
            topCamera.orthographicSize = zoomValue;
            topCamera.transform.position = new Vector3(zoomState * 30 * cameraPosX, 200, zoomState * 30 * cameraPosY);
        }
    }

    void moveRight()
    {
        if (cameraPosX < 1)
        {
            cameraPosX = cameraPosX + 1;
            topCamera.transform.position = new Vector3(zoomState * 30 * cameraPosX, 200, zoomState * 30 * cameraPosY);
        }
    }

    void moveLeft()
    {
        if (cameraPosX > -1)
        {
            cameraPosX = cameraPosX - 1;
            topCamera.transform.position = new Vector3(zoomState * 30 * cameraPosX, 200, zoomState * 30 * cameraPosY);
        }
    }

    void moveUp()
    {
        if (cameraPosY < 1)
        {
            cameraPosY = cameraPosY + 1;
            topCamera.transform.position = new Vector3(zoomState * 30 * cameraPosX, 200, zoomState * 30 * cameraPosY);
        }
    }

    void moveDown()
    {
        if (cameraPosY > -1)
        {
            cameraPosY = cameraPosY - 1;
            topCamera.transform.position = new Vector3(zoomState * 30 * cameraPosX, 200, zoomState * 30 * cameraPosY);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
