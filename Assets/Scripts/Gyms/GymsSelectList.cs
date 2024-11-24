using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GymsSelectList : MonoBehaviour
{
    readonly Color CLEARED_COLOR = new(1.0f, 1.0f, 1.0f, 0.0f);
    readonly Color SELECTED_COLOR = new(0.5f, 0.5f, 0.5f, 1.0f);

    List<Gym> gyms;
    Gym selectedGym;

    [SerializeField] GameObject content;
    [SerializeField] GameObject listTextPrefab;

    public void UpdateGyms(List<Gym> gyms)
    {
        this.gyms = gyms;

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Gym gym in gyms)
        {
            GameObject listItem = Instantiate(listTextPrefab, content.transform);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = gym.Name;
            listItem.GetComponent<Button>()
                .onClick
                .AddListener(() => GymsState.Instance.PreSelectGym(gym));

            if (gym == selectedGym)
            {
                listItem.transform.GetComponent<Image>().color = SELECTED_COLOR;
            }
        }
    }

    public void UpdateSelectedGym(Gym selectedGym)
    {
        this.selectedGym = selectedGym;
        UpdateGyms(gyms);
    }
}
