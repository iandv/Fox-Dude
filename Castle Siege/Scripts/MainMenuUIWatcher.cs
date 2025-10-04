using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUIWatcher : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gemsText, starsText, playerLvlText, playerExpText, playerName, energyText, gemsRewardText;
    [SerializeField] Slider expBar, energyBar;
    [SerializeField] StageButtonScript[] stageButtons;
    [SerializeField] GameObject optionsMenu, missionRewardPopUp;

    public static MainMenuUIWatcher Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
            Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        optionsMenu.SetActive(false);
    }

    public void ExchangeStamina(int num)
    {
        StaminaSystem.Instance.UseStamina(num);
    }

    public void UpdateUI()
    {
        var pdp = PlayerSaveProfile.Instance.saveData;
        gemsText.text = pdp.gems.ToString();
        starsText.text = pdp.stars.ToString();
        UpdateStageButtons();

        if (pdp.gemPopUp && pdp.lastReward > 0)
        {
            missionRewardPopUp.SetActive(true);
            gemsRewardText.text = pdp.lastReward.ToString();
            PlayerSaveProfile.Instance.saveData.gemPopUp = false;
            PlayerSaveProfile.Instance.SaveGame();
        }
        //playerLvlText.text = "LVL " + pdp.lvl.ToString();
        //playerExpText.text = pdp.exp.ToString() + "/" + pdp.expCap.ToString();
        //expBar.value = pdp.exp / pdp.expCap;
        //playerName.text = pdp.playerName.ToString();
    }

    public void UpdateStageButtons()
    {
        foreach (var item in stageButtons)
            item.UpdateStage();
    }
}
