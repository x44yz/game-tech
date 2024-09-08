using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using QuickDemo;

[Serializable]
public class ItemData
{
    public const int INVALID_UID = -1;

    public int uid;
    public ItemId id;
    public int count;
    public int markCostCount;
}

[Serializable]
public class ActorData
{
    public const int INVALID_UID = -1;

    public int uid;
    public int id;
}

[CreateAssetMenu(fileName = "GameData", menuName = "GAME/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public const int INVALID_UID = 0;

    private static GameData _inst = null;
    public static GameData Inst
    {
        get
        {
            if (_inst == null)
            {
                var kk = UnityEditor.AssetDatabase.LoadAssetAtPath<GameData>("Assets/GameData.asset");
                _inst = Instantiate(kk);
            }
            return _inst;
            // return UnityEditor.AssetDatabase.LoadAssetAtPath<GameData>("Assets/GameData.asset");
        }
    }

    public List<ItemData> itemDatas = new List<ItemData>();
    public List<ActorData> actorDatas = new List<ActorData>();

    public static int NewUid()
    {
        return Guid.NewGuid().GetHashCode();
    }

    public ActorData GetActor(int uid)
    {
        return actorDatas.Find(x => x.uid == uid);
    }

    public ActorData NewActor(int id)
    {
        var cfg = GameConfig.Inst.GetActor(id);

        ActorData dat = new ActorData();
        dat.uid = NewUid();
        dat.id = id;
        actorDatas.Add(dat);
        SetDataDirty();
        return dat;
    }

    public void RemoveActor(int uid)
    {
        var dat = GetActor(uid);
        if (dat != null)
        {
            actorDatas.Remove(dat);
            SetDataDirty();
        }
    }

    public ItemData GetItem(int uid) 
    {
        return itemDatas.Find(x => x.uid == uid);
    }

    public ItemData GetItem(ItemId id)
    {
        return itemDatas.Find(x => x.id == id);
    }

    public int GetItemCount(ItemId id)
    {
        int count = 0;
        for (int i = 0; i < itemDatas.Count; ++i)
        {
            if (itemDatas[i].id == id)
                count += itemDatas[i].count;
        }
        return count;
    }

    public ItemData AddItem(ItemId id, int count)
    {
        Debug.Log($"[DATA]add item > {id} - {count}");
        var itemCfg = GameConfig.Inst.GetItem(id);

        var itemData = GetItem(id);
        if (itemData != null)
        {
            itemData.count += count;
        }
        else
        {
            itemData = new ItemData();
            itemData.uid = NewUid();
            itemData.id = id;
            itemData.count = count;
            itemDatas.Add(itemData);
        }

        SetDataDirty();
        return itemData;
    }

    public bool RemoveItem(ItemId id, int count)
    {
        Debug.Log($"[DATA]remove item > {id} - {count}");
        var curCount = GetItemCount(id);
        if (curCount < count)
        {
            Debug.LogWarning($"cant remove item because not enough count > {curCount}/{count}");
            return false;
        }
        ItemData itemData = GetItem(id);
        itemData.count -= count;
        SetDataDirty();
        return true;
    }

    public void SetDataDirty()
    {
        return;
#if UNITY_EDITOR
        // EditorUtility.SetDirty(GameData.Inst);
#endif
    }
}