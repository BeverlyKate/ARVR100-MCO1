using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDB
{
    public PlayerDB() {
        // TODO: Initialize database connection
    }

    // TODO: Add the corresponding parameters
    public void CreateAccount(string name)
    {
        // TODO: Make the proper call
    }

    public void DeleteAccount(Account account)
    {
        // TODO: Make the proper call
    }

    public List<Account> FetchAccounts()
    {
        // TODO: Make the proper call
        return new List<Account>() { new("name1"), new("name2") };
    }

    public void SetGym(Account account, Gym gym)
    {
        // TODO: Make the proper call
    }
}
