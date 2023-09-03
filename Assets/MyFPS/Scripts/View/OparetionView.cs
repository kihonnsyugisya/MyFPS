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
    public ResultPanel resultPanel;
    public Button gunShootingButton;
    public Button jumpButton;
    public Button reLoadButton;
    public Button aimButton;
    public GunItemSlider gunItemSlider;
    public TextMeshProUGUI hpText;
    public MMProgressBar lifeGage;
    public MMFeedbacks damageFeedBack;

    
    public TextMeshProUGUI rankingText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI announceText;
    public TextMeshProUGUI killedName;
    public VictoryPanel victoryPanel;

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
        resultPanel.DispResultPanel(true);
        resultPanel.killerName.text = p.name;
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
    public async void DispAnnounce(string leftPlayer,string killerName,int killerID)
    {
        announceText.gameObject.SetActive(true);
        string announceMessage;
        if (killerID == 0) announceMessage = leftPlayer + " lefted";
        else announceMessage = killerName + " が " + leftPlayer + "を殺した";
        announceText.text = announceMessage;
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
        resultPanel.killerName.text = "";
    }

    public async void DispVictoryPanel(string winnerName)
    {
        resultPanel.DispResultPanel(false);
        UndispOparationCanvas();
        victoryPanel.DispVictoryPanel(in winnerName);
        await Task.Delay(2000);
        resultPanel.DispGoToLobyButton();
    }
}
