using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{

    public int health = 100;
    
    public abstract void InflictDamage(PizzaBullet bullet);
    public GameObject Player;
    public FOVConeMesh SightCone;
    public float radius = 5f;
    public float angle = 180f;
    public int numSegments = 16;
    private Mesh mesh;
    Collider2D collisionBox;

    protected Rigidbody2D rb2d;

    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collisionBox = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        checkForPlayer();
    }

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

    void checkForPlayer()
    {
        if (!Player)
        {
            return;
        }
        
        Vector2 directionToPlayer = Player.transform.position - transform.position;
        float angleToPlayer = Vector2.Angle(transform.up, directionToPlayer);

        collisionBox.enabled= false;
        RaycastHit2D hit= Physics2D.Raycast(transform.position,directionToPlayer, radius );
        collisionBox.enabled = true;

        if (directionToPlayer.magnitude <= radius && angleToPlayer <= angle / 2f&& !SightCone.alerted&&hit.collider.tag=="Player")
        {
            SightCone.SetAlert(true);
        }
        else
        {
            SightCone.SetAlert(false);

        }
    }


    public Vector2 DirFromAngle(float angleDeg, bool global)
    {
        if (!global)
        {
            angleDeg += transform.eulerAngles.z+90;
        }
        return new Vector2(Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Sin(angleDeg * Mathf.Deg2Rad));
    }


}
