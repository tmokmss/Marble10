using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip correct;
    [SerializeField] AudioClip wrong;

    [SerializeField] AudioSource seSource;
    [SerializeField] AudioSource bgmSource;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAnswer(Result result)
    {
        var target = result == Result.Correct ? correct : wrong;
        seSource.PlayOneShot(target);
    }

    public void StartBGM()
    {
        bgmSource.Play();
    }
}
