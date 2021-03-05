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
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            if (task.Exception != null)
            {
                Debug.LogError("Failed to initialize firebase with" + task.Exception);
                return;
            }
        });
    }
}
