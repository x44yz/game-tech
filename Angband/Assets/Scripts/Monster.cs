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

public class Monster : Actor
{
    public monster_race race;
}