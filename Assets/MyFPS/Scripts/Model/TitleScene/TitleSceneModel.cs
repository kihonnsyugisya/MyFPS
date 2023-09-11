using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public class TitleSceneModel : MonoBehaviour
{
    public AuthModel authModel;

    public void MoveToStartScene()
    {
        Debug.Log("ログイン処理を行なってから画面遷移　lastlogin:leble,resource check");
        FireStoreModel.Init(AuthModel.auth.CurrentUser.UserId);
        FireStoreModel.UpdateLastLogin();
        SceneManager.LoadSceneAsync("StartScene");
    }

    public void MoveToTutorialScene()
    {
        SceneManager.LoadSceneAsync("TutrialScene");
    }

}
