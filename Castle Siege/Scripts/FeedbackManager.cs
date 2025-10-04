using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] AudioClip feedbackSound;
    [SerializeField] ParticleSystem winFeedbackParticle;
    [SerializeField] Camera particleCamera;

    AudioSource _au;

    public static FeedbackManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _au = GetComponent<AudioSource>();

        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera != particleCamera)
        {
            canvas.worldCamera = particleCamera;
        }
    }

    public void PlayTouchSoundOnce()
    {
        _au.PlayOneShot(feedbackSound);
    }

    public void PlayWinParticle()
    {
        winFeedbackParticle.Play();
    }
}
