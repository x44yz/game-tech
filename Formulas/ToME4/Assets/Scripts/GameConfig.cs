using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ItemId
{
    None = 0,
    // res
    Coin = 4,
}

[Flags]
public enum ItemType : byte
{
    None = 0,
    Resource = 1 << 0,
    Tool = 1 << 1,
    Weapon = 1 << 2,
    Armor = 1 << 3,
}

public static class ItemTypeEnumExt
{
    public static bool IsFlagIncluded(this ItemType flags, ItemType flag)
    {
        return (flags & flag) == flag;
    }
}

[Serializable]
public class ItemCfg
{
    public ItemId id;
    public string name;
    public string asset;
    public ItemType itemType;
    public int price;
}

[Serializable]
public class ActorCfg
{
    public int id;
    public string name;
    public string asset;
    public float walkSpeed;
}

[Serializable]
public class MonsterCfg
{
    public int id;
    public string name;
    public string asset;
    public float walkSpeed;
}

[CreateAssetMenu(fileName = "GameConfig", menuName = "GAME/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public const int INVALID_ID = -1;

    public static GameConfig Inst
    {
        get
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<GameConfig>("Assets/GameConfig.asset");
        }
    }

    public List<ItemCfg> itemCfgs = new List<ItemCfg>();
    public List<ActorCfg> actorCfgs = new List<ActorCfg>();
    public List<MonsterCfg> monsterCfgs = new List<MonsterCfg>();

    public ItemCfg GetItem(ItemId id)
    {
        var cfg = itemCfgs.Find(x => x.id == id);
        if (cfg == null)
            Debug.LogError("cant find item cfg > " + id);
        return cfg;
    }

    public ActorCfg GetActor(int id)
    {
        var cfg = actorCfgs.Find(x => x.id == id);
        if (cfg == null)
            Debug.LogError("cant find actor cfg > " + id);
        return cfg;
    }

    public MonsterCfg GetMonster(int id)
    {
        var cfg = monsterCfgs.Find(x => x.id == id);
        if (cfg == null)
            Debug.LogError("cant find monster cfg > " + id);
        return cfg;
    }
}
