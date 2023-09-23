using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class ResourceModel : MonoBehaviourPunCallbacks, IPunPrefabPool
{
    const string avatarGroupName = "AvatarRemoteGroup";
    public List<GameObject> avatars = new();

    public void Destroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        foreach (var s in avatars)
        {
            if (s.name == prefabId)
            {
                var go = Instantiate(s, position, rotation);
                go.SetActive(false);
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
        Debug.Log("callll");
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(avatarGroupName, null);
        // ロードが完了するまで待機
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // ロードが成功したらリストに追加
            avatars.AddRange(handle.Result);
            foreach (var avatar in avatars)
            {
                Debug.Log(avatar.name);
            }
            // ロードされたプレハブの数を表示
            Debug.Log("Loaded " + avatars.Count + " GameObjects");
        }
        else
        {
            Debug.LogError("Failed to load assets from group: " + avatarGroupName);
        }

        // ロードハンドルをリリース
        Addressables.Release(handle);
    }


}
