using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    private AudioHelper _audioHelper;
    public AudioClip itemPickupSound;

    private void Start()
    {
        _audioHelper = GameObject.Find("Audio Manager").GetComponent<AudioHelper>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var pb = other.GetComponent<PlayerBehavior>();
        pb.OnItemCollected();
        StartCoroutine(nameof(PlayPickupSound));
        Destroy(transform.parent.gameObject);
    }

    private IEnumerator PlayPickupSound()
    {
        _audioHelper.PlaySound(itemPickupSound, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.25f);
        _audioHelper.PlaySound(itemPickupSound, 1.5f, 1.0f);
    }
}