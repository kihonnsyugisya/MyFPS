using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public Button goToLobyButton;
    public TextMeshProUGUI killerName;

    public void DispResultPanel(bool isShow)
    {
        gameObject.SetActive(isShow);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isShow);
        }
    }

    public void DispGoToLobyButton()
    {
        this.gameObject.SetActive(true);
        goToLobyButton.gameObject.SetActive(true);
    }

    
}

