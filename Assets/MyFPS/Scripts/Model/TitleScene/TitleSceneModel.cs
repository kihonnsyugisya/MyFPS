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
        SceneManager.LoadSceneAsync("StartScene");
    }

}
