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
                    model.playerModel.OnpointerUpGunShoot(model.itemManager.GetGunItem());
                    model.itemManager.CheckCanReload();
                })
                .RepeatUntilDestroy(view.oparetionView.gunShootingButton)
                .Subscribe(unit =>
                {
                    Debug.Log("pressing...");
                    GunItemData gid = model.itemManager.GetGunItemData();
                    GunItem gi = model.itemManager.GetGunItem();
                    model.playerModel.OnclickGunShoot(gid,gi);       
                });
        model.itemManager.canReload.Subscribe(value => view.oparetionView.reLoadButton.interactable = value).AddTo(this);

        view.oparetionView.aimButton.OnClickAsObservable().Subscribe(_=> model.playerModel.PlayAiming()).AddTo(this);
        view.oparetionView.reLoadButton.OnClickAsObservable().Subscribe(_ => {
            model.playerModel.ReloadGun();
            model.itemManager.ReloadGun();
            model.itemManager.canReload.Value = false;
        }).AddTo(this);
        view.oparetionView.jumpButton.OnClickAsObservable().Subscribe(_ => model.playerModel.PlayJump()).AddTo(this);
        model.playerModel.isAiming.Subscribe(value => { model.playerModel.gameObject.layer = value ? 2 : 0; }).AddTo(this);

        model.itemManager.gunItemSlot.ObserveAdd().Subscribe(value => {
            GunItem g = model.itemManager.gunitemHolder[value.Index];
            if (value.Index == 0) view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(value.Index,value.Value, g, model.itemManager.bulletHolder[value.Value.bulletType]);
            else view.oparetionView.gunItemSlider.SetGunItemSlotView(value.Value, g, model.itemManager.bulletHolder[value.Value.bulletType]);
        }).AddTo(this);
        model.itemManager.gunItemSlot.ObserveReplace().Subscribe(value => {
            GunItem g = model.itemManager.gunitemHolder[value.Index];
            view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(value.Index, value.NewValue, g, model.itemManager.bulletHolder[value.NewValue.bulletType]);
        }).AddTo(this);

        view.oparetionView.gunItemSlider.horizontalScrollSnap._page.SkipLatestValueOnSubscribe().Subscribe(value => {
            model.itemManager.currentGunItemSlotIndex = value;
            model.playerModel.PlaySwitchWeapon();
        }).AddTo(this);

        model.itemManager.hasHandWeapon.Subscribe(value => {
            if (value) model.playerModel.PlayHasGun();
            foreach (var gunButton in view.oparetionView.gunButtons) gunButton.gameObject.SetActive(value);
            view.oparetionView.gunItemSlider.gameObject.SetActive(value);
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
