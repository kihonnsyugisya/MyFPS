using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TutrialScenePresenter : MonoBehaviour
{
    public TutrialSceneModel model;
    public TutrialSceneView view;
    // Start is called before the first frame update
    void Start()
    {
        view.inputField.onEndEdit.AddListener(value => {
            bool check = model.nGWordSettings.IsWordSafe(value);
            if (check)
            {
                view.submitButton.interactable = true;
                view.submitButton.OnClickAsObservable().TakeUntilDestroy(this).ThrottleFirst(System.TimeSpan.FromMilliseconds(2000)).Subscribe(_ => {
                    AuthModel.UpdateNickName(value);
                    model.MoveToStartScene();
                });
            }
            else {
                view.submitButton.interactable = false;
                view.ShowRetryNickNameAlert();
            }
        });


    }
}
