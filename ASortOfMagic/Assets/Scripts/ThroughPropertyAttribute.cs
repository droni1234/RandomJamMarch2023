using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited =  true)]
public class ThroughPropertyAttribute : PropertyAttribute
{
    public string propertySourceField;
    public ThroughPropertyAttribute(string propertySourceField)
    {
        this.propertySourceField = propertySourceField;
    }
}