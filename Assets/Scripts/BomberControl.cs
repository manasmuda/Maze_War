using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BomberControl : MonoBehaviour
{
    public WalkButtonControl buttonControl;
    private Animator animator;
    private bool trigger;
    private CanvasGroup BombTypePanel;
    private Button landMineButton;
    private Button timerBombButton;
    private Button closeButton;
    private CanvasGroup PlayPanel;

    public GameObject bombPrefab;

    private CharacterScript tempScript;

    // Start is called before the first frame update
    void Start()
    {
        BombTypePanel = GameObject.Find("Canvas/ControlsPanel/BomberPanel/BombTypePanel").GetComponent<CanvasGroup>();
        buttonControl = GameObject.Find("Canvas/ControlsPanel/BomberPanel/PlantButton").GetComponent<WalkButtonControl>();
        landMineButton = GameObject.Find("Canvas/ControlsPanel/BomberPanel/BombTypePanel/MineButton").GetComponent<Button>();
        timerBombButton = GameObject.Find("Canvas/ControlsPanel/BomberPanel/BombTypePanel/TimerButton").GetComponent<Button>();
        closeButton = GameObject.Find("Canvas/ControlsPanel/BomberPanel/BombTypePanel/CloseButton").GetComponent<Button>();
        PlayPanel= GameObject.Find("Canvas/ControlsPanel/PlayPanel").GetComponent<CanvasGroup>();
        bombPrefab = Resources.Load<GameObject>("Bomb");
        animator = GetComponent<Animator>();
        trigger = true;
        tempScript = gameObject.GetComponent<CharacterScript>();
        landMineButton.onClick.AddListener(delegate { plantBomb(0); });
        timerBombButton.onClick.AddListener(delegate { plantBomb(1); });
        closeButton.onClick.AddListener(delegate { closeButtonAction(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonControl.pressed && trigger && tempScript.ammo>0)
        {
            trigger = false;
            showPanel(BombTypePanel);
            PlayPanel.blocksRaycasts = false;
        }

        if (!buttonControl.pressed)
        {
            trigger = true;
        }
    }

    IEnumerator HoldWalkControls()
    {
        WalkButtonControl temp = GameObject.Find("Canvas/ControlsPanel/PlayPanel/SprintButton").GetComponent<WalkButtonControl>();
        temp.interactable = false;

        yield return new WaitForSeconds(2);

        temp.interactable = true;
    }

    void hidePanel(CanvasGroup x)
    {
        x.alpha = 0f;
        x.blocksRaycasts = false;
    }

    void showPanel(CanvasGroup x)
    {
        x.alpha = 1f;
        x.blocksRaycasts = true;
    }

    void plantBomb(int type)
    {
        closeButtonAction();
        animator.SetTrigger("plantBomb");
        tempScript.ammo = tempScript.ammo - 1;
        GameObject tempBomb=Instantiate(bombPrefab, new Vector3(transform.position.x, 0.4f, transform.position.z), Quaternion.identity) as GameObject;
        if (gameObject.GetComponent<CharacterScript>().skillPower)
        {
            tempBomb.GetComponent<BombScript>().damage = 150;
        }
        GameObject.Find("GameManager").GetComponent<GameManagerScript>().updateBombList(tempScript.position, type,tempBomb);
        StartCoroutine(HoldWalkControls());
    }

    void closeButtonAction()
    {
        hidePanel(BombTypePanel);
        PlayPanel.blocksRaycasts = true;
    }

}
