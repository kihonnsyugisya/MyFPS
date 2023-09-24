using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class TitleSceneModel : MonoBehaviour
{
    public AuthModel authModel;
    public ResourceModel resourceModel;

    public async void MoveToStartScene()
    {
        Debug.Log("ログイン処理を行なってから画面遷移　lastlogin:leble,resource check");
        FireStoreModel.Init(AuthModel.auth.CurrentUser.UserId);
        await FireStoreModel.UpdateLastLogin();
        var ud = await FireStoreModel.GetUserDataAsync();        
        Debug.Log("リソースmodelからチェックメソッドを呼び出す");
        await resourceModel.LoadAvatarModels();
        SceneManager.LoadSceneAsync("StartScene");
    }

    public void MoveToTutorialScene()
    {
        SceneManager.LoadSceneAsync("TutrialScene");
    }

}
