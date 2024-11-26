using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Account
{
    public string userId;
    public string Name;
    public int Streak;

    public string gymId;
    public List<int> DailyProgress;
    public string StreakLastUpdateDate;
    public string ProgressLastLoginDate;
    public int TotalReps;
    public int TotalScore;

    public Account(string userId, string name)
    {
        this.userId = userId;
        this.Name = name;
        Streak = 0;
        DailyProgress = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
        TotalReps = 0;
        TotalScore = 0;
        gymId = "";
        StreakLastUpdateDate = "";
        ProgressLastLoginDate = "";
    }
}