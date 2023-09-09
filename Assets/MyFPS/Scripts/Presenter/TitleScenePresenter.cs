using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class TitleScenePresenter : MonoBehaviour
{
    public TitleSceneModel model;
    public TitleSceneView view;

    // Start is called before the first frame update
    void Start()
    {

        view.titleCanvas.AddScreenTapEvent().OnPointerClickAsObservable().Subscribe(_=> {
            model.SignIn();
        }).AddTo(this);

        view.termsOfService.scrollRect.verticalScrollbar.OnValueChangedAsObservable().Where(value => value < 0).First().Subscribe(value => {
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
