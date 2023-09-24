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
        await userRef.UpdateAsync("LastLogin", Timestamp.GetCurrentTimestamp());
    }

    public static async Task UpdateResorceVersion(float newLevel)
    {
        await userRef.UpdateAsync("Level", newLevel);
    }
    public static async Task UpdateNickName(string newName)
    {
        await userRef.UpdateAsync("NickName", newName);
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
        }
        else
        {
            Debug.Log($"Document {snapshot.Id} does not exist!");
        }
        userDataCash = result;
        return result;
    }

}
