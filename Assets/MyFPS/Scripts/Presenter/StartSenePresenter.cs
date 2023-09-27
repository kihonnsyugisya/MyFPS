using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class StartSenePresenter : MonoBehaviour
{
    public StartSceneModel model;
    public StartSceneVeiw view;
    // Start is called before the first frame update
    void Start()
    {
        view.SetNickName(FireStoreModel.userDataCash.NickName);

        model.photonManager.isConnectedRandomRoom.Subscribe(value => {
            if (value)
            {                
                view.SuccessRandomRoom();
                model.MoveToGameScene();
            }
        }).AddTo(this);

        view.randomMatchButton.OnClickAsObservable()
            .TakeUntilDestroy(this)
            .ThrottleFirst(TimeSpan.FromMilliseconds(2000))
            .Subscribe(_=> {
                view.ShowConnectionStatusMessage(true);
                model.photonManager.GoToRandomMatchRoom();
            })
            .AddTo(this);

        view.configButton.onClick.AddListener(()=> {
            view.bottomButtons.SetActive(false);
            view.configPanel.DispConfigPanel(true);
        });

        view.configPanel.backButton.onClick.AddListener(()=> {
            view.bottomButtons.SetActive(true);
            view.configPanel.DispConfigPanel(false);
        });

        view.configPanel.nickNameInputField.onEndEdit.AddListener(async nickName => {
            bool check = model.nGWordSettings.IsWordSafe(nickName);
            if (check)
            {
                view.nickNameText.text = nickName;
                await FireStoreModel.UpdateNickName(nickName);
                view.SetNickName(nickName);
            }
            else {
                view.configPanel.ShowNGNickNameArelt();
            }
        });
        
        model.photonManager.ConnectionMastarServer();


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
