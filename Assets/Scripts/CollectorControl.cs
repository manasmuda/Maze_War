using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorControl : MonoBehaviour
{

    public WalkButtonControl buttonControl;
    // Start is called before the first frame update
    void Start()
    {
        buttonControl = GameObject.Find("Canvas/ControlsPanel/CollectorPanel/StoreButton").GetComponent<WalkButtonControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonControl.pressed)
        {
            gameObject.GetComponent<CharacterScript>().store = new List<int> { };
        }
    }
}
