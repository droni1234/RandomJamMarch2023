using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Debug.Log("Hit");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Debug.Log("Hit2D");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(200*Time.deltaTime, 300* Time.deltaTime, 0f);
    }


}
