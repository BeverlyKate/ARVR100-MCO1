using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginSelectList : MonoBehaviour
{
    readonly Color CLEARED_COLOR = new(1.0f, 1.0f, 1.0f, 0.0f);
    readonly Color SELECTED_COLOR = new(0.5f, 0.5f, 0.5f, 1.0f);

    List<Account> accounts;
    Account selectedAccount;

    [SerializeField] GameObject content;
    [SerializeField] GameObject listTextPrefab;

    public void UpdateAccounts(List<Account> accounts)
    {
        this.accounts = accounts;

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Account account in accounts)
        {
            GameObject listItem = Instantiate(listTextPrefab, content.transform);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = account.Name;
            listItem.GetComponent<Button>()
                .onClick
                .AddListener(() => LoginState.Instance.PreSelectAccount(account));

            if (account == selectedAccount)
            {
                listItem.transform.GetComponent<Image>().color = SELECTED_COLOR;
            }
        }
    }

    public void UpdateSelectedAccount(Account selectedAccount)
    {
        this.selectedAccount = selectedAccount;
        UpdateAccounts(accounts);
    }
}