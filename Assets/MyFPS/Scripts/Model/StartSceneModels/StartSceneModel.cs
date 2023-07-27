using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartSceneModel : MonoBehaviour
{
    [HideInInspector] public PhotonManager photonManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToGameScene()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
