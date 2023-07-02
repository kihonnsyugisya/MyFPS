using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Presenter : MonoBehaviour
{
    public View view;
    public Model model;

    // Start is called before the first frame update
    void Start()
    {
        model.emoteModel.MakeEmoteButtonList(model.playerModel.animator);
        model.itemManager.hasHandWeapon.Subscribe(value => { if(value) model.playerModel.PlayHasGun(); }).AddTo(this);
        view.oparetionView.gunShootingButton.OnPointerDownAsObservable()
                .SelectMany(_ => view.oparetionView.gunShootingButton.UpdateAsObservable())
                .TakeUntil(view.oparetionView.gunShootingButton.OnPointerUpAsObservable())
                .DoOnCompleted(() =>
                {
                    Debug.Log("released!");
                })
                .RepeatUntilDestroy(view.oparetionView.gunShootingButton)
                .Subscribe(unit =>
                {
                    Debug.Log("pressing...");
                    model.playerModel.OnclickGunShoot(model.itemManager.gunItemSlot[model.itemManager.currentGunItemSlotIndex], model.itemManager.currentGunItem);
                });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
