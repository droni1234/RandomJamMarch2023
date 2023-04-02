using System.Dynamic;
using Unity.Collections;
using UnityEngine;

public class Gamemaster : MonoBehaviour
{

    private static Gamemaster instance;
    public static Gamemaster Instance => getInstance();

    public bool stealth = true;

    public PlayerController player;

    public void Death()
    {
        
    }
    
    private static Gamemaster getInstance()
    {
        if (instance)
            return instance;
        
        var myGameobject = new GameObject("Gamemaster");
        instance = myGameobject.AddComponent<Gamemaster>();
        DontDestroyOnLoad(myGameobject);
        return instance;
    }
}
