using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;

public class MyCharacterModel
{
    public int index;
    public float posX;
    public float posY;
    public bool invisiblePower;
    public bool skillPower;
    public bool hpPower;
    public int hp;
    public int ammo;
    public string charType;
    public int store;
    public bool newC;

    public void Initialize(string s,float x,float y,int z)
    {
        index = z;
        charType = s;
        if (charType.Equals("Shooter"))
        {
            ammo = 10;
        }
        else if (charType.Equals("Collector"))
        {
            ammo = 0;
        }
        else if(charType.Equals("Bomber"))
        {
            ammo = 3;
        }
        store=0;
        hp = 100;
        invisiblePower = false;
        skillPower = false;
        hpPower = false;
        posX = x;
        posY = y;
        newC = true;
    }

    public Dictionary<string,object> toDict()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("index", index);
        dict.Add("posX", posX);
        dict.Add("posY", posY);
        dict.Add("store", store);
        dict.Add("invisiblePower", invisiblePower);
        dict.Add("skillPower", skillPower);
        dict.Add("hpPower", hpPower);
        dict.Add("hp", hp);
        dict.Add("ammo", ammo);
        dict.Add("charType", charType);
        dict.Add("newC", newC);
        return dict;
    }

    public static MyCharacterModel toObject(DocumentSnapshot dict)
    {
        MyCharacterModel myCharacterModel = new MyCharacterModel();
        myCharacterModel.index = dict.GetValue<int>("index");
        myCharacterModel.ammo = dict.GetValue<int>("ammo");
        myCharacterModel.charType = dict.GetValue<string>("charType");
        myCharacterModel.hp = dict.GetValue<int>("hp");
        myCharacterModel.posX = dict.GetValue<float>("posX");
        myCharacterModel.posY = dict.GetValue<float>("posY");
        myCharacterModel.store = dict.GetValue<int>("store");
        myCharacterModel.invisiblePower = dict.GetValue<bool>("invisiblePower");
        myCharacterModel.hpPower= dict.GetValue<bool>("hpPower");
        myCharacterModel.skillPower = dict.GetValue<bool>("skillPower");
        myCharacterModel.newC = dict.GetValue<bool>("newC");
        return myCharacterModel;
    }

    public void convertData(GameObject character) 
    {
        posX = character.transform.position.x;
        posY = character.transform.position.z;
        CharacterScript script = character.GetComponent<CharacterScript>();
        invisiblePower = script.invisiblePower;
        skillPower = script.skillPower;
        hpPower = script.hpPower;
        hp = script.hp;
        ammo = script.ammo;
        store = script.store;
    }

}
