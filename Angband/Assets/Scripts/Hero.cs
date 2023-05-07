using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A single equipment slot
 */
public struct equip_slot {
	public int type;
	public string name;
    public GObject obj;
};

public struct player_body {
	string name;
	// equip_slot slots;
};


    /**
    * Option indexes 
    */
    enum OPT
    {
        OPT_none,
        OPT_rogue_like_commands,
        OPT_use_sound,
        OPT_show_damage,
        OPT_use_old_target,
        OPT_pickup_always,
        OPT_pickup_inven,
        OPT_show_flavors,
        OPT_show_target,
        OPT_highlight_player,
        OPT_disturb_near,
        OPT_solid_walls,
        OPT_hybrid_walls,
        OPT_view_yellow_light,
        OPT_animate_flicker,
        OPT_center_player,
        OPT_purple_uniques,
        OPT_auto_more,
        OPT_hp_changes_color,
        OPT_mouse_movement,
        OPT_notify_recharge,
        OPT_effective_speed,
        OPT_cheat_hear,
        OPT_score_hear,
        OPT_cheat_room,
        OPT_score_room,
        OPT_cheat_xtra,
        OPT_score_xtra,
        OPT_cheat_live,
        OPT_score_live,
        OPT_birth_randarts,
        OPT_birth_connect_stairs,
        OPT_birth_force_descend,
        OPT_birth_no_recall,
        OPT_birth_no_artifacts,
        OPT_birth_stacking,
        OPT_birth_lose_arts,
        OPT_birth_feelings,
        OPT_birth_no_selling,
        OPT_birth_start_kit,
        OPT_birth_ai_learn,
        OPT_birth_know_runes,
        OPT_birth_know_flavors,
        OPT_birth_levels_persist,
        OPT_birth_percent_damage,
        OPT_MAX
    };

public class player_options {
	public bool[] opt = new bool[(int)OPT.OPT_MAX];		/**< Options */

	int hitpoint_warn;		/**< Hitpoint warning (0 to 9) */
	int lazymove_delay;		/**< Delay in cs before moving to allow another key */
	int delay_factor;		/**< Delay factor (0 to 9) */

	int name_suffix;		/**< Numeric suffix for player name */
};

/**
* Indexes of the player stats (hard-coded by savefiles).
*/
public enum STAT
{
    STAT_STR,
    STAT_INT,
    STAT_WIS,
    STAT_DEX,
    STAT_CON,
    STAT_MAX
};

/**
* Skill indexes
*/
public enum SKILL {
    SKILL_DISARM_PHYS,		/* Disarming - physical */
    SKILL_DISARM_MAGIC,		/* Disarming - magical */
    SKILL_DEVICE,			/* Magic Devices */
    SKILL_SAVE,				/* Saving throw */
    SKILL_SEARCH,			/* Searching ability */
    SKILL_STEALTH,			/* Stealth factor */
    SKILL_TO_HIT_MELEE,		/* To hit (normal) */
    SKILL_TO_HIT_BOW,		/* To hit (shooting) */
    SKILL_TO_HIT_THROW,		/* To hit (throwing) */
    SKILL_DIGGING,			/* Digging */

    SKILL_MAX
};

 /**
* All the variable state that changes when you put on/take off equipment.
* Player flags are not currently variable, but useful here so monsters can
* learn them.
*/
public class player_state {
    public int[] stat_add = new int[(int)STAT.STAT_MAX];	/**< Equipment stat bonuses */
    public int[] stat_ind = new int[(int)STAT.STAT_MAX];	/**< Indexes into stat tables */
    public int[] stat_use = new int[(int)STAT.STAT_MAX];	/**< Current modified stats */
    public int[] stat_top = new int[(int)STAT.STAT_MAX];	/**< Maximal modified stats */

    public int[] skills = new int[(int)SKILL.SKILL_MAX];		/**< Skills */

    public int speed;			/**< Current speed */

    public int num_blows;		/**< Number of blows x100 */
    public int num_shots;		/**< Number of shots x10 */
    public int num_moves;		/**< Number of extra movement actions */

    public int ammo_mult;		/**< Ammo multiplier */
    public int ammo_tval;		/**< Ammo variety */

    public int ac;				/**< Base ac */
    public int dam_red;		/**< Damage reduction */
    public int perc_dam_red;	/**< Percentage damage reduction */
    public int to_a;			/**< Bonus to ac */
    public int to_h;			/**< Bonus to hit */
    public int to_d;			/**< Bonus to dam */

    public int see_infra;		/**< Infravision range */

    public int cur_light;		/**< Radius of light (if any) */

    public bool heavy_wield;	/**< Heavy weapon */
    public bool heavy_shoot;	/**< Heavy shooter */
    public bool bless_wield;	/**< Blessed (or blunt) weapon */

    public bool cumber_armor;	/**< Mana draining armor */

    // public bitflag[] flags = new bitflag[OF_SIZE];					/**< Status flags from race and items */
    // public bitflag[] pflags = new bitflag[PF_SIZE];				/**< Player intrinsic flags */
    // public element_info[] el_info = new element_info[(int)ELEM.ELEM_MAX];	/**< Resists from race and items */
};

[Serializable]
public class Hero : Actor
{
    public RaceCfg race;
    public ClassCfg cls;

    public player_state state;			/* Calculatable state */

