using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseLoadData : MonoBehaviour
{
    public static FirebaseLoadData Instance { get; private set; }
    private FirebaseDatabase database => FirebaseInitialization.Instance.database;

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
    public void GetIntKey(Action<int> callback, string key)
    {
        if (!FirebaseInitialization.Instance.isInitialized)
        {
            callback?.Invoke(1);
            return;
        }

        database
            .GetReference(key)
            .GetValueAsync()
            .ContinueWithOnMainThread(t =>
            {
                if (t.IsCompleted && t.Result.Exists)
                {
                    if (int.TryParse(t.Result.Value.ToString(), out int parsed))
                    {
                        callback?.Invoke(parsed);
                    }
                    else
                    {
                        callback?.Invoke(1);
                    }
                }
                else
                {
                    callback?.Invoke(1);
                }
            });
    }
}
