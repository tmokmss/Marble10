using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip correct;
    [SerializeField] AudioClip wrong;
    [SerializeField] List<AudioClip> levelUps;

    [SerializeField] AudioSource seSource;
    [SerializeField] AudioSource bgmSource;

    public void PlayAnswer(Result result)
    {
        var target = result == Result.Correct ? correct : wrong;
        seSource.PlayOneShot(target);
    }

    public void PlayLevelUp()
    {
        var levelUp = PickRandom(levelUps);
        seSource.PlayOneShot(levelUp);
    }

    public void StartBGM()
    {
        Beat.Bpm = 150;
        bgmSource.Play();
    }

    T PickRandom<T>(IList<T> source)
    {
        var idx = Mathf.FloorToInt(Random.Range(0, source.Count()));
        return source[idx];
    }
}
