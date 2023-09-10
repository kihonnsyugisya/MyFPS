using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutrialSceneView : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button submitButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowRetryNickNameAlert()
    {
        inputField.text = "";
        Debug.Log("nannbyouka tekisuto hyoujisuru");
    }
}
