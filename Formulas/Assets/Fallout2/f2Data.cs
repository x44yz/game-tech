using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using WorldTilePosition = UnityEngine.Vector2Int;

namespace f2
{
    public class PerkRankData 
    {
        public int[] ranks = new int[(int)Perk.PERK_COUNT];
    };

    public class InventoryItem {
        public f2Object item;
        public int quantity; // 数量
    };

    // Represents inventory of the object.
    public class Inventory {
        public int length;
        public int capacity;
        public List<InventoryItem> items;
    };

    public struct WeaponObjectData {
        public int ammoQuantity; // obj_pudg.pudweapon.cur_ammo_quantity
        public int ammoTypePid; // obj_pudg.pudweapon.cur_ammo_type_pid
    };

    public struct AmmoItemData {
        public int quantity; // obj_pudg.pudammo.cur_ammo_quantity
    };

    public struct MiscItemData {
        public int charges; // obj_pudg.pudmisc_item.curr_charges
    };

    public struct KeyItemData {
        public int keyCode; // obj_pudg.pudkey_item.cur_key_code
    };

    public class ItemObjectData {
        public WeaponObjectData weapon;
        public AmmoItemData ammo;
        public MiscItemData misc;
        public KeyItemData key;
    };

    public class ObjectData {
        public Inventory inventory;
        public CritterObjectData critter;
        public int flags;
        public ItemObjectData item;
        // SceneryObjectData scenery;
        // MiscObjectData misc;
    };

    public class CritterCombatData 
    {
        public int maneuver; // obj_pud.combat_data.maneuver
        public int ap; // obj_pud.combat_data.curr_mp
        public int results; // obj_pud.combat_data.results
        public int damageLastTurn; // obj_pud.combat_data.damage_last_turn
        public int aiPacket; // obj_pud.combat_data.ai_packet
        public int team; // obj_pud.combat_data.team_num
        public f2Object whoHitMe; // obj_pud.combat_data.who_hit_me
        public int whoHitMeCid;
    };

    public class CritterObjectData 
    {
        public int field_0; // obj_pud.reaction_to_pc
        public CritterCombatData combat; // obj_pud.combat_data
        public int hp; // obj_pud.curr_hp
        public int radiation; // obj_pud.curr_rad
        public int poison; // obj_pud.curr_poison
    };

    public class CritterProtoData 
    {
        public int flags; // d.flags
        public int[] baseStats = new int[35]; // d.stat_base
        public int[] bonusStats = new int[35]; // d.stat_bonus
        public int[] skills = new int[18]; // d.stat_points
        public int bodyType; // d.body
        public int experience;
        public int killType;
        // Looks like this is the "native" damage type when critter is unarmed.
        public int damageType;
    };

    public class CritterProto 
    {
        public int pid; // pid
        public int messageId; // message_num
        public int fid; // fid
        public int lightDistance; // light_distance
        public int lightIntensity; // light_intensity
        public int flags; // flags
        public int extendedFlags; // flags_ext
        public int sid; // sid
        public CritterProtoData data; // d
        public int headFid; // head_fid
        public int aiPacket; // ai_packet
        public int team; // team_num
    };

    public struct ProtoItemWeaponData{
        public int animationCode; // d.animation_code
        public int minDamage; // d.min_damage
        public int maxDamage; // d.max_damage
        public int damageType; // d.dt
        public int maxRange1; // d.max_range1
        public int maxRange2; // d.max_range2
        public int projectilePid; // d.proj_pid
        public int minStrength; // d.min_st
        public int actionPointCost1; // d.mp_cost1
        public int actionPointCost2; // d.mp_cost2
        public int criticalFailureType; // d.crit_fail_table
        public int perk; // d.perk
        public int rounds; // d.rounds
        public int caliber; // d.caliber
        public int ammoTypePid; // d.ammo_type_pid
        public int ammoCapacity; // d.max_ammo
        public byte soundCode; // d.sound_id
    };

    public struct ProtoItemAmmoData {
        public int caliber; // d.caliber
        public int quantity; // d.quantity
        public int armorClassModifier; // d.ac_adjust
        public int damageResistanceModifier; // d.dr_adjust
        public int damageMultiplier; // d.dam_mult
        public int damageDivisor; // d.dam_div
    };

    public struct ItemProtoData {
        // union {
        //     struct {
        //         int field_0;
        //         int field_4;
        //         int field_8; // max charges
        //         int field_C;
        //         int field_10;
        //         int field_14;
        //         int field_18;
        //     } unknown;
            // ProtoItemArmorData armor;
            // ProtoItemContainerData container;
            // ProtoItemDrugData drug;
        public ProtoItemWeaponData weapon;
        public ProtoItemAmmoData ammo;
            // ProtoItemMiscData misc;
            // ProtoItemKeyData key;
        // };
    };

