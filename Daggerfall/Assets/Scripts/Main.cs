using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickDemo;

public class Main : MonoBehaviour
{
    public static Main Inst;

    public string heroRace;
    public string heroClass;
    public Weapons heroWeapon;
    public WeaponMaterialTypes heroWeaponMat;
    public int heroLevel;
    public int enemyIndex;
    public ActorRender heroRender;
    public ActorRender monsterRender;
    public Missile misslePrefab;

    [Header("DEBUG")]
    public bool debugHeroState;
    public int debugHeroStrength;
    public int debugHeroIntelligence;
    public int debugHeroWillpower;
    public int debugHeroAgility;
    public int debugHeroEndurance;
    public int debugHeroPersonality;
    public int debugHeroSpeed;
    public int debugHeroLuck;

    public bool debugSkill;
    public int debugSkillMedical;
    public int debugSkillEtiquette;
    public int debugSkillStreetwise;
    public int debugSkillJumping;
    public int debugSkillOrcish;
    public int debugSkillHarpy;
    public int debugSkillGiantish;
    public int debugSkillDragonish;
    public int debugSkillNymph;
    public int debugSkillDaedric;
    public int debugSkillSpriggan;
    public int debugSkillCentaurian;
    public int debugSkillImpish;
    public int debugSkillLockpicking;
    public int debugSkillMercantile;
    public int debugSkillPickpocket;
    public int debugSkillStealth;
    public int debugSkillSwimming;
    public int debugSkillClimbing;
    public int debugSkillBackstabbing;
    public int debugSkillDodging;
    public int debugSkillRunning;
    public int debugSkillDestruction;
    public int debugSkillRestoration;
    public int debugSkillIllusion;
    public int debugSkillAlteration;
    public int debugSkillThaumaturgy;
    public int debugSkillMysticism;
    public int debugSkillShortBlade;
    public int debugSkillLongBlade;
    public int debugSkillHandToHand;
    public int debugSkillAxe;
    public int debugSkillBluntWeapon;
    public int debugSkillArchery;
    public int debugSkillCriticalStrike;

    [Header("RUNTIME")]
    public Hero hero;
    public Monster monster;

