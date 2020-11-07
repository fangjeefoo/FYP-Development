using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;
using UnityEngine;

public class Setting : MonoBehaviour
{

    private FirebaseDatabase _database;

   
    void Start()
    {
        _database = FirebaseDatabase.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Push data to firebase
    /// </summary>
    public void PostData()
    {
        Customization customize = new Customization(1,"s", 2, 2.3f);

        _database.GetReference("Customization").SetRawJsonValueAsync(JsonUtility.ToJson(customize));


    }

    /// <summary>
    /// Retrieve data from firebase
    /// </summary>
    public async Task<Customization> RetrieveData()
    {
        var dataSnapshot = await _database.GetReference("Customization").GetValueAsync();
        if (!dataSnapshot.Exists)
        {
            return null;
        }
        return JsonUtility.FromJson<Customization>(dataSnapshot.GetRawJsonValue());
    }

    /// <summary>
    /// Check data is save properly in database
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Save()
    {
        var dataSnapshot = await _database.GetReference("Customization").GetValueAsync();
        return dataSnapshot.Exists;
    }

    public void OnDestroy()
    {
        _database = null;
    }
}
