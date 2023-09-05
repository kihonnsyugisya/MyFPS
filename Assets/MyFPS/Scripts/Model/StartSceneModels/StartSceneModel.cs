using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class StartSceneModel : MonoBehaviour
{
    [HideInInspector] public PhotonManager photonManager;

    public void MoveToGameScene()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
