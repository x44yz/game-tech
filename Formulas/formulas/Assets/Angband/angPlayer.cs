// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using NaughtyAttributes;

// namespace angband
// {

//     /**
//     * Indexes of the player stats (hard-coded by savefiles).
//     */
//     enum STAT {
//         STAT_STR,
//         STAT_INT,
//         STAT_WIS,
//         STAT_DEX,
//         STAT_CON,
//         STAT_MAX
//     };


//     /**
//     * Player race info
//     */
//     class player_race {
//         string name;

//         int ridx;

//         int r_mhp;		/**< Hit-dice modifier */
//         int r_exp;		/**< Experience factor */

//         int b_age;		/**< Base age */
//         int m_age;		/**< Mod age */

//         int base_hgt;	/**< Base height */
//         int mod_hgt;	/**< Mod height */
//         int base_wgt;	/**< Base weight */
//         int mod_wgt;	/**< Mod weight */

//         int infra;		/**< Infra-vision range */

//         int body;		/**< Race body */

//         int[] r_adj = new int[(int)STAT.STAT_MAX];		/**< Stat bonuses */

//         // int r_skills[SKILL_MAX];	/**< Skills */

//         // int flags[OF_SIZE];		/**< Racial (object) flags */
//         // int pflags[PF_SIZE];	/**< Racial (player) flags */

//         // struct history_chart *history;

//         // struct element_info el_info[ELEM_MAX]; /**< Resists */
//     };


//     public class angPlayer : Unit
//     {   
//         player_race race;
//         player_class _class;

//         // loc grid;	/* Player location */
//         // loc old_grid;/* Player location before leaving for an arena */

//         int hitdie;		/* Hit dice (sides) */
//         int expfact;	/* Experience factor */

//         int age;		/* Characters age */
//         int ht;		/* Height */
//         int wt;		/* Weight */

//         int au;		/* Current Gold */

//         int max_depth;	/* Max depth */
//         int recall_depth;	/* Recall depth */
//         int depth;		/* Cur depth */

//         int max_lev;	/* Max level */
//         int lev;		/* Cur level */

//         int max_exp;	/* Max experience */
//         int exp;		/* Cur experience */
//         int exp_frac;	/* Cur exp frac (times 2^16) */

//         int mhp;		/* Max hit pts */
//         int chp;		/* Cur hit pts */
//         int chp_frac;	/* Cur hit frac (times 2^16) */

//         int msp;		/* Max mana pts */
//         int csp;		/* Cur mana pts */
//         int csp_frac;	/* Cur mana frac (times 2^16) */

//         int[] stat_max = new int[(int)STAT.STAT_MAX];	/* Current "maximal" stat values */
//         int[] stat_cur = new int[(int)STAT.STAT_MAX];	/* Current "natural" stat values */
//         int[] stat_map = new int[(int)STAT.STAT_MAX];	/* Tracks remapped stats from temp stat swap */

//         int timed;				/* Timed effects */

//         int word_recall;			/* Word of recall counter */
//         int deep_descent;			/* Deep Descent counter */

//         int energy;				/* Current energy */
//         int total_energy;			/* Total energy used (including resting) */
//         int resting_turn;			/* Number of player turns spent resting */

//         int food;				/* Current nutrition */

//         int unignoring;			/* Unignoring */

//         int spell_flags;			/* Spell flags */
//         int spell_order;			/* Spell order */

//         // char full_name[PLAYER_NAME_LEN];	/* Full name */
//         // char died_from[80];					/* Cause of death */
//         // char *history;						/* Player history */
//         // struct quest *quests;				/* Quest history */
//         // int total_winner;			/* Total winner */

//         int noscore;			/* Cheating flags */

//         bool is_dead;				/* Player is dead */

//         bool wizard;				/* Player is in wizard mode */

//         // int player_hp[PY_MAX_LEVEL];	/* HP gained per level */

