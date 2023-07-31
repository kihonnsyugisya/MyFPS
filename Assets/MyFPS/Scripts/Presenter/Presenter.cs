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
        model.emoteModel.MakeEmoteButtonList(model.avatarManager.playerModel.animator);
        view.oparetionView.gunShootingButton.OnPointerDownAsObservable()
                .SelectMany(_ => view.oparetionView.gunShootingButton.UpdateAsObservable())
                .TakeUntil(view.oparetionView.gunShootingButton.OnPointerUpAsObservable())
                .ThrottleFirst(System.TimeSpan.FromSeconds(0.1))
                .DoOnCompleted(() =>
                {
                    Debug.Log("released!");
                    model.avatarManager.playerModel.PlayGunHipFire(false);
                    model.avatarManager.itemManager.gunModel.OnpointerUpGunShoot();
                    model.avatarManager.itemManager.gunModel.CheckCanReload();
                })
                .RepeatUntilDestroy(view.oparetionView.gunShootingButton)
                .Subscribe(unit =>
                {
                    Debug.Log("pressing...");
                    model.avatarManager.playerModel.PlayGunHipFire(true);
                    model.avatarManager.itemManager.gunModel.OnclickGunShoot();       
                });
        model.avatarManager.itemManager.gunModel.canReload.Subscribe(value => view.oparetionView.reLoadButton.interactable = value).AddTo(this);

        view.oparetionView.aimButton.OnClickAsObservable().Subscribe(_=> model.avatarManager.playerModel.PlayAiming()).AddTo(this);
        view.oparetionView.reLoadButton.OnClickAsObservable().Subscribe(_ => {
            model.avatarManager.playerModel.ReloadGun();
            model.avatarManager.itemManager.gunModel.ReloadGun();
            model.avatarManager.itemManager.gunModel.canReload.Value = false;
        }).AddTo(this);
        view.oparetionView.jumpButton.OnClickAsObservable().Subscribe(_ => model.avatarManager.playerModel.PlayJump()).AddTo(this);
        model.avatarManager.playerModel.isAiming.Subscribe(value => {
            model.avatarManager.playerModel.gameObject.layer = value ? 2 : 0;
            foreach (ItemInfoPlate currentDispItemPlate in model.avatarManager.itemManager.dispItemPlateList) if(currentDispItemPlate != null) currentDispItemPlate.gameObject.SetActive(!value);
        }).AddTo(this);

        model.avatarManager.itemManager.gunModel.gunItemDataSlot.ObserveAdd().Subscribe(value => {
            GunItem g = model.avatarManager.itemManager.gunModel.gunItemSlot[value.Key];
            if (value.Key == WeaponPoint.Hand) view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(0,value.Value, g, model.avatarManager.itemManager.gunModel.bulletHolder[value.Value.bulletType]);
            else view.oparetionView.gunItemSlider.SetGunItemSlotView(value.Value, g, model.avatarManager.itemManager.gunModel.bulletHolder[value.Value.bulletType]);
        }).AddTo(this);
        model.avatarManager.itemManager.gunModel.gunItemDataSlot.ObserveReplace().Subscribe(value => {
            GunItem g = model.avatarManager.itemManager.gunModel.gunItemSlot[value.Key];
            view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(view.oparetionView.gunItemSlider.horizontalScrollSnap._page.Value, value.NewValue, g, model.avatarManager.itemManager.gunModel.bulletHolder[value.NewValue.bulletType]);
        }).AddTo(this);

        view.oparetionView.gunItemSlider.horizontalScrollSnap._page.SkipLatestValueOnSubscribe().Subscribe(value => {
            model.avatarManager.itemManager.gunModel.SwitchWeapon();
            model.avatarManager.playerModel.PlaySwitchWeapon();
        }).AddTo(this);

        model.avatarManager.itemManager.gunModel.hasHandWeapon.Subscribe(value => {
            if (value) model.avatarManager.playerModel.PlayHasGun();
            foreach (var gunButton in view.oparetionView.gunButtons) gunButton.gameObject.SetActive(value);
            view.oparetionView.gunItemSlider.gameObject.SetActive(value);
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        //if (model.avatarManager.playerModel.isAiming)
        //{
        //    model.avatarManager.itemManager.currentGunItem.gunPoint.LookAt(model.avatarManager.playerModel.GetWorldPositionFromAimPoint());
        //    model.avatarManager.playerModel.OnclickGunShoot(model.avatarManager.itemManager.gunItemSlot[model.avatarManager.itemManager.currentGunItemSlotIndex], model.avatarManager.itemManager.currentGunItem);

        //}
    }
}
