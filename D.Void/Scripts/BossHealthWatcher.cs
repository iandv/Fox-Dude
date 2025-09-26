using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthWatcher : MonoBehaviour
{
    private Image _healthBar;
    private float _fullHealth;
    public EnemyHealth _enemyHealth;

    private void Awake()
    {
        _fullHealth = (float)_enemyHealth.health;
        _healthBar = GetComponent<Image>();
    }

    private void Update()
    {
        _healthBar.fillAmount = (float)_enemyHealth.health / _fullHealth;
    }
}
