using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Selectable Selected;
    public Selectable[] sb;

    int _selected = 0;

    void Start()
    {
        Cursor.visible = false;
        sb[_selected].Select();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _selected++;
            if (_selected >= sb.Length)
            {
                _selected = 0;
            }
            sb[_selected].Select();

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _selected--;
            if (_selected < 0)
            {
                _selected = sb.Length - 1;
            }
            sb[_selected].Select();

        }
    }

}
