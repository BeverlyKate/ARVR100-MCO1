using System.Collections;
using System.Collections.Generic;

public class Gym
{
    public string gymId {get; set;}
    public string Name { get; private set; }

    public Gym(string gymId, string name)
    {
        this.gymId = gymId;
        Name = name;
    }
}