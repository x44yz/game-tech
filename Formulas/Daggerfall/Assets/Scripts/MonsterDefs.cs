using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entity monster careers.
/// </summary>
public enum MonsterCareers
{
    None = -1,
    Rat = 0,
    Imp = 1,
    Spriggan = 2,
    GiantBat = 3,
    GrizzlyBear = 4,
    SabertoothTiger = 5,
    Spider = 6,
    Orc = 7,
    Centaur = 8,
    Werewolf = 9,
    Nymph = 10,
    Slaughterfish = 11,
    OrcSergeant = 12,
    Harpy = 13,
    Wereboar = 14,
    SkeletalWarrior = 15,
    Giant = 16,
    Zombie = 17,
    Ghost = 18,
    Mummy = 19,
    GiantScorpion = 20,
    OrcShaman = 21,
    Gargoyle = 22,
    Wraith = 23,
    OrcWarlord = 24,
    FrostDaedra = 25,
    FireDaedra = 26,
    Daedroth = 27,
    Vampire = 28,
    DaedraSeducer = 29,
    VampireAncient = 30,
    DaedraLord = 31,
    Lich = 32,
    AncientLich = 33,
    Dragonling = 34,
    FireAtronach = 35,
    IronAtronach = 36,
    FleshAtronach = 37,
    IceAtronach = 38,
    Horse_Invalid = 39,             // Not used and no matching texture (294 missing). Crashes DF when spawned in-game.
    Dragonling_Alternate = 40,      // Another dragonling. Seems to work fine when spawned in-game.
    Dreugh = 41,
    Lamia = 42,
}

/// <summary>
/// Mobile affinity for resists/weaknesses, grouping, etc.
/// This could be extended into a set of flags for multi-affinity creatures.
/// </summary>
public enum MobileAffinity
{
    None,               // No special affinity
    Daylight,           // Daylight creatures (centaur, giant, nymph, spriggan, harpy, dragonling)
    Darkness,           // Darkness creatures (imp, gargoyle, orc, vampires, werecreatures)
    Undead,             // Undead monsters (skeleton, liches, zombie, mummy, ghosts)
    Animal,             // Animals (bat, rat, bear, tiger, spider, scorpion)
    Daedra,             // Daedra (daedroth, fire, frost, lord, seducer)
    Golem,              // Golems (flesh, fire, frost, iron)
    Water,              // Water creatures (dreugh, slaughterfish, lamia)
    Human,              // A human creature
}