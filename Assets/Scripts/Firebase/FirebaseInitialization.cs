using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInitialization : MonoBehaviour, IInitialize
{
    public static FirebaseInitialization Instance { get; private set; }
    public bool isInitialized { get; private set; } = false;
    public FirebaseDatabase database { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
    public Task InitializeAsync()
    {
        var tcs = new TaskCompletionSource<bool>();

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                var app = FirebaseApp.DefaultInstance;
                database = FirebaseDatabase.GetInstance(app, "https://testtasknovoplex-default-rtdb.firebaseio.com/");
                isInitialized = true;
                tcs.SetResult(true);
            }
          
        });

        return tcs.Task;
    }
}