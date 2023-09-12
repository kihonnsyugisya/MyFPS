using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ConfigPanel : MonoBehaviour
{
    public GameObject configPanelObj;
    public Button backButton;
    public TMP_InputField nickNameInputField;
    public TextMeshProUGUI ngNickNameAreltText;
    public Slider volueSlider;
    public Toggle isVibration;
    public Button oparationSettingButton;
    public Button shareXButton;
    public Button reviewButton;

    public void DispConfigPanel(in bool isShow)
    {
        configPanelObj.SetActive(isShow);
    }

    public async void ShowNGNickNameArelt()
    {
        nickNameInputField.text = "";
        ngNickNameAreltText.gameObject.SetActive(true);
        await Task.Delay(3500);
        ngNickNameAreltText.gameObject.SetActive(false);
    }

}
