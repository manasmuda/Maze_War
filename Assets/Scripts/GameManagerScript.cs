using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private int shooterPoints = 7;
    private int bomberPoints = 12;
    private int collectorPoints = 5;

    public GameObject Shooter;
    public GameObject Collector;
    public GameObject Bomber;

    public GameObject charCamera;
    public GameObject shooterCamera;
    public GameObject collectorCamera;
    public GameObject bomberCamera;
    public GameObject blueIndicator;
    public GameObject redIndicator;

    public GameObject ScrollContent;
    public GameObject MyTeamScroll;
    public GameObject OppTeamScroll;

    public GameObject CallCharListItem;

    public CanvasGroup AddCharacterPanel;
    public CanvasGroup ShooterPanel;
    public CanvasGroup BomberPanel;
    public CanvasGroup PlayPanel;
    public CanvasGroup MyCharactersPanel;
    public CanvasGroup MapsPanel;
    public CanvasGroup CharacterSelectionPanel;
    public CanvasGroup WaitingPanel;
    public CanvasGroup ViewCharactersPanel;
    public CanvasGroup CurCharDataPanel;
    public CanvasGroup CharacterListPanel;

    public Text curCharNameText;
    public Text curCharHpText;
    public Slider curCharSlider;

    public Button callCharButton;
    public RectTransform CharcterContent;

    public GameObject wallPrefab;
    public GameObject itemPrefab;
    public GameObject tilePrefab;

    public Text waitingText;
    public Button viewMapButton;
    public Button viewCharactersButton;

    public GameObject PowersList;

    public Button addShooterButton;
    public Button addCollectorButton;
    public Button addBomberButton;
    public Button characterPanelButton;

    public Text TimerText;

    public Text bluePoints;
    //public Text redPoints;

    private GameObject curObject;

    private List<GameObject> characters;
    private List<string> characterType;
    private List<GameObject> cameras;
    private List<GameObject> characterItemList;
    private List<GameObject> oppCharacters;
    private List<GameObject> myBombs;
    private List<GameObject> oppBombs;
    private int characterLength = 0;
    private int currentCharacterIndex = -1;
    private int selectedCharacterIndex = -1;
    private List<Dictionary<string,object>> mybombLocale;
    private List<Dictionary<string, object>> oppbombLocale;

    private int characterPoints = 50;
    private int powerPoints = 50;

    public GameObject blankCamera;

    public Transform MyCharactersPanelT;

    public GameObject CharacterItem;

    private float timer=0;
    private bool timerMode=false;

    private bool ddset=false;
    private bool charset=false;

    private MyData myData;
    private OppData oppData;

    private FirebaseApp app;
    private FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private Firebase.Firestore.FirebaseFirestore db;

    private int[,] mazeData;
    private int[] itemsData;
    private bool[] itemsEnabledData;
    private GameObject[] itemObjects;

    private string turn = "0";
    private int turnsCount = 0;
    private System.DateTime pauseTime;
    private int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = 0;
        app = Firebase.FirebaseApp.DefaultInstance;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        user = auth.CurrentUser;
        oppCharacters = new List<GameObject> { };
        characters = new List<GameObject> { };
        characterType = new List<string> { };
        cameras = new List<GameObject> { };
        characterItemList = new List<GameObject> { };
        myBombs = new List<GameObject> { };
        oppBombs = new List<GameObject> { };
        mybombLocale = new List<Dictionary<string,object>> { };
        oppbombLocale = new List<Dictionary<string, object>> { };
        myData = new MyData();
        myData.initialize();
        oppData = new OppData();
        oppData.initialize();
        addShooterButton.onClick.AddListener(addShooter);
        addCollectorButton.onClick.AddListener(addCollector);
        addBomberButton.onClick.AddListener(addBomber);
        characterPanelButton.onClick.AddListener(showCSPanel);
        viewMapButton.onClick.AddListener(showMapsPanel);
        callCharButton.onClick.AddListener(showCharactersList);
        MyCharactersPanelT = GameObject.Find("Canvas/CharacterSelectionPanel/MyCharactersPanel/ScrollView/Viewport/Content").transform;
        showPanel(CharacterSelectionPanel);
        hidePanel(PlayPanel);
        hidePanel(ShooterPanel);
        hidePanel(BomberPanel);
        hidePanel(MapsPanel);
        hidePanel(AddCharacterPanel);
        hidePanel(MyCharactersPanel);
        showPanel(WaitingPanel);
        hidePanel(CurCharDataPanel);
        hidePanel(CharacterListPanel);
        waitingText.text = "SETTING UP GAME";
        mazeData = new int[30, 30];
        itemsData = new int[20];
        itemsEnabledData = new bool[20];
        itemObjects = new GameObject[20];
        StartCoroutine(StartGame());
       /* Vector2 source = new Vector2(1,2);
        Vector2 dest = new Vector2(8,9);
        List<Vector2> pathList = PathFindingScript.findPath(source, dest);
        for(int i = 0; i < pathList.Count; i++)
        {
            Debug.Log(pathList[i].x.ToString() + "," +pathList[i].y.ToString());
        }*/
    }

    void showCharactersList()
    {
        if (CharacterListPanel.blocksRaycasts)
        {
            showPanel(PlayPanel);
            hidePanel(CharacterListPanel);
        }
        else
        {
            hidePanel(PlayPanel);
            showPanel(CharacterListPanel);
        }
    }

    void addShooter()
    {
        MyCharacterModel newChar = new MyCharacterModel();
        if (Playerprefs.playerPos == 1)
        {
            float x = Random.Range(-89.5f, -81.5f);
            float y = Random.Range(-89.5f, -81.5f);
            if((x>-87.3 && x<-86.7) || (x>-84.3 && x < -83.7))
            {
                x = x + 1;
            }
            if ((y > -87.3 && y < -86.7) || (y > -84.3 && y < -83.7))
            {
                y = y + 1;
            }
            curObject = Instantiate(Shooter, new Vector3(x, 0.8f, y), Quaternion.identity);
            characters.Add(curObject);
            characterType.Add("Shooter");
            Transform t1 = curObject.transform;
            newChar.Initialize("Shooter",t1.position.x, t1.position.z,characterLength);
            GameObject curObject1 = Instantiate(shooterCamera, t1.position, Quaternion.Euler(45, 0, 0));
            curObject1.GetComponent<FollowPlayer>().playerPos = t1;
            cameras.Add(curObject1);
            curObject1 = Instantiate(blueIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;

        }
        else
        {
            float x = Random.Range(81.5f, 89.5f);
            float y = Random.Range(81.5f, 89.5f);
            if ((x < 87.3 && x > 86.7) || (x < 84.3 && x > 83.7))
            {
                x = x - 1;
            }
            if ((y < 87.3 && y > 86.7) || (y < 84.3 && y > 83.7))
            {
                y = y - 1;
            }
            curObject = Instantiate(Shooter, new Vector3(x, 0.8f, y), Quaternion.identity);
            characters.Add(curObject);
            characterType.Add("Shooter");
            Transform t1 = curObject.transform;
            newChar.Initialize("Shooter", t1.position.x, t1.position.z, characterLength);
            GameObject curObject1 = Instantiate(shooterCamera, t1.position, Quaternion.Euler(45, 180, 0));
            curObject1.GetComponent<FollowPlayer>().playerPos = t1;
            cameras.Add(curObject1);
            curObject1 = Instantiate(blueIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;
        }
        GameObject newItemcontainer = Instantiate(CharacterItem, new Vector2(0, -200 * characterLength - 100), CharacterItem.transform.rotation);
        CharacterIntemScript cis = newItemcontainer.GetComponent<CharacterIntemScript>();
        cis.position = characterLength;
        cis.charTypeTxt.text = "Shooter";
        cis.hpText.text = "100";
        cis.hpSlider.value = 100;
        cis.InitializeCharacter();
        newItemcontainer.transform.SetParent(MyCharactersPanelT, false);
        characterItemList.Add(newItemcontainer);
        ScrollContent.GetComponent<RectTransform>().offsetMax = new Vector2(0, 200);
        CharcterContent.offsetMax = new Vector2(0, 120);
        myData.charactersList.Add(newChar);
        curObject.GetComponent<CharacterScript>().Initialize("Shooter",characterLength,true);
        GameObject newItem = Instantiate(CallCharListItem, new Vector2(0, -120 * characterLength - 60), CallCharListItem.transform.rotation);
        newItem.GetComponent<CallCharacterButtonScript>().Initialize("Shooter",characterLength);
        newItem.transform.SetParent(CharcterContent, false);
        characterLength++;
        myData.characterPoints = myData.characterPoints - shooterPoints;
        disableMovement(curObject);
    }

    void addCollector()
    {
        MyCharacterModel newChar = new MyCharacterModel();
        if (Playerprefs.playerPos == 1)
        {
            float x = Random.Range(-89.5f, -81.5f);
            float y = Random.Range(-89.5f, -81.5f);
            if ((x > -87.3 && x < -86.7) || (x > -84.3 && x < -83.7))
            {
                x = x + 1;
            }
            if ((y > -87.3 && y < -86.7) || (y > -84.3 && y < -83.7))
            {
                y = y + 1;
            }
            curObject = Instantiate(Collector, new Vector3(x, 0.8f, y), Quaternion.identity);
            characters.Add(curObject);
            characterType.Add("Collector");
            Transform t1 = curObject.transform;
            newChar.Initialize("Collector", t1.position.x, t1.position.z, characterLength);
            GameObject curObject1 = Instantiate(collectorCamera, t1.position, Quaternion.Euler(45, 0, 0));
            curObject1.GetComponent<FollowPlayer>().playerPos = t1;
            cameras.Add(curObject1);
            curObject1 = Instantiate(blueIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;

        }
        else
        {
            float x = Random.Range(81.5f, 89.5f);
            float y = Random.Range(81.5f, 89.5f);
            if ((x < 87.3 && x > 86.7) || (x < 84.3 && x > 83.7))
            {
                x = x - 1;
            }
            if ((y < 87.3 && y > 86.7) || (y < 84.3 && y > 83.7))
            {
                y = y - 1;
            }
            curObject = Instantiate(Collector, new Vector3(x, 0.8f, y), Quaternion.identity);
            characters.Add(curObject);
            characterType.Add("Collector");
            Transform t1 = curObject.transform;
            newChar.Initialize("Collector", t1.position.x, t1.position.z, characterLength);
            GameObject curObject1 = Instantiate(collectorCamera, t1.position, Quaternion.Euler(45, 180, 0));
            curObject1.GetComponent<FollowPlayer>().playerPos = t1;
            cameras.Add(curObject1);
            curObject1 = Instantiate(blueIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;
        }
        GameObject newItemcontainer = Instantiate(CharacterItem, new Vector2(0, -200 * characterLength - 100), CharacterItem.transform.rotation);
        CharacterIntemScript cis = newItemcontainer.GetComponent<CharacterIntemScript>();
        cis.position = characterLength;
        cis.charTypeTxt.text = "Collector";
        cis.hpText.text = "100";
        cis.hpSlider.value = 100;
        cis.InitializeCharacter();
        newItemcontainer.transform.SetParent(MyCharactersPanelT, false);
        characterItemList.Add(newItemcontainer);
        ScrollContent.GetComponent<RectTransform>().offsetMax = new Vector2(0, 200);
        CharcterContent.offsetMax = new Vector2(0, 120);
        myData.charactersList.Add(newChar);
        curObject.GetComponent<CharacterScript>().Initialize("Collector",characterLength,true);
        GameObject newItem = Instantiate(CallCharListItem, new Vector2(0, -120 * characterLength - 60), CallCharListItem.transform.rotation);
        newItem.GetComponent<CallCharacterButtonScript>().Initialize("Collector",characterLength);
        newItem.transform.SetParent(CharcterContent, false);
        characterLength++;
        myData.characterPoints = myData.characterPoints - collectorPoints;
        disableMovement(curObject);
    }

    void addBomber()
    {
        MyCharacterModel newChar = new MyCharacterModel();
        if (Playerprefs.playerPos == 1)
        {
            float x = Random.Range(-89.5f, -81.5f);
            float y = Random.Range(-89.5f, -81.5f);
            if ((x > -87.3 && x < -86.7) || (x > -84.3 && x < -83.7))
            {
                x = x + 1;
            }
            if ((y > -87.3 && y < -86.7) || (y > -84.3 && y < -83.7))
            {
                y = y + 1;
            }
            curObject = Instantiate(Bomber, new Vector3(x, 0.8f, y), Quaternion.identity);
            characters.Add(curObject);
            characterType.Add("Bomber");
            Transform t1 = curObject.transform;
            newChar.Initialize("Bomber", t1.position.x, t1.position.z, characterLength);
            GameObject curObject1 = Instantiate(bomberCamera, t1.position, Quaternion.Euler(45, 0, 0));
            curObject1.GetComponent<FollowPlayer>().playerPos = t1;
            cameras.Add(curObject1);
            curObject1 = Instantiate(blueIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;

        }
        else
        {
            float x = Random.Range(81.5f, 89.5f);
            float y = Random.Range(81.5f, 89.5f);
            if ((x < 87.3 && x > 86.7) || (x < 84.3 && x > 83.7))
            {
                x = x - 1;
            }
            if ((y < 87.3 && y > 86.7) || (y < 84.3 && y > 83.7))
            {
                y = y - 1;
            }
            curObject = Instantiate(Bomber, new Vector3(x, 0.8f, y), Quaternion.identity);
            characters.Add(curObject);
            characterType.Add("Bomber");
            Transform t1 = curObject.transform;
            newChar.Initialize("Bomber", t1.position.x, t1.position.z, characterLength);
            GameObject curObject1 = Instantiate(bomberCamera, t1.position, Quaternion.Euler(45, 180, 0));
            curObject1.GetComponent<FollowPlayer>().playerPos = t1;
            cameras.Add(curObject1);
            curObject1 = Instantiate(blueIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;
        }
        GameObject newItemcontainer = Instantiate(CharacterItem, new Vector2(0, -200 * characterLength - 100), CharacterItem.transform.rotation);
        CharacterIntemScript cis = newItemcontainer.GetComponent<CharacterIntemScript>();
        cis.position = characterLength;
        cis.charTypeTxt.text = "Bomber";
        cis.hpText.text = "100";
        cis.hpSlider.value = 100;
        cis.InitializeCharacter();
        newItemcontainer.transform.SetParent(MyCharactersPanelT, false);
        characterItemList.Add(newItemcontainer);
        ScrollContent.GetComponent<RectTransform>().offsetMax = new Vector2(0, 200);
        CharcterContent.offsetMax = new Vector2(0, 120);
        myData.charactersList.Add(newChar);
        curObject.GetComponent<CharacterScript>().Initialize("Bomber",characterLength,true);
        GameObject newItem = Instantiate(CallCharListItem, new Vector2(0, -120 * characterLength - 60), CallCharListItem.transform.rotation);
        newItem.GetComponent<CallCharacterButtonScript>().Initialize("Bomber",characterLength);
        newItem.transform.SetParent(CharcterContent, false);
        characterLength++;
        myData.characterPoints = myData.characterPoints - bomberPoints;
        disableMovement(curObject);
    }

    void addOppShooter()
    {
        if (Playerprefs.playerPos == -1)
        {
            float x = Random.Range(-89.5f, -81.5f);
            float y = Random.Range(-89.5f, -81.5f);
            if ((x > -87.3 && x < -86.7) || (x > -84.3 && x < -83.7))
            {
                x = x + 1;
            }
            if ((y > -87.3 && y < -86.7) || (y > -84.3 && y < -83.7))
            {
                y = y + 1;
            }
            curObject = Instantiate(Shooter, new Vector3(x, 0.8f, y), Quaternion.identity);
            oppCharacters.Add(curObject);
            Transform t1 = curObject.transform;
            GameObject curObject1 = Instantiate(redIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;

        }
        else
        {
            float x = Random.Range(81.5f, 89.5f);
            float y = Random.Range(81.5f, 89.5f);
            if ((x < 87.3 && x > 86.7) || (x < 84.3 && x > 83.7))
            {
                x = x - 1;
            }
            if ((y < 87.3 && y > 86.7) || (y < 84.3 && y > 83.7))
            {
                y = y - 1;
            }
            curObject = Instantiate(Shooter, new Vector3(x, 0.8f, y), Quaternion.identity);
            oppCharacters.Add(curObject);
            Transform t1 = curObject.transform;
            GameObject curObject1 = Instantiate(redIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;
        }
        curObject.GetComponent<CharacterScript>().Initialize("Shooter", oppCharacters.Count-1,false);
        disableMovement(curObject);
    }

    void AddOppCollector()
    {
        if (Playerprefs.playerPos == -1)
        {
            float x = Random.Range(-89.5f, -81.5f);
            float y = Random.Range(-89.5f, -81.5f);
            if ((x > -87.3 && x < -86.7) || (x > -84.3 && x < -83.7))
            {
                x = x + 1;
            }
            if ((y > -87.3 && y < -86.7) || (y > -84.3 && y < -83.7))
            {
                y = y + 1;
            }
            curObject = Instantiate(Collector, new Vector3(x, 0.8f, y), Quaternion.identity);
            oppCharacters.Add(curObject);
            Transform t1 = curObject.transform;
            GameObject curObject1 = Instantiate(redIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;

        }
        else
        {
            float x = Random.Range(81.5f, 89.5f);
            float y = Random.Range(81.5f, 89.5f);
            if ((x < 87.3 && x > 86.7) || (x < 84.3 && x > 83.7))
            {
                x = x - 1;
            }
            if ((y < 87.3 && y > 86.7) || (y < 84.3 && y > 83.7))
            {
                y = y - 1;
            }
            curObject = Instantiate(Collector, new Vector3(x, 0.8f, y), Quaternion.identity);
            oppCharacters.Add(curObject);
            Transform t1 = curObject.transform;
            GameObject curObject1 = Instantiate(redIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;
        }
        curObject.GetComponent<CharacterScript>().Initialize("Collector", oppCharacters.Count - 1,false);
        disableMovement(curObject);
    }

    void addOppBomber()
    {
        if (Playerprefs.playerPos == -1)
        {
            float x = Random.Range(-89.5f, -81.5f);
            float y = Random.Range(-89.5f, -81.5f);
            if ((x > -87.3 && x < -86.7) || (x > -84.3 && x < -83.7))
            {
                x = x + 1;
            }
            if ((y > -87.3 && y < -86.7) || (y > -84.3 && y < -83.7))
            {
                y = y + 1;
            }
            curObject = Instantiate(Bomber, new Vector3(x, 0.8f, y), Quaternion.identity);
            oppCharacters.Add(curObject);
            Transform t1 = curObject.transform;
            GameObject curObject1 = Instantiate(redIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;

        }
        else
        {
            float x = Random.Range(81.5f, 89.5f);
            float y = Random.Range(81.5f, 89.5f);
            if ((x < 87.3 && x > 86.7) || (x < 84.3 && x > 83.7))
            {
                x = x - 1;
            }
            if ((y < 87.3 && y > 86.7) || (y < 84.3 && y > 83.7))
            {
                y = y - 1;
            }
            curObject = Instantiate(Bomber, new Vector3(x, 0.8f, y), Quaternion.identity);
            oppCharacters.Add(curObject);
            Transform t1 = curObject.transform;
            GameObject curObject1 = Instantiate(redIndicator, t1.position, Quaternion.Euler(180, 0, 0));
            curObject1.GetComponent<IndicatorScript>().playerTransform = t1;
        }
        curObject.GetComponent<CharacterScript>().Initialize("Bomber", oppCharacters.Count - 1,false);
        disableMovement(curObject);
    }


    // Update is called once per frame
    void Update()
    {
        bluePoints.text = myData.storage.ToString();
       
        if (timerMode)
        {
            timer = (timer - Time.deltaTime);
            TimerText.text = "Time: " + ((int)timer).ToString();
            if (timer <= 0)
            {
                stopTimer();
                stopTurnUI();
                if (turn.Equals(Playerprefs.playerPos.ToString()))
                {
                    turn = ((-1) * Playerprefs.playerPos).ToString();
                    SetData();
                }
                else
                {
                    turn = (Playerprefs.playerPos).ToString();
                    StartCoroutine(waitForTurn());
                }
            }
        }
        else
        {
            TimerText.text = "";
        }

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

    public void SetCharSelected(int x)
    {
        selectedCharacterIndex = x;
        cameras[selectedCharacterIndex].SetActive(true);
        blankCamera.SetActive(false);
        showPanel(PlayPanel);
        showPanel(CurCharDataPanel);
        hidePanel(CharacterSelectionPanel);
        CharacterScript tempScript = characters[selectedCharacterIndex].GetComponent<CharacterScript>();
        curCharNameText.text =((tempScript.position+1).ToString())+") "+tempScript.charType;
        curCharHpText.text = tempScript.hp.ToString();
        curCharSlider.value = tempScript.hp;

        if (characterType[selectedCharacterIndex].Equals("Shooter"))
        {
            showPanel(ShooterPanel);
        }
        else if (characterType[selectedCharacterIndex].Equals("Bomber"))
        {
            showPanel(BomberPanel);
            
        }
        enableMovement(characters[selectedCharacterIndex]);
    }

    void showCSPanel()
    {
        disableMovement(characters[selectedCharacterIndex]);
        hidePanel(PlayPanel);
        hidePanel(ShooterPanel);
        hidePanel(BomberPanel);
        showPanel(CharacterSelectionPanel);
        hidePanel(CurCharDataPanel);
        blankCamera.SetActive(true);
        cameras[selectedCharacterIndex].SetActive(false);
        characterItemList[selectedCharacterIndex].GetComponent<CharacterIntemScript>().selected = false;
        selectedCharacterIndex = -1;
    }

    public void SetDDPos(int x,bool y)
    {
        if (y)
        {
            currentCharacterIndex = x;
            PowersList.SetActive(true);
            GameObject.Find("Canvas/PowersList/InvisiblePanel").GetComponent<PowerActiveScript>().active = characters[currentCharacterIndex].GetComponent<CharacterScript>().invisiblePower;
            GameObject.Find("Canvas/PowersList/SkillPanel").GetComponent<PowerActiveScript>().active = characters[currentCharacterIndex].GetComponent<CharacterScript>().skillPower;
            //GameObject.Find("Canvas/PowersList/TeleportPanel").GetComponent<PowerActiveScript>().active = characters[currentCharacterIndex].GetComponent<CharacterIntemScript>().powerList.Teleport;
            GameObject.Find("Canvas/PowersList/HPPanel").GetComponent<PowerActiveScript>().active = characters[currentCharacterIndex].GetComponent<CharacterScript>().hpPower;
        }
        else
        {
            currentCharacterIndex = -1;
            PowersList.SetActive(false);
        }
    }

    public void SetActivePower(string x)
    {
        if (x.Equals("Invisible"))
        {
            characters[currentCharacterIndex].GetComponent<CharacterScript>().invisiblePower = true;
        }
        else if (x.Equals("Skill"))
        {
            characters[currentCharacterIndex].GetComponent<CharacterScript>().skillPower = true;
        }
        else if (x.Equals("Teleport"))
        {
            //characters[currentCharacterIndex].GetComponent<CharacterScript>().teleportPower = true;
        }
        else if(x.Equals("HP"))
        {
            characters[currentCharacterIndex].GetComponent<CharacterScript>().hpPower = true;
            characters[currentCharacterIndex].GetComponent<CharacterScript>().setHpPower();
        }
    }

    IEnumerator StartGame()
    {
        state = 1;
        WWWForm form = new WWWForm();
        form.AddField("roomCode", Playerprefs.roomCode);
        form.AddField("uid", user.UserId);

        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-maze-war.cloudfunctions.net/gameStart", form);
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError)
        {
            state = -1;
            Debug.Log(www.error);
            string data = www.downloadHandler.text;
            Debug.Log(data);
        }
        else
        {
            string data = www.downloadHandler.text;
            Debug.Log(data);
            DocumentReference docRef = db.Collection("ROOMS").Document(Playerprefs.roomId);

            docRef.GetSnapshotAsync().ContinueWith((task) =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log(string.Format("Document data for {0} document:", snapshot.Id));
                    Dictionary<string, object> dataDoc = snapshot.ToDictionary();
                    List<int> maze = snapshot.GetValue<List<int>>("maze");
                    List<int> items = snapshot.GetValue<List<int>>("items");
                    List<bool> itemsEnabled = snapshot.GetValue<List<bool>>("itemsEnabled");
                    turn = snapshot.GetValue<string>("turn");
                    Debug.Log(maze[0]);
                    mazeData = new int[30, 30];
                    for (int i = 0; i < 30; i++)
                    {
                        for (int j = 0; j < 30; j++)
                        {
                            Debug.Log(maze[30 * i + j]);
                            mazeData[i, j] = maze[30 * i + j];
                        }
                    }
                    PathFindingScript.maze = mazeData;
                    for(int i = 0; i < 20; i++)
                    {
                        itemsData[i] = items[i];
                        itemsEnabledData[i] = itemsEnabled[i];
                    }
                    Debug.Log(mazeData);
                    initilaizeMaze(mazeData);
                    stopTurnUI();
                    CheckTheTurn();
                }
                else
                {
                    Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
                }
            });
        }
    }

    public void initilaizeMaze(int[,] maze)
    {
        if (wallPrefab == null)
        {
            Debug.Log("1");
        }
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                Vector3 position = new Vector3(0, 0, 0);
                //Instantiate(tilePrefab, new Vector3(6 * j - 87, 0.5f, 87 - 6 * i), Quaternion.identity);
                if (maze[i, j] == -1)
                {
                    
                    position = new Vector3(6 * j - 87, 1, 84 - 6 * i);
                    
                    Instantiate(wallPrefab, position, Quaternion.Euler(0, 90, 0));
                }
                else if (maze[i, j] == 1)
                {
                   
                    position = new Vector3(6 * j - 84, 1, 87 - 6 * i);
                    
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }

            }
        }
    }

    public void setItems(int[] x,bool[] y)
    {
        for(int i = 0; i < 20; i++)
        {
            if (y[i])
            {
                Debug.Log("item active");
                if (itemObjects[i] == null)
                {
                    int tempx = (int)(x[i] / 30);
                    int tempz = (int)(x[i] % 30);
                    GameObject tempItem = Instantiate(itemPrefab, new Vector3(6 * tempx - 87, 1.35f, 87 - 6 * tempz), Quaternion.identity);
                    tempItem.GetComponent<CollectionItemScript>().pos=i;
                    Debug.Log("item added");
                    itemObjects[i] = tempItem;
                }
                else if(!itemObjects[i].active)
                {
                    itemObjects[i].SetActive(true);
                }
            }
            else
            {
                Debug.Log("item not active");
                if (itemObjects[i] != null)
                {
                    itemObjects[i].SetActive(false);
                }
            }
        }
    }

    void showMyCharactersPanel()
    {
        showPanel(MyCharactersPanel);
        showPanel(AddCharacterPanel);
        hidePanel(MapsPanel);
    }

    void showMapsPanel()
    {
        showPanel(MapsPanel);
        hidePanel(MyCharactersPanel);
        hidePanel(AddCharacterPanel);
    }

    public void startTurnUI()
    {
        showPanel(CharacterSelectionPanel);
        hidePanel(AddCharacterPanel);
        hidePanel(MyCharactersPanel);
        hidePanel(PlayPanel);
        hidePanel(ShooterPanel);
        hidePanel(BomberPanel);
        hidePanel(WaitingPanel);
        showPanel(MapsPanel);
        showPanel(AddCharacterPanel);
        viewCharactersButton.interactable = true;
        viewCharactersButton.onClick.AddListener(showMyCharactersPanel);
    }

    public void stopTurnUI()
    {
        if (selectedCharacterIndex != -1)
        {
            disableMovement(characters[selectedCharacterIndex]);
        }
        showPanel(CharacterSelectionPanel);
        hidePanel(AddCharacterPanel);
        hidePanel(MyCharactersPanel);
        hidePanel(PlayPanel);
        hidePanel(ShooterPanel);
        hidePanel(BomberPanel);
        hidePanel(CurCharDataPanel);
        hidePanel(CharacterListPanel);
        showPanel(WaitingPanel);
        showPanel(MapsPanel);
        viewCharactersButton.interactable = false;
    }

    public void setSettingOpponentTurn()
    {
        waitingText.text = "SETTING UP OPPONENT TURN";
    }

    public void setWaitingOpponent()
    {
        waitingText.text = "OPPONENT TURN";
    }

    public void setSettingYourTurn()
    {
        waitingText.text = "SETTING UP YOUR TURN";
    }

    public void setCheckingStatus()
    {
        waitingText.text = "CHECKING STATUS";
    }

    public void setUploadingStatus()
    {
        waitingText.text = "UPLOADING STATUS";
    }

    void CheckTheTurn()
    {
        state = 2;
        setCheckingStatus();
        Debug.Log("Checking the turn");
        DocumentReference docRef = db.Collection("ROOMS").Document(Playerprefs.roomId);
        Debug.Log("Checking the turn1");
        docRef.GetSnapshotAsync().ContinueWith((task) =>
        {
            Debug.Log("Checking the turn2");
            DocumentSnapshot snapshot = task.Result;
            Debug.Log("Checking the turn3");
            if (snapshot.Exists)
            {
                Debug.Log(string.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> dataDoc = snapshot.ToDictionary();
                turn = snapshot.GetValue<string>("turn");
                Debug.Log(turn);
                if (turn.Equals(Playerprefs.playerPos.ToString()))
                {
                    Debug.Log("checkDataStatus");
                    StartCoroutine(checkDataStatus());
                }
                else
                {
                    StartCoroutine(CheckOppStart());
                }

            }
            else
            {
                state = -2;
                Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
    }

    IEnumerator checkDataStatus()
    {
        state = 3;
        setCheckingStatus();
        WWWForm form = new WWWForm();
        Debug.Log("startedCDS0");
        form.AddField("roomId", Playerprefs.roomId);
        form.AddField("playerPos", Playerprefs.playerPos.ToString());
        form.AddField("turnsCount", turnsCount);
        Debug.Log("startedCDS1");
        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-maze-war.cloudfunctions.net/checkDataStatus", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            state = -3;
            Debug.Log(www.error);
            string data = www.downloadHandler.text;
            Debug.Log(data);
        }
        else
        {
            string data = www.downloadHandler.text;
            Debug.Log(data);
            if (data.Equals("1"))
            {
                GetData();
            }
            else
            {
                state = -30;
                //Opponent left the Game
            }
        }
    }

    IEnumerator CheckOppStart()
    {
        state = 9;
        WWWForm form = new WWWForm();
        form.AddField("roomId", Playerprefs.roomId);
        form.AddField("playerPos", Playerprefs.playerPos.ToString());
        form.AddField("turnsCount", turnsCount);
        setSettingOpponentTurn();

        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-maze-war.cloudfunctions.net/checkOpponentStart", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            state = -9;
            Debug.Log(www.error);
            string data = www.downloadHandler.text;
            Debug.Log(data);
        }
        else
        {
            string data = www.downloadHandler.text;
            Debug.Log("COS:" + data);
            if (data.Equals("1"))
            {
                turnsCount++;
                startTimer();
                setWaitingOpponent();

            }
            else
            {
                state = -90;
                //Opponent left the Game
            }
        }

    }

    public void GetData()
    {
        state = 4;
        setSettingYourTurn();
        myData = new MyData();
        myData.initialize();
        oppData = new OppData();
        oppData.initialize();
        DocumentReference docRef = db.Collection("ROOMS").Document(Playerprefs.roomId);
        docRef.GetSnapshotAsync().ContinueWith((task) =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log(string.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> dataDoc = snapshot.ToDictionary();
                Dictionary<string, int> playerData;
                if (Playerprefs.playerPos == 1)
                {
                    playerData = snapshot.GetValue<Dictionary<string, int>>("player1Data");
                    if (dataDoc.ContainsKey("player1Bombs"))
                    {
                        mybombLocale = snapshot.GetValue<List<Dictionary<string, object>>>("player1Bombs");
                    }
                    if (dataDoc.ContainsKey("player2Bombs"))
                    {
                        oppbombLocale= snapshot.GetValue<List<Dictionary<string, object>>>("player2Bombs");
                    }
                }
                else
                {
                    playerData = snapshot.GetValue<Dictionary<string, int>>("player2Data");
                    if (dataDoc.ContainsKey("player1Bombs"))
                    {
                        oppbombLocale = snapshot.GetValue<List<Dictionary<string, object>>>("player1Bombs");
                    }
                    if (dataDoc.ContainsKey("player2Bombs"))
                    {
                        mybombLocale = snapshot.GetValue<List<Dictionary<string, object>>>("player2Bombs");
                    }
                }

                myData.convertData(playerData);
                List<bool> itemsEnabled = snapshot.GetValue<List<bool>>("itemsEnabled");
                for (int i = 0; i < 20; i++)
                {
                    itemsEnabledData[i] = itemsEnabled[i];
                }
                setItems(itemsData, itemsEnabledData);
                string myDoc;
                string oppDoc;
                if (Playerprefs.playerPos == 1)
                {
                    myDoc = "player1Characters";
                    oppDoc = "player2Characters";
                }
                else
                {
                    oppDoc = "player1Characters";
                    myDoc = "player2Characters";
                }
                Query myCharQuery = db.Collection("ROOMS").Document(Playerprefs.roomId).Collection(myDoc);
                myCharQuery.GetSnapshotAsync().ContinueWith(task1 => {
                    QuerySnapshot myCharSnapshot = task1.Result;
                    characterLength = myCharSnapshot.Count;
                    foreach (DocumentSnapshot documentSnapshot in myCharSnapshot.Documents)
                    {
                        Debug.Log(string.Format("My Document data for {0} document:", documentSnapshot.Id));
                        myData.charactersList.Add(MyCharacterModel.toObject(documentSnapshot));
                    };
                    Query oppCharQuery = db.Collection("ROOMS").Document(Playerprefs.roomId).Collection(oppDoc);
                    oppCharQuery.GetSnapshotAsync().ContinueWith(task2 => {
                        QuerySnapshot oppCharSnapshot = task2.Result;
                        foreach (DocumentSnapshot documentSnapshot in oppCharSnapshot.Documents)
                        {
                            Debug.Log(string.Format("Opp Document data for {0} document:", documentSnapshot.Id));
                            oppData.charactersList.Add(MyCharacterModel.toObject(documentSnapshot));
                        };
                        db.Collection("ROOMS").Document(Playerprefs.roomId).UpdateAsync(new Dictionary<string, object> { { "dataUpdated", false } }).ContinueWith((task3) =>
                           {
                               Debug.Log("Data updated");
                               StartCoroutine(SetStartTurn());
                           });
                    });
                });

            }
            else
            {
                state = -4;
                Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
    }

    IEnumerator SetStartTurn()
    {
        state = 5;
        setSettingYourTurn();
        WWWForm form = new WWWForm();
        form.AddField("roomId", Playerprefs.roomId);
        form.AddField("playerPos", Playerprefs.playerPos.ToString());

        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-maze-war.cloudfunctions.net/setStartTurn", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            state = -5;
            Debug.Log(www.error);
            string data = www.downloadHandler.text;
            Debug.Log(data);
        }
        else
        {
            Debug.Log("turnStarted");
            turnsCount++;
            synchronizeData();
            startTurnUI();    
            startTimer();
        }
    }

    public void SetData()
    {
        state = 7;
        setUploadingStatus();
        convertCharacterData();
        string myDoc;
        string oppDoc;
        string playerKey;
        string myBombs;
        string oppBombs;
        if (Playerprefs.playerPos == 1)
        {
            myDoc = "player1Characters";
            oppDoc = "player2Characters";
            playerKey = "player1Data";
            myBombs = "player1Bombs";
            oppBombs = "player2Bombs";
        }
        else
        {
            oppDoc = "player1Characters";
            myDoc = "player2Characters";
            playerKey = "player2Data";
            myBombs = "player2Bombs";
            oppBombs = "player1Bombs";

        }
        List<bool> tempList = new List<bool>();
        for (int i = 0; i < 20; i++)
        {
            tempList.Add(itemsEnabledData[i]);
        }
        Dictionary<string, object> playerData = new Dictionary<string, object>{{playerKey,myData.toDict()},{"dataUpdated",true},{"itemsEnabled",tempList},{myBombs,mybombLocale},{oppBombs,oppbombLocale}};
        DocumentReference docRef = db.Collection("ROOMS").Document(Playerprefs.roomId);
        docRef.UpdateAsync(playerData).ContinueWith((task) =>
        {
            if (!task.IsFaulted)
            {
                Debug.Log("Sent Player Data");
                WriteBatch myPlayersBatch = db.StartBatch();
                for(int i = 0; i < myData.charactersList.Count; i++)
                {
                    DocumentReference tempRef = db.Collection("ROOMS").Document(Playerprefs.roomId).Collection(myDoc).Document(i.ToString());
                    if (myData.charactersList[i].newC)
                    {
                        myData.charactersList[i].newC = false;
                        myPlayersBatch.Set(tempRef, myData.charactersList[i].toDict());
                    }
                    else
                    {
                        myPlayersBatch.Update(tempRef, myData.charactersList[i].toDict());
                    }
                }
                myPlayersBatch.CommitAsync().ContinueWith((task1) =>
                {
                    Debug.Log("Sent My Players Data");
                    WriteBatch oppPlayersBatch = db.StartBatch();
                    for (int j = 0; j < oppData.charactersList.Count; j++)
                    {
                        DocumentReference tempRef = db.Collection("ROOMS").Document(Playerprefs.roomId).Collection(oppDoc).Document(j.ToString());
                        oppPlayersBatch.Update(tempRef, oppData.charactersList[j].toDict());
                        
                    }
                    oppPlayersBatch.CommitAsync().ContinueWith((task2) =>
                    {

                        StartCoroutine(CheckOppStart());
                        Debug.Log("Sent Opp Players Data");
                    });
                });
            }
            else
            {
                state = -7;
                Debug.Log("failed");
            }
        });
        
    }

    IEnumerator waitForTurn()
    {
        state = 8;
        setCheckingStatus();
        WWWForm form = new WWWForm();
        Debug.Log("startedWFT");
        form.AddField("roomId", Playerprefs.roomId);
        form.AddField("playerPos", Playerprefs.playerPos.ToString());
        
        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-maze-war.cloudfunctions.net/waitForTurn", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            string data = www.downloadHandler.text;
            Debug.Log(data);
            state = -8;
        }
        else
        {
            string data = www.downloadHandler.text;
            Debug.Log(data);
            if (data.Equals("1"))
            {
                StartCoroutine(checkDataStatus());
            }
            else
            {
                state = -80;
                //Opponent left the Game
            }
        }
    }

    public void startTimer()
    {
        timer = 45;
        timerMode = true;
    }

    public void stopTimer()
    {
        timer = 0;
        timerMode = false;
    }

    void synchronizeData()
    {
        Debug.Log("SD:" + oppCharacters.Count.ToString() + "," + oppData.charactersList.Count.ToString());
        for (int i = 0; i < myData.charactersList.Count; i++)
        {
            Debug.Log("SD:myData" + i.ToString());
            if (myData.charactersList[i].hp <= 0)
            {
                characters[i].SetActive(false);
                characterItemList[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                characterItemList[i].GetComponent<CharacterIntemScript>().assignData(myData.charactersList[i]);
                characters[i].GetComponent<CharacterScript>().assignData(myData.charactersList[i]);
                Vector3 temp=characters[i].transform.position;
                temp.x = myData.charactersList[i].posX;
                temp.z = myData.charactersList[i].posY;
                characters[i].transform.position = temp;
            }
        }
        for (int i = 0; i < oppData.charactersList.Count; i++)
        {
            Debug.Log("SD:oppData" + i.ToString());
            if (oppCharacters.Count<=i)
            {
                Debug.Log("adding a opp charater");
                if (oppData.charactersList[i].charType.Equals("Shooter"))
                {
                    addOppShooter();
                    Debug.Log("Opp Shooter");
                }
                else if (oppData.charactersList[i].charType.Equals("Collector"))
                {
                    AddOppCollector();
                    Debug.Log("Opp Collecter");
                }
                else if (oppData.charactersList[i].charType.Equals("Bomber"))
                {
                    addOppBomber();
                    Debug.Log("Opp Bomber");
                }
                Debug.Log(oppCharacters.Count);
                Debug.Log("i:"+i.ToString());
            }
            
            if (oppData.charactersList[i].hp <= 0)
            {
               oppCharacters[i].SetActive(false);
            }
            else
            {
                oppCharacters[i].GetComponent<CharacterScript>().assignData(oppData.charactersList[i]);
                Vector3 temp = oppCharacters[i].transform.position;
                temp.x = oppData.charactersList[i].posX;
                temp.z = oppData.charactersList[i].posY;
                oppCharacters[i].transform.position = temp;
            }
        }
    }

    void OnApplicationPause(bool isGamePause)
    {
        if (isGamePause)
        {
            Debug.Log("paused");
            pauseTime = System.DateTime.Now;
            Debug.Log(pauseTime.ToString("hh:mm:ss"));

        }
    }

    void OnApplicationFocus(bool isGameFocus)
    {
        if (isGameFocus)
        {
            Debug.Log("started");
            int timediff = System.DateTime.Now.Subtract(pauseTime).Seconds;
            Debug.Log("timeDiff:" + timediff.ToString());
            if (timerMode)
            {
                Debug.Log("start:timerMode");
                if (timer < timediff)
                {
                    stopTimer();
                    checkCurrentStatus();
                }
                else
                {
                    timer = timer - timediff;
                }
            }
            else 
            {
                Debug.Log("start:timerModeFalse");
                if (state % 10 == 0)
                {

                }
                else
                {
                    checkCurrentStatus();
                }
            }
            Debug.Log("start:"+timer.ToString());
        }
    }

    public void convertCharacterData()
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if (characters[i].active)
            {
                myData.charactersList[i].convertData(characters[i]);
            }
        }
        for (int i = 0; i < oppCharacters.Count; i++)
        {
            if (oppCharacters[i].active)
            {
                oppData.charactersList[i].convertData(oppCharacters[i]);
            }
        }
    }

    public void checkCurrentStatus()
    {

    }

    public void enableMovement(GameObject characterSelected)
    {
        characterSelected.GetComponent<PlayerAnimator>().enabled=true;
        CharacterScript characterScript = characterSelected.GetComponent<CharacterScript>();
        if (characterScript.charType.Equals("Shooter"))
        {
            characterSelected.GetComponent<ShooterControl>().enabled = true;
        }
        else if (characterScript.charType.Equals("Collector"))
        {

        }
        else if (characterScript.charType.Equals("Bomber"))
        {
            characterSelected.GetComponent<BomberControl>().enabled = true;
        }
    }

    public void disableMovement(GameObject characterSelected)
    {
        characterSelected.GetComponent<PlayerAnimator>().enabled = false;
        CharacterScript characterScript = characterSelected.GetComponent<CharacterScript>();
        if (characterScript.charType.Equals("Shooter"))
        {
            characterSelected.GetComponent<ShooterControl>().enabled = false;
        }
        else if (characterScript.charType.Equals("Collector"))
        {

        }
        else if (characterScript.charType.Equals("Bomber"))
        {
            characterSelected.GetComponent<BomberControl>().enabled = false;
        }
    }

    public void characterDead(int x,bool y)
    {
        if (y)
        {
            myData.charactersList[x].convertData(characters[x]);
            characters[x].SetActive(false);
        }
        else
        {
            oppData.charactersList[x].convertData(oppCharacters[x]);
            oppCharacters[x].SetActive(false);
        }
    }

    public void updateBombList(int x,int type,GameObject tempBomb)
    {
        tempBomb.GetComponent<BombScript>().Initialize(type,true,myBombs.Count);
        myBombs.Add(tempBomb);
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("charPos", x);
        dict.Add("turnCount", turnsCount);
        dict.Add("bombType", type);
        dict.Add("active", true);
        dict.Add("posX",characters[x].transform.position.x);
        dict.Add("posY", characters[x].transform.position.z);
        mybombLocale.Add(dict);
    }

    public void updateBombActivated(int x,bool y)
    {
        if (y)
        {
            mybombLocale[x]["active"] = false;
        }
        else
        {
            oppbombLocale[x]["active"] = false;
        }
    }

    public void callCharacter(int x)
    {
        if (x != selectedCharacterIndex)
        {
            showCharactersList();
            Debug.Log(selectedCharacterIndex.ToString() + " follows " + x.ToString());
            CallAutoMoveScript tempScript = characters[x].GetComponent<CallAutoMoveScript>();
            if (tempScript.onCallMove)
            {
                tempScript.StopMove();
            }
            Vector3 sourceChar = characters[x].transform.position;
            Vector3 destChar = characters[selectedCharacterIndex].transform.position;
            Vector2 source = new Vector2((int)((90.0f-sourceChar.z) / 6), (int)((sourceChar.x+90.0f)/6));
            Vector2 dest = new Vector2((int)((90.0f - destChar.z) / 6), (int)((destChar.x + 90.0f) / 6));
            List<Vector2> pathList = PathFindingScript.findPath(source,dest);
            for(int i = 0; i < pathList.Count; i++)
            {
                Debug.Log(pathList[i].x.ToString()+","+ pathList[i].y.ToString());
            }
            tempScript.StartMove(pathList);
        }
    }

    public void ItemDeactivate(int x)
    {
        itemsEnabledData[x] = false;
    }
}
