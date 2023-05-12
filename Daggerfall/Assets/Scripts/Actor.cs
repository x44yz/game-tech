using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;
using QuickDemo.FSM;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Actor : MonoBehaviour
{
    public virtual bool TakeHit(int value, Actor src)
    {
        throw new System.NotImplementedException();
    }

    public int GetStr()
    {
        throw new System.NotImplementedException();
    }
}