    public struct ItemProto {
        public int pid; // pid
        public int messageId; // message_num
        public int fid; // fid
        public int lightDistance; // light_distance
        public int lightIntensity; // light_intensity
        public int flags; // flags
        public int extendedFlags; // flags_ext
        public int sid; // sid
        public int type; // type
        public ItemProtoData data; // d
        public int material; // material
        public int size; // size
        public int weight; // weight
        public int cost; // cost
        public int inventoryFid; // inv_fid
        public byte field_80;
    };

    public class Proto 
    {
        // struct 
        // {
        public int pid; // pid
        //     int messageId; // message_num
        //     int fid; // fid

        //     // TODO: Move to NonTile props?
        //     int lightDistance;
        //     int lightIntensity;
        //     int flags;
        //     int extendedFlags;
        //     int sid;
        // };
        public ItemProto item;
        public CritterProto critter;
        // SceneryProto scenery;
        // WallProto wall;
        // TileProto tile;
        // MiscProto misc;
    };

    public class Attack 
    {
        public f2Object attacker;
        public int hitMode;
        public f2Object weapon;
        public int attackHitLocation;
        public int attackerDamage;
        public int attackerFlags;
        public int ammoQuantity;
        public int criticalMessageId;
        public f2Object defender;
        public int tile;
        public int defenderHitLocation;
        public int defenderDamage;
        public int defenderFlags;
        public int defenderKnockback;
        // public Object* oops;
        public int extrasLength;
        // public Object* extras[EXPLOSION_TARGET_COUNT];
        public int[] extrasHitLocation = new int[f2DEF.EXPLOSION_TARGET_COUNT];
        public int[] extrasDamage = new int[f2DEF.EXPLOSION_TARGET_COUNT];
        public int[] extrasFlags = new int[f2DEF.EXPLOSION_TARGET_COUNT];
        public int[] extrasKnockback = new int[f2DEF.EXPLOSION_TARGET_COUNT];
    }

    // Provides metadata about stats.
    public class StatDescription {
        public string name;
        public string description;
        public int frmId;
        public int minimumValue;
        public int maximumValue;
        public int defaultValue;

        public StatDescription(string name,
                            string description,
                            int frmId,
                            int minimumValue,
                            int maximumValue,
                            int defaultValue)
        {
            this.name = name;
            this.description = description;
            this.frmId = frmId;
            this.minimumValue = minimumValue;
            this.maximumValue = maximumValue;
            this.defaultValue = defaultValue;
        }
    };

    public struct SkillDescription 
    {
        public string name;
        public string description;
        public string attributes;
        public int frmId;
        public int defaultValue;
        public int statModifier;
        public int stat1;
        public int stat2;
        public int field_20;
        public int experience;
        public int field_28;

        public SkillDescription(string name,
                            string description,
                            string attributes,
                            int frmId,
                            int defaultValue,
                            int statModifier,
                            int stat1,
                            int stat2,
                            int field_20,
                            int experience,
                            int field_28)
        {
            this.name = name;
            this.description = description;
            this.attributes = attributes;
            this.frmId = frmId;
            this.defaultValue = defaultValue;
            this.statModifier = statModifier;
            this.stat1 = stat1;
            this.stat2 = stat2;
            this.field_20 = field_20;
            this.experience = experience;
            this.field_28 = field_28;
        }
    };
 

