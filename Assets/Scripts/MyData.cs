using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyData
{
    public int characterPoints;
    public int powerPoints;

    public int storage;
    public int kills;

    public int playerPos;

    public List<MyCharacterModel> charactersList;

    public void initialize()
    {
        characterPoints = 50;
        powerPoints = 50;
        kills = 0;
        storage = 0;
        charactersList = new List<MyCharacterModel> { };
        playerPos = Playerprefs.playerPos;
    }
    
   public void convertData(Dictionary<string,int> data)
    {
        characterPoints = data["characterPoints"];
        powerPoints = data["powerPoints"];
        kills = data["kills"];
        storage = data["storage"];
        playerPos = data["playerPos"];
    }

    public Dictionary<string,object> toDict()
    {
        Dictionary<string, object> myDict = new Dictionary<string, object>();
        myDict.Add("characterPoints", characterPoints);
        myDict.Add("powerPoints", powerPoints);
        myDict.Add("kills", kills);
        myDict.Add("storage", storage);
        myDict.Add("playerPos", playerPos);
        return myDict;
    }
}
