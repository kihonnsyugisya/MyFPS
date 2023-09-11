using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FireStoreModel : MonoBehaviour
{
    public static DocumentReference userRef;
    const string USER_COLLECTION = "users";

    public static void Init(in string UID)
    { 
        userRef = FirebaseFirestore.DefaultInstance.Collection(USER_COLLECTION).Document(UID);
    }

    public static async void AddInitialUserData(UserData initialData)
    {
        await userRef.SetAsync(initialData);
    }

    public static async void UpdateLastLogin()
    {
        await userRef.UpdateAsync("LastLogin", Timestamp.GetCurrentTimestamp());
    }
}
