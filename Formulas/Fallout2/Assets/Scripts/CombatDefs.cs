using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Attack {
    Object* attacker;
    int hitMode;
    Object* weapon;
    int attackHitLocation;
    int attackerDamage;
    int attackerFlags;
    int ammoQuantity;
    int criticalMessageId;
    Object* defender;
    int tile;
    int defenderHitLocation;
    int defenderDamage;
    int defenderFlags;
    int defenderKnockback;
    Object* oops;
    int extrasLength;
    Object* extras[EXPLOSION_TARGET_COUNT];
    int extrasHitLocation[EXPLOSION_TARGET_COUNT];
    int extrasDamage[EXPLOSION_TARGET_COUNT];
    int extrasFlags[EXPLOSION_TARGET_COUNT];
    int extrasKnockback[EXPLOSION_TARGET_COUNT];
};