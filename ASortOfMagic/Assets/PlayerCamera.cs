using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    
    public Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = Gamemaster.Instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, -10F), 10F * Time.deltaTime);
    }
}
