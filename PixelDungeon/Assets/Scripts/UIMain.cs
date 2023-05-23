using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public UIWidget panelHUD;

    void Start()
    {
        InitPanelHUD();
    }

    void Update()
    {
        
    }

    void InitPanelHUD()
    {
        panelHUD.Get<Button>("BtnHeroAttack").onClick.AddListener(()=>{
            Debug.Log("xx-- Hero Attack");
            // Main.Inst.HeroAttack();
        });
        panelHUD.Get<Button>("BtnHeroSpell").onClick.AddListener(()=>{
            Debug.Log("xx-- Hero Spell");
            // Main.Inst.HeroSpell();
        });

        panelHUD.Get<Button>("BtnMonsterAttack").onClick.AddListener(()=>{
            Debug.Log("xx-- Monster Attack");
            // Main.Inst.MonsterAttack();
        });
    }
}
