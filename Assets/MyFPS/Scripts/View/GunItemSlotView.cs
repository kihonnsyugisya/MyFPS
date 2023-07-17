using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunItemSlotView : MonoBehaviour
{
    [HideInInspector] public Button button;
    public TextMeshProUGUI bulletSize;
    public TextMeshProUGUI magazineSize;
 
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
