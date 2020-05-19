using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;

public class PlayButtonScript : MonoBehaviour
{

    public Button playerButton;
    public Dropdown gameModeDD;

    private FirebaseFirestore db;

    // Start is called before the first frame update
    void Start()
    {
        playerButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Playerprefs.gameMode = gameModeDD.value;     
        SceneManager.LoadScene("MatchMacking");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
