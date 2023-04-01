using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class WalkingEnemy : Enemy
{
    [Space]
    [Header("Movement")]
    public PathPoint startPoint;
    private PathPoint currentPoint;

    private Vector2 direction;

    public float speed = 5;
    public float pathPointProximity = 0.1F;
    private bool isPathingTowardsNext;

    protected override void Awake()
    {
        base.Awake();
        currentPoint = startPoint;
    }

    protected void PathToNextPoint()
    {
        if (Vector2.Distance(transform.position, currentPoint.transform.position) < pathPointProximity)
        {
            setNextPathPoint();
        }
        direction = (currentPoint.transform.position - transform.position).normalized;
        transform.up = direction;
        rb2d.MovePosition(rb2d.position + direction * (speed * Time.fixedDeltaTime));
    }

    private void setNextPathPoint()
    {
        if (isPathingTowardsNext)
        {
            isPathingTowardsNext = currentPoint.NextPathPoint != null;
            currentPoint = currentPoint.NextPathPoint ?? currentPoint.LastPathPoint;
        }
        else
        {
            isPathingTowardsNext = currentPoint.LastPathPoint == null;
            currentPoint = currentPoint.LastPathPoint ?? currentPoint.NextPathPoint;
        }
    }
}
