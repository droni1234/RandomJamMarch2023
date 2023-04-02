using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBullet : MonoBehaviour
{

    public Type type;

    public int damage = 1;

    public int special = 0; // Can be used for special values like Quick or Toxic
    
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Debug.Log("Hit");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cultist"))
        {
            collision.gameObject.GetComponent<Enemy>().InflictDamage(this);
        }
        
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(200*Time.deltaTime, 300* Time.deltaTime, 0f);
    }


    public enum Type
    {
        Blunt, // Normal Pizza
        Stealth, // This Pizza can one-shot an enemy if he is in stealth Mode
        Toxic, // This will inflict Toxic damage
        Quick, // This Pizza flies quicker and more often but does less damage (Damage output in total should be higher in damage per seconds)
        Bomb, // Experimental Pizza which will inflict AOE damage on detonation with an enemy or Wall
    }
    
}
