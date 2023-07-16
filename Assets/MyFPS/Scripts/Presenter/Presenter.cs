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
                .ThrottleFirst(System.TimeSpan.FromSeconds(0.1))
                .DoOnCompleted(() =>
                {
                    Debug.Log("released!");
                    model.playerModel.OnpointerUpGunShoot(model.itemManager.currentGunItem);
                })
                .RepeatUntilDestroy(view.oparetionView.gunShootingButton)
                .Subscribe(unit =>
                {
                    Debug.Log("pressing...");
                    model.playerModel.OnclickGunShoot(model.itemManager.gunItemSlot[model.itemManager.currentGunItemSlotIndex], model.itemManager.currentGunItem);
                });

        view.oparetionView.aimButton.OnClickAsObservable().Subscribe(_=> model.playerModel.PlayAiming()).AddTo(this);
        view.oparetionView.reLoadButton.OnClickAsObservable().Subscribe(_ => model.playerModel.ReloadGun()).AddTo(this);
        view.oparetionView.jumpButton.OnClickAsObservable().Subscribe(_ => model.playerModel.PlayJump()).AddTo(this);
        model.playerModel.isAiming.Subscribe(value => { model.playerModel.gameObject.layer = value ? 2 : 0; }).AddTo(this);

        model.itemManager.gunItemSlot.ObserveAdd().Subscribe(value => {
            if (value.Index == 0) view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(value.Index,value.Value, model.itemManager.currentGunItem, model.itemManager.bullets);
            else view.oparetionView.gunItemSlider.SetGunItemSlotView(value.Value, model.itemManager.currentGunItem, model.itemManager.bullets);
        }).AddTo(this);
        model.itemManager.gunItemSlot.ObserveReplace().Subscribe(value => {
            view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(value.Index, value.NewValue, model.itemManager.currentGunItem, model.itemManager.bullets);
        }).AddTo(this);
    }
    [SerializeField] bool set;
    // Update is called once per frame
    void Update()
    {
        //if (model.playerModel.isAiming)
        //{
        //    model.itemManager.currentGunItem.gunPoint.LookAt(model.playerModel.GetWorldPositionFromAimPoint());
        //    model.playerModel.OnclickGunShoot(model.itemManager.gunItemSlot[model.itemManager.currentGunItemSlotIndex], model.itemManager.currentGunItem);

        //}
    }
}