    public void Init(string heroRace, string heroClass)
    {
        race = Races.raceCfgs.Find(x => x.name == heroRace);
        cls = Classes.classCfgs.Find(x => x.name == heroClass);

        slots = new List<equip_slot>();
        slots.Add(new equip_slot(){name = "weapon"});
        slots.Add(new equip_slot(){name = "shooting"});
        slots.Add(new equip_slot(){name = "right hand"});
        slots.Add(new equip_slot(){name = "left hand"});
        slots.Add(new equip_slot(){name = "neck"});
        slots.Add(new equip_slot(){name = "light"});
        slots.Add(new equip_slot(){name = "body"});
        slots.Add(new equip_slot(){name = "back"});
        slots.Add(new equip_slot(){name = "arm"});
        slots.Add(new equip_slot(){name = "head"});
        slots.Add(new equip_slot(){name = "hands"});
        slots.Add(new equip_slot(){name = "feet"});

        state = new player_state();
    }

    public void Attack(Monster mon)
    {
        /* Reward BGs with 5% of max SPs, min 1/2 point */
        // if (player_has(p, PF_COMBAT_REGEN)) {
        //     int32_t sp_gain = (((int32_t)MAX(p->msp, 10)) * 16384) / 5;
        //     player_adjust_mana_precise(p, sp_gain);
        // }

        /* Player attempts a shield bash if they can, and if monster is visible
        * and not too pathetic */
        // if (player_has(p, PF_SHIELD_BASH) && monster_is_visible(mon)) {
        //     /* Monster may die */
        //     if (attempt_shield_bash(p, mon, &fear)) return;
        // }

        AttackImpl(mon);
    }

    /**
    * Return the slot number for a given name, or quit game
    */
    int slot_by_name(string name)
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            if (slots[i].name == name)
                return i;
        }
        
        Debug.Assert(false, "cant find slot > " + name);
        return -1;
    }

    public GObject equipped_item_by_slot_name(string name)
    {
        /* Ensure a valid body */
        if (slots != null) {
            return slot_object(slot_by_name(name));
        }

        Debug.LogError("cant find slot > " + name);
        return null;
    }

    public const int BTH_PLUS_ADJ    = 3; 		/* Adjust BTH per plus-to-hit */

    public int chance_of_melee_hit(GObject weapon)
    {
        int bonus = state.to_h + (weapon != null ? weapon.to_h : 0);
        return state.skills[(int)SKILL.SKILL_TO_HIT_MELEE] + bonus * BTH_PLUS_ADJ;
    }

    /**
    * Determine if a hit roll is successful against the target AC.
    * See also: hit_chance
    *
    * \param to_hit To total to-hit value to use
    * \param ac The AC to roll against
    */
    bool test_hit(int to_hit, int ac)
    {
        // random_chance c;
        // hit_chance(&c, to_hit, ac);
        // return random_chance_check(c);
        throw new System.NotImplementedException();
    }

    /**
    * Get the object in a specific slot (if any).  Quit if slot index is invalid.
    */
    GObject slot_object(int slot)
    {
        /* Check bounds */
        // assert(slot >= 0 && slot < p->body.count);

        // /* Ensure a valid body */
        // if (p->body.slots && p->body.slots[slot].obj) {
        //     return p->body.slots[slot].obj;
        // }

        return slots[slot].obj;

        // return NULL;
        // throw new System.NotImplementedException();
    }

    /**
    * Extract the multiplier from a given object hitting a given monster.
    *
    * \param player is the player performing the attack
    * \param obj is the object being used to attack
    * \param mon is the monster being attacked
    * \param brand_used is the brand that gave the best multiplier, or NULL
    * \param slay_used is the slay that gave the best multiplier, or NULL
    * \param verb is the verb used in the attack ("smite", etc)
    * \param range is whether or not this is a ranged attack
    */
    void improve_attack_modifier(Hero p, GObject obj,
        Monster mon, ref int brand_used, ref int slay_used, bool range)
    {
        throw new System.NotImplementedException();
        // bool pctdam = OPT(p, birth_percent_damage);
        // int i, best_mult = 1;

        // /* Set the current best multiplier */
        // if (*brand_used) {
        //     struct brand *b = &brands[*brand_used];
        //     best_mult = MAX(best_mult,
        //         get_monster_brand_multiplier(mon, b, pctdam));
        // } else if (*slay_used) {
        //     struct slay *s = &slays[*slay_used];
        //     int mult = (pctdam) ? s->o_multiplier : s->multiplier;
        //     best_mult = MAX(best_mult, mult);
        // }

        // /* Brands */
        // for (i = 1; i < z_info->brand_max; i++) {
        //     struct brand *b = &brands[i];
        //     if (obj) {
        //         /* Brand is on an object */
        //         if (!obj->brands || !obj->brands[i]) continue;
        //     } else {
        //         /* Temporary brand */
        //         if (!player_has_temporary_brand(p, i)) continue;
        //     }
    
        //     /* Is the monster vulnerable? */
        //     if (!rf_has(mon->race->flags, b->resist_flag)) {
        //         int mult = get_monster_brand_multiplier(mon, b, pctdam);

        //         /* Record the best multiplier */
        //         if (best_mult < mult) {
        //             best_mult = mult;
        //             *brand_used = i;
        //             my_strcpy(verb, b->verb, 20);
        //             if (range)
        //                 my_strcat(verb, "s", 20);
        //         }
        //     }
        // }

        // /* Slays */
        // for (i = 1; i < z_info->slay_max; i++) {
        //     struct slay *s = &slays[i];
        //     if (obj) {
        //         /* Slay is on an object */
        //         if (!obj->slays || !obj->slays[i]) continue;
        //     } else {
        //         /* Temporary slay */
        //         if (!player_has_temporary_slay(p, i)) continue;
        //     }
    
        //     /* Is the monster is vulnerable? */
        //     if (react_to_specific_slay(s, mon)) {
        //         int mult = pctdam ? s->o_multiplier : s->multiplier;

        //         /* Record the best multiplier */
        //         if (best_mult < mult) {
        //             best_mult = mult;
        //             *brand_used = 0;
        //             *slay_used = i;
        //             if (range) {
        //                 my_strcpy(verb, s->range_verb, 20);
        //             } else {
        //                 my_strcpy(verb, s->melee_verb, 20);
        //             }
        //         }
        //     }
        // }
    }

    public List<player_body> bodys;
    public List<equip_slot> slots;

    
    public player_options opts = new player_options();			/* Player options */
    // #define OPT(p, opt_name)	p->opts.opt[OPT_##opt_name]
    public bool _OPT(Hero p, int opt_name) => p.opts.opt[opt_name];

    /**
    * Determine standard melee damage.
    *
    * Factor in damage dice, to-dam and any brand or slay.
    */
    int melee_damage(Monster mon, GObject obj, int b, int s)
    {
        throw new System.NotImplementedException();
        // int dmg = (obj != null) ? damroll(obj.dd, obj.ds) : 1;

        // if (s != 0) {
        //     dmg *= slays[s].multiplier;
        // } else if (b != 0) {
        //     dmg *= get_monster_brand_multiplier(mon, brands[b], false);
        // }

        // if (obj != null) dmg += obj.to_d;

        // return dmg;
    }

