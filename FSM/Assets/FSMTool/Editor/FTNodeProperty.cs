using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class FTNodeProperty
{
    public const string IntType = "int";
    public const string FloatType = "float";
    public const string StringType = "string";

    public static readonly string[] SupportTypes = new string[]
    {
        IntType, StringType, FloatType,
    };

    public FTNodeProperty()
    {
    }

    public FTNodeProperty(string key, int value)
    {
        this.ValueType = IntType;
        this.Key = key;
        this.IntValue = value;
    }

    public FTNodeProperty(string key, float value)
    {
        this.ValueType = FloatType;
        this.Key = key;
        this.FloatValue = value;
    }

    public FTNodeProperty(string key, string value)
    {
        this.ValueType = StringType;
        this.Key = key;
        this.StringValue = value;
    }

    // this will be seriablied
    [SerializeField]private string valueType = "";
    [SerializeField]private string key;
    [SerializeField]private string value;

    public string ValueType
    {
        get { return valueType; }
        set 
        {
            if (valueType == value)
                return;

            valueType = value;
            // reset value
            if (valueType == IntType) IntValue = 0;
            else if (valueType == FloatType) FloatValue = 0f;
            else if (valueType == StringType) StringValue = "value";
            else
                Debug.LogError("FTNodeProperty.ValueType cant implement valueType > " + valueType);
        }
    }

    public string Key
    {
        get { return key; }
        set { 
            key = value; 
        }
    }

    public object Value
    {
        get
        {
            if (valueType == IntType) return IntValue;
            else if (valueType == FloatType) return (double)FloatValue;
            else if (valueType == StringType) return StringValue;
            else
                Debug.LogError("FTNodeProperty.Value cant implement valueType > " + valueType);
            return "null";
        }
    }

    public int IntValue
    {
        get { return Convert.ToInt32(value); }
        set {
            this.value = Convert.ToString(value);
        }
    }

    public float FloatValue
    {
        get { return Convert.ToSingle(value);; }
        set {
            this.value = Convert.ToString(value);
        }
    }

    public string StringValue
    {
        get { return value; }
        set {
            this.value = value;
        }
    }

    public FTNodeProperty Clone()
    {
        FTNodeProperty np = new FTNodeProperty();
        np.valueType = this.valueType;
        np.key = this.key;
        np.value = this.value;
        return np;
    }
}
