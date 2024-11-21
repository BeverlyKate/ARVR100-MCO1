using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDB
{
    public PlayerDB() {
        // TODO: Initialize database connection
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
}
