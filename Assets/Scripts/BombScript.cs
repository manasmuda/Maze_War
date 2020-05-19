using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{

    public int bombType;
    public bool myTeam;
    public int pos;
    public bool active;
    public int damage = 100;

    void Start()
    {
        bombType = 0;
        damage = 100;
    }

    public void Initialize(int type,bool team,int x)
    {
        bombType = type;
        myTeam = team;
        pos = x;
        active = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("trigger started");
            CharacterScript tempScript = collider.gameObject.GetComponent<CharacterScript>();
            if ((!tempScript.myTeam && myTeam) || (tempScript.myTeam && !myTeam))
            {
                if (bombType == 0)
                {
                    Debug.Log("Bomb blasted");
                    gameObject.GetComponent<ParticleSystem>().Play();
                    GameObject.Find("GameManager").GetComponent<GameManagerScript>().updateBombActivated(pos, myTeam);
                    tempScript.OnBombHit(damage);
                    StartCoroutine(deactivateBomb());
                }
            }
        }
    }

    IEnumerator deactivateBomb()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Bomb deactivated");
        active = false;
        gameObject.SetActive(false);
    }
}
