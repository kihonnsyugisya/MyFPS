using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartSceneVeiw : MonoBehaviour
{
    public Button randomMatchButton;
    public TextMeshProUGUI connetionStatusMessage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowConnectionStatusMessage(bool isShow)
    {
        connetionStatusMessage.gameObject.SetActive(isShow);
    }

    public void SuccessMaster()
    {
        connetionStatusMessage.text = "connect room...";
    }

    public void SuccessRandomRoom()
    {
        connetionStatusMessage.text = "joind random room!";
    }
}
