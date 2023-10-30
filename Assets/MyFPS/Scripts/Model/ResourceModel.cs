using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class ResourceModel : MonoBehaviourPunCallbacks, IPunPrefabPool
{
    
    public static List<GameObject> avatars = new();
    public AssetLabelReference avatarLabel;

    public void Destroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        Debug.Log("instantiate call");
        foreach (var s in avatars)
        {
            if (s.name == prefabId)
            {
                var go = Instantiate(s, position, rotation);
                go.SetActive(false);
                Debug.Log("instantiate");
                return go;
            }
        }
        Debug.Log(prefabId + " がリストに含まれていないのでエラー");
        return null;
    }

    public void Start()
    {
        // Poolの生成イベントを書き換える
        PhotonNetwork.PrefabPool = this;
    }

    public async Task LoadAvatarModels()
    {
        if (avatars.Count != 0) return;
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(avatarLabel, null);
        // ロードが完了するまで待機
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // ロードが成功したらリストに追加
            avatars.AddRange(handle.Result);
            // ロードされたプレハブの数を表示
            Debug.Log("Loaded " + avatars.Count + " GameObjects");
        }
        else
        {
            Debug.LogError("Failed to load assets from group: " + avatarLabel);
        }

        // ロードハンドルをリリース
        //Addressables.Release(handle);
    }


}
