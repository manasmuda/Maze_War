using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIntemScript : MonoBehaviour
{

    public int position;
    public Button select;
    public Button dropDownButton;

    public Text charTypeTxt;
    public Text characterPosTxt;
    public Text hpText;
    public Slider hpSlider;

    public bool selected=false;
    public bool dropDownSelected=false;

    public int hp;
    public int ammo;
    public List<int> store;

    public GameManagerScript gameManagerScript;
    
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        select.onClick.AddListener(selectedF);
        dropDownButton.onClick.AddListener(DDSelectedF);
    }

    public void InitializeCharacter()
    {
        hp = 100;
        store = new List<int> { };
        if (charTypeTxt.text.Equals("Shooter"))
        {
            ammo = 10;
        }
        else if (charTypeTxt.text.Equals("Collector"))
        {
            ammo = 0;
        }
        else if (charTypeTxt.text.Equals("Bomber"))
        {
            ammo = 3;
        }
        characterPosTxt.text = (position + 1).ToString() + ")";
    }

    void selectedF()
    {
        selected = true;
        gameManagerScript.SetCharSelected(position);
    }

    void DDSelectedF()
    {
        dropDownSelected = !dropDownSelected;
        gameManagerScript.SetDDPos(position,dropDownSelected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void assignData(MyCharacterModel model)
    {
        hp = model.hp;
        store = model.store;
        ammo = model.ammo;
        hpSlider.value = hp;
        hpText.text = hp.ToString();
    }
}
