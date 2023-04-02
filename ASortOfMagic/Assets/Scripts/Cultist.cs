using System;
using Unity.VisualScripting;
using UnityEngine;
using static PizzaBullet.Type;

public class Cultist : WalkingEnemy
{
    public int damage = 5;

    public float damageCooldown = 1F;
    public float damageCooldownTimer;
    
    
    public override void InflictDamage(PizzaBullet bullet)
    {
        switch (bullet.type)
        {
            case Blunt:
                InflictDamage(bullet.damage);
                break;
            case Stealth:
                if (Gamemaster.Instance)
                {
                    if (Gamemaster.Instance.stealth)
                    {
                        Die();
                    }
                    else
                    {
                        InflictDamage(bullet.damage);
                    }
                }
                break;
            case Toxic:
            case Quick:
            case Bomb:
            default:
                throw new ArgumentOutOfRangeException(nameof(bullet), bullet, null);
        }
    }

    protected void FixedUpdate()
    {
        Pathing();
    }

    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(Gamemaster.Instance.player.transform.position, transform.position) <= 0.5F && damageCooldownTimer + damageCooldown < Time.time)
        {
            alerted = true;
            Gamemaster.Instance.player.ReceiveDamage(damage);
            damageCooldownTimer = Time.time;
        }
    }
}
