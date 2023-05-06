using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public struct monster_race
{
    public int ac; /* Armour Class */
}

[Serializable]
public class Monster : Actor
{
    public monster_race race;
    public int hp;
}