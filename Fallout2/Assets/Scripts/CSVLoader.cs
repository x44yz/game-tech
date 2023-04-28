using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using QuickDemo;
using LumenWorks.Framework.IO.Csv;

public interface ICSVParser
{
    void ParseCSV(CSVLoader loader);
}

// like:| STR_0 | STR_1 |
//      |  7    |   6   |
public interface ICSVPrefixParser
{
    void ParseCSV(CSVLoader loader, string prefix);
}

// like: STR|7|HEA|6
public interface ICSVStrParser
{
    void ParseStr(string[] values);
}

// like: STR:7;HEA:6
public interface ICSVDictParser
{
    void ParseDict(Dictionary<string, string> dict);
}

public class CSVLoader
{
    public string[] headers;
    public string[] values;
    public long curRowIndex;
    public int fieldCount;

    public static List<T> LoadCSV<T>(string asset) where T : ICSVParser
    {
        var loader = new CSVLoader();
        return loader.LoadCSVImpl<T>(asset);
    }

    public List<T> LoadCSVImpl<T>(string asset) where T : ICSVParser
    {
        var texts = AssetMgr.Inst.LoadAsset<TextAsset>(asset, false);
        if (texts == null)
        {
            Debug.LogError($"[CSV]load csv failed because load asset null > asset");
            return null;
        }

        var csvText = Encoding.UTF8.GetString(texts.bytes);
        if (string.IsNullOrEmpty(csvText))
        {
            return null;
        }

        TextReader reader = new StringReader(csvText);
        using (var csv = new CsvReader(reader, true))
        {
            fieldCount = csv.FieldCount;
            headers = csv.GetFieldHeaders();

            var result = new List<T>();
            while (csv.ReadNextRecord())
            {
                curRowIndex = csv.CurrentRecordIndex;
                values = new string[fieldCount];
                csv.CopyCurrentRecordTo(values);

                var obj = (T)Activator.CreateInstance(typeof(T));
                obj.ParseCSV(this);
                result.Add(obj);
            }
            return result;
        }
    }

    public bool HasField(string fieldName)
    {
        return GetFieldValue(fieldName, false) != null;
    }

    public bool HasFiledValue(string fieldName)
    {
        return !string.IsNullOrEmpty(GetFieldValue(fieldName, false));
    }

    public string GetFieldValue(string fieldName, bool log = true)
    {
        string result = null;
        var index = -1;
        if (headers != null && headers.Length > 0 && !string.IsNullOrEmpty(fieldName))
        {
            for (int i = 0, count = headers.Length; i < count; i++)
            {
                var head = headers[i];
                if (string.Equals(fieldName, head))
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0 && index < values.Length)
            {
                result = values[index];
            }
            else if (log)
            {
                Debug.LogError($"[CSV]cant find fieldName {fieldName}");
            }
        }
        return result;
    }

    public float ReadFloat(string fieldName, float? defaultValue = null)
    {
        var value = GetFieldValue(fieldName, defaultValue == null);
        return Utils.ToFloat(value, defaultValue ?? 0f);
    }

    public float? ReadFloatNull(string fieldName)
    {
        var value = GetFieldValue(fieldName, false);
        if (string.IsNullOrEmpty(value)) return null;
        return Utils.ToFloat(value);
    }

    public int? ReadIntNull(string fieldName)
    {
        var value = GetFieldValue(fieldName, false);
        if (string.IsNullOrEmpty(value)) return null;
        return Utils.ToInt(value);
    }

    public int ReadInt(string fieldName, int? defaultValue = null)
    {
        var value = GetFieldValue(fieldName, defaultValue == null);
        return Utils.ToInt(value, defaultValue ?? 0);
    }

    public uint ReadUInt(string fieldName, uint? defaultValue = null)
    {
        var value = GetFieldValue(fieldName, defaultValue == null);
        return Utils.ToUInt(value, defaultValue ?? 0);
    }

    public string ReadString(string fieldName, string defaultValue = null)
    {
        var value = GetFieldValue(fieldName, defaultValue == null);
        if (string.IsNullOrEmpty(value)) return defaultValue;
        return value;
    }

    public string[] ReadStringArray(string fieldName, char inSplitChar = ';')
    {
        var value = GetFieldValue(fieldName);
        // TODO: opt
        value = value.Replace($"{inSplitChar}\n", $"{inSplitChar}");
        var result = value?.Split(new char[] { inSplitChar }, StringSplitOptions.RemoveEmptyEntries);
        return result;
    }

