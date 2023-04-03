using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickUp : MonoBehaviour
{
    public string popUpText;
    public Sprite Image;
    public bool useImage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DialogueMaster popUp = FindObjectOfType<DialogueMaster>();
            popUp.PopUpText(popUpText, useImage,Image);
        }
    }
}