//         /* Saved values for quickstart */
//         int au_birth;			/* Birth gold when option birth_money is false */
//         int[] stat_birth = new int[(int)STAT.STAT_MAX];		/* Birth "natural" stat values */
//         int ht_birth;			/* Birth Height */
//         int wt_birth;			/* Birth Weight */

//         // struct player_options opts;			/* Player options */
//         // struct player_history hist;			/* Player history (see player-history.c) */

//         struct player_body body;			/* Equipment slots available */
//         struct player_shape shape;			/* Current player shape */

//         struct object *gear;				/* Real gear */
//         struct object *gear_k;				/* Known gear */

//         struct object *obj_k;				/* Object knowledge ("runes") */
//         struct chunk *cave;					/* Known version of current level */

//         struct player_state state;			/* Calculatable state */
//         struct player_state known_state;	/* What the player can know of the above */
//         struct player_upkeep *upkeep;		/* Temporary player-related values */

//         protected override void OnStart()
//         {
//             base.OnStart();

//             handWeapon = GetComponent<angWeapon>();
//         }

//         protected override void OnUpdate(float dt)
//         {
//             base.OnUpdate(dt);
//         }

//         public angWeapon GetWeapon()
//         {
//             return handWeapon;
//         }

//         /**
//         * Attack the monster at the given location
//         *
//         * We get blows until energy drops below that required for another blow, or
//         * until the target monster dies. Each blow is handled by py_attack_real().
//         * We don't allow @ to spend more than 1 turn's worth of energy,
//         * to avoid slower monsters getting double moves.
//         */
//         void py_attack(angMonster mon)
//         {
//             /* Disturb the player */
//             // disturb(p);

//             /* Initialize the energy used */
//             // p->upkeep->energy_use = 0;

//             /* Reward BGs with 5% of max SPs, min 1/2 point */
//             // if (player_has(p, PF_COMBAT_REGEN)) {
//             //     int sp_gain = (((int)MAX(p->msp, 10)) * 16384) / 5;
//             //     player_adjust_mana_precise(p, sp_gain);
//             // }

//             /* Player attempts a shield bash if they can, and if monster is visible
//             * and not too pathetic */
//             // if (player_has(p, PF_SHIELD_BASH) && monster_is_visible(mon)) {
//             //     /* Monster may die */
//             //     if (attempt_shield_bash(p, mon, &fear)) return;
//             // }

//             /* Attack until the next attack would exceed energy available or
//             * a full turn or until the enemy dies. We limit energy use
//             * to avoid giving monsters a possible double move. */
//             // while (avail_energy - p->upkeep->energy_use >= blow_energy && !slain) {
//             //     slain = py_attack_real(p, grid, &fear);
//             //     p->upkeep->energy_use += blow_energy;
//             // }
//             py_attack_real(mon);

//             /* Hack - delay fear messages */
//             // if (fear && monster_is_visible(mon)) {
//             //     add_monster_message(mon, MON_MSG_FLEE_IN_TERROR, true);
//             // }
//         }

//         _object equipped_item_by_slot_name(string name)
//         {
//             /* Ensure a valid body */
//             if (p->body.slots) {
//                 return slot_object(p, slot_by_name(p, name));
//             }

//             return NULL;
//         }


//         /**
//         * Attack the monster at the given location with a single blow.
//         */
//         public bool py_attack_real(angMonster mon)
//         {
//             // size_t i;

//             /* Information about the target of the attack */
//             // struct monster *mon = square_monster(cave, grid);
//             // char m_name[80];
//             // bool stop = false;

//             /* The weapon used */
//             struct object *obj = equipped_item_by_slot_name(p, "weapon");

//             /* Information about the attack */
//             int drain = 0;
//             int splash = 0;
//             bool do_quake = false;
//             bool success = false;

//             char verb[20];
//             int msg_type = MSG_HIT;
//             int j, b, s, weight, dmg;

