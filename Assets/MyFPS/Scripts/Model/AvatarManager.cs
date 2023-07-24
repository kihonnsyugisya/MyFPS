using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarManager : MonoBehaviourPunCallbacks
{
    public List<Transform> spawnPoint = new();
    [HideInInspector] public GameObject myAvatar;
    // Start is called before the first frame update
    void Start()
    {
        myAvatar = PhotonNetwork.Instantiate("3RD Person",spawnPoint[Random.Range(0,spawnPoint.Count)].position,Quaternion.identity);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
