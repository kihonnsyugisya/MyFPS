using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteModel : MonoBehaviour
{

    public Transform scrollContent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakeEmoteButtonList(Animator animator)
    {
        var clips = animator.runtimeAnimatorController.animationClips;
        GameObject emoteButtonObj = (GameObject)Resources.Load("emoteButton");

        foreach (AnimationClip clip in clips)
        {
            GameObject i = Instantiate(emoteButtonObj, scrollContent);
            EmoteButton emoteButton = i.GetComponent<EmoteButton>();
            emoteButton.emoteName.text = clip.name;
            emoteButton.emoteButton.onClick.AddListener(()=> OnclickEmoteButon(animator,clip.name));
        }

    }

    private void OnclickEmoteButon(Animator animator, string emoteName)
    {
        Debug.Log(emoteName);
        //animator.CrossFadeInFixedTime(emoteName, 0);
        animator.SetLayerWeight(5,1f);
        animator.Play(emoteName);
        Debug.Log("エモートレイヤーのwight入ったままになってるから、終了時のコールバックで０にすること");
    }


}
