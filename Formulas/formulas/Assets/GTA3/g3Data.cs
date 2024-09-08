using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using WorldTilePosition = UnityEngine.Vector2Int;

namespace gta3
{
    // https://gtamods.com/wiki/Weapon.dat_(VC)

    public struct FightMove
    {
        public AnimationId animId;
        public float startFireTime;
        public float endFireTime;
        public float comboFollowOnTime;
        public float strikeRadius;
        public int hitLevel; // FightMoveHitLevel
        public int damage;
        public int flags;
    };

    public class CWeaponInfo 
    {
        public string m_Name;
        public eWeaponFire m_eWeaponFire;
        public float m_fRange;
        public int m_nFiringRate;
        public int m_nReload;
        public int m_nAmountofAmmunition;
        public int m_nDamage;
        public float m_fSpeed;
        public float m_fRadius;
        public float m_fLifespan;
        public float m_fSpread;
        public Vector3 m_vecFireOffset;
        public string m_AnimToPlay; // AnimationId
        public string m_Anim2ToPlay; // AnimationId
        public float m_fAnimLoopStart;
        public float m_fAnimLoopEnd;
        public float m_fAnimFrameFire;
        public float m_fAnim2FrameFire;
        public int m_nModelId;
        public int m_Flags;

        public CWeaponInfo(string Name,
                        eWeaponFire eWeaponFire,
                        float fRange,
                        int nFiringRate,
                        int nReload,
                        int nAmountofAmmunition,
                        int nDamage,
                        float fSpeed,
                        float fRadius,
                        float fLifespan,
                        float fSpread,
                        float vecFireOffsetX,
                        float vecFireOffsetY,
                        float vecFireOffsetZ,
                        string AnimToPlay, // AnimationId
                        float fAnimLoopStart,
                        float fAnimLoopEnd,
                        float fAnimFrameFire,
                        float fAnim2LoopStart,
                        float fAnim2LoopEnd,
                        float fAnim2FrameFire,
                        float fAnimFrameBreakout,
                        int nModelId,
                        int nModel2Id,
                        int Flags,
                        int weaponSlot)
        {
            m_Name = Name;
            m_eWeaponFire = eWeaponFire;
            m_fRange = fRange;
            m_nFiringRate = nFiringRate;
            m_nReload = nReload;
            m_nAmountofAmmunition = nAmountofAmmunition;
            m_nDamage = nDamage;
            m_fSpeed = fSpeed;
            m_fRadius = fRadius;
            m_fLifespan = fLifespan;
            m_fSpread = fSpread;
            m_vecFireOffset = new Vector3(vecFireOffsetX, vecFireOffsetY, vecFireOffsetZ);
            m_AnimToPlay = AnimToPlay;
            m_fAnimLoopStart = fAnimLoopStart;
            m_fAnimLoopEnd = fAnimLoopEnd;
            m_fAnimFrameFire = fAnimFrameFire;
            m_fAnim2FrameFire = fAnim2FrameFire;
            m_nModelId = nModelId;
            m_Flags = Flags;
        }

        // static void Initialise(void);
        // static void LoadWeaponData(void);
        // static CWeaponInfo *GetWeaponInfo(eWeaponType weaponType);
        // static eWeaponFire FindWeaponFireType(char *name);
        // static eWeaponType FindWeaponType(char *name);
        // static void Shutdown(void);
        // bool IsFlagSet(uint32 flag) const { return (m_Flags & flag) != 0; }

