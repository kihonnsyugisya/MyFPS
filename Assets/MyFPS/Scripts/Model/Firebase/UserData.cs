using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class UserData 
{
    [FirestoreProperty] public string NickName { get; set; }
    [FirestoreProperty] public float ResorceVersion { get; set; }
    [FirestoreProperty] public Timestamp CreatedDate { get; set; }
    [FirestoreProperty] public Timestamp LastLogin { get; set; }
    [FirestoreProperty] public float Level { get; set; }
    [FirestoreProperty] public string Avatar { get; set; }
}
