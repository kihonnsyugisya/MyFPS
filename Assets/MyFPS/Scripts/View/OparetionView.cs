using MoreMountains.Feedbacks;
using MoreMountains.Tools;
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
    public MMProgressBar lifeGage;
    public MMFeedbacks damageFeedBack;
    [HideInInspector] public List<Button> gunButtons;

    private void Awake()
    {
        gunButtons = new()
        {
            gunShootingButton,reLoadButton,aimButton
        };
    }

    public void DecreaseHpGage(int damage)
    {
        int timer = damage / 10;
        for (int i = 0; i <= timer; i++)
        {
            lifeGage.Minus10Percent();
        }
        damageFeedBack.PlayFeedbacks();
    }

}
