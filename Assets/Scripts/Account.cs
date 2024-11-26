using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Account
{
    public string userId { get; private set; }
    public string Name { get; private set; }
    public int Streak { get; private set; }
    
    public string gymId { get;  set; }
    public List<int> DailyProgress { get; private set; }

    public Account(string userId, string name)
    {
        this.userId = userId;
        this.Name = name;
        Streak = 0; // TODO
        DailyProgress = new List<int>() { 1, 2, 3, 4, 5, 6, 7 }; // TODO
        gymId = "";
    }
}