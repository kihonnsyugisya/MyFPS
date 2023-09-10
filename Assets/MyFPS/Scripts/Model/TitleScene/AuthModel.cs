using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UniRx;

public class AuthModel : MonoBehaviour
{
    public FirebaseAuth auth;
    public FirebaseUser user;
    [HideInInspector] public BoolReactiveProperty isLogin = new(false);
    [HideInInspector] public bool isFirstLogin = false;
    //private void Awake()
    //{

    //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
    //    var dependencyStatus = task.Result;
    //    if (dependencyStatus == Firebase.DependencyStatus.Available)
    //    {
    //        // Create and hold a reference to your FirebaseApp,
    //        // where app is a Firebase.FirebaseApp property of your application class.
    //        auth = FirebaseAuth.DefaultInstance;
    //        if (auth.CurrentUser.UserId == null) isFirstLogin = true;
    //        else isFirstLogin = false;
    //        // Set a flag here to indicate whether Firebase is ready to use by your app.
    //    }
    //    else
    //    {
    //        Debug.LogError(System.String.Format(
    //          "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
    //        // Firebase Unity SDK is not safe to use here.
    //    }
    //});

    //}

    public void Init()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                auth = FirebaseAuth.DefaultInstance;
                auth.StateChanged += AuthStateChanged;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    // Track state changes of the auth object.
    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
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
                isLogin.Value = true;
            }
        }
        else {
            Debug.Log("新規ユーザーきたー");
            Create();
        }
    }

    // Handle removing subscription and reference to the Auth instance.
    // Automatically called by a Monobehaviour after Destroy is called on it.
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    private void Create()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            isLogin.Value = isFirstLogin = true;
        });
    }

    public void Delete()
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            user.DeleteAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("DeleteAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User deleted successfully.");
            });
        }
    }
}