        public static CWeaponInfo GetWeaponInfo(string name)
        {
            foreach (var wp in weaponInfos)
            {
                if (wp.m_Name == name)
                    return wp;
            }
            Debug.LogError("cant get weapon info > " + name);
            return null;
        }

// #
// # Weapons data
// #	A: Weapon name
// #	B: Fire type
// #   C: Range
// #	D: Firing Rate
// #	E: Reload
// #	F: Amount of Ammunition
// #	G: Damage
// #	H: Speed
// #	I: Radius
// #	J: Life span
// #	K: Spread
// #	L,M,N: Fire offset vector
// #	P: animation to play
// #	Q: animation to play
// #	R: animation loop start
// #	S: animation loop end
// #	T: point in animation where weapon is fired
// #	U: animation2 loop start
// #	V: animation2 loop end
// #	W: point in animation2 where weapon is fired
// #	X: point in anim where we can breakout of anim/attack and run away
// #	Y: model id
// #	Z: model2 id
// #	a: Flags -> Stored in HEX... so (from right to left)
// #		1st digit	1:USE_GRAVITY  	2:SLOWS_DOWN  	4:DISSIPATES  	8:RAND_SPEED  
// #		2nd digit	1:EXPANDS  		2:EXPLODES  	4:CANAIM		8:CANAIM_WITHARM
// #		3rd digit	1:1ST_PERSON  	2:HEAVY  		4:THROW			8:RELOAD_LOOP2START
// #		4th digit	1:USE_2ND		2:GROUND_2ND	4:FINISH_3RD	8:RELOAD
// #		5th digit	1:FIGHTMODE		2:CROUCHFIRE	4:COP3_RD		8:GROUND_3RD
// #		6th digit	1:PARTIALATTACK 2:ANIMDETONATE
// #
// #   b: Weapon Slot (Group to which this weapon belongs to, different from Fire type)
// #
// ###### NOT USED IN THIS VERSION ##############
// #	a:	1st person version of anim (using orig HGUN & PUMP cause they work better at mo')
// #	b:	1st person vers of 2nd anim
// #
// #	FPS anims loaded (ie available):	FPS_PUNCH, FPS_BAT, FPS_UZI, FPS_PUMP, FPS_AK, FPS_M16, FPS_ROCKET
// ##############################################
// #
// # A            	B           C     D    E    F    G    H   I     J      K    L	 M		N		P       	R  S  T  	U  V  W		X	 	Y	Y	  a			b
// Unarmed        	eWeaponFire.WEAPON_FIRE_MELEE       2.4	  250  100  1000 8   -1.0 0.6   -1.0   -1.0	0.1	 0.65	0.30	unarmed	   	0  99 6  	0  99 12	99		-1 	-1 	  102000 	0
// BrassKnuckle    eWeaponFire.WEAPON_FIRE_MELEE       2.0   250  100  1000 16  -1.0 0.8   -1.0   -1.0	0.1  0.8	0.30	unarmed		5  20 14 	3  17 11	99		259 -1 	  102000 	0
// ScrewDriver     eWeaponFire.WEAPON_FIRE_MELEE       1.8   250  100  1000 45  -1.0 0.5   -1.0   -1.0	0.0  0.7	0.20	screwdrv   	5  99 14 	3  17 11	99		260 -1 	  115000 	1
// GolfClub       	eWeaponFire.WEAPON_FIRE_MELEE       1.5   250  100  1000 21  -1.0 0.8   -1.0   -1.0	0.1  0.8	0.30	golfclub    5  20 16 	3  15 12	99		261 -1 	  102000 	1
// NightStick     	eWeaponFire.WEAPON_FIRE_MELEE       1.5   250  100  1000 21  -1.0 0.8   -1.0   -1.0	0.1  0.8	0.30	baseball    5  20 14 	3  17 11	99		262 -1 	  102000 	1
// Knife          	eWeaponFire.WEAPON_FIRE_MELEE       1.8   250  100  1000 21  -1.0 0.5   -1.0   -1.0	0.0  0.8	0.20	knife       5  99 14 	3  17 11	99		263 -1 	  115000 	1
// BaseballBat    	eWeaponFire.WEAPON_FIRE_MELEE       2.0   250  100  1000 21  -1.0 0.8   -1.0   -1.0	0.1  0.8	0.30	baseball    5  20 16 	3  17 11	99		264 -1 	  102000	1
// Hammer	       	eWeaponFire.WEAPON_FIRE_MELEE       1.5   250  100  1000 21  -1.0 0.8   -1.0   -1.0	0.1  0.8	0.30	baseball    5  20 14 	3  17 11	99		265 -1 	  102000	1
// Cleaver	       	eWeaponFire.WEAPON_FIRE_MELEE       1.9   250  100  1000 24  -1.0 0.6   -1.0   -1.0	0.0  0.9	0.20	knife       5  99 14 	3  17 11	99		266 -1 	  115000	1
// Machete	       	eWeaponFire.WEAPON_FIRE_MELEE       2.0   250  100  1000 24  -1.0 0.6   -1.0   -1.0	0.0  1.0	0.20	knife       5  99 14 	3  17 11	99		267 -1 	  115000 	1
// Katana         	eWeaponFire.WEAPON_FIRE_MELEE       2.1   250  100  1000 30  -1.0 0.7   -1.0   -1.0	0.0  1.2	0.30	knife       5  99 14 	3  17 11	99		268 -1 	  115000 	1
// Chainsaw       	eWeaponFire.WEAPON_FIRE_MELEE       1.7   250  100  1000 35  -1.0 0.55  -1.0   -1.0	0.0  1.3	0.10	chainsaw    5  35 30 	3  99 48	99		269 -1 	  102000 	1

// Grenade        	eWeaponFire.WEAPON_FIRE_PROJECTILE,  30.0  100  1    1    75  0.25 -1.0  800.0  1.0	0.0	 0.0	0.0		grenade 	0  99 10 	0  99 6 	99		270 -1 	  2424		2
// DetonateGrenade eWeaponFire.WEAPON_FIRE_PROJECTILE,  30.0  100  1    1    75  0.25 -1.0  800.0  1.0	0.0	 0.0	0.0		grenade		0  99 10 	0  99 6		99		270	291	  2424		2
// TearGas         eWeaponFire.WEAPON_FIRE_PROJECTILE,  30.0  100  1    1    75  0.25 -1.0  800.0  1.0	0.0	 0.0	0.0		grenade 	0  99 10 	0  99 6 	99		271 -1 	  2424		2
// Molotov        	eWeaponFire.WEAPON_FIRE_PROJECTILE,  25.0  100  1    1    75  0.25 -1.0  2000.0 5.0	0.0	 0.0	0.0		grenade 	0  99 10 	0  99 6 	99		272 -1 	  2424		2
// Rocket        	eWeaponFire.WEAPON_FIRE_PROJECTILE,  30.0  100  1    1    75  0.25 -1.0  800.0  1.0	0.0	 0.0	0.0		unarmed		0  99 10 	0  99 6 	99		273 -1 	  2424		2

// Colt45         	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 30.0  250  450  17   25  -1.0 -1.0  -1.0   -1.0	0.30 0.0	0.09	colt45   	11 18 14  	11 18 12	99		274 -1	  680C0		3
// Python         	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 30.0  250  1000 6    135 -1.0 -1.0  -1.0   -1.0	0.41 0.03	0.12	python   	10 31 14 	14 37 16	40		275 -1	  28040		3

// Shotgun        	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 40.0  250  450	1    80  -1.0 -1.0  -1.0   -1.0	0.86 -0.02	0.28	shotgun     12 34 15 	9  26 13	30		277 -1 	  20040		4
// Spas12Shotgun   eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 30.0  250  750  7    100 -1.0 -1.0  -1.0   -1.0	0.85 -0.06	0.19	buddy       14 20 15 	14 20 14 	25		278 -1 	  20040		4
// StubbyShotgun   eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 15.0  250  750  1    120 -1.0 -1.0  -1.0   -1.0	0.55 -0.02	0.13	buddy       14 50 15 	14 56 15	25		279 -1 	  20040		4

// Tec9            eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 30.0  250  500  50   20  -1.0 -1.0  -1.0   -1.0	0.45 -0.05	0.11	colt45 		11 15 14  	11 15 12 	99		281 -1 	  280C0		5
// Uzi            	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 45.0  250  500  30   20  -1.0 -1.0  -1.0   -1.0	0.45  0.00	0.12	uzi		   	12 14 12  	11 13 12 	25		282 -1 	  28840		5
// SilencedIngram 	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 30.0  250  500  30   15  -1.0 -1.0  -1.0   -1.0	0.36  0.02	0.11	colt45 		11 13 13  	11 13 11 	99		283 -1 	  280C0		5
// Mp5            	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 45.0  250  500  30   35  -1.0 -1.0  -1.0   -1.0	0.51 -0.01	0.20	uzi		   	11 14 11  	11 14 12 	30		284 -1 	  28840		5

// m4  			eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 90.0  250  1000 30   40  -1.0 -1.0  -1.0   -1.0	0.88 -0.04	0.16	rifle       14 17 15 	14 17 15	99		280 -1 	  28050		6
// Ruger           eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 90.0  250  1000 30   35  -1.0 -1.0  -1.0   -1.0	1.00 -0.06	0.17	rifle   	12 17 14  	11 16 13 	99		276 -1 	  28040		6
// SniperRifle    	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 100.0 500  1401 1    125 -1.0 -1.0  -1.0   -1.0	0.0	 0.66	0.05	unarmed     12 15 14   	0  10 3 	99		285 -1 	  100		8
// LaserScope      eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 100.0 150  1401 7    125 -1.0 -1.0  -1.0   -1.0	0.0	 0.66	0.05	unarmed     2  8  3  	0  10 3 	99		286 -1 	  100		8

// RocketLauncher 	eWeaponFire.WEAPON_FIRE_PROJECTILE,  55.0  100  1200 1    75  2.0  -1.0  1000.0 1.0	0.42 0.0	0.05	unarmed     0  99 14 	0  99 14	99		287 -1	  324		7
// FlameThrower   	AREA_EFFECT 5.1   100  100  500  25  0.65 0.075 1000.0 3.0	1.30 0.0	0.56	flame       12 13 12  	6  7  7 	25		288 -1	  1E		7

// M60    	       	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 75.0  1    500  100  130 -1.0 -1.0  -1.0   -1.0	1.0  0.00	0.23	m60     	12 16 13  	10 12 10	30		289 -1	  8040 		7
// Minigun        	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 75.0  1    350  500  140 -1.0 -1.0  -1.0   -1.0	1.28 0.00	0.50	flame     	11 12 11 	11 12 11	35		290 294	  200		7

// Detonator      	eWeaponFire.WEAPON_FIRE_PROJECTILE,  25.0  100  1    1    0   2.0  -1.0  2000.0 5.0	0.0	 0.0	0.13	man		    0  10 3  	0  10 3  	99		291 -1	  200024	9
// HeliCannon	   	eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 100.0 1    100  150  100 -1.0 -1.0  -1.0   -1.0 1.00 0.00   0.23    m60			12 16 13 	10 14 12 	30		289 -1	  8040		7

// Camera    		CAMERA 		100.0 500  1401 36   0 	-1.0 -1.0  -1.0   -1.0	0.0	 0.66	0.05	unarmed     0  10 3  	0  10 3  	25		292 -1	  100		9

// ENDWEAPONDATA
    
