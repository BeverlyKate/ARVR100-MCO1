using System.Collections;
using System.Collections.Generic;

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
    public GroupLeaderboardDB()
    {
        // TODO: Initialize database connection
        // It may be better to just wrap around PlayerDB and do the grouping here
    }

    public List<LeaderboardRow> FetchLeaderboardRows()
    {
        // TODO
        return new List<LeaderboardRow>() { new("group1", 100), new("group2", 200) };
    }
}