    public uint[] ReadUIntArray(string fieldName, char inSplitChar = ';')
    {
        var value = GetFieldValue(fieldName);
        uint[] result;
        if (string.IsNullOrEmpty(value))
        {
            result = new uint[0];
        }
        else
        {
            var strResult = value.Split(inSplitChar);
            result = new uint[strResult.Length];
            for (var i = 0; i < strResult.Length; i++)
            {
                result[i] = Utils.ToUInt(strResult[i]);
            }
        }
        return result;
    }

    public int[] ReadIntArray(string fieldName, char inSplitChar = ';')
    {
        var value = GetFieldValue(fieldName);
        int[] result;
        if (string.IsNullOrEmpty(value))
        {
            result = new int[0];
        }
        else
        {
            var strResult = value.Split(inSplitChar);
            result = new int[strResult.Length];
            for (var i = 0; i < strResult.Length; i++)
            {
                result[i] = Utils.ToInt(strResult[i]);
            }
        }
        return result;
    }

    public Color ReadColor(string fieldName, Color defaultValue = new Color())
    {
        Color c = defaultValue;
        int[] rgba = ReadIntArray(fieldName);
        if (rgba.Length > 0)
        {
            c.r = rgba[0] / 255.0f;
        }
        if (rgba.Length > 1)
        {
            c.g = rgba[1] / 255.0f;
        }
        if (rgba.Length > 2)
        {
            c.b = rgba[2] / 255.0f;
        }
        if (rgba.Length > 3)
        {
            c.a = rgba[3] / 255.0f;
        }
        return c;
    }

    public float[] ReadFloatArray(string fieldName, float defaultValue = 0, char inSplitChar = ';')
    {
        var value = GetFieldValue(fieldName);
        float[] result;
        if (string.IsNullOrEmpty(value))
        {
            result = new float[0];
        }
        else
        {
            var strResult = value.Split(inSplitChar);
            int len = strResult.Length;
            result = new float[len];
            for (var i = 0; i < len; i++)
            {
                result[i] = Utils.ToFloat(strResult[i]);
            }
        }
        return result;
    }

    public Vector3 ReadVector3(string fieldName, Vector3 defaultValue = new Vector3(), char inSplitChar = ';')
    {
        var value = GetFieldValue(fieldName);
        Vector3 result = Vector3.zero;
        if (string.IsNullOrEmpty(value))
        {
            result = defaultValue;
        }
        else
        {
            var strResult = value.Split(inSplitChar);
            int len = strResult.Length;
            for (var i = 0; i < len; i++)
            {
                result[i] = Utils.ToFloat(strResult[i]);
            }
        }
        return result;
    }

    public Vector3 ReadVector2(string fieldName, Vector2 defaultValue = new Vector2(), char inSplitChar = ';')
    {
        var value = GetFieldValue(fieldName);
        Vector2 result = Vector2.zero;
        if (string.IsNullOrEmpty(value))
        {
            result = defaultValue;
        }
        else
        {
            var strResult = value.Split(inSplitChar);
            int len = strResult.Length;
            for (var i = 0; i < len; i++)
            {
                result[i] = Utils.ToFloat(strResult[i]);
            }
        }
        return result;
    }

