using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    protected AudioClip doorSound;
    [SerializeField]
    protected float closeTime = 0.5f;
    [SerializeField]
    protected TextMeshProUGUI displayedKeyText;
    [SerializeField]
    protected string interactKeyText = "Press E";
    [SerializeField]
    private bool automaticShut = false;

    private PlayerAuxBool _auxBool;
    private Animator _anim;
    private AudioSource _audio;
    private bool _open = false;
    void Awake()
    {
        _auxBool = FindObjectOfType<PlayerAuxBool>();
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider col)
    {
        if (automaticShut)
        {
            if (col.gameObject.tag == "Player" && _open == false)
            {
                displayedKeyText.text = interactKeyText;

                if (_auxBool.interact == true)
                {
                    displayedKeyText.text = "";
                    _open = !_open;
                    _audio.Play();
                    _anim.SetTrigger("Open");
                }
            }
        }        

        if (!automaticShut)
        {
            if (col.gameObject.tag == "Player")
            {
                displayedKeyText.text = interactKeyText;

                if (_auxBool.interact == true && _open == true)
                {
                    _open = !_open;
                    _audio.Play();
                    _anim.enabled = true;
                }

                else if (_auxBool.interact == true && _open == false)
                {
                    _open = !_open;
                    _audio.Play();
                    _anim.SetTrigger("Open");
                }
            }
        }        
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            displayedKeyText.text = "";

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
