using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthUIWatcher : MonoBehaviour
{
    [SerializeField]
    protected PlayerHealth watchedPlayerHealth;
    [SerializeField]
    protected TextMeshProUGUI healthCounter;

    protected void Update()
    {
        healthCounter.text = watchedPlayerHealth.HealthToString;
    }
}
