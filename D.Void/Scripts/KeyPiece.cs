using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyPiece : MonoBehaviour
{
    [SerializeField]
    protected Material newMaterial;
    [SerializeField]
    protected TextMeshProUGUI displayedKeyText;
    [SerializeField]
    protected TextMeshProUGUI displayedText;
    [SerializeField]
    protected string objectiveTextOne = "Activated ";
    [SerializeField]
    protected string objectiveTextTwo = " out of ";
    [SerializeField]
    protected string objectiveTextThree = " nodes";
    [SerializeField]
    protected string interactKeyText = "Press E";
    [SerializeField]
    protected float clearTextTime = 3f;

    protected KeyCounter _keyCounter;
    protected MeshRenderer _myMeshRenderer;
    protected bool _canInteract = true;

    void Start()
    {
        _keyCounter = FindObjectOfType<KeyCounter>();
        _myMeshRenderer = GetComponent<MeshRenderer>();
    }

    protected void OnTriggerStay(Collider collision)
    {

        if (collision.CompareTag("Player") && _canInteract == true)
        {
            displayedKeyText.text = interactKeyText;
            var playerBool = FindObjectOfType<PlayerAuxBool>();

            if (playerBool.interact == true && _canInteract == true)
            {
                displayedKeyText.text = "";
               _canInteract = false;
                _myMeshRenderer.material = newMaterial;
                _keyCounter.keysCollected++;
                displayedText.text = objectiveTextOne + _keyCounter.keysCollected + objectiveTextTwo + _keyCounter.keysToCollect + objectiveTextThree;
                StartCoroutine(ClearTextCoroutine());
            }
        }
    }

    protected void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            displayedKeyText.text = "";
        }
    }

    IEnumerator ClearTextCoroutine()
    {
        yield return new WaitForSeconds(clearTextTime);
        displayedText.text = "";

    }
}
