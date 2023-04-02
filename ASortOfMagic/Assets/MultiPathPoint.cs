using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiPathPoint : MonoBehaviour
{
    public List<MultiPathPoint> knoten;
    
    private static Color pathColor = Color.green;
    void OnDrawGizmos()
    {
        foreach (MultiPathPoint point in knoten.Where(point => point))
        {
            Gizmos.color = pathColor;
            Gizmos.DrawLine(transform.position, point.transform.position);
        }
    }
}
