using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageButtonScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] GameObject[] stars;
    [SerializeField] int stageIndexInList, stageEnergyPrice;

    public void UpdateStage()
    {
        Button b = GetComponent<Button>();
        var psp = PlayerSaveProfile.Instance.saveData;

        b.interactable = false;
        foreach(var item in stars)
        {
            item.SetActive(false);
        }

        if (PlayerPrefs.GetInt("currentStamina") > stageEnergyPrice)
        {
            if (stageIndexInList == 0)
            {
                b.interactable = true;
            }

            if (stageIndexInList > 0 && psp.stageResults[stageIndexInList - 1] != SaveData.StageResult.zero)
            {
                b.interactable = true;
            }
        }

        switch (psp.stageResults[stageIndexInList])
        {
            case SaveData.StageResult.zero:
                break;
            case SaveData.StageResult.one:
                stars[0].SetActive(true);
                break;
            case SaveData.StageResult.two:
                for (int i = 0; i < 2; i++)
                    stars[i].SetActive(true);
                break;
            case SaveData.StageResult.three:
                for (int i = 0; i < 3; i++)
                    stars[i].SetActive(true);
                break;
            default:
                break;
        }

        price.text = stageEnergyPrice.ToString();
    }
}
