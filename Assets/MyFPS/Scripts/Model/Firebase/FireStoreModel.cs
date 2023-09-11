using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FireStoreModel : MonoBehaviour
{
    public static FirebaseFirestore db;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    { 
        db = FirebaseFirestore.DefaultInstance;
    }
}
