using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

public class Main : MonoBehaviour
{
    public static Main Inst;

    [Header("RUNTIME")]
    public List<Actor> actors = new List<Actor>();
    public List<Monster> monsters = new List<Monster>();

    public GameConfig gCfgs { get { return GameConfig.Inst; } }
    public GameData gDatas { get { return GameData.Inst; } }

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        // // load actors
        // for (int i = 0; i < GameData.Inst.humanDatas.Count; ++i)
        // {
        //     var actorData = GameData.Inst.humanDatas[i];
        //     var actor = Human.Create(actorData);
        //     humans.Add(actor);
        // }
    }

    void Update()
    {
        float dt = Time.deltaTime;
    }
}
