using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SuperText : MonoBehaviour
{
    [SerializeField]
    protected string superText = "Press Q";

    private TextMeshProUGUI _text;
    private PlayerAuxBool _player;
    private PlayerSuper _super;
    private bool _active = false;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _player = FindObjectOfType<PlayerAuxBool>();
        _super = FindObjectOfType<PlayerSuper>();
    }

    private void Update()
    {
        if (_player.superIsCharged && !_active)
        {
            _active = !_active;
            _text.text = superText;
        }

        if (_super.skillActive && _active)
        {
            _active = !_active;
            _text.text = "";
        }
    }
}
