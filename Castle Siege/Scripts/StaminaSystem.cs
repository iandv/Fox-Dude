using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class StaminaSystem : MonoBehaviour
{
    [SerializeField] int maxStamina;
    [SerializeField] float timeToRecharge;
    int currentStamina;

    [SerializeField] TextMeshProUGUI staminaText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Slider staminaBar;

    bool recharging;

    DateTime nextStaminaTime;
    DateTime lastStaminaTime;

    [SerializeField] string notifTitle = "Full Energy";
    [SerializeField] string notifText = "You're full of Energy, time to play!";

    int id;

    TimeSpan timer;

    public static StaminaSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
            Instance = this;
    }


    void Start()
    {
        if (!PlayerPrefs.HasKey("currentStamina"))
        {
            PlayerPrefs.SetInt("currentStamina", maxStamina);
        }

        Load();
        StartCoroutine(RechargeStamina());
        UpdateStamina();

        if (currentStamina < maxStamina)
        {
            timer = nextStaminaTime - DateTime.Now;
            id = NotificationManager.Instance.DisplayNotification(notifTitle, notifText,
                AddDuration(DateTime.Now, ((maxStamina - (currentStamina) + 1) * timeToRecharge) + 1 + (float)timer.TotalSeconds));
        }
    }

    //Uso de lamda y goes to
    public bool HasEnoughStamina(int stamina) => currentStamina - stamina >= 0;

    IEnumerator RechargeStamina()
    {
        UpdateTimer();
        recharging = true;

        //Quizas queres recargar varias staminas a la vez
        while(currentStamina < maxStamina)
        {
            //Checkeos de tiempo
            DateTime currentTime = DateTime.Now;
            DateTime nextTime = nextStaminaTime;

            bool staminaAdd = false;

            while(currentTime > nextTime)
            {
                //No quiero superar mi maximo de stamina
                if (currentStamina >= maxStamina) break;

                currentStamina++;
                staminaAdd = true;
                UpdateStamina();

                //Predecir cual va a ser mi proximo momento a recargar stamina
                DateTime timeToAdd = nextTime;

                //Mas que nada para checkear bien tu estado de stamina en caso que cerraste la app
                if (lastStaminaTime > nextTime)
                    timeToAdd = lastStaminaTime;

                // creo una funcion para agregar el tiempo a nextTime
                nextTime = AddDuration(timeToAdd, timeToRecharge);

            }

            //Si se recargo stamina...
            if(staminaAdd)
            {
                nextStaminaTime = nextTime;
                lastStaminaTime = DateTime.Now;
            }

            UpdateTimer();
            UpdateStamina();
            Save();
            MainMenuUIWatcher.Instance.UpdateStageButtons();

            yield return new WaitForEndOfFrame();
        }

        NotificationManager.Instance.CancelNotification(id);
        recharging = false;
    }

    DateTime AddDuration(DateTime date, float duration)
    {
        //En nuestro caso, queremos testear rapido agregando solo segundos.
        return date.AddSeconds(duration);
        //date.AddMinutes(duration);
        //date.AddYears((int)duration);
        //date.AddMonths((int)duration);
        //date.AddHours((int)duration);
    }

    public void UseStamina(int staminaToUse)
    {
        if(currentStamina + staminaToUse >= 0)
        {
            currentStamina += staminaToUse;
            UpdateStamina();
            UpdateTimer();

            NotificationManager.Instance.CancelNotification(id);
            id = NotificationManager.Instance.DisplayNotification(notifTitle, notifText,
                AddDuration(DateTime.Now, ((maxStamina - (currentStamina) + 1) * timeToRecharge) + 1 + (float)timer.TotalSeconds));

            //Si no estoy recargando stamina
            if (!recharging)
            {
                //Setear el next Stamina Time y comienzo la carga.
                nextStaminaTime = AddDuration(DateTime.Now, timeToRecharge);
                StartCoroutine(RechargeStamina());
            }
        }
        else
        {
            Debug.Log("No tengo Stamina!");
        }
    }

    void UpdateTimer()
    {
        if(currentStamina >= maxStamina)
        {
            timerText.text = "00:00";
            return;
        }

        //Estructura que nos da un intervalo de tiempo
        timer = nextStaminaTime - DateTime.Now;

        //Formato "00" para representar el horario con 2 digitos.
        timerText.text = timer.Minutes.ToString("00") + ":" + timer.Seconds.ToString("00");
    }

    void UpdateStamina()
    {
        staminaText.text = currentStamina.ToString() + " / " + maxStamina.ToString();
        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;
    }

    void Save()
    {
        PlayerPrefs.SetInt("currentStamina", currentStamina);
        PlayerPrefs.SetString("nextStaminaTime", nextStaminaTime.ToString());
        PlayerPrefs.SetString("lastStaminaTime", lastStaminaTime.ToString());
    }

    void Load()
    {
        currentStamina = PlayerPrefs.GetInt("currentStamina");

        // nextStaminaTime = DateTime.Parse(PlayerPrefs.GetString("nextStaminaTime"));
        // lastStaminaTime = DateTime.Parse(PlayerPrefs.GetString("lastStaminaTime"));

        nextStaminaTime = StringToDateTime(PlayerPrefs.GetString("nextStaminaTime"));
        lastStaminaTime = StringToDateTime(PlayerPrefs.GetString("lastStaminaTime"));
    }

    DateTime StringToDateTime(string date)
    {
        if(string.IsNullOrEmpty(date))
        {
            return DateTime.Now; //Este mismo momento
            //DateTime.Today; //Este mismo dia a las 00:00 horas
            //DateTime.UtcNow; //Tiempo universal coordinado (Argentina UTC-3)
        }
        else
        {
            return DateTime.Parse(date);
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) Save();
    }
}
