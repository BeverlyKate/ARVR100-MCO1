using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeContainer : MonoBehaviour
{
    [SerializeField] private GameObject userWelcomeText;
    [SerializeField] private GameObject streakText;

    void Start()
    {
        if (CurrentAccount.Account == null)
        {
            return;
        }

        var userWelcomeTextComponent = userWelcomeText.GetComponent<TMPro.TextMeshProUGUI>();
        userWelcomeTextComponent.text = "Welcome, " + CurrentAccount.Account.Name + "!";

        var streakTextComponent = streakText.GetComponent<TMPro.TextMeshProUGUI>();
        streakTextComponent.text = "~" + CurrentAccount.Account.Streak + "~";
    }
}
