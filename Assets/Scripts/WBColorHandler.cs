using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WBColorHandler : MonoBehaviour
{
    public WalkButtonControl walkButtonControl;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (walkButtonControl.pressed)
        {
            this.gameObject.GetComponent<Image>().color = new Color32(191, 238, 182, 255);
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
}
