using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class StartSceneModel : MonoBehaviour
{
    [HideInInspector] public PhotonManager photonManager;
    public NGWordSettings nGWordSettings;

    public void MoveToGameScene()
    {

        PhotonNetwork.LoadLevel("GameScene");
    }

    public void MoveToShopScene()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public int GetAvatarIndex()
    {
        for (int i = 0; i < ResourceModel.avatars.Count; i++)
        {
            if (ResourceModel.avatars[i].name == FireStoreModel.userDataCash.Avatar)
            {
                return i;
            }
        }
        return 0;
    }

}
