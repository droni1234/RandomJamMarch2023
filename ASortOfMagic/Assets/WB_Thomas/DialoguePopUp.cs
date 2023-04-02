using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePopUp : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator popUpAnimator;
    public TMP_Text popUpText;

    public void PopUpText(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        popUpAnimator.SetTrigger("pop");
    }


    /*
     * public void DisplayText(string text){
     * PopUpSystem popUp=GameObject.FindG
     * 
     */
}
