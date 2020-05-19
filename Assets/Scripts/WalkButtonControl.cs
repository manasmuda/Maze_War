using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WalkButtonControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool pressed;
    public bool interactable;
    // Start is called before the first frame update
    void Start()
    {
        interactable = true;
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (interactable)
        {
            pressed = true;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
}
