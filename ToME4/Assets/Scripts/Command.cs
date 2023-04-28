using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandId
{
    None = 0,
    Idle,
    Gather,
}

[Serializable]
public class Command
{
    public CommandId id;

    public virtual string name
    {
        get { return id.ToString(); }
    }
    public virtual string desc
    {
        get { return ""; }
    }
}

[Serializable]
public class CommandGather : Command
{
    public ItemId itemId;
    // if -1, dont stop gather
    public int itemCount;

    public override string desc
    {
        get { return $"Gather-{itemId}x{itemCount}"; }
    }

    public CommandGather(ItemId itemId, int itemCount = -1)
    {
        id = CommandId.Gather;
        this.itemId = itemId;
        this.itemCount = itemCount;
    }
}
