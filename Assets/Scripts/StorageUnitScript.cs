using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageUnitScript : MonoBehaviour
{

    public bool myTeam;
    public int store;

    // Start is called before the first frame update
    void Start()
    {
        myTeam = true;
        store = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Collector")
        {
            Debug.Log("Near Storage");
            CharacterScript tempScript = collider.gameObject.GetComponent<CharacterScript>();
            if (tempScript.myTeam && myTeam)
            {
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().showCollectorPanel();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Collector")
        {
            Debug.Log("Exit Storage");
            CharacterScript tempScript = collider.gameObject.GetComponent<CharacterScript>();
            if (tempScript.myTeam && myTeam)
            {
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().hideCollectorPanel();
            }
        }
    }

}
