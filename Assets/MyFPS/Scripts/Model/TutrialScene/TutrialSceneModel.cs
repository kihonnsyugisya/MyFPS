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
        SceneManager.LoadSceneAsync("StartScene");
    }

    public async void SetInitalUserData(string nickName)
    {
        FireStoreModel.Init(AuthModel.auth.CurrentUser.UserId);
        var ud = new UserData
        {
            CreatedDate = Timestamp.GetCurrentTimestamp(),
            LastLogin = Timestamp.GetCurrentTimestamp(),
            Level = 0,
            NickName = nickName,
            Avatar = "3RDPerson"
        };
        await FireStoreModel.AddInitialUserData(ud);
    }
}
