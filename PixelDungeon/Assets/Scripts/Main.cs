using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main Inst;

    public Hero hero;
    public Mob mob;
    public int heroLvl;

    private void Awake() {
        Inst = this;
    }

    void Start()
    {
        hero.Init(heroLvl);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
