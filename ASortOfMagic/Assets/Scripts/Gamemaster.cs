using UnityEngine;

public class Gamemaster : MonoBehaviour
{

    public static Gamemaster instance;

    public bool stealth = true;

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }    
        else
        {
            Destroy(gameObject);
        }
    }
}
