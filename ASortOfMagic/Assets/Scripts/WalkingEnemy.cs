using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    
    public float followingCoolDown = 4.0F;
    private float followingTimer = float.NegativeInfinity;

    private Vector3 lastPlayerPos;


    private const int PLAYERMASK = 1<<8;


    protected override void Awake()
    {
        base.Awake();
        currentWayPoint = startPoint;
    }

    protected void Pathing()
    {
        rb2d.velocity = Vector2.zero;
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

    private void skipWayPoints()
    {
        var saveCurrent = currentWayPoint;
        do
        {
            setNextPathPoint();
        } while (saveCurrent != currentWayPoint && !checkIfReachableDirectlyByDistance(currentWayPoint.transform));

        if (saveCurrent == currentWayPoint)
        {
            onMultiPath = true;
            currentMultiPath = findMultiPathPointInReach();
            //Insert here HMMM sound??
        }
    }
    
    protected void PathToNextWayPoint()
    {
        speed = baseSpeed;
        if (checkProximity(transform, currentWayPoint.transform))
        {
            setNextPathPoint();
        } else if (!checkIfReachableDirectlyByDistance(currentWayPoint.transform, PLAYERMASK))
        {
            skipWayPoints();
        }
        else
        {
            moveTowards(currentWayPoint.transform);
        }
    }

    protected void ToPlayerPathing()
    {
        // if (checkProximity(transform.position, lastPlayerPos, orientationPuffer))
        //     followingTimer = Single.NegativeInfinity;
        if (isInSight())
        {
            onMultiPath = false;
            lastPlayerPos = Gamemaster.Instance.player.transform.position;
            moveTowards(Gamemaster.Instance.player.transform);
            followingTimer = Time.time;
            return;

        }
        if (Time.time <= followingTimer + followingCoolDown && checkIfReachableDirectlyByDistance(Gamemaster.Instance.player.transform, PLAYERMASK))
        {
            onMultiPath = false;
            lastPlayerPos = Gamemaster.Instance.player.transform.position;
            moveTowards(Gamemaster.Instance.player.transform);
            return;
        }
        if (Time.time <= followingTimer + followingCoolDown && !checkProximity(transform.position, lastPlayerPos, orientationPuffer) && checkIfReachableDirectlyByDistance(lastPlayerPos))
        {
            moveTowards(lastPlayerPos);
            if (checkProximity(transform.position, lastPlayerPos, orientationPuffer))
                followingTimer = Single.NegativeInfinity;
            return;
        }
        if (Time.time > followingTimer + followingCoolDown && onMultiPath == false)
        {
            onMultiPath = true;
            currentMultiPath = findMultiPathPointInReach();
        }
        TraverseRandomWaypoints();
    }

    protected void ToWayPointPathing()
    {
        if (checkIfReachableDirectlyByDistance(currentWayPoint.transform, PLAYERMASK))
        {
            moveTowards(currentWayPoint);
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
        if (currentMultiPath == null)
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
            if (!checkIfReachableDirectlyByDistance(currentMultiPath.transform, PLAYERMASK))
            {
                onMultiPath = true;
                currentMultiPath = findMultiPathPointInReach();
            }
            
            moveTowards(currentMultiPath);
        }
    }
    
    private MultiPathPoint findMultiPathPointInReach()
    {
        foreach (var point in GameObject.FindGameObjectsWithTag("MultiPath"))
        {
            if (checkIfReachableDirectlyByDistance(point.transform, PLAYERMASK))
            {
                return point.GetComponent<MultiPathPoint>();
            }
        }

        return null;
    }

    private bool checkIfReachableDirectlyByDistance(Transform other, int layer = 1 << 2)
    {
        return checkIfReachableDirectlyByDistance(other.position, layer);
    }

    private bool checkIfReachableDirectlyByDistance(Vector3 other, int layer = 1<<2)
    {
        Vector2 distance = (new Vector2(other.x, other.y) - rb2d.position);
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, distance, raycastCap, ~(layer | 1<<2));

        Debug.DrawLine(transform.position, rb2d.position + distance.normalized * (hit.distance + orientationPuffer), Color.red, 0.1F);
        
        return hit.collider == null || hit.distance + orientationPuffer > distance.magnitude;
    }

    private bool checkProximity(Vector3 pointA, Vector3 pointB)
    {
        return checkProximity(pointA, pointB, proximityDelta);
    }
    
    private bool checkProximity(Transform pointA, Transform pointB)
    {
        return checkProximity(pointA.position, pointB.position, proximityDelta);
    }
    
    private bool checkProximity(Vector3 pointA, Vector3 pointB, float delta)
    {
        return Vector2.Distance(pointA, pointB) <= delta;
    }

    private void moveTowards(Component target)
    {
        if (target == null)
            return;
        moveTowards(target.transform.position);
    }

    private void moveTowards(Vector3 target)
    {
        if (checkProximity(transform.position, target))
            return;
        direction = (new Vector2(target.x, target.y) - rb2d.position).normalized;
        transform.up = direction;
        rb2d.velocity = (direction.normalized * (speed * Time.fixedDeltaTime) * 100);
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