        public static readonly CWeaponInfo[] weaponInfos = new CWeaponInfo[]{
            //               Name                            eWeaponFire            fRange  nFiringRate nReload nAmountofAmmunition nDamage  fSpeed   fRadius fLifespan fSpread   vecFireOffset  AnimToPlay fAnimLoopStart fAnimLoopEnd  fAnimFrameFire fAnim2LoopStart fAnim2LoopEnd  fAnim2FrameFire fAnimFrameBreakout nModelId nModel2Id Flags weaponSlot
            new CWeaponInfo("Unarmed",       eWeaponFire.WEAPON_FIRE_MELEE,       2.4f,  250,       100,         1000,                8,       -1.0f,     0.6f,  -1.0f,   -1.0f,  0.1f, 0.65f,  0.30f,	 "unarmed",	   	 0,            99,        6,                0,              99,              12,	         99,		   -1, 	      -1, 	  102000, 	  0),
            new CWeaponInfo("BrassKnuckle",   eWeaponFire.WEAPON_FIRE_MELEE,      2.0f,  250,       100,         1000,               16,       -1.0f,     0.8f,  -1.0f,   -1.0f,  0.1f,  0.8f,  0.30f,	 "unarmed",		 5,            20,       14, 	            3,              17,              11,	         99,		  259,        -1, 	  102000, 	  0),
            new CWeaponInfo("ScrewDriver",   eWeaponFire.WEAPON_FIRE_MELEE,       1.8f,  250,       100,         1000,               45,       -1.0f,     0.5f,  -1.0f,   -1.0f,  0.0f,  0.7f,  0.20f,	 "screwdrv",   	 5,            99,       14, 	            3,              17,              11,	         99,		  260,        -1, 	  115000, 	  1),
            new CWeaponInfo("GolfClub",       eWeaponFire.WEAPON_FIRE_MELEE,      1.5f,  250,       100,         1000,               21,       -1.0f,     0.8f,  -1.0f,   -1.0f,  0.1f,  0.8f,  0.30f,	 "golfclub",     5,            20,       16, 	            3,              15,              12,	         99,		  261,        -1, 	  102000,     1),
            new CWeaponInfo("NightStick",     eWeaponFire.WEAPON_FIRE_MELEE,      1.5f,  250,       100,         1000,               21,       -1.0f,     0.8f,  -1.0f,   -1.0f,  0.1f,  0.8f,  0.30f,	 "baseball",     5,            20,       14, 	            3,              17,              11,	         99,		  262,        -1, 	  102000,     1),
            new CWeaponInfo("Knife",          eWeaponFire.WEAPON_FIRE_MELEE,      1.8f,  250,       100,         1000,               21,       -1.0f,     0.5f,  -1.0f,   -1.0f,  0.0f,  0.8f,  0.20f,	 "knife",        5,            99,       14, 	            3,              17,              11,	         99,		  263,        -1, 	  115000,     1),
            new CWeaponInfo("BaseballBat",    eWeaponFire.WEAPON_FIRE_MELEE,      2.0f,  250,       100,         1000,               21,       -1.0f,     0.8f,  -1.0f,   -1.0f,  0.1f,  0.8f,  0.30f,	 "baseball",     5,            20,       16, 	            3,              17,              11,	         99,		  264,        -1, 	  102000,     1),
            new CWeaponInfo("Hammer",	      eWeaponFire.WEAPON_FIRE_MELEE,      1.5f,  250,       100,         1000,               21,       -1.0f,     0.8f,  -1.0f,   -1.0f,  0.1f,  0.8f,  0.30f,	 "baseball",     5,            20,       14, 	            3,              17,              11,	         99,		  265,        -1, 	  102000,     1),
            new CWeaponInfo("Cleaver",	      eWeaponFire.WEAPON_FIRE_MELEE,      1.9f,  250,       100,         1000,               24,       -1.0f,     0.6f,  -1.0f,   -1.0f,  0.0f,  0.9f,  0.20f,	 "knife",        5,            99,       14, 	            3,              17,              11,	         99,		  266,        -1, 	  115000,     1),
            new CWeaponInfo("Machete",	      eWeaponFire.WEAPON_FIRE_MELEE,      2.0f,  250,       100,         1000,               24,       -1.0f,     0.6f,  -1.0f,   -1.0f,  0.0f,  1.0f,  0.20f,	 "knife",        5,            99,       14, 	            3,              17,              11,	         99,		  267,        -1, 	  115000,     1),
            new CWeaponInfo("Katana",         eWeaponFire.WEAPON_FIRE_MELEE,      2.1f,  250,       100,         1000,               30,       -1.0f,     0.7f,  -1.0f,   -1.0f,  0.0f,  1.2f,  0.30f,	 "knife",        5,            99,       14, 	            3,              17,              11,	         99,		  268,        -1, 	  115000,     1),
            new CWeaponInfo("Chainsaw",       eWeaponFire.WEAPON_FIRE_MELEE,      1.7f,  250,       100,         1000,               35,       -1.0f,    0.55f,  -1.0f,   -1.0f,  0.0f,  1.3f,  0.10f,	 "chainsaw",     5,            35,       30, 	            3,              99,              48,	         99,		  269,        -1, 	  102000,     1),
            new CWeaponInfo("Grenade",        eWeaponFire.WEAPON_FIRE_PROJECTILE,30.0f,  100,         1,            1,               75,       0.25f,    -1.0f, 800.0f,    1.0f,  0.0f,  0.0f,   0.0f,	 "grenade", 	 0,            99,       10, 	            0,              99,               6, 	         99,		  270,        -1, 	    2424,     2),
            new CWeaponInfo("DetonateGrenade",eWeaponFire.WEAPON_FIRE_PROJECTILE,30.0f,  100,         1,            1,               75,       0.25f,    -1.0f, 800.0f,    1.0f,  0.0f,  0.0f,   0.0f,	 "grenade",		 0,            99,       10, 	            0,              99,               6,		     99,		  270,	     291,	    2424,	  2),
            new CWeaponInfo("TearGas", eWeaponFire.WEAPON_FIRE_PROJECTILE,       30.0f,  100,         1,            1,               75,       0.25f,    -1.0f, 800.0f,    1.0f,  0.0f,  0.0f,   0.0f,	 "grenade", 	 0,            99,       10, 	            0,              99,               6, 	         99,    	  271,        -1, 	    2424,	  2),
            new CWeaponInfo("Molotov",  eWeaponFire.WEAPON_FIRE_PROJECTILE,      25.0f,  100,         1,            1,               75,       0.25f,    -1.0f,  2000f,    5.0f,  0.0f,	 0.0f,	 0.0f,	 "grenade",      0,            99,       10, 	            0,              99,               6, 	         99,    	  272,        -1, 	    2424,	  2),
            new CWeaponInfo("Rocket",   eWeaponFire.WEAPON_FIRE_PROJECTILE,      30.0f,  100,         1,            1,               75,       0.25f,    -1.0f, 800.0f,    1.0f,  0.0f,	 0.0f,	 0.0f,	 "unarmed",		 0,            99,       10, 	            0,              99,               6, 	         99,    	  273,        -1, 	    2424,	  2),
            new CWeaponInfo("Colt45",   eWeaponFire.WEAPON_FIRE_INSTANT_HIT,     30.0f,  250,       450,           17,               25,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.30f,  0.0f,	0.09f,	 "colt45", 	    11,            18,       14,  	           11,              18,              12,	         99,    	  274,        -1,	     680,	  3),
            new CWeaponInfo("Python",   eWeaponFire.WEAPON_FIRE_INSTANT_HIT,     30.0f,  250,      1000,            6,              135,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.41f, 0.03f,	0.12f,	 "python",      10,            31,       14, 	           14,              37,              16,	         40,    	  275,        -1,	   28040,	  3),
            new CWeaponInfo("Shotgun",  eWeaponFire.WEAPON_FIRE_INSTANT_HIT,     40.0f,  250,       450,            1,               80,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.86f,-0.02f,	0.28f,	 "shotgun",     12,            34,       15, 	            9,              26,              13,	         30,    	  277,        -1, 	   20040,	  4),
            new CWeaponInfo("Spas12Shotgun",eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 30.0f,  250,       750,            7,              100,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.85f,-0.06f,	0.19f,	 "buddy",       14,            20,       15, 	           14,              20,              14, 	         25,    	  278,        -1, 	   20040,	  4),
            new CWeaponInfo("StubbyShotgun",eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 15.0f,  250,       750,            1,              120,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.55f,-0.02f,	0.13f,	 "buddy",       14,            50,       15, 	           14,              56,              15,	         25,    	  279,        -1, 	   20040,	  4),
            new CWeaponInfo("Tec9",         eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 30.0f,  250,       500,           50,               20,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.45f,-0.05f,	0.11f,	 "colt45", 	    11,            15,       14,  	           11,              15,              12, 	         99,    	  281,        -1, 	     280,	  5),
            new CWeaponInfo("Uzi",            eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 45.0f,250,       500,           30,               20,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.45f, 0.00f,	0.12f,	 "uzi",		    12,            14,       12,  	           11,              13,              12, 	         25,    	  282,        -1, 	   28840,	  5),
            new CWeaponInfo("SilencedIngram", eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 30.0f,250,       500,           30,               15,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.36f, 0.02f,	0.11f,	 "colt45", 	    11,            13,       13,  	           11,              13,              11, 	         99,    	  283,        -1, 	    280,	  5),
            new CWeaponInfo("Mp5",            eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 45.0f,250,       500,           30,               35,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.51f,-0.01f,	0.20f,	 "uzi",		    11,            14,       11,  	           11,              14,              12, 	         30,    	  284,        -1, 	   28840,	  5),
            new CWeaponInfo("m4",  		    eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 90.0f,  250,      1000,           30,               40,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 0.88f,-0.04f,	0.16f,	 "rifle",       14,            17,       15, 	           14,              17,              15,	         99,    	  280,        -1, 	   28050,	  6),
            new CWeaponInfo("Ruger",     eWeaponFire.WEAPON_FIRE_INSTANT_HIT,    90.0f,  250,      1000,           30,               35,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 1.00f,-0.06f,	0.17f,	 "rifle",  	    12,            17,       14,  	           11,              16,              13, 	         99,    	  276,        -1, 	   28040,	  6),
            new CWeaponInfo("SniperRifle", eWeaponFire.WEAPON_FIRE_INSTANT_HIT, 100.0f,  500,      1401,            1,              125,       -1.0f,    -1.0f,  -1.0f,   -1.0f,  0.0f,	0.66f,	0.05f,	 "unarmed",     12,            15,       14,   	            0,              10,               3, 	         99,    	  285,        -1, 	     100,	  8),
            new CWeaponInfo("LaserScope",eWeaponFire.WEAPON_FIRE_INSTANT_HIT,   100.0f,  150,      1401,            7,              125,       -1.0f,    -1.0f,  -1.0f,   -1.0f,  0.0f,	0.66f,	0.05f,	 "unarmed",      2,             8,        3,  	            0,              10,               3, 	         99,    	  286,        -1, 	     100,	  8),
            new CWeaponInfo("RocketLauncher", eWeaponFire.WEAPON_FIRE_PROJECTILE,55.0f,  100,      1200,            1,               75,        2.0f,    -1.0f,  1000f,    1.0f, 0.42f,  0.0f,	0.05f,	 "unarmed",      0,            99,       14,            	0,              99,              14,         	 99,		  287,        -1,	     324,	  7),
            new CWeaponInfo("FlameThrower",  eWeaponFire.WEAPON_FIRE_AREA_EFFECT, 5.1f,  100,       100,          500,               25,       0.65f,   0.075f,  1000f,    3.0f, 1.30f,  0.0f,	0.56f,	 "flame",       12,            13,       12,             	6,               7,               7,          	 25,		  288,        -1,	       1,	  7),
            new CWeaponInfo("M60",    	   eWeaponFire.WEAPON_FIRE_INSTANT_HIT,  75.0f,    1,       500,          100,              130,       -1.0f,    -1.0f,  -1.0f,   -1.0f,  1.0f,  0.0f,	0.23f,	 "m60",     	12,            16,       13,               10,              12,              10,         	 30,		  289,        -1,	    8040, 	  7),
            new CWeaponInfo("Minigun",    eWeaponFire.WEAPON_FIRE_INSTANT_HIT,   75.0f,    1,       350,          500,              140,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 1.28f,  0.0f,	0.50f,	 "flame",       11,            12,       11,               11,              12,              11,         	 35,		  290,       294,	     200,	  7),
            new CWeaponInfo("Detonator",  eWeaponFire.WEAPON_FIRE_PROJECTILE,    25.0f,  100,         1,            1,                0,        2.0f,    -1.0f,2000.0f,    5.0f,  0.0f,	 0.0f,	0.13f,	 "man",		     0,            10,        3,             	0,              10,               3,           	 99,		  291,        -1,	  200024,	  9),
            new CWeaponInfo("HeliCannon", eWeaponFire.WEAPON_FIRE_INSTANT_HIT,  100.0f,    1,       100,          150,              100,       -1.0f,    -1.0f,  -1.0f,   -1.0f, 1.00f,  0.0f,  0.23f,   "m60",		    12,            16,       13,               10,              14,              12,          	 30,		  289,        -1,	    8040,	  7),
            // new CWeaponInfo("Camera",    eWeaponFire.WEAPON_FIRE_CAMERA,                        100.0   500,      1401,          36 ,               0 	      -1.0      -1.0    -1.0      -1.0f,  0.0f,	0.66f,	0.05f,	 "unarmed",      0,        10 3  	0  10 3  	25		292 -1	  100		9
        };
    };

