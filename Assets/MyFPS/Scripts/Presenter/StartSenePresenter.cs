using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class StartSenePresenter : MonoBehaviour
{
    public StartSceneModel startSceneModel;
    public StartSceneVeiw startSceneVeiw;
    // Start is called before the first frame update
    void Start()
    {
        startSceneModel.photonManager.isConnectedRandomRoom.Subscribe(value => {
            if (value)
            {                
                startSceneVeiw.SuccessRandomRoom();
                startSceneModel.MoveToGameScene();
            }
        }).AddTo(this);

        startSceneVeiw.randomMatchButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            .ThrottleFirst(TimeSpan.FromMilliseconds(2000))
            .Subscribe(_=> {
                startSceneVeiw.ShowConnectionStatusMessage(true);
                startSceneModel.photonManager.GoToRandomMatchRoom();
            })
            .AddTo(this);

        startSceneVeiw.configButton.onClick.AddListener(()=> {
            startSceneVeiw.bottomButtons.SetActive(false);
            startSceneVeiw.configPanel.DispConfigPanel(true);
        });

        startSceneVeiw.configPanel.backButton.onClick.AddListener(()=> {
            startSceneVeiw.bottomButtons.SetActive(true);
            startSceneVeiw.configPanel.DispConfigPanel(false);
        });
        
        startSceneModel.photonManager.ConnectionMastarServer();


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
