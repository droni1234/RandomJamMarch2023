using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Door : MonoBehaviour
{

    private bool playerNearby = false;

    private bool isOpen = false;

    [SerializeField]
    private AudioSource doorSound;

    [SerializeField]
    private GameObject doorObject;

    public bool triggerOnce = true;
    private bool triggered = false;
    
    [SerializeField] private AudioClip[] openingSounds;
    [SerializeField] private AudioClip[] closingSounds;
    
    void ToggleDoor()
    {
        if (triggerOnce)
        {
            if (!triggered)
            {
                triggered = true;
                
            }
            else
            {
                return;
            }
        }
        
        
        isOpen = !isOpen;

        if (isOpen)
        {
            doorObject.SetActive(false);
            if (!doorSound || openingSounds.Length <= 0) return;
            doorSound.clip = openingSounds[Random.Range(0, openingSounds.Length)];
            doorSound.pitch = Random.Range(0.85F, 1.15F);
            doorSound.Play();
        }
        else
        {
            doorObject.SetActive(true);
            if (!doorSound || closingSounds.Length <= 0) return;
            doorSound.clip = closingSounds[Random.Range(0, openingSounds.Length)];
            doorSound.pitch = Random.Range(0.85F, 1.15F);
            doorSound.Play();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerNearby && Input.GetButtonDown("Jump"))
        {
            ToggleDoor();
        }       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }
    
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}
