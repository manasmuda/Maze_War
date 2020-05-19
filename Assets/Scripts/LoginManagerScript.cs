using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Unity.Editor;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class LoginManagerScript : MonoBehaviour
{
    private bool firebaseInitialized;
    private bool loadingBool;

    private FirebaseApp app;
    private FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private Firebase.Firestore.FirebaseFirestore db;

    public GameObject loadingPanel;
    public GameObject loginSignUpPanel;
    public GameObject loginPanel;
    public GameObject signUpPanel;

    public Button loginButton;
    public Button signUpButton;
    public InputField LPEmailEdit;
    public InputField LPPwdEdit;
    public Button LPloginButton;
    public InputField SPEmailEdit;
    public InputField SPPwdEdit;
    public InputField SPUNEdit;
    public Button SPSignUpButton;

    // Start is called before the first frame update
    void Start()
    {
        loadingPanel.SetActive(true);
        loginPanel.SetActive(false);
        loginSignUpPanel.SetActive(false);
        signUpPanel.SetActive(false);
        Screen.SetResolution(Screen.width / 2, Screen.height / 2, true);
        InitializeFirebaseAndStart();
        InitializeButtonListeners();
        InitializeFirebaseAndStart();
    }

    void InitializeFirebaseAndStart()
    {
        Firebase.DependencyStatus dependencyStatus;

       
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                dependencyStatus = Firebase.FirebaseApp.CheckDependencies();
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    Debug.Log("FirebaseInitiated");
                    app = Firebase.FirebaseApp.DefaultInstance;
                    //InitializeFirebaseComponents();
                   InitializeFirebase();

                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies:" + dependencyStatus);
                    Application.Quit();
                }
            });
        
    }

    void InitializeFirebaseComponents()
    {
        Debug.Log("3");
        System.Threading.Tasks.Task.WhenAll(InitializeRemoteConfig()).ContinueWith(task => { firebaseInitialized = true; });

    }

    void InitializeButtonListeners()
    {
        loginButton.onClick.AddListener(LoginButtonCLickEvent);
        signUpButton.onClick.AddListener(SignUpButtonCLickEvent);
        LPloginButton.onClick.AddListener(LPloginButtonEvent);
        SPSignUpButton.onClick.AddListener(SPSignButtonEvent);

    }

    void LoginButtonCLickEvent()
    {
        loadingPanel.SetActive(false);
        loginPanel.SetActive(true);
        loginSignUpPanel.SetActive(false);
        signUpPanel.SetActive(false);

    }

    void SignUpButtonCLickEvent()
    {
        loadingPanel.SetActive(false);
        loginPanel.SetActive(false);
        loginSignUpPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    void LPloginButtonEvent()
    {
        loadingBool = true;
        loadingPanel.SetActive(true);
        FirebaseLogin();
    }

    void FirebaseLogin()
    {
        auth.SignInWithEmailAndPasswordAsync(LPEmailEdit.text, LPPwdEdit.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                loadingPanel.SetActive(false);
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                loadingPanel.SetActive(false);
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            this.user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", this.user.DisplayName, this.user.UserId);
            loadingPanel.SetActive(false);
            SceneManager.LoadScene("Home");
        });
    }

    void SPSignButtonEvent()
    {
        loadingBool = true;
        loadingPanel.SetActive(true);
        FirebaseSignUp();
    }

    void FirebaseSignUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(SPEmailEdit.text, SPPwdEdit.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                loadingPanel.SetActive(false);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                loadingPanel.SetActive(false);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            this.user = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", this.user.DisplayName, this.user.UserId);
            DocumentReference docRef = db.Collection("Users").Document(user.UserId);
            Dictionary<string, object> userDoc = new Dictionary<string, object>
            {
                { "email", SPEmailEdit.text },
                { "uid", this.user.UserId },
                { "name", SPPwdEdit.text },
            };
            docRef.SetAsync(userDoc).ContinueWithOnMainThread(task1 => {
                loadingPanel.SetActive(false);
                Debug.Log("Added data to the alovelace document in the users collection.");
                SceneManager.LoadScene("Home");
            });
        });
    }

    System.Threading.Tasks.Task InitializeRemoteConfig()
    {
        Dictionary<string, object> defaults = new Dictionary<string, object>();

        // VR Viewing height:
        defaults.Add(StringConstants.RemoteConfigVRHeightScale, 0.65f);
        // Physics defaults:
        defaults.Add(StringConstants.RemoteConfigPhysicsGravity, -20.0f);
        // Invites defaults:
        defaults.Add(StringConstants.RemoteConfigInviteTitleText,
            StringConstants.DefaultInviteTitleText);
        defaults.Add(StringConstants.RemoteConfigInviteMessageText,
            StringConstants.DefaultInviteMessageText);
        defaults.Add(StringConstants.RemoteConfigInviteCallToActionText,
            StringConstants.DefaultInviteCallToActionText);
        defaults.Add(StringConstants.RemoteConfigEmailContentHtml,
            StringConstants.DefaultEmailContentHtml);
        defaults.Add(StringConstants.RemoteConfigEmailSubjectText,
            StringConstants.DefaultEmailSubjectText);

        // Defaults for Map Objects:
        // Acceleration Tile
        defaults.Add(StringConstants.RemoteConfigAccelerationTileForce, 24.0f);
        // Drag Tile
        defaults.Add(StringConstants.RemoteConfigSandTileDrag, 5.0f);
        // Jump Tile
        defaults.Add(StringConstants.RemoteConfigJumpTileVelocity, 8.0f);
        // Mine Tile
        defaults.Add(StringConstants.RemoteConfigMineTileForce, 10.0f);
        defaults.Add(StringConstants.RemoteConfigMineTileRadius, 2.0f);
        defaults.Add(StringConstants.RemoteConfigMineTileUpwardsMod, 0.2f);
        // Spikes Tile
        defaults.Add(StringConstants.RemoteConfigSpikesTileForce, 10.0f);
        defaults.Add(StringConstants.RemoteConfigSpikesTileRadius, 1.0f);
        defaults.Add(StringConstants.RemoteConfigSpikesTileUpwardsMod, -0.5f);
        // Feature Flags
        defaults.Add(StringConstants.RemoteConfigGameplayRecordingEnabled, false);

        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
        return Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(System.TimeSpan.Zero);
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        user = auth.CurrentUser;
        if (user != null)
        {
            loadingPanel.SetActive(false);
            Debug.Log("FireBase Started Successfully");
            SceneManager.LoadScene("Home");
        }
        else
        {
            loadingPanel.SetActive(false);
            loginPanel.SetActive(false);
            loginSignUpPanel.SetActive(true);
            signUpPanel.SetActive(false);
            Debug.Log("FireBase Started Successfully");
        }
        //auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
