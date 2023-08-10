using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OparetionView : MonoBehaviour
{
    public Button gunShootingButton;
    public Button jumpButton;
    public Button reLoadButton;
    public Button aimButton;
    public GunItemSlider gunItemSlider;
    [HideInInspector] public List<Button> gunButtons;

    private void Awake()
    {
        gunButtons = new()
        {
            gunShootingButton,reLoadButton,aimButton
        };
    }
}