    public static class f2Data
    {
        public static SkillDescription[] skill_data = new SkillDescription[(int)Skill.SKILL_COUNT] {
                new SkillDescription( "", "", "", 28, 5, 4, (int)Stat.STAT_AGILITY, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 29, 0, 2, (int)Stat.STAT_AGILITY, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 30, 0, 2, (int)Stat.STAT_AGILITY, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 31, 30, 2, (int)Stat.STAT_AGILITY, (int)Stat.STAT_STRENGTH, 1, 0, 0 ),
                new SkillDescription( "", "", "", 32, 20, 2, (int)Stat.STAT_AGILITY, (int)Stat.STAT_STRENGTH, 1, 0, 0 ),
                new SkillDescription( "", "", "", 33, 0, 4, (int)Stat.STAT_AGILITY, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 34, 0, 2, (int)Stat.STAT_PERCEPTION, (int)Stat.STAT_INTELLIGENCE, 1, 25, 0 ),
                new SkillDescription( "", "", "", 35, 5, 1, (int)Stat.STAT_PERCEPTION, (int)Stat.STAT_INTELLIGENCE, 1, 50, 0 ),
                new SkillDescription( "", "", "", 36, 5, 3, (int)Stat.STAT_AGILITY, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 37, 10, 1, (int)Stat.STAT_PERCEPTION, (int)Stat.STAT_AGILITY, 1, 25, 1 ),
                new SkillDescription( "", "", "", 38, 0, 3, (int)Stat.STAT_AGILITY, f2DEF.STAT_INVALID, 1, 25, 1 ),
                new SkillDescription( "", "", "", 39, 10, 1, (int)Stat.STAT_PERCEPTION, (int)Stat.STAT_AGILITY, 1, 25, 1 ),
                new SkillDescription( "", "", "", 40, 0, 4, (int)Stat.STAT_INTELLIGENCE, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 41, 0, 3, (int)Stat.STAT_INTELLIGENCE, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 42, 0, 5, (int)Stat.STAT_CHARISMA, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 43, 0, 4, (int)Stat.STAT_CHARISMA, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 44, 0, 5, (int)Stat.STAT_LUCK, f2DEF.STAT_INVALID, 1, 0, 0 ),
                new SkillDescription( "", "", "", 45, 0, 2, (int)Stat.STAT_ENDURANCE, (int)Stat.STAT_INTELLIGENCE, 1, 100, 0 ),
            };

        public static readonly StatDescription[] stat_data = new StatDescription[]{
            new StatDescription( "", "", 0, f2DEF.PRIMARY_STAT_MIN, f2DEF.PRIMARY_STAT_MAX, 5 ),
            new StatDescription( "", "", 1, f2DEF.PRIMARY_STAT_MIN, f2DEF.PRIMARY_STAT_MAX, 5 ),
            new StatDescription( "", "", 2, f2DEF.PRIMARY_STAT_MIN, f2DEF.PRIMARY_STAT_MAX, 5 ),
            new StatDescription( "", "", 3, f2DEF.PRIMARY_STAT_MIN, f2DEF.PRIMARY_STAT_MAX, 5 ),
            new StatDescription( "", "", 4, f2DEF.PRIMARY_STAT_MIN, f2DEF.PRIMARY_STAT_MAX, 5 ),
            new StatDescription( "", "", 5, f2DEF.PRIMARY_STAT_MIN, f2DEF.PRIMARY_STAT_MAX, 5 ),
            new StatDescription( "", "", 6, f2DEF.PRIMARY_STAT_MIN, f2DEF.PRIMARY_STAT_MAX, 5 ),
            new StatDescription( "", "", 10, 0, 999, 0 ),
            new StatDescription( "", "", 75, 1, 99, 0 ),
            new StatDescription( "", "", 18, 0, 999, 0 ),
            new StatDescription( "", "", 31, 0, int.MaxValue, 0 ),
            new StatDescription( "", "", 32, 0, 500, 0 ),
            new StatDescription( "", "", 20, 0, 999, 0 ),
            new StatDescription( "", "", 24, 0, 60, 0 ),
            new StatDescription( "", "", 25, 0, 30, 0 ),
            new StatDescription( "", "", 26, 0, 100, 0 ),
            new StatDescription( "", "", 94, -60, 100, 0 ),
            new StatDescription( "", "", 0, 0, 100, 0 ),
            new StatDescription( "", "", 0, 0, 100, 0 ),
            new StatDescription( "", "", 0, 0, 100, 0 ),
            new StatDescription( "", "", 0, 0, 100, 0 ),
            new StatDescription( "", "", 0, 0, 100, 0 ),
            new StatDescription( "", "", 0, 0, 100, 0 ),
            new StatDescription( "", "", 0, 0, 100, 0 ),
            new StatDescription( "", "", 22, 0, 90, 0 ),
            new StatDescription( "", "", 0, 0, 90, 0 ),
            new StatDescription( "", "", 0, 0, 90, 0 ),
            new StatDescription( "", "", 0, 0, 90, 0 ),
            new StatDescription( "", "", 0, 0, 90, 0 ),
            new StatDescription( "", "", 0, 0, 100, 0 ),
            new StatDescription( "", "", 0, 0, 90, 0 ),
            new StatDescription( "", "", 83, 0, 95, 0 ),
            new StatDescription( "", "", 23, 0, 95, 0 ),
            new StatDescription( "", "", 0, 16, 101, 25 ),
            new StatDescription( "", "", 0, 0, 1, 0 ),
            new StatDescription( "", "", 10, 0, 2000, 0 ),
            new StatDescription( "", "", 11, 0, 2000, 0 ),
            new StatDescription( "", "", 12, 0, 2000, 0 ),
        };
    }
}