/**
    * Determine damage for critical hits from melee.
    *
    * Factor in weapon weight, total plusses, player level.
    */
    int critical_melee(Hero p, Monster monster, int weight, int plus, int dam, ref int msg_type)
    {
        throw new System.NotImplementedException();
        // int debuff_to_hit = is_debuffed(monster) ? DEBUFF_CRITICAL_HIT : 0;
        // int power = weight + randint1(650);
        // int chance = weight + (p.state.to_h + plus + debuff_to_hit) * 5
        //     + (p.state.skills[(int)SKILL.SKILL_TO_HIT_MELEE] - 60);
        // int new_dam = dam;

        // if (randint1(5000) > chance) {
        //     msg_type = (uint32_t)MSG.MSG_HIT;
        // } else if (power < 400) {
        //     msg_type = (uint32_t)MSG.MSG_HIT_GOOD;
        //     new_dam = 2 * dam + 5;
        // } else if (power < 700) {
        //     msg_type = (uint32_t)MSG.MSG_HIT_GREAT;
        //     new_dam = 2 * dam + 10;
        // } else if (power < 900) {
        //     msg_type = (uint32_t)MSG.MSG_HIT_SUPERB;
        //     new_dam = 3 * dam + 15;
        // } else if (power < 1300) {
        //     msg_type = (uint32_t)MSG.MSG_HIT_HI_GREAT;
        //     new_dam = 3 * dam + 20;
        // } else {
        //     msg_type = (uint32_t)MSG.MSG_HIT_HI_SUPERB;
        //     new_dam = 4 * dam + 20;
        // }

        // return new_dam;
    }

    /**
    * Determine O-combat melee damage.
    *
    * Deadliness and any brand or slay add extra sides to the damage dice,
    * criticals add extra dice.
    */
    static int o_melee_damage(Hero p, Monster mon,
            GObject obj, int b, int s, ref int msg_type)
    {
        throw new System.NotImplementedException();
        // int dice = (obj) ? obj->dd : 1;
        // int sides, dmg, add = 0;
        // bool extra;

        // /* Get the average value of a single damage die. (x10) */
        // int die_average = (10 * (((obj) ? obj->ds : 1) + 1)) / 2;

        // /* Adjust the average for slays and brands. (10x inflation) */
        // if (s) {
        //     die_average *= slays[s].o_multiplier;
        //     add = slays[s].o_multiplier - 10;
        // } else if (b) {
        //     int bmult = get_monster_brand_multiplier(mon, &brands[b], true);

        //     die_average *= bmult;
        //     add = bmult - 10;
        // } else {
        //     die_average *= 10;
        // }

        // /* Apply deadliness to average. (100x inflation) */
        // apply_deadliness(&die_average,
        //     MIN(((obj) ? obj->to_d : 0) + p->state.to_d, 150));

        // /* Calculate the actual number of sides to each die. */
        // sides = (2 * die_average) - 10000;
        // extra = randint0(10000) < (sides % 10000);
        // sides /= 10000;
        // sides += (extra ? 1 : 0);

        // /*
        // * Get number of critical dice; for now, excluding criticals for
        // * unarmed combat
        // */
        // if (obj) dice += o_critical_melee(p, mon, obj, msg_type);

        // /* Roll out the damage. */
        // dmg = damroll(dice, sides);

        // /* Apply any special additions to damage. */
        // dmg += add;

        // return dmg;
    }

    /**
    * Check if the player state has the given OF_ flag.
    */
    bool player_of_has(Hero p, int flag)
    {
        throw new System.NotImplementedException();
        // Debug.Assert(p);
        // return of_has(p.state.flags, flag);
    }

    void equip_learn_flag(Hero p, int flag)
    {
        throw new System.NotImplementedException();
    }

    void equip_learn_on_melee_attack(Hero p)
    {
        throw new System.NotImplementedException();
    }

    void learn_brand_slay_from_melee(Hero p, GObject weapon,
		Monster mon)
    {
        throw new System.NotImplementedException();
    }

    bool player_is_shapechanged(Hero p)
    {
        throw new System.NotImplementedException();
    }

    /**
    * Apply the player damage bonuses
    */
    static int player_damage_bonus(player_state state)
    {
        return state.to_d;
    }

    bool mon_take_hit(Monster mon, Hero p, int dam, ref bool fear)
    {
        throw new System.NotImplementedException();
        // char k = char.MinValue;
        // string k = "";
        // return mon_take_hit(mon, p, dam, ref fear, ref k);
    }

    public bool AttackImpl(Monster mon)
    {
        var p = this;

        /* The weapon used */
        var obj = equipped_item_by_slot_name("weapon");

        /* Information about the attack */
        int drain = 0;
        int splash = 0;
        bool do_quake = false;
        bool success = false;
        int dmg = 0;
        int weight = 0;

        int msg_type = 0;

        /* Auto-Recall and track if possible and visible */
        // if (monster_is_visible(mon)) {
        //     monster_race_track(p->upkeep, mon->race);
        //     health_track(p->upkeep, mon);
        // }

        /* Handle player fear (only for invisible monsters) */
        // if (player_of_has(p, OF_AFRAID)) {
        //     equip_learn_flag(p, OF_AFRAID);
        //     msgt(MSG_AFRAID, "You are too afraid to attack %s!", m_name);
        //     return false;
        // }

        /* Disturb the monster */
        // monster_wake(mon, false, 100);
        // mon_clear_timed(mon, MON_TMD_HOLD, MON_TMD_FLG_NOTIFY);

        /* See if the player hit */
        success = test_hit(chance_of_melee_hit(obj), mon.race.ac);

        /* If a miss, skip this hit */
        if (!success) 
        {
            // msgt(MSG_MISS, "You miss %s.", m_name);

            /* Small chance of bloodlust side-effects */
            // if (p->timed[TMD_BLOODLUST] && one_in_(50)) 
            // {
            //     msg("You feel strange...");
            //     player_over_exert(p, PY_EXERT_SCRAMBLE, 20, 20);
            // }

            return false;
        }

        if (obj != null) {
            /* Handle normal weapon */
            weight = obj.weight;
            // my_strcpy(verb, "hit", sizeof(verb));
        } else {
            weight = 0;
        }

        /* Best attack from all slays or brands on all non-launcher equipment */
        int b = 0;
        int s = 0;
        for (int j = 2; j < bodys.Count; j++) 
        {
            var obj_local = slot_object(j);
            if (obj_local != null)
                improve_attack_modifier(this, obj_local, mon, ref b, ref s, false);
        }

        /* Get the best attack from all slays or brands - weapon or temporary */
        if (obj != null) {
            improve_attack_modifier(this, obj, mon, ref b, ref s, false);
        }
        improve_attack_modifier(this, null, mon, ref b, ref s, false);

        /* Get the damage */
        if (!_OPT(this, (int)OPT.OPT_birth_percent_damage)) {
            dmg = melee_damage(mon, obj, b, s);
            /* For now, exclude criticals on unarmed combat */
            if (obj != null) dmg = critical_melee(this, mon, weight, obj.to_h,
                dmg, ref msg_type);
        } else {
            dmg = o_melee_damage(this, mon, obj, b, s, ref msg_type);
        }

        /* Splash damage and earthquakes */
        splash = (weight * dmg) / 100;
        if (player_of_has(this, (int)OF.OF_IMPACT) && dmg > 50) {
            do_quake = true;
            equip_learn_flag(p, (int)OF.OF_IMPACT);
        }

        /* Learn by use */
        equip_learn_on_melee_attack(p);
        learn_brand_slay_from_melee(p, obj, mon);

        /* Apply the player damage bonuses */
        if (!_OPT(p, (int)OPT.OPT_birth_percent_damage)) {
            dmg += player_damage_bonus(p.state);
        }

        /* Substitute shape-specific blows for shapechanged players */
        // if (player_is_shapechanged(p)) {
        //     int choice = randint0(p->shape->num_blows);
        //     player_blow blow = p->shape->blows;
        //     while (choice--) {
        //         blow = blow->next;
        //     }
        //     // my_strcpy(verb, blow->name, sizeof(verb));
        // }

        /* No negative damage; change verb if no damage done */
        if (dmg <= 0) {
            dmg = 0;
            // msg_type = MSG_MISS;
            // my_strcpy(verb, "fail to harm", sizeof(verb));
            Debug.Log("xx-- fail to harm");
        }

        // for (i = 0; i < N_ELEMENTS(melee_hit_types); i++) {
        //     const char *dmg_text = "";

        //     if (msg_type != melee_hit_types[i].msg_type)
        //         continue;

        //     if (OPT(p, show_damage))
        //         dmg_text = format(" (%d)", dmg);

        //     if (melee_hit_types[i].text)
        //         msgt(msg_type, "You %s %s%s. %s", verb, m_name, dmg_text,
        //                 melee_hit_types[i].text);
        //     else
        //         msgt(msg_type, "You %s %s%s.", verb, m_name, dmg_text);
        // }

        /* Pre-damage side effects */
        // 攻击副作用
        // blow_side_effects(p, mon);

        /* Damage, check for hp drain, fear and death */
        drain = Mathf.Min(mon.hp, dmg);
        bool fear = false;
        bool stop = mon_take_hit(mon, p, dmg, ref fear);

        /* Small chance of bloodlust side-effects */
        // 副作用 - 嗜血
        // if (p->timed[TMD_BLOODLUST] && one_in_(50)) {
        //     msg("You feel something give way!");
        //     player_over_exert(p, PY_EXERT_CON, 20, 0);
        // }

        if (!stop) {
            // if (p->timed[TMD_ATT_VAMP] && monster_is_living(mon)) {
            //     effect_simple(EF_HEAL_HP, source_player(), format("%d", drain),
            //                 0, 0, 0, 0, 0, NULL);
            // }
        }

        // if (stop)
        //     (*fear) = false;

        /* Post-damage effects */
        // if (blow_after_effects(grid, dmg, splash, fear, do_quake))
        //     stop = true;

        return stop;
    }

    /**
    * Calculate the effect of a shapechange on player state
    */
    static void calc_shapechange(struct player_state *state,
                                struct player_shape *shape,
                                int *blows, int *shots, int *might, int *moves)
    {
        int i;

        /* Combat stats */
        state->to_a += shape->to_a;
        state->to_h += shape->to_h;
        state->to_d += shape->to_d;

        /* Skills */
        for (i = 0; i < SKILL_MAX; i++) {
            state->skills[i] += shape->skills[i];
        }

        /* Object flags */
        of_union(state->flags, shape->flags);

        /* Player flags */
        pf_union(state->pflags, shape->pflags);

        /* Stats */
        for (i = 0; i < STAT_MAX; i++) {
            state->stat_add[i] += shape->modifiers[i];
        }

        /* Other modifiers */
        state->skills[SKILL_STEALTH] += shape->modifiers[OBJ_MOD_STEALTH];
        state->skills[SKILL_SEARCH] += (shape->modifiers[OBJ_MOD_SEARCH] * 5);
        state->see_infra += shape->modifiers[OBJ_MOD_INFRA];
        state->skills[SKILL_DIGGING] += (shape->modifiers[OBJ_MOD_TUNNEL] * 20);
        state->speed += shape->modifiers[OBJ_MOD_SPEED];
        state->dam_red += shape->modifiers[OBJ_MOD_DAM_RED];
        *blows += shape->modifiers[OBJ_MOD_BLOWS];
        *shots += shape->modifiers[OBJ_MOD_SHOTS];
        *might += shape->modifiers[OBJ_MOD_MIGHT];
        *moves += shape->modifiers[OBJ_MOD_MOVES];

        /* Resists and vulnerabilities */
        for (i = 0; i < ELEM_MAX; i++) {
            if (state->el_info[i].res_level == 0) {
                /* Simple, just apply shape res/vuln */
                state->el_info[i].res_level = shape->el_info[i].res_level;
            } else if (state->el_info[i].res_level == -1) {
                /* Shape resists cancel, immunities trump, vulnerabilities */
                if (shape->el_info[i].res_level == 1) {
                    state->el_info[i].res_level = 0;
                } else if (shape->el_info[i].res_level == 3) {
                    state->el_info[i].res_level = 3;
                }
            } else if (state->el_info[i].res_level == 1) {
                /* Shape vulnerabilities cancel, immunities enhance, resists */
                if (shape->el_info[i].res_level == -1) {
                    state->el_info[i].res_level = 0;
                } else if (shape->el_info[i].res_level == 3) {
                    state->el_info[i].res_level = 3;
                }
            } else if (state->el_info[i].res_level == 3) {
                /* Immmunity, shape has no effect */
            }
        }

    }

    /**
    * Calculate the players current "state", taking into account
    * not only race/class intrinsics, but also objects being worn
    * and temporary spell effects.
    *
    * See also calc_mana() and calc_hitpoints().
    *
    * Take note of the new "speed code", in particular, a very strong
    * player will start slowing down as soon as he reaches 150 pounds,
    * but not until he reaches 450 pounds will he be half as fast as
    * a normal kobold.  This both hurts and helps the player, hurts
    * because in the old days a player could just avoid 300 pounds,
    * and helps because now carrying 300 pounds is not very painful.
    *
    * The "weapon" and "bow" do *not* add to the bonuses to hit or to
    * damage, since that would affect non-combat things.  These values
    * are actually added in later, at the appropriate place.
    *
    * If known_only is true, calc_bonuses() will only use the known
    * information of objects; thus it returns what the player _knows_
    * the character state to be.
    */
    void calc_bonuses(struct player *p, struct player_state *state, bool known_only,
                    bool update)
    {
        int i, j, hold;
        int extra_blows = 0;
        int extra_shots = 0;
        int extra_might = 0;
        int extra_moves = 0;
        struct object *launcher = equipped_item_by_slot_name(p, "shooting");
        struct object *weapon = equipped_item_by_slot_name(p, "weapon");
        bitflag f[OF_SIZE];
        bitflag collect_f[OF_SIZE];
        bool vuln[ELEM_MAX];

        /* Hack to allow calculating hypothetical blows for extra STR, DEX - NRM */
        int str_ind = state->stat_ind[STAT_STR];
        int dex_ind = state->stat_ind[STAT_DEX];

        /* Reset */
        memset(state, 0, sizeof *state);

        /* Set various defaults */
        state->speed = 110;
        state->num_blows = 100;

        /* Extract race/class info */
        state->see_infra = p->race->infra;
        for (i = 0; i < SKILL_MAX; i++) {
            state->skills[i] = p->race->r_skills[i]	+ p->class->c_skills[i];
        }
        for (i = 0; i < ELEM_MAX; i++) {
            vuln[i] = false;
            if (p->race->el_info[i].res_level == -1) {
                vuln[i] = true;
            } else {
                state->el_info[i].res_level = p->race->el_info[i].res_level;
            }
        }

        /* Base pflags */
        pf_wipe(state->pflags);
        pf_copy(state->pflags, p->race->pflags);
        pf_union(state->pflags, p->class->pflags);

        /* Extract the player flags */
        player_flags(p, collect_f);

        /* Analyze equipment */
        for (i = 0; i < p->body.count; i++) {
            int index = 0;
            struct object *obj = slot_object(p, i);
            struct curse_data *curse = obj ? obj->curses : NULL;

            while (obj) {
                int dig = 0;

                /* Extract the item flags */
                if (known_only) {
                    object_flags_known(obj, f);
                } else {
                    object_flags(obj, f);
                }
                of_union(collect_f, f);

                /* Apply modifiers */
                state->stat_add[STAT_STR] += obj->modifiers[OBJ_MOD_STR]
                    * p->obj_k->modifiers[OBJ_MOD_STR];
                state->stat_add[STAT_INT] += obj->modifiers[OBJ_MOD_INT]
                    * p->obj_k->modifiers[OBJ_MOD_INT];
                state->stat_add[STAT_WIS] += obj->modifiers[OBJ_MOD_WIS]
                    * p->obj_k->modifiers[OBJ_MOD_WIS];
                state->stat_add[STAT_DEX] += obj->modifiers[OBJ_MOD_DEX]
                    * p->obj_k->modifiers[OBJ_MOD_DEX];
                state->stat_add[STAT_CON] += obj->modifiers[OBJ_MOD_CON]
                    * p->obj_k->modifiers[OBJ_MOD_CON];
                state->skills[SKILL_STEALTH] += obj->modifiers[OBJ_MOD_STEALTH]
                    * p->obj_k->modifiers[OBJ_MOD_STEALTH];
                state->skills[SKILL_SEARCH] += (obj->modifiers[OBJ_MOD_SEARCH] * 5)
                    * p->obj_k->modifiers[OBJ_MOD_SEARCH];

                state->see_infra += obj->modifiers[OBJ_MOD_INFRA]
                    * p->obj_k->modifiers[OBJ_MOD_INFRA];
                if (tval_is_digger(obj)) {
                    if (of_has(obj->flags, OF_DIG_1))
                        dig = 1;
                    else if (of_has(obj->flags, OF_DIG_2))
                        dig = 2;
                    else if (of_has(obj->flags, OF_DIG_3))
                        dig = 3;
                }
                dig += obj->modifiers[OBJ_MOD_TUNNEL]
                    * p->obj_k->modifiers[OBJ_MOD_TUNNEL];
                state->skills[SKILL_DIGGING] += (dig * 20);
                state->speed += obj->modifiers[OBJ_MOD_SPEED]
                    * p->obj_k->modifiers[OBJ_MOD_SPEED];
                state->dam_red += obj->modifiers[OBJ_MOD_DAM_RED]
                    * p->obj_k->modifiers[OBJ_MOD_DAM_RED];
                extra_blows += obj->modifiers[OBJ_MOD_BLOWS]
                    * p->obj_k->modifiers[OBJ_MOD_BLOWS];
                extra_shots += obj->modifiers[OBJ_MOD_SHOTS]
                    * p->obj_k->modifiers[OBJ_MOD_SHOTS];
                extra_might += obj->modifiers[OBJ_MOD_MIGHT]
                    * p->obj_k->modifiers[OBJ_MOD_MIGHT];
                extra_moves += obj->modifiers[OBJ_MOD_MOVES]
                    * p->obj_k->modifiers[OBJ_MOD_MOVES];

                /* Apply element info, noting vulnerabilites for later processing */
                for (j = 0; j < ELEM_MAX; j++) {
                    if (!known_only || obj->known->el_info[j].res_level) {
                        if (obj->el_info[j].res_level == -1)
                            vuln[j] = true;

                        /* OK because res_level hasn't included vulnerability yet */
                        if (obj->el_info[j].res_level > state->el_info[j].res_level)
                            state->el_info[j].res_level = obj->el_info[j].res_level;
                    }
                }

                /* Apply combat bonuses */
                state->ac += obj->ac;
                if (!known_only || obj->known->to_a)
                    state->to_a += obj->to_a;
                if (!slot_type_is(p, i, EQUIP_WEAPON)
                        && !slot_type_is(p, i, EQUIP_BOW)) {
                    if (!known_only || obj->known->to_h) {
                        state->to_h += obj->to_h;
                    }
                    if (!known_only || obj->known->to_d) {
                        state->to_d += obj->to_d;
                    }
                }

                /* Move to any unprocessed curse object */
                if (curse) {
                    index++;
                    obj = NULL;
                    while (index < z_info->curse_max) {
                        if (curse[index].power) {
                            obj = curses[index].obj;
                            break;
                        } else {
                            index++;
                        }
                    }
                } else {
                    obj = NULL;
                }
            }
        }

        /* Apply the collected flags */
        of_union(state->flags, collect_f);

        /* Now deal with vulnerabilities */
        for (i = 0; i < ELEM_MAX; i++) {
            if (vuln[i] && (state->el_info[i].res_level < 3))
                state->el_info[i].res_level--;
        }

        /* Add shapechange info */
        calc_shapechange(state, p->shape, &extra_blows, &extra_shots, &extra_might,
            &extra_moves);

        /* Calculate light */
        calc_light(p, state, update);

        /* Unlight - needs change if anything but resist is introduced for dark */
        if (player_has(p, PF_UNLIGHT) && character_dungeon) {
            state->el_info[ELEM_DARK].res_level = 1;
        }

        /* Evil */
        if (player_has(p, PF_EVIL) && character_dungeon) {
            state->el_info[ELEM_NETHER].res_level = 1;
            state->el_info[ELEM_HOLY_ORB].res_level = -1;
        }

        /* Calculate the various stat values */
        for (i = 0; i < STAT_MAX; i++) {
            int add, use, ind;

            add = state->stat_add[i];
            add += (p->race->r_adj[i] + p->class->c_adj[i]);
            state->stat_top[i] =  modify_stat_value(p->stat_max[i], add);
            use = modify_stat_value(p->stat_cur[i], add);

            state->stat_use[i] = use;

            if (use <= 3) {/* Values: n/a */
                ind = 0;
            } else if (use <= 18) {/* Values: 3, 4, ..., 18 */
                ind = (use - 3);
            } else if (use <= 18+219) {/* Ranges: 18/00-18/09, ..., 18/210-18/219 */
                ind = (15 + (use - 18) / 10);
            } else {/* Range: 18/220+ */
                ind = (37);
            }

            assert((0 <= ind) && (ind < STAT_RANGE));

            /* Hack for hypothetical blows - NRM */
            if (!update) {
                if (i == STAT_STR) {
                    ind += str_ind;
                    ind = MIN(ind, 37);
                    ind = MAX(ind, 3);
                } else if (i == STAT_DEX) {
                    ind += dex_ind;
                    ind = MIN(ind, 37);
                    ind = MAX(ind, 3);
                }
            }

            /* Save the new index */
            state->stat_ind[i] = ind;
        }

        /* Effects of food outside the "Fed" range */
        if (!player_timed_grade_eq(p, TMD_FOOD, "Fed")) {
            int excess = p->timed[TMD_FOOD] - PY_FOOD_FULL;
            int lack = PY_FOOD_HUNGRY - p->timed[TMD_FOOD];
            if ((excess > 0) && !p->timed[TMD_ATT_VAMP]) {
                /* Scale to units 1/10 of the range and subtract from speed */
                excess = (excess * 10) / (PY_FOOD_MAX - PY_FOOD_FULL);
                state->speed -= excess;
            } else if (lack > 0) {
                /* Scale to units 1/20 of the range */
                lack = (lack * 20) / PY_FOOD_HUNGRY;

                /* Apply effects progressively */
                state->to_h -= lack;
                state->to_d -= lack;
                if ((lack > 10) && (lack <= 15)) {
                    adjust_skill_scale(&state->skills[SKILL_DEVICE],
                        -1, 10, 0);
                } else if ((lack > 15) && (lack <= 18)) {
                    adjust_skill_scale(&state->skills[SKILL_DEVICE],
                        -1, 5, 0);
                    state->skills[SKILL_DISARM_PHYS] *= 9;
                    state->skills[SKILL_DISARM_PHYS] /= 10;
                    state->skills[SKILL_DISARM_MAGIC] *= 9;
                    state->skills[SKILL_DISARM_MAGIC] /= 10;
                } else if (lack > 18) {
                    adjust_skill_scale(&state->skills[SKILL_DEVICE],
                        -3, 10, 0);
                    state->skills[SKILL_DISARM_PHYS] *= 8;
                    state->skills[SKILL_DISARM_PHYS] /= 10;
                    state->skills[SKILL_DISARM_MAGIC] *= 8;
                    state->skills[SKILL_DISARM_MAGIC] /= 10;
                    state->skills[SKILL_SAVE] *= 9;
                    state->skills[SKILL_SAVE] /= 10;
                    state->skills[SKILL_SEARCH] *=9;
                    state->skills[SKILL_SEARCH] /= 10;
                }
            }
        }

        /* Other timed effects */
        player_flags_timed(p, state->flags);

        if (player_timed_grade_eq(p, TMD_STUN, "Heavy Stun")) {
            state->to_h -= 20;
            state->to_d -= 20;
            adjust_skill_scale(&state->skills[SKILL_DEVICE], -1, 5, 0);
            if (update) {
                p->timed[TMD_FASTCAST] = 0;
            }
        } else if (player_timed_grade_eq(p, TMD_STUN, "Stun")) {
            state->to_h -= 5;
            state->to_d -= 5;
            adjust_skill_scale(&state->skills[SKILL_DEVICE], -1, 10, 0);
            if (update) {
                p->timed[TMD_FASTCAST] = 0;
            }
        }
        if (p->timed[TMD_INVULN]) {
            state->to_a += 100;
        }
        if (p->timed[TMD_BLESSED]) {
            state->to_a += 5;
            state->to_h += 10;
            adjust_skill_scale(&state->skills[SKILL_DEVICE], 1, 20, 0);
        }
        if (p->timed[TMD_SHIELD]) {
            state->to_a += 50;
        }
        if (p->timed[TMD_STONESKIN]) {
            state->to_a += 40;
            state->speed -= 5;
        }
        if (p->timed[TMD_HERO]) {
            state->to_h += 12;
            adjust_skill_scale(&state->skills[SKILL_DEVICE], 1, 20, 0);
        }
        if (p->timed[TMD_SHERO]) {
            state->skills[SKILL_TO_HIT_MELEE] += 75;
            state->to_a -= 10;
            adjust_skill_scale(&state->skills[SKILL_DEVICE], -1, 10, 0);
        }
        if (p->timed[TMD_FAST] || p->timed[TMD_SPRINT]) {
            state->speed += 10;
        }
        if (p->timed[TMD_SLOW]) {
            state->speed -= 10;
        }
        if (p->timed[TMD_SINFRA]) {
            state->see_infra += 5;
        }
        if (p->timed[TMD_TERROR]) {
            state->speed += 10;
        }
        if (p->timed[TMD_OPP_ACID] && (state->el_info[ELEM_ACID].res_level < 2)) {
                state->el_info[ELEM_ACID].res_level++;
        }
        if (p->timed[TMD_OPP_ELEC] && (state->el_info[ELEM_ELEC].res_level < 2)) {
                state->el_info[ELEM_ELEC].res_level++;
        }
        if (p->timed[TMD_OPP_FIRE] && (state->el_info[ELEM_FIRE].res_level < 2)) {
                state->el_info[ELEM_FIRE].res_level++;
        }
        if (p->timed[TMD_OPP_COLD] && (state->el_info[ELEM_COLD].res_level < 2)) {
                state->el_info[ELEM_COLD].res_level++;
        }
        if (p->timed[TMD_OPP_POIS] && (state->el_info[ELEM_POIS].res_level < 2)) {
                state->el_info[ELEM_POIS].res_level++;
        }
        if (p->timed[TMD_CONFUSED]) {
            adjust_skill_scale(&state->skills[SKILL_DEVICE], -1, 4, 0);
        }
        if (p->timed[TMD_AMNESIA]) {
            adjust_skill_scale(&state->skills[SKILL_DEVICE], -1, 5, 0);
        }
        if (p->timed[TMD_POISONED]) {
            adjust_skill_scale(&state->skills[SKILL_DEVICE], -1, 20, 0);
        }
        if (p->timed[TMD_IMAGE]) {
            adjust_skill_scale(&state->skills[SKILL_DEVICE], -1, 5, 0);
        }
        if (p->timed[TMD_BLOODLUST]) {
            state->to_d += p->timed[TMD_BLOODLUST] / 2;
            extra_blows += p->timed[TMD_BLOODLUST] / 20;
        }
        if (p->timed[TMD_STEALTH]) {
            state->skills[SKILL_STEALTH] += 10;
        }

        /* Analyze flags - check for fear */
        if (of_has(state->flags, OF_AFRAID)) {
            state->to_h -= 20;
            state->to_a += 8;
            adjust_skill_scale(&state->skills[SKILL_DEVICE], -1, 20, 0);
        }

        /* Analyze weight */
        j = p->upkeep->total_weight;
        i = weight_limit(state);
        if (j > i / 2)
            state->speed -= ((j - (i / 2)) / (i / 10));
        if (state->speed < 0)
            state->speed = 0;
        if (state->speed > 199)
            state->speed = 199;

        /* Apply modifier bonuses (Un-inflate stat bonuses) */
        state->to_a += adj_dex_ta[state->stat_ind[STAT_DEX]];
        state->to_d += adj_str_td[state->stat_ind[STAT_STR]];
        state->to_h += adj_dex_th[state->stat_ind[STAT_DEX]];
        state->to_h += adj_str_th[state->stat_ind[STAT_STR]];


        /* Modify skills */
        state->skills[SKILL_DISARM_PHYS] += adj_dex_dis[state->stat_ind[STAT_DEX]];
        state->skills[SKILL_DISARM_MAGIC] += adj_int_dis[state->stat_ind[STAT_INT]];
        state->skills[SKILL_DEVICE] += adj_int_dev[state->stat_ind[STAT_INT]];
        state->skills[SKILL_SAVE] += adj_wis_sav[state->stat_ind[STAT_WIS]];
        state->skills[SKILL_DIGGING] += adj_str_dig[state->stat_ind[STAT_STR]];
        for (i = 0; i < SKILL_MAX; i++)
            state->skills[i] += (p->class->x_skills[i] * p->lev / 10);

        if (state->skills[SKILL_DIGGING] < 1) state->skills[SKILL_DIGGING] = 1;
        if (state->skills[SKILL_STEALTH] > 30) state->skills[SKILL_STEALTH] = 30;
        if (state->skills[SKILL_STEALTH] < 0) state->skills[SKILL_STEALTH] = 0;
        hold = adj_str_hold[state->stat_ind[STAT_STR]];


        /* Analyze launcher */
        state->heavy_shoot = false;
        if (launcher) {
            if (hold < launcher->weight / 10) {
                state->to_h += 2 * (hold - launcher->weight / 10);
                state->heavy_shoot = true;
            }

            state->num_shots = 10;

            /* Type of ammo */
            if (kf_has(launcher->kind->kind_flags, KF_SHOOTS_SHOTS))
                state->ammo_tval = TV_SHOT;
            else if (kf_has(launcher->kind->kind_flags, KF_SHOOTS_ARROWS))
                state->ammo_tval = TV_ARROW;
            else if (kf_has(launcher->kind->kind_flags, KF_SHOOTS_BOLTS))
                state->ammo_tval = TV_BOLT;

            /* Multiplier */
            state->ammo_mult = launcher->pval;

            /* Apply special flags */
            if (!state->heavy_shoot) {
                state->num_shots += extra_shots;
                state->ammo_mult += extra_might;
                if (player_has(p, PF_FAST_SHOT)) {
                    state->num_shots += p->lev / 3;
                }
            }

            /* Require at least one shot */
            if (state->num_shots < 10) state->num_shots = 10;
        }


        /* Analyze weapon */
        state->heavy_wield = false;
        state->bless_wield = false;
        if (weapon) {
            /* It is hard to hold a heavy weapon */
            if (hold < weapon->weight / 10) {
                state->to_h += 2 * (hold - weapon->weight / 10);
                state->heavy_wield = true;
            }

            /* Normal weapons */
            if (!state->heavy_wield) {
                state->num_blows = calc_blows(p, weapon, state, extra_blows);
                state->skills[SKILL_DIGGING] += (weapon->weight / 10);
            }

            /* Divine weapon bonus for blessed weapons */
            if (player_has(p, PF_BLESS_WEAPON) && of_has(state->flags, OF_BLESSED)){
                state->to_h += 2;
                state->to_d += 2;
                state->bless_wield = true;
            }
        } else {
            /* Unarmed */
            state->num_blows = calc_blows(p, NULL, state, extra_blows);
        }

        /* Mana */
        calc_mana(p, state, update);
        if (!p->msp) {
            pf_on(state->pflags, PF_NO_MANA);
        }

        /* Movement speed */
        state->num_moves = extra_moves;

        return;
    }
}
