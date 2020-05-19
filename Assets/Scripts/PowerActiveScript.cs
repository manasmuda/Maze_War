using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerActiveScript : MonoBehaviour
{

    public bool active = false;
    public Image image;
    public Button button;
    public string type;

    public GameManagerScript gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(setActive);
    }

    void setActive()
    {
        active = true;
        gameManagerScript.SetActivePower(type);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            image.color = new Color32(50, 0, 0, 155);
        }
        else
        {
            image.color = new Color32(255,255, 255, 255);
        }
    }
}
