using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TitleScenePresenter : MonoBehaviour
{
    public TitleSceneModel model;
    public TitleSceneView view;

    // Start is called before the first frame update
    void Start()
    {
        view.termsOfService.scrollRect.verticalScrollbar.OnValueChangedAsObservable().Where(value => value < 0).First().Subscribe(value => {
            model.isPrivacyPolicyRead.Value = true;
        }).AddTo(this);

        model.isPrivacyPolicyRead.Where(value => value == true).Take(1).Subscribe(_=> {
            view.termsOfService.agree.interactable = true;
        }).AddTo(this);

        view.termsOfService.agree.onValueChanged.AddListener(value => {
            view.termsOfService.nextButton.interactable = value;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
