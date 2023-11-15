using System;
using UnityEngine;
#if UNITY_EDITOR

[Serializable]
public class ConditionalFieldAttribute : PropertyAttribute
{
    string fieldName;
    string[] otherConditions;
    bool inverse;
    public string FieldName => fieldName;
    public string[] OtherConditions => otherConditions;
    public bool Inverse => inverse;

    public ConditionalFieldAttribute(string fieldName, bool inverse = false, params string [] otherConditions)
    { 
        this.fieldName = fieldName;
        this.otherConditions = otherConditions;
        this.inverse = inverse;
    }
}

#endif
