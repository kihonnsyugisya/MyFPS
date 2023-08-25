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
    public TextMeshProUGUI rankingText;
    public TextMeshProUGUI killCountText;
    public Button goToLobyButton;
    public TextMeshProUGUI announceText;
    public TextMeshProUGUI killedName;

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
        PlayerView p = GameSystemModel.playerList[killerID];
        ResultPanel.SetActive(true);
        this.killerName.text = p.name;
        await Task.Delay(4500);
        stateDrivenCamera.Follow = p.transform;
        stateDrivenCamera.LookAt = p.eye;
        stateDrivenCamera.ChildCameras[1].LookAt = p.Aim;
        stateDrivenCamera.m_AnimatedTarget = p.GetComponent<Animator>();
    }

    public void DispRankingText(int rest)
    { 
        rankingText.text = rest.ToString();
    }

    //asyncをなくしたら、引数にinをつけてくれ
    public async void DispKillAnnounce(string victim,string killerName)
    {
        announceText.gameObject.SetActive(true);
        announceText.text = killerName + " が " + victim + "を殺した";
        await Task.Delay(4000);
        announceText.gameObject.SetActive(false);
    }

    //asyncをなくしたら、引数にinをつけてくれ
    public async void DispKilledLog(string killed)
    {
        var killLogPanel = killedName.transform.parent.gameObject;
        killLogPanel.SetActive(true);
        killedName.text = killed;
        await Task.Delay(4000);
        killLogPanel.SetActive(false);
        killerName.text = "";
    }

}
