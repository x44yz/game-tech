using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

public class Main : MonoBehaviour
{
    public static Main Inst;

    public string heroRace;
    public string heroClass;

    public ActorRender heroRender;
    public ActorRender monsterRender;

    [Header("RUNTIME")]
    public Hero hero;

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
