using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to initialize fire base with" + task.Exception);
                return;
            }
        });
    }
}
