using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;


public class VictoryPanel : MonoBehaviour
{
    public MMFeedbacks victoryFeedbacks;
    public TextMeshProUGUI winner;

    public void DispVictoryPanel(in string winnerName)
    {
        victoryFeedbacks.gameObject.SetActive(true);
        winner.text = winnerName;
        victoryFeedbacks.PlayFeedbacks();
    }
}
