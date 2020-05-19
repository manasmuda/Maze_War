using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppData 
{
    public List<MyCharacterModel> charactersList;

    public void initialize()
    {
        charactersList = new List<MyCharacterModel> { };
    }
}
