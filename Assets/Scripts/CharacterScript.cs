using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{

    public string charType;
    public bool invisiblePower;
    public bool skillPower;
    public bool hpPower;
    public int hp;
    public int ammo;
    public int store;
    public List<int> storeList;
    public int damage;
    public int position;
    public bool myTeam;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Initialize(string s,int z,bool b)
    {
        position = z;
        myTeam = b;
        charType = s;
        if (charType.Equals("Shooter"))
        {
            ammo = 10;
            damage = 10;
        }
        else if (charType.Equals("Collector"))
        {
            ammo = 0;
            damage = 0;
        }
        else if (charType.Equals("Bomber"))
        {
            ammo = 3;
            damage = 300;
        }
        store = 0;
        hp = 100;
        invisiblePower = false;
        skillPower = false;
        hpPower = false;
        storeList = new List<int> { };
    }

    public void assignData(MyCharacterModel model)
    {
        hp = model.hp;
        store = model.store;
        ammo = model.ammo;
        invisiblePower = model.invisiblePower;
        skillPower = model.skillPower;
        hpPower = model.hpPower;
        if (myTeam)
        {
            invisiblePower = false;
            skillPower = false;
            hpPower = false;
        }
        synchronizePowers();
    }

    public void synchronizePowers()
    {
        setInvisiblePower();
        setHpPower();
    }

    public void setInvisiblePower()
    {
        if (invisiblePower)
        {
            if (!myTeam)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void setHpPower()
    {
        if (hpPower)
        {
            hp = hp + 50;
            hpPower = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "bullet")
        {
            Debug.Log("bullet hit");
            hp = hp - collision.collider.gameObject.GetComponent<BulletScript>().damage;
            if (hp <= 0)
            {
                hp = 0;
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().characterDead(position,myTeam);
            }
        }
        if (collision.collider.tag == "bomb")
        {
            Debug.Log("bomb hit");
            hp = hp - collision.collider.gameObject.GetComponent<BombScript>().damage;
            if (hp <= 0)
            {
                hp = 0;
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().characterDead(position, myTeam);
            }
        }
    }

    public void OnBulletHit(int x)
    {
        Debug.Log("bullet hit");
        hp = hp - x;
        if (hp <= 0)
        {
            hp = 0;
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().characterDead(position, myTeam);
        }
    }

    public void OnBombHit(int x)
    {
        Debug.Log("bomb hit");
        hp = hp - x;
        if (hp <= 0)
        {
            hp = 0;
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().characterDead(position, myTeam);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        
    }

    public void AddItem(int position)
    {
        store++;
        storeList.Add(position);
    }

}
