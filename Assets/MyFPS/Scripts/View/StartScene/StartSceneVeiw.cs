using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class StartSceneVeiw : MonoBehaviour
{
    public Button randomMatchButton;
    public TextMeshProUGUI connetionStatusMessage;

    public TextMeshProUGUI nickNameText;

    public ConfigPanel configPanel;
    public Button configButton;

    public GameObject bottomButtons;

    public Button shopButton;

    public Transform avatarStage;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowConnectionStatusMessage(bool isShow)
    {
        connetionStatusMessage.gameObject.SetActive(isShow);
    }

    public void SuccessRandomRoom()
    {
        connetionStatusMessage.text = "joind random room!";
    }

    public void SetNickName(in string nickName)
    {
        nickNameText.text = nickName;
        configPanel.nickNameInputField.text = nickName;
        PhotonNetwork.NickName = nickName;
    }

}
