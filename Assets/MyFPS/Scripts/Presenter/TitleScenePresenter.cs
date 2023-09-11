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

        model.authModel.isLogin.Where(value => value == true).Subscribe(_=> {
            view.titleCanvas.AddScreenTapEvent().OnPointerClickAsObservable().TakeUntilDestroy(this).ThrottleFirst(System.TimeSpan.FromMilliseconds(3000)).Subscribe(_ => {
                if (model.authModel.isFirstLogin)
                {
                    view.termsOfService.scrollRect.verticalScrollbar.OnValueChangedAsObservable().Where(value => value < 0).First().Subscribe(value => {
                        view.termsOfService.agree.interactable = true;
                    }).AddTo(this);

                    view.termsOfService.agree.onValueChanged.AddListener(value => {
                        view.termsOfService.nextButton.interactable = value;
                    });

                    view.termsOfService.nextButton.OnClickAsObservable().TakeUntilDestroy(this).ThrottleFirst(System.TimeSpan.FromMilliseconds(3000)).Subscribe(_=> {
                        model.MoveToTutorialScene();
                    });

                    view.termsOfService.gameObject.SetActive(true);
                }
                else {
                    model.MoveToStartScene();
                }
            }).AddTo(this);
        }).AddTo(this);

        model.authModel.Init();




        view.authremobedebugbutton.onClick.AddListener(()=> model.authModel.Delete());

    }

}
