using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class LeaderboardRow
{
    public string GroupName { get; private set; }
    public int GroupScore { get; private set; }

    public LeaderboardRow(string groupName, int groupScore)
    {
        GroupName = groupName;
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
        List<LeaderboardRow> leaderboardRows = new List<LeaderboardRow>();
        try
        {
            DataSnapshot snapshot = await _reference.Child("Leaderboard").GetValueAsync();
            if (snapshot.Exists)
            {
                foreach (var snapshotChild in snapshot.Children)
                {
                    LeaderboardRow leaderboardRow = JsonUtility.FromJson<LeaderboardRow>(snapshotChild.GetRawJsonValue());
                    leaderboardRows.Add(leaderboardRow);
                }
            }
            else
            {
                Debug.LogWarning("Leaderboard is null");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return leaderboardRows;
    }
}