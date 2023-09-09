using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TitleSceneModel : MonoBehaviour
{
    public AuthModel authModel;

    public void SignIn()
    {
        authModel.Init();
    }
}
