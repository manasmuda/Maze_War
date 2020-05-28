using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionItemScript : MonoBehaviour
{

    public GameManagerScript gameManagerScript;
    public int pos=0;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.tag);
        if (collider.tag == "Collector")
        {
            gameObject.SetActive(false);
            Debug.Log("Collection Item Triggered");
            CharacterScript tempScript = collider.gameObject.GetComponent<CharacterScript>();
            if (tempScript.charType=="Collector")
            {
                tempScript.AddItem(pos);
                gameManagerScript.ItemDeactivate(pos);
            }
        }
    }
}
