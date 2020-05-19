using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallCharacterButtonScript : MonoBehaviour
{

    public GameManagerScript gameManagerScript;
    public int position;
    public string name;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        GetComponent<Button>().onClick.AddListener(callCharacter);
    }

    public void Initialize(string n,int x)
    {
        position = x;
        name = n;
        text.text = position.ToString() + ") " + name;
    }

    void callCharacter()
    {
        gameManagerScript.callCharacter(position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
