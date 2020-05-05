using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    public void PlaySound(AudioClip clip, float pitch = 1.0f, float volume = 1.0f)
    {
        var sfx = new GameObject().AddComponent<AudioSource>();
        sfx.clip = clip;
        sfx.pitch = pitch;
        sfx.volume = volume;
        sfx.Play();
        StartCoroutine(nameof(CleanupAudio), sfx);
    }

    private IEnumerator CleanupAudio(AudioSource sfx)
    {
        yield return new WaitUntil(() => !sfx.isPlaying);
        Destroy(sfx.transform.gameObject);
    }
}