    void Awake()
    {
        Inst = this;

        UnityEngine.Random.InitState((int)Time.time);

        misslePrefab.gameObject.SetActive(false);

        // Races.Init();
        Classes.Init();
        Items.Init();
        Spells.Init();

        hero = heroRender.gameObject.AddComponent<Hero>();
        hero.gameObject.AddComponent<ActorEffect>();
        hero.AssignCharacter(heroLevel);
        hero.StatReroll();
        if (debugHeroState)
        {
            hero.stats.SetPermanentStatValue(Stats.Strength, debugHeroStrength);
            hero.stats.SetPermanentStatValue(Stats.Intelligence, debugHeroIntelligence);
            hero.stats.SetPermanentStatValue(Stats.Willpower, debugHeroWillpower);
            hero.stats.SetPermanentStatValue(Stats.Agility, debugHeroAgility);
            hero.stats.SetPermanentStatValue(Stats.Endurance, debugHeroEndurance);
            hero.stats.SetPermanentStatValue(Stats.Personality, debugHeroPersonality);
            hero.stats.SetPermanentStatValue(Stats.Speed, debugHeroSpeed);
            hero.stats.SetPermanentStatValue(Stats.Luck, debugHeroLuck);
        }
        if (debugSkill)
        {
            hero.dSkills.SetPermanentSkillValue(Skills.Medical, debugSkillMedical);
            hero.dSkills.SetPermanentSkillValue(Skills.Etiquette, debugSkillEtiquette);
            hero.dSkills.SetPermanentSkillValue(Skills.Streetwise, debugSkillStreetwise);
            hero.dSkills.SetPermanentSkillValue(Skills.Jumping, debugSkillJumping);
            hero.dSkills.SetPermanentSkillValue(Skills.Orcish, debugSkillOrcish);
            hero.dSkills.SetPermanentSkillValue(Skills.Harpy, debugSkillHarpy);
            hero.dSkills.SetPermanentSkillValue(Skills.Giantish, debugSkillGiantish);
            hero.dSkills.SetPermanentSkillValue(Skills.Dragonish, debugSkillDragonish);
            hero.dSkills.SetPermanentSkillValue(Skills.Nymph, debugSkillNymph);
            hero.dSkills.SetPermanentSkillValue(Skills.Daedric, debugSkillDaedric);
            hero.dSkills.SetPermanentSkillValue(Skills.Spriggan, debugSkillSpriggan);
            hero.dSkills.SetPermanentSkillValue(Skills.Centaurian, debugSkillCentaurian);
            hero.dSkills.SetPermanentSkillValue(Skills.Impish, debugSkillImpish);
            hero.dSkills.SetPermanentSkillValue(Skills.Lockpicking, debugSkillLockpicking);
            hero.dSkills.SetPermanentSkillValue(Skills.Mercantile, debugSkillMercantile);
            hero.dSkills.SetPermanentSkillValue(Skills.Pickpocket, debugSkillPickpocket);
            hero.dSkills.SetPermanentSkillValue(Skills.Stealth, debugSkillStealth);
            hero.dSkills.SetPermanentSkillValue(Skills.Swimming, debugSkillSwimming);
            hero.dSkills.SetPermanentSkillValue(Skills.Climbing, debugSkillClimbing);
            hero.dSkills.SetPermanentSkillValue(Skills.Backstabbing, debugSkillBackstabbing);
            hero.dSkills.SetPermanentSkillValue(Skills.Dodging, debugSkillDodging);
            hero.dSkills.SetPermanentSkillValue(Skills.Running, debugSkillRunning);
            hero.dSkills.SetPermanentSkillValue(Skills.Destruction, debugSkillDestruction);
            hero.dSkills.SetPermanentSkillValue(Skills.Restoration, debugSkillRestoration);
            hero.dSkills.SetPermanentSkillValue(Skills.Illusion, debugSkillIllusion);
            hero.dSkills.SetPermanentSkillValue(Skills.Alteration, debugSkillAlteration);
            hero.dSkills.SetPermanentSkillValue(Skills.Thaumaturgy, debugSkillThaumaturgy);
            hero.dSkills.SetPermanentSkillValue(Skills.Mysticism, debugSkillMysticism);
            hero.dSkills.SetPermanentSkillValue(Skills.ShortBlade, debugSkillShortBlade);
            hero.dSkills.SetPermanentSkillValue(Skills.LongBlade, debugSkillLongBlade);
            hero.dSkills.SetPermanentSkillValue(Skills.HandToHand, debugSkillHandToHand);
            hero.dSkills.SetPermanentSkillValue(Skills.Axe, debugSkillAxe);
            hero.dSkills.SetPermanentSkillValue(Skills.BluntWeapon, debugSkillBluntWeapon);
            hero.dSkills.SetPermanentSkillValue(Skills.Archery, debugSkillArchery);
            hero.dSkills.SetPermanentSkillValue(Skills.CriticalStrike, debugSkillCriticalStrike);
        }
        hero.EquipWeapon(heroWeapon, heroWeaponMat);
        heroRender.actor = hero;

        monster = monsterRender.gameObject.AddComponent<Monster>();
        monster.gameObject.AddComponent<ActorEffect>();
        monsterRender.actor = monster;


        MobileEnemy mobileEnemy = null;
        for (int i = 0; i < MobileEnemies.Enemies.Length; ++i)
        {
            if (MobileEnemies.Enemies[i].ID == enemyIndex)
            {
                mobileEnemy = MobileEnemies.Enemies[i];
                break;
            }
        }
        if (enemyIndex >= 0 && enemyIndex <= 42)
        {
            monster.EntityType = EntityTypes.EnemyMonster;
            monster.SetEnemyCareer(mobileEnemy, EntityTypes.EnemyMonster);
        }
        else if (enemyIndex >= 128 && enemyIndex <= 146)
        {
            monster.EntityType = EntityTypes.EnemyClass;
            monster.SetEnemyCareer(mobileEnemy, EntityTypes.EnemyClass);
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // hero.WeaponDamage(null, false, false, monster, Vector3.zero, Vector3.zero);
            // heroRender.Attack();
        }
    }

    public uint ToClassicDaggerfallTime()
    {
        return (uint)Time.time;
    }

    public void HeroAttack()
    {
        var weapon = hero.ItemEquipTable.GetItem(EquipSlots.RightHand);
        if (weapon == null)
            weapon = hero.ItemEquipTable.GetItem(EquipSlots.LeftHand);

        WeaponTypes weaponType = WeaponTypes.Melee;
        if (weapon != null)
            weaponType = ItemUtils.ConvertItemToAPIWeaponType(weapon);

        if (weaponType != WeaponTypes.Bow)
            hero.MeleeDamage(weapon);
        else
        {
            // Missile missile = Instantiate(misslePrefab);
            // if (missile)
            // {
            //     missile.gameObject.SetActive(true);
            //     // Remove arrow
            //     // ItemCollection playerItems = playerEntity.Items;
            //     // DaggerfallUnityItem arrow = playerItems.GetItem(ItemGroups.Weapons, (int)Weapons.Arrow, allowQuestItem: false, priorityToConjured: true);
            //     // bool isArrowSummoned = arrow.IsSummoned;
            //     // playerItems.RemoveOne(arrow);

            //     missile.Caster = hero;
            //     missile.TargetType = TargetTypes.SingleTargetAtRange;
            //     missile.ElementType = ElementTypes.None;
            //     missile.IsArrow = true;
            //     missile.IsArrowSummoned = false; // 非魔法召唤的箭会添加到被攻击方的物品里

            //     hero.lastBowUsed = weapon;
            // }
            hero.WeaponDamage(weapon, true, false, monster, Vector3.zero, Vector3.zero);
        }

        heroRender.Attack();
    }

    public void MonsterAttack()
    {
        monster.MeleeDamage(hero);
        monsterRender.Attack();
    }
}
