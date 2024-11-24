using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GymsState : MonoBehaviour
{
    public static GymsState Instance { get; private set; }

    [SerializeField] GameObject inputPanel;
    [SerializeField] GameObject selectList;
    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject deleteButton;

    TMP_InputField gymInputField { 
        get => inputPanel.transform.Find("InputField (TMP)").GetComponent<TMP_InputField>(); 
    }

    public List<Gym> Gyms { get; private set; }
    public Gym SelectedGym { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateGyms();
    }

    void UpdateGyms()
    {
        GymDB gdb = new();
        Gyms = gdb.FetchGyms();

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

    public void CreateGym()
    {
        GymDB gdb = new();
        gdb.AddGym(gymInputField.text);

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

    public void SelectGym()
    {
        PlayerDB pdb = new();
        pdb.SetGym(CurrentAccount.Account, SelectedGym);

        SceneManager.LoadScene("Main Menu Scene", LoadSceneMode.Single);
    }

    public void DeleteGym()
    {
        GymDB gdb = new();
        gdb.DeleteGym(SelectedGym);

        UpdateGyms();
        PreSelectGym(null);
    }
}
