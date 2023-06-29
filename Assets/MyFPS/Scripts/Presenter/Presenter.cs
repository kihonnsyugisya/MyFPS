using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Presenter : MonoBehaviour
{
    public View view;
    public Model model;

    // Start is called before the first frame update
    void Start()
    {
        model.emoteModel.MakeEmoteButtonList(model.playerModel.animator);
        model.itemManager.hasHandWeapon.Subscribe(value => { if(value) model.playerModel.PlayHasGun(); }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
