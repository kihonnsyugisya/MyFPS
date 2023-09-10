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
                AuthModel.UpdateNickName(value);
            }
            else {
                view.ShowRetryNickNameAlert();
            }
        });

        view.submitButton.OnClickAsObservable().TakeUntilDestroy(this).ThrottleFirst(System.TimeSpan.FromMilliseconds(2000)).Subscribe(_=> {

        });
    }
}