//             /* Default to punching */
//             // my_strcpy(verb, "punch", sizeof(verb));

//             /* Extract monster name (or "it") */
//             // monster_desc(m_name, sizeof(m_name), mon, MDESC_TARG);

//             /* Auto-Recall and track if possible and visible */
//             // if (monster_is_visible(mon)) {
//             //     monster_race_track(p->upkeep, mon->race);
//             //     health_track(p->upkeep, mon);
//             // }

//             /* Handle player fear (only for invisible monsters) */
//             // if (player_of_has(p, OF_AFRAID)) {
//             //     equip_learn_flag(p, OF_AFRAID);
//             //     msgt(MSG_AFRAID, "You are too afraid to attack %s!", m_name);
//             //     return false;
//             // }

//             /* Disturb the monster */
//             // monster_wake(mon, false, 100);
//             // mon_clear_timed(mon, MON_TMD_HOLD, MON_TMD_FLG_NOTIFY);

//             /* See if the player hit */
//             success = test_hit(chance_of_melee_hit(p, obj, mon), mon->race->ac);

//             /* If a miss, skip this hit */
//             if (!success) {
//                 msgt(MSG_MISS, "You miss %s.", m_name);

//                 // 嗜血 1/50
//                 /* Small chance of bloodlust side-effects */
//                 if (p->timed[TMD_BLOODLUST] && one_in_(50)) {
//                     msg("You feel strange...");
//                     player_over_exert(p, PY_EXERT_SCRAMBLE, 20, 20);
//                 }

//                 return false;
//             }

//             if (obj) {
//                 /* Handle normal weapon */
//                 weight = obj->weight;
//                 my_strcpy(verb, "hit", sizeof(verb));
//             } else {
//                 weight = 0;
//             }

//             /* Best attack from all slays or brands on all non-launcher equipment */
//             b = 0;
//             s = 0;
//             for (j = 2; j < p->body.count; j++) {
//                 struct object *obj_local = slot_object(p, j);
//                 if (obj_local)
//                     improve_attack_modifier(p, obj_local, mon, &b, &s,
//                         verb, false);
//             }

//             /* Get the best attack from all slays or brands - weapon or temporary */
//             if (obj) {
//                 improve_attack_modifier(p, obj, mon, &b, &s, verb, false);
//             }
//             improve_attack_modifier(p, NULL, mon, &b, &s, verb, false);

//             /* Get the damage */
//             if (!OPT(p, birth_percent_damage)) {
//                 dmg = melee_damage(mon, obj, b, s);
//                 /* For now, exclude criticals on unarmed combat */
//                 if (obj) dmg = critical_melee(p, mon, weight, obj->to_h,
//                     dmg, &msg_type);
//             } else {
//                 dmg = o_melee_damage(p, mon, obj, b, s, &msg_type);
//             }

//             /* Splash damage and earthquakes */
//             splash = (weight * dmg) / 100;
//             if (player_of_has(p, OF_IMPACT) && dmg > 50) {
//                 do_quake = true;
//                 equip_learn_flag(p, OF_IMPACT);
//             }

//             /* Learn by use */
//             equip_learn_on_melee_attack(p);
//             learn_brand_slay_from_melee(p, obj, mon);

//             /* Apply the player damage bonuses */
//             if (!OPT(p, birth_percent_damage)) {
//                 dmg += player_damage_bonus(&p->state);
//             }

//             /* Substitute shape-specific blows for shapechanged players */
//             if (player_is_shapechanged(p)) {
//                 int choice = randint0(p->shape->num_blows);
//                 struct player_blow *blow = p->shape->blows;
//                 while (choice--) {
//                     blow = blow->next;
//                 }
//                 my_strcpy(verb, blow->name, sizeof(verb));
//             }

