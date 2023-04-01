using System;
using static PizzaBullet.Type;

public class Cultist : WalkingEnemy
{
    
    
    public override void InflictDamage(PizzaBullet bullet)
    {
        switch (bullet.type)
        {
            case Blunt:
                InflictDamage(bullet.damage);
                break;
            case Stealth:
                if (Gamemaster.instance)
                {
                    if (Gamemaster.instance.stealth)
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
        PathToNextPoint();
    }
}
