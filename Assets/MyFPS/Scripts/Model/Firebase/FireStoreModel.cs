using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;

public class FireStoreModel
{
    public static DocumentReference userRef;
    public static UserData userDataCash;

    const string USER_COLLECTION = "users";

    public static void Init(in string UID)
    { 
        userRef = FirebaseFirestore.DefaultInstance.Collection(USER_COLLECTION).Document(UID);
    }

    public static async Task AddInitialUserData(UserData initialData)
    {
        await userRef.SetAsync(initialData);
    }

    public static async Task UpdateLastLogin()
    {
        var now = Timestamp.GetCurrentTimestamp();
        await userRef.UpdateAsync("LastLogin", now);
        //userDataCash.LastLogin = now;
    }

    public static async Task UpdateAvatar(string newName)
    {
        await userRef.UpdateAsync("Avatar", newName);
        userDataCash.Avatar = newName;
    }

    public static async Task UpdateNickName(string newName)
    {
        await userRef.UpdateAsync("NickName", newName);
        userDataCash.NickName = newName;
    }

    public static async Task IncrementKillCount(int killCount)
    {
        int current = userDataCash.KillCount + killCount;
        Dictionary<string, object> data = new()
        {
            { "KillCount",  current }
        };
        await userRef.UpdateAsync(data);
        userDataCash.KillCount = current;
        Debug.Log("killcount cash: " + userDataCash.KillCount + "に更新");
        Debug.Log("killcount store" + current + "に更新");
    }

    public static async Task IncrementDeathCount()
    {
        int current = userDataCash.DeathCount;
        Dictionary<string, object> data = new()
        {
            { "DeathCount", current++ }
        };
        await userRef.UpdateAsync(data);
        userDataCash.DeathCount++;
    }

    public static async Task IncrementVictoryCount()
    {
        int current = userDataCash.VictoryCount;
        Dictionary<string, object> data = new()
        {
            { "VictoryCount", current++ }
        };
        await userRef.UpdateAsync(data);
        userDataCash.VictoryCount++;
    }

    public static async Task IncrementGameMatchCount()
    {
        int current = userDataCash.GameMathcCount;
        Dictionary<string, object> data = new()
        {
            { "GameMatchCount", current++ }
        };
        await userRef.UpdateAsync(data);
        userDataCash.GameMathcCount++;
    }

    public static async Task<UserData> GetUserDataAsync()
    {
        var snapshot = await userRef.GetSnapshotAsync();
        UserData result = new(); // デフォルト値をセットしておく
        if (snapshot.Exists)
        {
            result = snapshot.ConvertTo<UserData>();
            Debug.Log($"Document data for {snapshot.Id} document:");
            Debug.Log($"Name: {result.NickName}");
            Debug.Log($"Level: {result.Level}");
            Debug.Log($"CreatedData: {result.CreatedDate}");
            Debug.Log($"LastLogin: {result.LastLogin}");
            Debug.Log($"ResorceVersion: {result.ResorceVersion}");
            Debug.Log($"Avatar: {result.Avatar }");
            Debug.Log($"KillCount: {result.KillCount }");
            Debug.Log($"DeathCount: {result.DeathCount }");
            Debug.Log($"GameMatchCount: {result.GameMathcCount }");
            Debug.Log($"Victory: {result.VictoryCount }");
        }
        else
        {
            Debug.Log($"Document {snapshot.Id} does not exist!");
        }
        userDataCash = result;
        return result;
    }

}
