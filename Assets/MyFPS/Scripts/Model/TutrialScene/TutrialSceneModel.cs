using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Firestore;

public class TutrialSceneModel : MonoBehaviour
{
    public NGWordSettings nGWordSettings;

    public void MoveToStartScene()
    {
        FireStoreModel.Init(AuthModel.auth.CurrentUser.UserId);
        var ud = new UserData
        {
            CreatedDate = Timestamp.GetCurrentTimestamp(),
            Level = 0,

            

        };
        FireStoreModel.AddInitialUserData(ud);
        SceneManager.LoadSceneAsync("StartScene");
    }
}
