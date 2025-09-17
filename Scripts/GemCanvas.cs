using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCanvas : MonoBehaviour
{
    Animator _anim;
    public bool feedback;

    float _feedbackTime;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _feedbackTime = 0;
        feedback = false;
    }

    void Update()
    {
        Feedback();
    }


    void Feedback()
    {
        if (feedback == true)
        {
            _anim.SetBool("feedback", true);
            _feedbackTime += Time.deltaTime;
            if (_feedbackTime >= 0.333f)
            {
                _anim.SetBool("feedback", false);
                feedback = false;
                _feedbackTime = 0;
            }
        }
    }
}
