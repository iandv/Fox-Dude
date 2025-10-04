using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class AlternativeControls : MonoBehaviour
{
    public GameObject[] buttons;
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    bool mainMenu = false;

    bool _touch, _leftTouch;
    PlayerController _pc;

    float lastTapTime = 0;
    [SerializeField]
    float doubleTapThreshold = 0.3f;

    [SerializeField]
    private Vector2 joyStickSize = new Vector2(300, 300);
    [SerializeField]
    private VirtualJoystick joyStick;

    private Finger movementFinger;
    private Vector2 movementAmount;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            Vector2 knobPos;
            float maxMovement = joyStickSize.x / 2;
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, 
                joyStick.rectTransform.anchoredPosition) >
                maxMovement)
            {
                knobPos = (currentTouch.screenPosition -
                    joyStick.rectTransform.anchoredPosition).normalized
                    * maxMovement;
            }
            else
            {
                knobPos = currentTouch.screenPosition - joyStick.rectTransform.anchoredPosition;
            }

            joyStick.knob.anchoredPosition = knobPos;
            movementAmount = knobPos / maxMovement;
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == movementFinger)
        {
            movementFinger = null;
            joyStick.knob.anchoredPosition = Vector2.zero;
            joyStick.gameObject.SetActive(false);
            movementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (!_touch || mainMenu) return;

        if (movementFinger == null && touchedFinger.screenPosition.x <= Screen.width / 2 && touchedFinger.screenPosition.y <= (Screen.height * 0.8))
        {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            joyStick.gameObject.SetActive(true);
            joyStick.rectTransform.sizeDelta = joyStickSize;
            joyStick.rectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
            _leftTouch = true;
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < joyStickSize.x / 2)
        {
            startPosition.x = joyStickSize.x / 2;
        }

        if (startPosition.y < joyStickSize.y / 2)
        {
            startPosition.y = joyStickSize.y / 2;
        }

        else if (startPosition.y > Screen.height - joyStickSize.y / 2)
        {
            startPosition.y = Screen.height - joyStickSize.y / 2;
        }

        return startPosition;
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("touchControl"))
        {
            PlayerPrefs.SetInt("touchControl", 0);
            Load();
        }

        else
            Load();

        if (!mainMenu)
            _pc = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (_touch && !mainMenu)
        {
            TouchScreenControls();
        }
    }

    private void Load()
    {
        if (PlayerPrefs.GetInt("touchControl") == 0)
        {
            _touch = false;
        }

        if (PlayerPrefs.GetInt("touchControl") == 1)
        {
            _touch = true;
        }

        toggle.isOn = _touch;
        HideUI();
    }

    private void Save()
    {
        if (_touch == false)
        {
            PlayerPrefs.SetInt("touchControl", 0);
        }

        if (_touch == true)
        {
            PlayerPrefs.SetInt("touchControl", 1);
        }
    }

    void HideUI()
    {
        if (_touch)
        {
            foreach (var item in buttons)
            {
                item.SetActive(false);
            }
        }

        if (!_touch)
        {
            foreach (var item in buttons)
            {
                item.SetActive(true);
            }
        }
    }

    public void ChangeToggle(bool touch)
    {
        _touch = touch;
        Save();
        HideUI();
    }

    public void ChangeToggleMM(bool touch)
    {
        _touch = touch;
        Save();
    }

    private void TouchScreenControls()
    {
        if (Time.timeScale > 0)
        {
            _pc.PressDownAxisJS(movementAmount.y);

            if (Input.touchCount >= 2)
            {
                movementFinger = null;
                joyStick.knob.anchoredPosition = Vector2.zero;
                joyStick.gameObject.SetActive(false);
                movementAmount = Vector2.zero;
            }

            if (Input.touchCount == 1)
            {
                UnityEngine.Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (Time.time - lastTapTime <= doubleTapThreshold)
                    {
                        lastTapTime = 0;

                        if (touch.position.x <= Screen.width / 2)
                        {
                            movementFinger = null;
                            joyStick.knob.anchoredPosition = Vector2.zero;
                            joyStick.gameObject.SetActive(false);
                            movementAmount = Vector2.zero;
                        }

                        if (touch.position.x >= Screen.width / 2)
                        {
                            _pc.PressKeyFire();
                            _pc.PressDownFire();
                        }
                    }
                    else
                    {
                        lastTapTime = Time.time;
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    if (lastTapTime == 0)
                    {
                        if (touch.position.x >= Screen.width / 2 && !_leftTouch)
                        {
                            _pc.PressUpFire();
                            UIGameWatcher.Instance.UpdateNumbers();
                        }
                    }
                    _leftTouch = false;
                }
            }
        }

    }
}
