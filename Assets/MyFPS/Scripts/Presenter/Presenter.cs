using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presenter : MonoBehaviour
{
    public View view;
    public Model model;

    // Start is called before the first frame update
    void Start()
    {
        model.emoteModel.MakeEmoteButtonList(model.playerModel.animator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
