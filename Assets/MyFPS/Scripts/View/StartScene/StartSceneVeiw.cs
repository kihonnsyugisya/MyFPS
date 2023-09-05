using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartSceneVeiw : MonoBehaviour
{
    public Button randomMatchButton;
    public TextMeshProUGUI connetionStatusMessage;

    public ConfigPanel configPanel;
    public Button configButton;

    public GameObject bottomButtons;

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

}