    public static class g3Data
    {
        // public static readonly FightMove[] tFightMoves = new FightMove[]{
        //     new FightMove(AnimationId.ANIM_STD_NUM, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_PUNCH, 0.2f, 8.0f / 30.0f, 0.0f, 0.3f, HITLEVEL_HIGH, 1, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_IDLE, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_SHUFFLE_F, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_KNEE, 4.0f / 30.0f, 0.2f, 0.0f, 0.6f, HITLEVEL_LOW, 2, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_HEAD, 4.0f / 30.0f, 0.2f, 0.0f, 0.7f, HITLEVEL_HIGH, 3, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_PUNCH, 4.0f / 30.0f, 7.0f / 30.0f, 10.0f / 30.0f, 0.4f, HITLEVEL_HIGH, 1, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_LHOOK, 8.0f / 30.0f, 10.0f / 30.0f, 0.0f, 0.4f, HITLEVEL_HIGH, 3, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_KICK, 8.0f / 30.0f, 10.0f / 30.0f, 0.0f, 0.5, HITLEVEL_MEDIUM, 2, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_LONGKICK, 8.0f / 30.0f, 10.0f / 30.0f, 0.0f, 0.5, HITLEVEL_MEDIUM, 4, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_ROUNDHOUSE, 8.0f / 30.0f, 10.0f / 30.0f, 0.0f, 0.6f, HITLEVEL_MEDIUM, 4, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_BODYBLOW, 5.0f / 30.0f, 7.0f / 30.0f, 0.0f, 0.35f, HITLEVEL_LOW, 2, 0},
        //     new FightMove(AnimationId.ANIM_STD_KICKGROUND, 10.0f / 30.0f, 14.0f / 30.0f, 0.0f, 0.4f, HITLEVEL_GROUND, 1, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_FRONT, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_BACK, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_RIGHT, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_LEFT, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_BODYBLOW, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_CHEST, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_HEAD, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_WALK, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_FLOOR, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_HIT_BEHIND, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        //     new FightMove(AnimationId.ANIM_STD_FIGHT_2IDLE, 0.0f, 0.0f, 0.0f, 0.0f, HITLEVEL_NULL, 0, 0},
        // };
    }
}
