using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Account
{
    public string Name { get; private set; }

    public Account(string name)
    {
        Name = name;
    }
}
