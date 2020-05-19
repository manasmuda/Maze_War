using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public int damage=10;
    public bool myTeam;
    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
        myTeam = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider collider)
    {
        gameObject.SetActive(false);
        if (collider.tag == "Player")
        {
            Debug.Log("trigger started");
            CharacterScript tempScript = collider.gameObject.GetComponent<CharacterScript>();
            if ((!tempScript.myTeam && myTeam) || (tempScript.myTeam && !myTeam))
            {
                tempScript.OnBulletHit(damage);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {

    }

}
