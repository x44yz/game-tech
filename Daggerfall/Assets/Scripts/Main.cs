using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

public class Main : MonoBehaviour
{
    public static Main Inst;

    public string heroRace;
    public string heroClass;
    public string heroWeapon;

    public ActorRender heroRender;
    public ActorRender monsterRender;

    [Header("RUNTIME")]
    public Hero hero;
    public Monster monster;

    void Awake()
    {
        Inst = this;

        Races.Init();
        Classes.Init();
        Talents.Init();

        hero = new Hero();
        heroRender.actor = hero;

        monster = new Monster();
        monsterRender.actor = monster;
    }

    void Start()
    {
        
    }

    void Update()
    {
        float dt = Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // hero.Attack(monster);
            heroRender.Attack();
        }
    }
}
