using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScenePersenter : MonoBehaviour
{
    public ShopSceneModel model;
    public ShopSceneView view;
    // Start is called before the first frame update
    void Start()
    {
        view.backButton.onClick.AddListener(async ()=> {
            await model.MoveToStartSceneAsync();
        });
        StartCoroutine(model.TakePictures(ResourceModel.avatars));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