//             /* No negative damage; change verb if no damage done */
//             if (dmg <= 0) {
//                 dmg = 0;
//                 msg_type = MSG_MISS;
//                 my_strcpy(verb, "fail to harm", sizeof(verb));
//             }

//             for (i = 0; i < N_ELEMENTS(melee_hit_types); i++) {
//                 const char *dmg_text = "";

//                 if (msg_type != melee_hit_types[i].msg_type)
//                     continue;

//                 if (OPT(p, show_damage))
//                     dmg_text = format(" (%d)", dmg);

//                 if (melee_hit_types[i].text)
//                     msgt(msg_type, "You %s %s%s. %s", verb, m_name, dmg_text,
//                             melee_hit_types[i].text);
//                 else
//                     msgt(msg_type, "You %s %s%s.", verb, m_name, dmg_text);
//             }

//             /* Pre-damage side effects */
//             blow_side_effects(p, mon);

//             /* Damage, check for hp drain, fear and death */
//             drain = MIN(mon->hp, dmg);
//             stop = mon_take_hit(mon, p, dmg, fear, NULL);

//             /* Small chance of bloodlust side-effects */
//             if (p->timed[TMD_BLOODLUST] && one_in_(50)) {
//                 msg("You feel something give way!");
//                 player_over_exert(p, PY_EXERT_CON, 20, 0);
//             }

//             if (!stop) {
//                 if (p->timed[TMD_ATT_VAMP] && monster_is_living(mon)) {
//                     effect_simple(EF_HEAL_HP, source_player(), format("%d", drain),
//                                 0, 0, 0, 0, 0, NULL);
//                 }
//             }

//             if (stop)
//                 (*fear) = false;

//             /* Post-damage effects */
//             if (blow_after_effects(grid, dmg, splash, fear, do_quake))
//                 stop = true;

//             return stop;
//         }

//         /**
//         * Learn things which happen on making a melee attack.
//         * Does not apply to bow
//         *
//         * \param p is the player
//         */
//         void equip_learn_on_melee_attack()
//         {
//             int i;

//             if (p->obj_k->to_h && p->obj_k->to_d)
//                 return;

//             for (i = 0; i < p->body.count; i++) {
//                 struct object *obj = slot_object(p, i);
//                 if (i == slot_by_name(p, "shooting")) continue;
//                 if (obj) {
//                     assert(obj->known);
//                     if (!object_has_standard_to_h(obj)) {
//                         int index = rune_index(RUNE_VAR_COMBAT, COMBAT_RUNE_TO_H);
//                         player_learn_rune(p, index, true);
//                     }
//                     if (obj->to_d) {
//                         int index = rune_index(RUNE_VAR_COMBAT, COMBAT_RUNE_TO_D);
//                         player_learn_rune(p, index, true);
//                     }
//                     object_curses_find_to_h(p, obj);
//                     object_curses_find_to_d(p, obj);
//                     if (p->obj_k->to_h && p->obj_k->to_d) return;
//                 }
//             }
//             if (p->shape) {
//                 struct player_shape *shape = lookup_player_shape(p->shape->name);
//                 if (shape->to_h != 0) {
//                     int index = rune_index(RUNE_VAR_COMBAT, COMBAT_RUNE_TO_H);
//                     player_learn_rune(p, index, true);
//                 }
//                 if (shape->to_d != 0) {
//                     int index = rune_index(RUNE_VAR_COMBAT, COMBAT_RUNE_TO_D);
//                     player_learn_rune(p, index, true);
//                 }
//             }
//         }

//         /**
//         * Determine if a hit roll is successful against the target AC.
//         * See also: hit_chance
//         *
//         * \param to_hit To total to-hit value to use
//         * \param ac The AC to roll against
//         */
//         bool test_hit(int to_hit, int ac)
//         {
//             random_chance c = new random_chance();
//             hit_chance(ref c, to_hit, ac);
//             return random_chance_check(c);
//         }

