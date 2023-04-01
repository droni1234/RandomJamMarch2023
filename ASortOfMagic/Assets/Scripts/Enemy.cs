using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{

    public int health = 100;
    
    public abstract void InflictDamage(PizzaBullet bullet);

    public virtual void InflictDamage(int damage)
    {
        Debug.Log("DamageInflicted");
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
