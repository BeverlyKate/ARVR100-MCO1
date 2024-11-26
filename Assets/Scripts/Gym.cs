using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Gym
{
    public string gymId;
    public string Name;

    public Gym(string gymId, string name)
    {
        this.gymId = gymId;
        Name = name;
    }
}