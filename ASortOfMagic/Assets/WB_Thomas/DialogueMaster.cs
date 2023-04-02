using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueMaster : MonoBehaviour
{
    private static DialogueMaster instance;
    public static DialogueMaster Instance => getInstance();

    

    private static DialogueMaster getInstance()
    {
        if (instance)
            return instance;

        var myGameobject = new GameObject("DialogueMaster");
        instance = myGameobject.AddComponent<DialogueMaster>();
        DontDestroyOnLoad(myGameobject);
        return instance;
    }

    public GameObject popUpBox;
    public Animator popUpAnimator;
    public TMP_Text popUpText;

    public void PopUpText(string text)
    {
        popUpBox.SetActive(true);
        popUpText.text = text;
        popUpAnimator.SetTrigger("pop");

    }
}
