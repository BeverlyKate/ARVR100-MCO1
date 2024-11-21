using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    Account selectedAccount;

    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject createButton;
    [SerializeField] GameObject deleteButton;

    public UnityEvent<Account> AccountsCreate;
    public UnityEvent AccountsUpdate;

    void Start()
    {
        selectButton.GetComponent<Button>().onClick.AddListener(OnSelectButtonClick);
        createButton.GetComponent<Button>().onClick.AddListener(OnCreateButtonClick);
        deleteButton.GetComponent<Button>().onClick.AddListener(OnDeleteButtonClick);
    }

    public void SelectAccount(Account account)
    {
        selectedAccount = account;

        selectButton.GetComponent<Button>().interactable = true;
        deleteButton.GetComponent<Button>().interactable = true;
    }

    void OnSelectButtonClick() {
        CurrentAccount.Account = selectedAccount;
        SceneManager.LoadScene("Main Menu Scene");
    }

    void OnCreateButtonClick() {
        AccountsCreate.Invoke(selectedAccount);
    }

    void OnDeleteButtonClick() {
        PlayerDB pdb = new();
        pdb.DeleteAccount(selectedAccount);

        AccountsUpdate.Invoke();
    }
}
