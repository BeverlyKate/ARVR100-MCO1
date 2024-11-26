using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using NUnit.Framework;
using UnityEngine;

public class PlayerDB
{
    private DatabaseReference _reference;

    public PlayerDB() {
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task UpdatePlayerWeeklyRepsInfo()
    {
        var accounts = await FetchAccounts();

        foreach (var account in accounts)
        {
            if (account.ProgressLastLoginDate == "")
            {
                continue;
            }

            DateTime lastUpdateDay = DateTime.Parse(account.StreakLastUpdateDate);
            DateTime lastUpdateWeekStart = lastUpdateDay.AddDays(-(int) lastUpdateDay.DayOfWeek);

            if (DateTime.Now >= lastUpdateWeekStart.AddDays(7))
            {
                account.DailyProgress = new() { 0, 0, 0, 0, 0, 0, 0 };
                string json = JsonUtility.ToJson(account);
                await _reference.Child("Accounts").Child(account.userId).SetRawJsonValueAsync(json);
            }
        }
    }

    public async Task UpdatePlayerStreakInfo()
    {
        var accounts = await FetchAccounts();
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        foreach (var account in accounts)
        {
            if (account.StreakLastUpdateDate == "")
            {
                continue;
            }

            DateTime lastUpdateDate = DateTime.Parse(account.StreakLastUpdateDate);
            DateTime currentDate = DateTime.Parse(today);

            if ((currentDate - lastUpdateDate).TotalDays >= 2)
            {
                account.Streak = 0;
                account.StreakLastUpdateDate = today;
                string json = JsonUtility.ToJson(account);
                await _reference.Child("Accounts").Child(account.userId).SetRawJsonValueAsync(json);
            }
        }
    }

    public async Task CreateAccount(string name)
    {
        string uuid = Guid.NewGuid().ToString();
        string json = JsonUtility.ToJson(new Account(uuid, name));
        await _reference.Child("Accounts").Child(uuid).SetRawJsonValueAsync(json);
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

    public async Task SetGym(Account account, Gym gym)
    {
        account.gymId = gym.gymId;
        string json = JsonUtility.ToJson(account);
        await _reference.Child("Accounts").Child(account.userId).SetRawJsonValueAsync(json);
        account.gymId = gym.gymId;

        CurrentAccount.Account = account;
    }

    public async Task UpdateProgressLastLoginDate(Account account)
    {
        account.ProgressLastLoginDate = DateTime.Now.ToString("yyyy-MM-dd");
        string json = JsonUtility.ToJson(account);
        await _reference.Child("Accounts").Child(account.userId).SetRawJsonValueAsync(json);

        CurrentAccount.Account = account;
    }

    public async Task UpdateStreak(Account account)
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        if (today == account.StreakLastUpdateDate)
        {
            return;
        }

        account.Streak += 1;
        account.StreakLastUpdateDate = today;
        string json = JsonUtility.ToJson(account);
        await _reference.Child("Accounts").Child(account.userId).SetRawJsonValueAsync(json);

        CurrentAccount.Account = account;
    }

    public async Task UpdateReps(Account account, int dReps)
    {
        int dayOfWeek = (int) DateTime.Now.DayOfWeek;
        account.DailyProgress[dayOfWeek] += dReps;
        account.TotalReps += dReps;
        string json = JsonUtility.ToJson(account);
        await _reference.Child("Accounts").Child(account.userId).SetRawJsonValueAsync(json);

        CurrentAccount.Account = account;
    }
}