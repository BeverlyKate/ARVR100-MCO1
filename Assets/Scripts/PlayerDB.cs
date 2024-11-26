using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class PlayerDB
{
    private DatabaseReference _reference;
    public PlayerDB() {
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    
    public async void CreateAccount(string name)
    {
        string json = JsonUtility.ToJson(new Account(SystemInfo.deviceUniqueIdentifier, name));
        await _reference.Child("Accounts").Child(SystemInfo.deviceUniqueIdentifier).SetRawJsonValueAsync(json);
    }

    public async void DeleteAccount(Account account)
    {
        await _reference.Child("Accounts").Child(account.userId).RemoveValueAsync();
    }

    public async Task<List<Account>> FetchAccounts()
    {
        List<Account> accounts = new List<Account>();
        try
        {
            DataSnapshot snapshot = await _reference.Child("Accounts").GetValueAsync();
            if (snapshot.Exists)
            {
                foreach (var snapshotChild in snapshot.Children)
                {
                    Account account = JsonUtility.FromJson<Account>(snapshotChild.GetRawJsonValue());
                    accounts.Add(account);
                }
            }
            else
            {
                Debug.LogWarning("User is null");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return accounts;
    }

    public async void SetGym(Account account, Gym gym)
    {
        account.gymId = gym.gymId;
        string json = JsonUtility.ToJson(account);
        await _reference.Child("Accounts").Child(account.userId).SetRawJsonValueAsync(json);
    }
}