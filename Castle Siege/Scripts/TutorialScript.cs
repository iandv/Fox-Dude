using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] prompts, promptsTwo;
    [SerializeField]
    GameObject blocker;
    bool _grounded, _touch;
    int index;

    private void Start()
    {
        if (PlayerPrefs.HasKey("touchControl"))
        {
            if (PlayerPrefs.GetInt("touchControl") == 0)
            {
                _touch = false;
            }

            if (PlayerPrefs.GetInt("touchControl") == 1)
            {
                _touch = true;
            }
        }

        _grounded = false;
        EventManager.Instance.Subscribe("bullet grounded", SlideEventGrounded);
        EventManager.Instance.Subscribe("can shoot", SlideEvent);
        EventManager.Instance.Subscribe("end game", SlideEvent);
        SlideCard();
    }

    public void SlideEvent(params object[] parameters)
    {
        SlideCard();
    }

    public void SlideEventGrounded(params object[] parameters)
    {
        if (!_grounded)
        {
            _grounded = true;
            SlideCard();
        }

    }

    public void SlideCard()
    {
        if (index < prompts.Length)
        {
            if (!_touch)
                prompts[index].SetActive(true);
            if (_touch)
                promptsTwo[index].SetActive(true);

            blocker.SetActive(true);

            index++;
            Time.timeScale = 0f;
        }
    }

    public void DeactivateBlocker()
    {
        blocker.SetActive(false);
        Time.timeScale = 1f;
    }
}
