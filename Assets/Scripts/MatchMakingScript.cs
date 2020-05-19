using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchMakingScript : MonoBehaviour
{

    private FirebaseApp app;
    private FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private Firebase.Firestore.FirebaseFirestore db;

    public GameObject MMPanel;
    public GameObject JCPanel;
    public GameObject LoadingPanel;
    public GameObject JoinRoomPanel;

    public Button joinButton;
    public Button createButton;
    public Button joinButton1;

    public InputField roomCodeInput;

    private bool startTimer = false;

    private float timer = 45f;

    public Text timerText;
    public Text roomCodeText;
   
    private bool gameStarted=false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log((Playerprefs.gameMode).ToString());
        JoinRoomPanel.SetActive(false);
        app = Firebase.FirebaseApp.DefaultInstance;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        user = auth.CurrentUser;
        if (Playerprefs.gameMode == 0) {
            MMPanel.SetActive(true);
            JCPanel.SetActive(false);
            startTimer = true;
        }
        else if (Playerprefs.gameMode == 1)
        {
            MMPanel.SetActive(false);
            JCPanel.SetActive(true);
            startTimer = false;
            joinButton.onClick.AddListener(delegate { setRoom(1); });
            createButton.onClick.AddListener(delegate { setRoom(0); });
            joinButton1.onClick.AddListener(setRoomCode);
        }
    }

    void setRoom(int x)
    {
        Playerprefs.game2Type = x;
        JCPanel.SetActive(false);
        if (x == 0)
        {
            LoadingPanel.SetActive(true);
            StartCoroutine(RoomCreateFriend());
        }
        else if (x == 1)
        {
            JoinRoomPanel.SetActive(true);
        }
    }

    void setRoomCode()
    {
        JoinRoomPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        StartCoroutine(RoomJoinFriend());
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer = (timer - Time.deltaTime);

            timerText.text = ((int)timer).ToString();
        }

        if(timer<=0 && !gameStarted)
        {
            SceneManager.LoadScene("Home");
        }

        
    }

    IEnumerator RoomCreationRandom()
    {
        WWWForm form = new WWWForm();
        form.AddField("uid",user.UserId);

        UnityWebRequest www = UnityWebRequest.Post("", form);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    IEnumerator RoomCreateFriend()
    {
        WWWForm form = new WWWForm();
        form.AddField("uid", user.UserId);

        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-maze-war.cloudfunctions.net/createRoomFriend", form);
        www.timeout = 40;
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            startTimer = true;
            LoadingPanel.SetActive(false);
            MMPanel.SetActive(true);
            string data = www.downloadHandler.text;
            string[] slist = Parse(data);
            Playerprefs.roomCode = slist[0];
            Playerprefs.roomId = slist[1];
            roomCodeText.text = "RoomId: " + Playerprefs.roomCode;
            Debug.Log(data);
            StartCoroutine(RoomWaitFriend());
        }
    }

    IEnumerator RoomJoinFriend()
    {
        WWWForm form = new WWWForm();
        form.AddField("roomCode", roomCodeInput.text);
        Playerprefs.roomCode = roomCodeInput.text;
        form.AddField("uid", user.UserId);

        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-maze-war.cloudfunctions.net/joinRoomFriend", form);
        yield return www.SendWebRequest();



        if (www.isNetworkError || www.isHttpError)
        {
            JoinRoomPanel.SetActive(true);
            LoadingPanel.SetActive(false);
            Debug.Log(www.error);
        }
        else
        {
            LoadingPanel.SetActive(false);
            string data = www.downloadHandler.text;
            Debug.Log(data);
            Playerprefs.playerPos = -1;
            Playerprefs.roomId = data;
            gameStarted = true;
            SceneManager.LoadScene("game_scene");
        }
    }

    IEnumerator RoomWaitFriend()
    {
        WWWForm form = new WWWForm();
        form.AddField("roomCode", Playerprefs.roomCode);
        form.AddField("uid", user.UserId);

        UnityWebRequest www = UnityWebRequest.Post("https://us-central1-maze-war.cloudfunctions.net/waitRoomFriend", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            Debug.Log(data);
            Playerprefs.playerPos = 1;
            Playerprefs.playerId = data;
            gameStarted = true;
            SceneManager.LoadScene("game_scene");
        }
    }

    string[] Parse(string x)
    {

        char[] spearator = {'[', ',',']'};
        string temp = "";
        for(int i=1; i < x.Length-1; i++)
        {
            temp = temp + x[i];
        }
        String[] strlist = x.Split(spearator);
        return strlist;
    }
}
