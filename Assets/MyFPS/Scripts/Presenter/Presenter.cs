using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

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
            foreach (ItemInfoPlate currentDispItemPlate in model.avatarManager.itemManager.dispItemPlateList) if (currentDispItemPlate != null) currentDispItemPlate.gameObject.SetActive(!value);
        }).AddTo(this);
        model.avatarManager.playerModel.isGrounded.Subscribe(value => {
            view.oparetionView.jumpButton.interactable = value;
            model.avatarManager.playerModel.PlayJumpPose(value);
        }).AddTo(this);

        model.avatarManager.itemManager.gunModel.gunItemSlot.ObserveAdd().Subscribe(value => {
            GunItem g = model.avatarManager.itemManager.gunModel.gunitemHolder[value.Index];
            if (value.Index == 0) view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(value.Index, value.Value, g, model.avatarManager.itemManager.gunModel.bulletHolder[value.Value.bulletType]);
            else view.oparetionView.gunItemSlider.SetGunItemSlotView(value.Value, g, model.avatarManager.itemManager.gunModel.bulletHolder[value.Value.bulletType]);
        }).AddTo(this);
        model.avatarManager.itemManager.gunModel.gunItemSlot.ObserveReplace().Subscribe(value => {
            GunItem g = model.avatarManager.itemManager.gunModel.gunitemHolder[value.Index];
            view.oparetionView.gunItemSlider.ReplaceGunItemSlotView(value.Index, value.NewValue, g, model.avatarManager.itemManager.gunModel.bulletHolder[value.NewValue.bulletType]);
        }).AddTo(this);

        view.oparetionView.gunItemSlider.horizontalScrollSnap._page.SkipLatestValueOnSubscribe().Subscribe(value => {
            model.avatarManager.itemManager.gunModel.currentGunItemSlotIndex = value;
            model.avatarManager.itemManager.gunModel.SwitchWeapon();
            model.avatarManager.playerModel.PlaySwitchWeapon();
        }).AddTo(this);

        model.avatarManager.itemManager.gunModel.hasHandWeapon.Subscribe(value => {
            if (value) model.avatarManager.playerModel.PlayHasGun();
            foreach (var gunButton in view.oparetionView.gunButtons) gunButton.gameObject.SetActive(value);
            view.oparetionView.gunItemSlider.gameObject.SetActive(value);
        }).AddTo(this);

        model.avatarManager.damageModel.damageSubject.Subscribe(damage => {
            view.oparetionView.DecreaseHpGage(damage);
        }).AddTo(this);

        model.avatarManager.damageModel.hp.Subscribe(hp => {
            view.oparetionView.hpText.text = hp.ToString();
        }).AddTo(this);

        model.avatarManager.damageModel.isDead.Subscribe(value => {
            if (value)
            {
                model.avatarManager.itemManager.UnDispItemInfoPlate();
                model.avatarManager.playerModel.PlayDead();
                view.oparetionView.UndispOparationCanvas();
                int killerID = model.avatarManager.myAvatar.GetComponent<DamageModel>().killerID;
                view.oparetionView.ShowResultView(model.avatarManager.stateDrivenCamera, killerID);
            }

        }).AddTo(this);

        view.oparetionView.resultPanel.goToLobyButton.OnClickAsObservable().TakeUntilDestroy(this).ThrottleFirst(TimeSpan.FromMilliseconds(5000))
            .Subscribe(_=> {
                GameSystemModel.ResetField();
                StageItemManager.stageItemInfo.Clear();
                PhotonManager.GoToLoby();
            }).AddTo(this);

        GameSystemModel.playerList.ObserveAdd().Subscribe(player => { 
            view.oparetionView.DispRankingText(GameSystemModel.playerList.Count);
            if (PhotonManager.isRoomMaxPlayer())
            {
                GameSystemModel.PlaceAllPlayer(in model.avatarManager.nextSpawnPoints);
                GameSystemModel.isGameStart = true;
            }
        }).AddTo(this);

        GameSystemModel.playerList.ObserveRemove().Subscribe(player => {
            view.oparetionView.DispRankingText(GameSystemModel.playerList.Count);
            view.oparetionView.DispAnnounce(player.Value.name,player.Value.killerName,player.Value.killerID);
            if (player.Value.killerID == AvatarManager.myViewID)
            {              
                if (model.avatarManager.playerView.killedInfo.TryAdd(player.Key, player.Value.name))
                {
                    //Debug.Log("player.Value.killerID == AvatarManager.myViewID");
                    view.oparetionView.killCountText.text = model.avatarManager.playerView.killedInfo.Count.ToString();
                    view.oparetionView.DispKilledLog(player.Value.name);
                }
            }
            if (GameSystemModel.CheckVictory())
            {
                model.avatarManager.itemManager.UnDispItemInfoPlate();
                view.oparetionView.DispVictoryPanel(player.Value.killerName);
            }
        }).AddTo(this);


        //var keyStream = Observable.EveryUpdate().Select(_ => Input.anyKey).Where(xs => Input.anyKeyDown).Subscribe(_=> {
        //    foreach (var Ga in GameSystemModel.playerList)
        //    {
        //        Debug.Log(Ga.Value.name);
        //    }
        //    foreach (var mode in model.avatarManager.playerView.killedInfo)
        //    {
        //        Debug.Log(mode.Value + "var mode in model.avatarManager.playerView.killedInfo");
        //    }            
        //});
    }
}