    public bool ReadBool(string fieldName, bool defaultValue = false)
    {
        var value = GetFieldValue(fieldName);
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }
        return Utils.ToBool(value, defaultValue);
    }

    public T ReadEnum<T>(string fieldName, T defaultVal = default(T), bool log = true)
    {
        Type enumType = typeof(T);
        var str = GetFieldValue(fieldName, log);
        if (string.IsNullOrEmpty(str))
        {
            if (log) Debug.LogError($"[CSV]failed read enum because {curRowIndex} - {fieldName} is null");
            return defaultVal;
        }

        try
        {
            return (T)Enum.Parse(enumType, str);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[CSV]failed read enum {enumType}-{curRowIndex}-{fieldName}-{str} > {ex}");
            return defaultVal;
        }
    }

    public T[] ReadEnumArray<T>(string fieldName, char inSplitChar = ';')
    {
        var value = GetFieldValue(fieldName);
        T[] result;
        if (string.IsNullOrEmpty(value))
        {
            result = new T[0];
        }
        else
        {
            Type enumType = typeof(T);
            var strResult = value.Split(inSplitChar);
            result = new T[strResult.Length];
            for (var i = 0; i < strResult.Length; i++)
            {
                result[i] = Utils.ToEnum<T>(strResult[i]);
            }
        }
        return result;
    }

    public static T ReadClassImpl<T>(string str, char split = ';') where T : ICSVStrParser
    {
        var strArrary = str?.Split(new char[] { split }, StringSplitOptions.RemoveEmptyEntries);
        if (strArrary == null || strArrary.Length == 0)
            return default(T);
        
        var tt = (T)Activator.CreateInstance(typeof(T));
        tt.ParseStr(strArrary);
        return tt;
    }

    public T ReadClass<T>(string fieldName, char inSplitChar = ';') where T : ICSVStrParser
    {
        var strArrary = ReadStringArray(fieldName, inSplitChar);
        if (strArrary == null || strArrary.Length == 0)
            return default(T);

        var tt = (T)Activator.CreateInstance(typeof(T));
        tt.ParseStr(strArrary);
        return tt;
    }

    public T[] ReadClassArray<T>(string fieldName, char split1 = ';', char split2 = ':') where T : ICSVStrParser
    {
        var strs = ReadStringArray(fieldName, split1);
        if (strs == null || strs.Length == 0)
            return null;

        T[] rt = new T[strs.Length];
        int idx = 0;
        for (int i = 0; i < strs.Length; ++i)
        {
            var strs2 = strs[i].Split(new char[]{split2}, StringSplitOptions.RemoveEmptyEntries);
            if (strs2 == null || strs2.Length == 0)
            {
                Debug.LogError($"[CSV]failed read class array > {fieldName} at index {i} - {strs[i]}");
                continue;
            }

            var t = (T)Activator.CreateInstance(typeof(T));
            t.ParseStr(strs2);
            rt[idx++] = t;
        }
        return rt;
    }

    public T ReadClassByDict<T>(string fieldName, char inSplitChar = ';', 
        char kvSplitChar = ':', Func<string, T> factory = null, 
        string factKey = null) where T : ICSVDictParser
    {
        var strArrary = ReadStringArray(fieldName, inSplitChar);
        if (strArrary == null || strArrary.Length == 0)
            return default(T);

        var dict = new Dictionary<string, string>();
        for (int i = 0; i < strArrary.Length; ++i)
        {
            var kv = strArrary[i].Split(new char[] { kvSplitChar }, StringSplitOptions.RemoveEmptyEntries);
            if (kv == null || kv.Length != 2)
            {
                Debug.LogError($"[CSV]failed ReadClass {fieldName} kv at index {i} > null");
                continue;
            }
            var key = kv[0];
            if (dict.ContainsKey(key))
            {
                Debug.LogError($"[CSV]failed ReadClass {fieldName} kv at index {i} > duplicated key {key}");
                continue;
            }
            dict[kv[0]] = kv[1];
        }

        T rt = default(T);
        if (factory == null || factKey == null)
        {
            rt = (T)Activator.CreateInstance(typeof(T));
        }
        else
        {
            Debug.Assert(dict.ContainsKey(factKey), $"[CSV]failed ReadClass {fieldName} > no factory key {factKey}");
            rt = factory.Invoke(dict[factKey]);
        }
        rt.ParseDict(dict);
        return rt;
    }

    public T ReadClass<T>() where T : ICSVParser
    {
        var tt = (T)Activator.CreateInstance(typeof(T));
        tt.ParseCSV(this);
        return tt;
    }

    public T ReadClass<T>(string prefix, string checkNullField) where T : ICSVPrefixParser
    {
        if (HasFiledValue(prefix + checkNullField) == false)
            return default(T);

        var tt = (T)Activator.CreateInstance(typeof(T));
        tt.ParseCSV(this, prefix);
        return tt;
    }

    public Dictionary<T1, T2> ReadDict<T1, T2>(string fieldName, char split = ';', char split2 = ':')
    {
        var strs = ReadStringArray(fieldName, split);
        if (strs == null || strs.Length == 0)
            return null;

        var rt = new Dictionary<T1, T2>();
        for (int i = 0; i < strs.Length; ++i)
        {
            var str2 = strs[i].Split(new char[]{split2}, StringSplitOptions.RemoveEmptyEntries);
            if (str2 == null || str2.Length != 2)
            {
                Debug.LogError($"[CSV] failed read dict {fieldName} at line {i} - {strs[i]}");
                continue;
            }

            var key = (T1)Convert.ChangeType(str2[0], typeof(T1));
            var val = (T2)Convert.ChangeType(str2[1], typeof(T2));
            rt[key] = val;
        }
        return rt;
    }
}
