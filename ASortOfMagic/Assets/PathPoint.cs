using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    [SerializeField, ThroughProperty(nameof(LastPathPoint))]
    public PathPoint _LastPathPoint;
    [SerializeField, ThroughProperty(nameof(NextPathPoint))]
    public PathPoint _NextPathPoint;
    public PathPoint NextPathPoint
    {
        get => _NextPathPoint;
        set
        {
            EditorUtility.SetDirty(this);
            if(_NextPathPoint)
                EditorUtility.SetDirty(_NextPathPoint);
            if(_LastPathPoint)
                EditorUtility.SetDirty(_LastPathPoint);
            if (_NextPathPoint)
            {
                if (value == null)
                {
                    _NextPathPoint._LastPathPoint = null;
                    _NextPathPoint = null;
                }
                else
                {
                    _NextPathPoint._LastPathPoint = null;
                    _NextPathPoint = value;
                    _NextPathPoint._LastPathPoint = this;
                }
            }
            else
            {
                _NextPathPoint = value;
                _NextPathPoint._LastPathPoint = this;
            }
        }
    }

    public PathPoint LastPathPoint
    {
        get => _LastPathPoint;
        set
        {
            EditorUtility.SetDirty(this);
            if(_NextPathPoint)
                EditorUtility.SetDirty(_NextPathPoint);
            if(_LastPathPoint)
                EditorUtility.SetDirty(_LastPathPoint);
            if (_LastPathPoint)
            {
                if (value == null)
                {
                    _LastPathPoint._NextPathPoint = null;
                    _LastPathPoint = null;
                }
                else
                {
                    _LastPathPoint._NextPathPoint = null;
                    _LastPathPoint = value;
                    _LastPathPoint._NextPathPoint = this;
                }
            }
            else
            {
                _LastPathPoint = value;
                _LastPathPoint._NextPathPoint = this;
            }
        }
    }


    private static Color pathColor = Color.blue;
    void OnDrawGizmos()
    {
        if (NextPathPoint != null)
        {
            Gizmos.color = pathColor;
            Gizmos.DrawLine(transform.position, NextPathPoint.transform.position);
        }
        if (LastPathPoint != null)
        {
            Gizmos.color = pathColor;
            Gizmos.DrawLine(transform.position, LastPathPoint.transform.position);
        }
    }
}
