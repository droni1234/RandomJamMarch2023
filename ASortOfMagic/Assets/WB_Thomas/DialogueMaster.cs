using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject imageBox;
    public Animator popUpAnimator;
    public TMP_Text popUpText;
    public Sprite defaultSprite;

    public void PopUpText(string text, bool useImage, Sprite image)
    {
        if (useImage)
        {
            imageBox.GetComponent<Image>().sprite = image;
        }
        else
        {
            imageBox.GetComponent<Image>().sprite = defaultSprite;
        }
        popUpBox.SetActive(true);
        popUpText.text = text;
        popUpAnimator.SetTrigger("pop");

    }
}
