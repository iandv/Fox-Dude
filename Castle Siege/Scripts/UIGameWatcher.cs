using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameWatcher : MonoBehaviour
{
    public GameObject endScreen;
    public Image[] starsImages;
    public TextMeshProUGUI currentPoints, finalScore, endScreenFinalScore, endScreenHighScore, gemsReward, gemsBonus, winText, loseText;

    [SerializeField]
    GameObject destroyBulletsButton, coverForFireButton, optionsMenu;
    [SerializeField]
    private Image[] ammoButtons;
    [SerializeField]
    private TextMeshProUGUI[] ammoTexts;
    [SerializeField]
    private Color32 black = new Color32(0, 0, 0, 112), red = new Color32(128, 0, 0, 112);

    private PlayerModel _player;
    int _bulletsAirborne;

    public static UIGameWatcher Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _bulletsAirborne = 0;
    }

    private void Start()
    {
        destroyBulletsButton.SetActive(false);
        coverForFireButton.SetActive(false);
        optionsMenu.SetActive(false);
        _player = FindObjectOfType<PlayerModel>();
        ChangeColor(0);
        UpdateNumbers();
        EventManager.Instance.Subscribe("bullet airborne", BulletsAirborne);
        EventManager.Instance.Subscribe("bullet grounded", BulletsGrounded);
        EventManager.Instance.Subscribe("all clear", HideDestroyBulletsButton);
    }

    public void UpdateNumbers()
    {
        for (int i = 0; i < ammoTexts.Length; i++)
        {
            ammoTexts[i].text = _player.ammoStowage[i].ToString();
        }
    }

    void ChangeColor(int num)
    {
        foreach (Image item in ammoButtons)
        {
            if (item != ammoButtons[num])
                item.color = black;
            else
                item.color = red;
        }
    }

    void HideDestroyBulletsButton(params object[] parameter)
    {
        destroyBulletsButton.SetActive(false);
        coverForFireButton.SetActive(false);
    }

    void BulletsAirborne(params object[] parameters)
    {
        ++_bulletsAirborne;
    }

    void BulletsGrounded(params object[] parameters)
    {
        --_bulletsAirborne;
        if (_bulletsAirborne <= 0)
        {
            destroyBulletsButton.SetActive(true);
            coverForFireButton.SetActive(true);
        }
    }

    public void DestroyBullets()
    {
        EventManager.Instance.Trigger("destroy all bullets");
        destroyBulletsButton.SetActive(false);
        coverForFireButton.SetActive(false);
    }

    public void BasicAmmo()
    {
        ChangeColor(0);
    }

    public void TripleAmmo()
    {
        ChangeColor(1);
    }

    public void ExplosiveAmmo()
    {
        ChangeColor(2);
    }
}
