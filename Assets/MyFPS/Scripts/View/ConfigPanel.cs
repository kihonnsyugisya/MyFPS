using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConfigPanel : MonoBehaviour
{
    public GameObject configPanelObj;
    public Button backButton;
    public TMP_InputField nickNameInputField;
    public Slider volueSlider;
    public Toggle isVibration;
    public Button oparationSettingButton;
    public Button shareXButton;
    public Button reviewButton;

    public void DispConfigPanel(bool isShow)
    {
        configPanelObj.SetActive(isShow);
    }

}
