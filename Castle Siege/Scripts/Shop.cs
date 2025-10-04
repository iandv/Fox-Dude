using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] SaveData.PlayerSkin skin;
    [SerializeField] int price;
    [SerializeField] GameObject notEnoughGemsMessage, changeSkinButton;
    int _skinIndex;

    private void Start()
    {
        var psp = PlayerSaveProfile.Instance.saveData;

        switch (skin)
        {
            case SaveData.PlayerSkin.knight:
                _skinIndex = 0;
                break;
            case SaveData.PlayerSkin.mage:
                _skinIndex = 1;
                break;
        }

        if (price > 0)
        {
            if (psp.unlockedSkins[_skinIndex])
                gameObject.SetActive(false);
        }

        else if (!psp.unlockedSkins[_skinIndex])
            gameObject.SetActive(false);
    }

    public void BuySkin()
    {
        PlayerSaveProfile psp = PlayerSaveProfile.Instance;
        if (psp.saveData.gems >= price)
        {
            psp.saveData.gems -= price;
            psp.saveData.unlockedSkins[_skinIndex] = true;
            psp.saveData.playerSkin = skin;
            psp.SaveGame();
            changeSkinButton.SetActive(true);
            gameObject.SetActive(false);
        }

        else
            notEnoughGemsMessage.SetActive(true);
    }

    public void ChangeSkin()
    {
        PlayerSaveProfile psp = PlayerSaveProfile.Instance;
        if (psp.saveData.playerSkin != skin)
        {
            psp.saveData.playerSkin = skin;
            psp.SaveGame();
        }
    }
}
