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
        Debug.Log("Bullet hit: " + collider.tag);
        if (collider.tag == "Collector" || collider.tag == "Shooter" || collider.tag == "Bomber")
        {
            Debug.Log("trigger started");
            CharacterScript tempScript = collider.gameObject.GetComponent<CharacterScript>();
            if ((!tempScript.myTeam && myTeam) || (tempScript.myTeam && !myTeam))
            {
                tempScript.OnBulletHit(damage);
                gameObject.SetActive(false);
            }
        }
        else if (collider.tag == "wall")
        {
            gameObject.SetActive(false);
        }
        
    }

    void OnCollisionExit(Collision collision)
    {

    }

}
