using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LockedDoor : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI displayedText;
    [SerializeField]
    protected TextMeshProUGUI displayedKeyText;
    [SerializeField]
    protected AudioClip doorSound;
    [SerializeField]
    protected string interactKeyText = "Press E";
    [SerializeField]
    protected string lockedText = "It's locked";
    [SerializeField]
    protected string unlockedText = "It's unlocked";
    [SerializeField]
    protected float closeTime = 0.5f;
    [SerializeField]
    protected int keysToOpen;
    [SerializeField]
    private bool automaticShut = false;

    private KeyCounter _keyCounter;
    private PlayerAuxBool _auxBool;
    private Animator _anim;
    private AudioSource _audio;
    private bool _displayOnce = false;
    private bool _open = false;
    void Awake()
    {
        _keyCounter = FindObjectOfType<KeyCounter>();
        _auxBool = FindObjectOfType<PlayerAuxBool>();
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && _keyCounter.keysCollected < keysToOpen)
        {
            displayedText.text = lockedText;
        }

        if (col.gameObject.tag == "Player" && _keyCounter.keysCollected >= keysToOpen && _displayOnce == false)
        {
            displayedText.text = unlockedText;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (automaticShut)
        {
            if (col.gameObject.tag == "Player" && _open == false && _keyCounter.keysCollected >= keysToOpen)
            {
                displayedKeyText.text = interactKeyText;

                if (_auxBool.interact == true)
                {
                    displayedKeyText.text = "";
                    _open = !_open;
                    _audio.Play();
                    _anim.SetTrigger("Open");
                    if (_displayOnce == false)
                        _displayOnce = true;
                }
            }
        }

        if (!automaticShut)
        {
            if (col.gameObject.tag == "Player" && _keyCounter.keysCollected >= keysToOpen)
            {
                displayedKeyText.text = interactKeyText;

                if (_auxBool.interact == true && _open == true)
                {
                    _open = !_open;
                    _audio.Play();
                    _anim.enabled = true;
                }

                if (_auxBool.interact == true && _open == false)
                {
                    displayedKeyText.text = "";
                    _open = !_open;
                    _audio.Play();
                    _anim.SetTrigger("Open");
                    if (_displayOnce == false)
                        _displayOnce = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        
        if (col.gameObject.tag == "Player")
        {
            displayedKeyText.text = "";
            displayedText.text = "";
            if (_open == true && automaticShut)
            {
                StartCoroutine(CloseCoroutine());
            }           
        }
    }

    IEnumerator CloseCoroutine()
    {
        yield return new WaitForSeconds(closeTime);
        _open = !_open;
        _audio.Play();
        _anim.enabled = true;
    }

    void PauseAnimationEvent()
    {
        _anim.enabled = false;
    }
}
