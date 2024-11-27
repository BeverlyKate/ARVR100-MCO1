using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GymsState : MonoBehaviour
{
    private DatabaseReference _reference;
    private Gym userCurrentGym;

    [SerializeField] GameObject inputPanel;
    [SerializeField] GameObject selectList;
    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject deleteButton;

    TMP_InputField gymInputField { 
        get => inputPanel.transform.Find("InputField (TMP)").GetComponent<TMP_InputField>(); 
    }

    public List<Gym> Gyms { get; private set; }
    public Gym SelectedGym { get; private set; }

    async void Start()
    {
        _reference = FirebaseDatabase.DefaultInstance.RootReference;

        await UpdateGyms();
        await SetStartingGym();
    }

    async Task SetStartingGym()
    {
        if (CurrentAccount.Account.gymId == "")
        {
            return;
        }

        DataSnapshot gymSS= await _reference.Child("Gyms").Child(CurrentAccount.Account.gymId).GetValueAsync();
        Gym gym = JsonUtility.FromJson<Gym>(gymSS.GetRawJsonValue());

        userCurrentGym = gym;
        PreSelectGym(userCurrentGym);
    }

    async Task UpdateGyms()
    {
        GymDB gdb = new();
        Gyms = await gdb.FetchGyms();

        selectList.GetComponent<GymsSelectList>().UpdateGyms(Gyms);
    }

    public void OpenGymInputScreen()
    {
        inputPanel.SetActive(true);
    }

    public void CloseGymInputScreen()
    {
        gymInputField.text = "";
        inputPanel.SetActive(false);
    }

    public async void CreateGym()
    {
        GymDB gdb = new();
        await gdb.AddGym(gymInputField.text);

        UpdateGyms();
        CloseGymInputScreen();
    }

    public void PreSelectGym(Gym gym)
    {
        SelectedGym = gym;
        selectList.GetComponent<GymsSelectList>().UpdateSelectedGym(SelectedGym);

        selectButton.GetComponent<Button>().interactable = gym != null;
        deleteButton.GetComponent<Button>().interactable = gym != null;
    }

    public async void SelectGym()
    {
        PlayerDB pdb = new();
        await pdb.SetGym(CurrentAccount.Account, SelectedGym);
        userCurrentGym = SelectedGym;

        SceneManager.LoadScene("Main Menu Scene", LoadSceneMode.Single);
    }

    public async void DeleteGym()
    {
        GymDB gdb = new();
        gdb.DeleteGym(SelectedGym);

        PlayerDB pdb = new();
        var accounts = await pdb.FetchAccounts();

        foreach (var account in accounts)
        {
            if (account.gymId == SelectedGym.gymId)
            {
                account.gymId = "";
                await pdb.SetGym(account, null);
            }
        }

        UpdateGyms();
        PreSelectGym(null);
    }
}
