using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public string popUpText;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DialogueMaster popUp = FindObjectOfType<DialogueMaster>();
            popUp.PopUpText(popUpText);
        }
    }
}
