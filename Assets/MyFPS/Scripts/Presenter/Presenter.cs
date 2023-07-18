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
        view.oparetionView.gunShootingButton.OnPointerDownAsObservable()
                .SelectMany(_ => view.oparetionView.gunShootingButton.UpdateAsObservable())
                .TakeUntil(view.oparetionView.gunShootingButton.OnPointerUpAsObservable())
                .ThrottleFirst(System.TimeSpan.FromSeconds(0.1))
                .DoOnCompleted(() =>
                {
                    Debug.Log("released!");
                    model.itemManager.gunModel.OnpointerUpGunShoot();
                    model.itemManager.gunModel.CheckCanReload();
                })
                .RepeatUntilDestroy(view.oparetionView.gunShootingButton)
                .Subscribe(unit =>
                {
                    Debug.Log("pressing...");
                    model.itemManager.gunModel.OnclickGunShoot();       
                });
        model.itemManager.gunModel.canReload.Subscribe(value => view.oparetionView.reLoadButton.interactable = value).AddTo(this);

        view.oparetionView.aimButton.OnClickAsObservable().Subscribe(_=> model.playerModel.PlayAiming()).AddTo(this);
        view.oparetionView.reLoadButton.OnClickAsObservable().Subscribe(_ => {
            model.playerModel.ReloadGun();
            model.itemManager.gunModel.ReloadGun();
            model.itemManager.gunModel.canReload.Value = false;
        }).AddTo(this);
        view.oparetionView.jumpButton.OnClickAsObservable().Subscribe(_ => model.playerModel.PlayJump()).AddTo(this);
        model.playerModel.isAiming.Subscribe(value => {
            model.playerModel.gameObject.layer = value ? 2 : 0;
            //foreach (GameObject itemPlate in model.itemManager.dispItemPlates) itemPlate.SetActive(value);
            Debug.Log("???");
            if(value) model.itemManager.UnDispItemInfoPlate();
        }).AddTo(this);

        model.itemManager.gunModel.gunItemSlot.ObserveAdd().Subscribe(value => {
            GunItem g = model.itemManager.gunModel.gunitemHolder[value.Index];
            if (value.Index == 0) view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(value.Index,value.Value, g, model.itemManager.gunModel.bulletHolder[value.Value.bulletType]);
            else view.oparetionView.gunItemSlider.SetGunItemSlotView(value.Value, g, model.itemManager.gunModel.bulletHolder[value.Value.bulletType]);
        }).AddTo(this);
        model.itemManager.gunModel.gunItemSlot.ObserveReplace().Subscribe(value => {
            GunItem g = model.itemManager.gunModel.gunitemHolder[value.Index];
            view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(value.Index, value.NewValue, g, model.itemManager.gunModel.bulletHolder[value.NewValue.bulletType]);
        }).AddTo(this);

        view.oparetionView.gunItemSlider.horizontalScrollSnap._page.SkipLatestValueOnSubscribe().Subscribe(value => {
            model.itemManager.gunModel.currentGunItemSlotIndex = value;
            model.playerModel.PlaySwitchWeapon();
        }).AddTo(this);

        model.itemManager.gunModel.hasHandWeapon.Subscribe(value => {
            if (value) model.playerModel.PlayHasGun();
            foreach (var gunButton in view.oparetionView.gunButtons) gunButton.gameObject.SetActive(value);
            view.oparetionView.gunItemSlider.gameObject.SetActive(value);
        }).AddTo(this);
    }

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
