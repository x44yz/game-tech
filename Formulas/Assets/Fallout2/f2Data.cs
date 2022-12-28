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
        public f2Item item;
        public int quantity; // 数量
    };

    // Represents inventory of the object.
    public class Inventory {
        public int length;
        public int capacity;
        public List<InventoryItem> items;
    };

    public class ObjectData {
        public Inventory inventory;
        public CritterObjectData critter;
        public int flags;
        // ItemObjectData item;
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
        public f2Unit whoHitMe; // obj_pud.combat_data.who_hit_me
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
            // ProtoItemAmmoData ammo;
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
        //     int pid; // pid
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
        public f2Unit attacker;
        public int hitMode;
        public f2Item weapon;
        public int attackHitLocation;
        public int attackerDamage;
        public int attackerFlags;
        public int ammoQuantity;
        public int criticalMessageId;
        public f2Unit defender;
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

    public static class f2Data
    {
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
