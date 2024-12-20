using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class GymDB
{
    private DatabaseReference _reference;
    public GymDB()
    {
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task AddGym(string name)
    {
        string uuid = Guid.NewGuid().ToString();
        string json = JsonUtility.ToJson(new Gym(uuid, name));
        await _reference.Child("Gyms").Child(uuid).SetRawJsonValueAsync(json);
    }

    public async void DeleteGym(Gym gym)
    {
        await _reference.Child("Gyms").Child(gym.gymId).RemoveValueAsync();
    }

    public async Task<List<Gym>> FetchGyms()
    {
        List<Gym> gyms = new List<Gym>();
        try
        {
            DataSnapshot snapshot = await _reference.Child("Gyms").GetValueAsync();
            if (snapshot.Exists)
            {
                foreach (var snapshotChild in snapshot.Children)
                {
                    Gym gym = JsonUtility.FromJson<Gym>(snapshotChild.GetRawJsonValue());
                    gyms.Add(gym);
                }
            }
            else
            {
                Debug.LogWarning("Gym is null");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return gyms;
    }
}