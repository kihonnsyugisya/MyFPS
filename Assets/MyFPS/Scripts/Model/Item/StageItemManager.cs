using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StageItemManager : MonoBehaviourPunCallbacks
{
    public List<Transform> itemPoints = new();
    public List<Item> stageItemList = new();
    public static Dictionary<int, Item> stageItemInfo = new();

    private List<int> randomInts = new();
    private bool receivedRandomInts = false; // ランダムな順序を受信したかを判定するフラグ

    void Start()
    {
        // マスタークライアントのみがランダムな順序を生成
        if (PhotonNetwork.IsMasterClient)
        {
            randomInts = GetShuffleList(itemPoints.Count);
            Debug.Log("マスタークライアント: ランダムな順序生成完了");
            photonView.RPC(nameof(ShareRandomInts), RpcTarget.OthersBuffered, randomInts.ToArray());
            PlaceItems(); // マスタークライアントの場合は待たずに配置
        }
        else
        {
            // クライアントの場合、まだランダムな順序が受信されていない状態にする
            receivedRandomInts = false;
            // 非同期処理を開始
            StartCoroutine(PlaceItemsAsync());
        }
    }

    [PunRPC]
    private void ShareRandomInts(int[] d)
    {
        if (receivedRandomInts) return;
        randomInts = new List<int>(d);
        Debug.Log("クライアント: ランダムな順序受信完了");
        receivedRandomInts = true; // ランダムな順序を受信したフラグを立てる
    }

    // アイテムの配置を行うメソッド
    private void PlaceItems()
    {
        Debug.Log("ランダムな順序を元にアイテムを配置開始");
        for (int i = 0; i < itemPoints.Count; i++)
        {
            if (randomInts.Count > i)
            {
                stageItemList[i].transform.position = itemPoints[randomInts[i]].position;
                stageItemInfo.Add(i, stageItemList[i]);
                stageItemList[i].stageId = i;
                Debug.Log(stageItemList[i].gameObject.name + " を" + i + " に登録");
            }
            else
            {
                Debug.LogError("ランダムな順序のリストがアイテムの数よりも小さいです。");
            }
        }
        Debug.Log("アイテムの配置完了");
    }

    // 非同期処理を行うメソッド
    private IEnumerator PlaceItemsAsync()
    {
        // ランダムな順序を受信するまで待機
        while (!receivedRandomInts)
        {
            Debug.Log("アイテムを配置中...(プログレスバーいる？)");
            yield return null;
        }

        // ランダムな順序を受信したら、アイテムの配置を行う
        PlaceItems();
    }

    private List<int> GetShuffleList(int itemCount)
    {
        List<int> randomList = new List<int>();
        for (int i = 0; i < itemCount; i++)
        {
            randomList.Add(i);
        }
        return Shuffle(randomList);
    }

    private List<int> Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log(newMasterClient.UserId + " がマスターに任命(stageItemManagerから実行)");
        if(PhotonNetwork.IsMasterClient) photonView.RPC(nameof(ShareRandomInts), RpcTarget.OthersBuffered, randomInts.ToArray());

    }

    public static void RemoveItem(int stageID)
    {
        stageItemInfo[stageID].gameObject.SetActive(false);
    }

}
