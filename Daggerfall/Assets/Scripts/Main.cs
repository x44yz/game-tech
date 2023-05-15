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
    public int heroLevel;
    public int enemyIndex;
    public ActorRender heroRender;
    public ActorRender monsterRender;

    [Header("RUNTIME")]
    public Hero hero;
    public Monster monster;

    void Awake()
    {
        Inst = this;

        // Races.Init();
        Classes.Init();
        Items.Init();
        Spells.Init();

        hero = heroRender.gameObject.AddComponent<Hero>();
        hero.AssignCharacter(heroLevel);
        heroRender.actor = hero;

        monster = monsterRender.gameObject.AddComponent<Monster>();
        monsterRender.actor = monster;
        
    }

    void Update()
    {
        float dt = Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hero.WeaponDamage(null, false, false, monster, Vector3.zero, Vector3.zero);
            // heroRender.Attack();
        }
    }
}
