using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] string victoryBoolName = "Victory", defeatBoolName = "Defeat", fireTrigger = "Fire";
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        EventManager.Instance.Subscribe("win", Victory);
        EventManager.Instance.Subscribe("lose", Defeat);
        EventManager.Instance.Subscribe("fire", Fire);
    }

    void Victory(params object[] parameters)
    {
        _animator.SetBool(victoryBoolName, true);
    }

    void Defeat(params object[] parameters)
    {
        _animator.SetBool(defeatBoolName, true);
    }

    void Fire(params object[] parameters)
    {
        _animator.SetTrigger(fireTrigger);
    }
}
