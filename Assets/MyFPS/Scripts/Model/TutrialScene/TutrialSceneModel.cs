using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutrialSceneModel : MonoBehaviour
{
    public NGWordSettings nGWordSettings;

    public void MoveToStartScene()
    {
        SceneManager.LoadSceneAsync("StartScene");
    }
}
