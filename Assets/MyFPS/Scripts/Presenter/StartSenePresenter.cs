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
        startSceneModel.photonManager.isConnectedMaster.Subscribe(_=> startSceneVeiw.SuccessMaster()).AddTo(this);
        startSceneModel.photonManager.isConnectedLobby.Subscribe(_=> startSceneVeiw.SuccessLobby()).AddTo(this);

        startSceneVeiw.randomMatchButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            .ThrottleFirst(TimeSpan.FromMilliseconds(5000))
            .Subscribe(_=> {
                startSceneVeiw.ShowConnectionStatusMessage(true);
                startSceneModel.photonManager.ConnectionMastarServer();
            })
            .AddTo(this);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
