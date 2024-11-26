using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginState : MonoBehaviour
{
    [SerializeField] GameObject inputPanel;
    [SerializeField] GameObject selectList;
    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject deleteButton;

    TMP_InputField nameInputField { 
        get => inputPanel.transform.Find("InputField (TMP)").GetComponent<TMP_InputField>(); 
    }

    public List<Account> Accounts { get; private set; }
    public Account SelectedAccount { get; private set; }   

    void Start()
    {
        UpdateAccountsWithStreakCheck();
    }

    async void UpdateAccountsWithStreakCheck()
    {
        PlayerDB pdb = new();
        await pdb.UpdatePlayerStreakInfo();
        await pdb.UpdatePlayerWeeklyRepsInfo();

        UpdateAccounts();
    }

    async void UpdateAccounts()
    {
        PlayerDB pdb = new();
        Accounts = await pdb.FetchAccounts();

        selectList.GetComponent<LoginSelectList>().UpdateAccounts(Accounts);
    }

    public void OpenAccountInputScreen()
    {
        inputPanel.SetActive(true);
    }

    public void CloseAccountInputScreen()
    {
        nameInputField.text = "";
        inputPanel.SetActive(false);
    }

    public async void CreateAccount()
    {
        PlayerDB pdb = new();
        await pdb.CreateAccount(nameInputField.text);

        UpdateAccounts();
        CloseAccountInputScreen();
    }

    public void PreSelectAccount(Account account)
    {
        SelectedAccount = account;
        selectList.GetComponent<LoginSelectList>().UpdateSelectedAccount(account);

        selectButton.GetComponent<Button>().interactable = account != null;
        deleteButton.GetComponent<Button>().interactable = account != null;
    }

    public async void SelectAccount()
    {
        PlayerDB pdb = new();
        await pdb.UpdateProgressLastLoginDate(SelectedAccount);

        CurrentAccount.Account = SelectedAccount;
        SceneManager.LoadScene("Main Menu Scene", LoadSceneMode.Single);
    }

    public void DeleteAccount()
    {
        PlayerDB pdb = new();
        pdb.DeleteAccount(SelectedAccount);
        CurrentAccount.Account = null;

        UpdateAccounts();
        PreSelectAccount(null);
    }
}
