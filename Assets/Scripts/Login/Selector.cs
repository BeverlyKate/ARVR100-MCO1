using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    readonly Color CLEARED_COLOR = new(1.0f, 1.0f, 1.0f, 0.0f);
    readonly Color SELECTED_COLOR = new(0.5f, 0.5f, 0.5f, 1.0f);

    Account selectedAccount;

    [SerializeField] GameObject content;
    [SerializeField] GameObject listTextPrefab;

    public UnityEvent<Account> AccountSelect;

    void Start()
    {
        FetchAccounts();
    }

    public void FetchAccounts()
    {
        PlayerDB pdb = new();
        List<Account> accounts = pdb.FetchAccounts();

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var account in accounts)
        {
            GameObject listItem = Instantiate(listTextPrefab, content.transform);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = account.Name;
            listItem.GetComponent<Button>().onClick.AddListener(() => OnAccountSelect(listItem.transform, account));

            if (account == selectedAccount)
            {
                listItem.transform.GetComponent<Image>().color = SELECTED_COLOR;
            }
        }
    }

    void OnAccountSelect(Transform selectedItem, Account account)
    {
        selectedAccount = account;
        AccountSelect.Invoke(account);

        foreach (Transform child in content.transform)
        {
            child.GetComponent<Image>().color = CLEARED_COLOR;
        }

        selectedItem.GetComponent<Image>().color = SELECTED_COLOR;
    }
}
