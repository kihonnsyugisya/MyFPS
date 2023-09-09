using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

public class TitleCanvas : MonoBehaviour
{
    public GameObject titleScreen;

    public ObservableEventTrigger AddScreenTapEvent()
    {
        return titleScreen.AddComponent<ObservableEventTrigger>();
    }
}
