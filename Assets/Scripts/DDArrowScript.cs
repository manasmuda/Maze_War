using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDArrowScript : MonoBehaviour
{
    public Button arrow;

    // Start is called before the first frame update
    void Start()
    {
        arrow.onClick.AddListener(setBool);
    }

    void setBool()
    {
        DropDownScript.clicked = !DropDownScript.clicked;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
