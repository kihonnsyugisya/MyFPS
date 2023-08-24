using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using System.Threading.Tasks;

public class OparetionView : MonoBehaviour
{
    public Transform OparetionCanvas;
    public GameObject ResultPanel;
    public Button gunShootingButton;
    public Button jumpButton;
    public Button reLoadButton;
    public Button aimButton;
    public GunItemSlider gunItemSlider;
    public TextMeshProUGUI hpText;
    public MMProgressBar lifeGage;
    public MMFeedbacks damageFeedBack;

    public TextMeshProUGUI killerName;
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

    public void UndispOparationCanvas()
    {
        foreach (Transform item in OparetionCanvas)
        {
            item.gameObject.SetActive(false);
        }
    }

    public async void ShowResultView(CinemachineStateDrivenCamera stateDrivenCamera, int killerID)
    {
        PlayerView p = AvatarManager.playerList[killerID];
        ResultPanel.SetActive(true);
        this.killerName.text = p.name;
        await Task.Delay(4500);
        stateDrivenCamera.Follow = p.transform;
        stateDrivenCamera.LookAt = stateDrivenCamera.ChildCameras[1].LookAt = p.eye;
        stateDrivenCamera.m_AnimatedTarget = p.GetComponent<Animator>();        
    }

}
