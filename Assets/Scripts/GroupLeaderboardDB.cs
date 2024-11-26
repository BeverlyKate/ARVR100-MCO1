using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class LeaderboardRow
{
    public string GymName;
    public int GroupScore;

    public LeaderboardRow(string gymName, int groupScore)
    {
        GymName = gymName;
        GroupScore = groupScore;
    }
}

public class GroupLeaderboardDB
{
    private DatabaseReference _reference;
    public GroupLeaderboardDB()
    {
        // TODO: Initialize database connection
        // It may be better to just wrap around PlayerDB and do the grouping here
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task<List<LeaderboardRow>> FetchLeaderboardRows()
    {
        Dictionary<string, LeaderboardRow> leaderboardRows = new Dictionary<string, LeaderboardRow>();
        List<Account> accounts = await new PlayerDB().FetchAccounts();
        List<Gym> gyms = await new GymDB().FetchGyms();

        foreach (Gym gym in gyms)
        {
            LeaderboardRow leaderboardRow = new LeaderboardRow(gym.Name, 0);
            leaderboardRows.Add(gym.gymId, leaderboardRow);
        }

        foreach (Account account in accounts)
        {
            if (String.IsNullOrEmpty(account.gymId))
            {
                continue;
            }
            leaderboardRows[account.gymId].GroupScore += account.TotalScore;
        }

        return leaderboardRows.Values.ToList();
    }
}