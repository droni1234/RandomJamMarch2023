using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class WalkingEnemy : Enemy
{
    [Space] [Header("Movement")] public PathPoint startPoint;

    public float baseSpeed = 5;
    private float speed = 5.0F;
    public float orientationPuffer = 0.5F;

    private Vector2 direction;
    private PathPoint currentWayPoint;
    private bool isPathingTowardsNext;
    private float raycastCap = 100F;
    private bool onMultiPath = false;
    private MultiPathPoint currentMultiPath;
    private float proximityDelta = 1.0F;
    


    protected override void Awake()
    {
        base.Awake();
        currentWayPoint = startPoint;
    }

    protected void Pathing()
    {
        if (alerted)
        {
            speed = baseSpeed * 1.5F;
            ToPlayerPathing();
            
        }
        else
        {
            speed = baseSpeed;
            if (onMultiPath)
            {
                ToWayPointPathing();
            }
            else
            {
                PathToNextWayPoint();
            }
        }
    }

    protected void PathToNextWayPoint()
    {
        if (currentWayPoint == null) {return;}
        if (!checkIfReachableDirectlyByDistance(currentWayPoint.transform))
        {
            onMultiPath = true;
            currentMultiPath = findMultiPathPointInReach();
        }
        
        if (checkProximity(transform, currentWayPoint.transform))
        {
            setNextPathPoint();
        }

        moveTowards(currentWayPoint.transform);
    }

    protected void ToPlayerPathing()
    {
        if (checkIfReachableDirectlyByDistance(Gamemaster.Instance.player.transform))
        {
            alerted = true;
            onMultiPath = false;
            moveTowards(Gamemaster.Instance.player.transform);
        }
        else if (onMultiPath == false)
        {
            onMultiPath = true;
            currentMultiPath = findMultiPathPointInReach();
        }

        TraverseRandomWaypoints();
    }

    protected void ToWayPointPathing()
    {
        if (checkIfReachableDirectlyByDistance(currentWayPoint.transform))
        {
            onMultiPath = false;
            return;
        }

        if (onMultiPath == false)
        {
            onMultiPath = true;
            currentMultiPath = findMultiPathPointInReach();
        }

        TraverseRandomWaypoints();
    }

    private void TraverseRandomWaypoints()
    {
        if (currentMultiPath == null || !onMultiPath)
        {
            onMultiPath = false;
            return;
        }
        
        if (checkProximity(transform, currentMultiPath.transform))
        {
            var nextList = currentMultiPath.knoten.Where(x => x != null).ToList();
            currentMultiPath = nextList[Random.Range(0, nextList.Count)];
        }
        else
        {
            moveTowards(currentMultiPath.transform);
        }
    }
    
    private MultiPathPoint findMultiPathPointInReach()
    {
        foreach (var point in GameObject.FindGameObjectsWithTag("MultiPath"))
        {
            if (checkIfReachableDirectlyByDistance(point.transform))
            {
                return point.GetComponent<MultiPathPoint>();
            }
        }

        return null;
    }
    
    private bool checkIfReachableDirectlyByDistance(Transform other)
    {
        float puffer = (collisionBox.radius + orientationPuffer);
        Vector3 distance = (other.position - transform.position);
        
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, collisionBox.radius, distance, raycastCap);

        Debug.DrawLine(transform.position, transform.position + distance.normalized * (hit.distance + puffer), Color.red, 0.1F);
        
        return hit.distance + puffer > distance.magnitude || hit.collider == null;
    }

    private bool checkProximity(Transform pointA, Transform pointB)
    {
        return checkProximity(pointA, pointB, proximityDelta);
    }
    
    private bool checkProximity(Transform pointA, Transform pointB, float delta)
    {
        return Vector2.Distance(pointA.position, pointB.position) <= delta;
    }
    
    private void moveTowards(Transform target)
    {
        direction = (target.position - transform.position).normalized;
        transform.up = direction;
        rb2d.MovePosition(rb2d.position + direction.normalized * (speed * Time.fixedDeltaTime));
    }
    
    private void setNextPathPoint()
    {
        if (isPathingTowardsNext)
        {
            isPathingTowardsNext = currentWayPoint.NextPathPoint != null;
            currentWayPoint = currentWayPoint.NextPathPoint ?? currentWayPoint.LastPathPoint;
        }
        else
        {
            isPathingTowardsNext = currentWayPoint.LastPathPoint == null;
            currentWayPoint = currentWayPoint.LastPathPoint ?? currentWayPoint.NextPathPoint;
        }
    }
}
