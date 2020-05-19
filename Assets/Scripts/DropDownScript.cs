using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownScript : MonoBehaviour
{

    public static bool clicked=false;

    public static CanvasGroup dropDown;

    private static bool[] activatedList;

    void Start()
    {
        dropDown = GameObject.Find("Canvas/CharacterSelectionPanel/MyCharactersPanel/PowersList").GetComponent<CanvasGroup>();
        hidePanel(dropDown);
        activatedList = new bool[3];
    }

    // Update is called once per frame
    void Update()
    {
        if (dropDown == null)
        {
            dropDown = GameObject.Find("Canvas/CharacterSelectionPanel/MyCharactersPanel/PowersList").GetComponent<CanvasGroup>();
        }
        if (clicked)
        {
            showPanel(dropDown);
        }
        else
        {
            hidePanel(dropDown);
        }
    }

    void hidePanel(CanvasGroup x)
    {
        x.alpha = 0f;
        x.blocksRaycasts = false;
    }

    void showPanel(CanvasGroup x)
    {
        x.alpha = 1f;
        x.blocksRaycasts = true;
    }

}
