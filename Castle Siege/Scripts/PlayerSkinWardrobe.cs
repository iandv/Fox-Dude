using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinWardrobe : MonoBehaviour
{
    [SerializeField] GameObject[] skins;

    private void Start()
    {
        ChangeSkin();
    }

    public void ChangeSkin()
    {
        var skin = PlayerSaveProfile.Instance.saveData.playerSkin;

        switch (skin)
        {
            case SaveData.PlayerSkin.knight:
                foreach (GameObject item in skins)
                {
                    DoCheck(item, 0);
                }
                break;
            case SaveData.PlayerSkin.mage:
                foreach (GameObject item in skins)
                {
                    DoCheck(item, 1);
                }
                break;
        }
    }

    void DoCheck(GameObject item, int i)
    {
        if (item != skins[i])
            item.SetActive(false);
        else
            item.SetActive(true);
    }
}
