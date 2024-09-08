using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

public class Main : MonoBehaviour
{
    public static Main Inst;

    // [Header("RUNTIME")]
    // public List<Actor> actors = new List<Actor>();
    // public List<Monster> monsters = new List<Monster>();

    public GameConfig gCfgs { get { return GameConfig.Inst; } }
    public GameData gDatas { get { return GameData.Inst; } }

    void Awake()
    {
        Inst = this;

        Races.Init();
        Talents.Init();
    }

    void Start()
    {
        
    }

    void Update()
    {
        float dt = Time.deltaTime;
    }
}