//         /**
//         * Return a random_chance by reference, which represents the likelihood of a
//         * hit roll succeeding for the given to_hit and ac values. The hit calculation
//         * will:
//         *
//         * Always hit 12% of the time
//         * Always miss 5% of the time
//         * Put a floor of 9 on the to-hit value
//         * Roll between 0 and the to-hit value
//         * The outcome must be >= AC*2/3 to be considered a hit
//         *
//         * \param chance The random_chance to return-by-reference
//         * \param to_hit The to-hit value to use
//         * \param ac The AC to roll against
//         */
//         void hit_chance(ref random_chance chance, int to_hit, int ac)
//         {
//             /* Percentages scaled to 10,000 to avoid rounding error */
//             const int HUNDRED_PCT = 10000;
//             const int ALWAYS_HIT = 1200;
//             const int ALWAYS_MISS = 500;

//             /* Put a floor on the to_hit */
//             to_hit = Mathf.Max(9, to_hit);

//             /* Calculate the hit percentage */
//             chance->numerator = Mathf.Max(0, to_hit - ac * 2 / 3);
//             chance->denominator = to_hit;

//             /* Convert the ratio to a scaled percentage */
//             chance->numerator = HUNDRED_PCT * chance->numerator / chance->denominator;
//             chance->denominator = HUNDRED_PCT;

//             /* The calculated rate only applies when the guaranteed hit/miss don't */
//             chance->numerator = chance->numerator *
//                     (HUNDRED_PCT - ALWAYS_MISS - ALWAYS_HIT) / HUNDRED_PCT;

//             /* Add in the guaranteed hit */
//             chance->numerator += ALWAYS_HIT;
//         }

//         /**
//         * Roll on a random chance and check for success.
//         *
//         * \param c The random_chance to roll on
//         */
//         bool random_chance_check(random_chance c)
//         {
//             /* Calculated so that high rolls pass the check */
//             return UnityEngine.Random.Range(c.denominator) >= c.denominator - c.numerator;
//         }

//         /**
//         * Calculate the player's base melee to-hit value without regard to a specific
//         * monster.
//         * See also: chance_of_missile_hit_base
//         *
//         * \param p The player
//         * \param weapon The player's weapon
//         */
//         int chance_of_melee_hit_base(const struct player *p,
//                 const struct object *weapon)
//         {
//             int bonus = p->state.to_h + (weapon ? weapon->to_h : 0);
//             return p->state.skills[SKILL_TO_HIT_MELEE] + bonus * BTH_PLUS_ADJ;
//         }

//         /**
//         * Calculate the player's melee to-hit value against a specific monster.
//         * See also: chance_of_missile_hit
//         *
//         * \param p The player
//         * \param weapon The player's weapon
//         * \param mon The monster
//         */
//         static int chance_of_melee_hit(const struct player *p,
//                 const struct object *weapon, const struct monster *mon)
//         {
//             int chance = chance_of_melee_hit_base(p, weapon);
//             /* Non-visible targets have a to-hit penalty of 50% */
//             return monster_is_visible(mon) ? chance : chance / 2;
//         }

//         /**
//         * Determine standard melee damage.
//         *
//         * Factor in damage dice, to-dam and any brand or slay.
//         */
//         static int melee_damage(const struct monster *mon, struct object *obj, int b, int s)
//         {
//             int dmg = (obj) ? damroll(obj->dd, obj->ds) : 1;

//             if (s) {
//                 dmg *= slays[s].multiplier;
//             } else if (b) {
//                 dmg *= get_monster_brand_multiplier(mon, &brands[b], false);
//             }

//             if (obj) dmg += obj->to_d;

//             return dmg;
//         }

//         // ---------------------------------------------------------------------
// #region EditorTest
//         [Button("LevelUp", EButtonEnableMode.Playmode)]
//         private void TestLevelUp()
//         {
//             // NextPlrLevel();
//         }
// #endregion // EditorTest
//     }
// }

