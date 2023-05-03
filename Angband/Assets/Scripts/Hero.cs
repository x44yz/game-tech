using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A single equipment slot
 */
public struct equip_slot {
	int type;
	string name;
    Entity obj;
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

public class Hero : Actor
{
    public RaceCfg race;
    public ClassCfg cls;

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

    public Entity equipped_item_by_slot_name(string slot)
    {
        throw new System.NotImplementedException();
    }

    public int chance_of_melee_hit(Entity weapon)
    {
        // int bonus = p->state.to_h + (weapon ? weapon->to_h : 0);
        // return p->state.skills[SKILL_TO_HIT_MELEE] + bonus * BTH_PLUS_ADJ;
        throw new System.NotImplementedException();
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
    Entity slot_object(int slot)
    {
        /* Check bounds */
        // assert(slot >= 0 && slot < p->body.count);

        // /* Ensure a valid body */
        // if (p->body.slots && p->body.slots[slot].obj) {
        //     return p->body.slots[slot].obj;
        // }

        // return NULL;
        throw new System.NotImplementedException();
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
    void improve_attack_modifier(Hero p, Entity obj,
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
    int melee_damage(Monster mon, Entity obj, int b, int s)
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


    public bool AttackImpl(Monster mon)
    {
        /* The weapon used */
        var obj = equipped_item_by_slot_name("weapon");

        /* Information about the attack */
        int drain = 0;
        int splash = 0;
        bool do_quake = false;
        bool success = false;
        int dmg = 0;
        int weight = 0;

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
                dmg, &msg_type);
        } else {
            dmg = o_melee_damage(p, mon, obj, b, s, &msg_type);
        }

        /* Splash damage and earthquakes */
        splash = (weight * dmg) / 100;
        if (player_of_has(p, OF_IMPACT) && dmg > 50) {
            do_quake = true;
            equip_learn_flag(p, OF_IMPACT);
        }

        /* Learn by use */
        equip_learn_on_melee_attack(p);
        learn_brand_slay_from_melee(p, obj, mon);

        /* Apply the player damage bonuses */
        if (!OPT(p, birth_percent_damage)) {
            dmg += player_damage_bonus(&p->state);
        }

        /* Substitute shape-specific blows for shapechanged players */
        if (player_is_shapechanged(p)) {
            int choice = randint0(p->shape->num_blows);
            struct player_blow *blow = p->shape->blows;
            while (choice--) {
                blow = blow->next;
            }
            my_strcpy(verb, blow->name, sizeof(verb));
        }

        /* No negative damage; change verb if no damage done */
        if (dmg <= 0) {
            dmg = 0;
            // msg_type = MSG_MISS;
            // my_strcpy(verb, "fail to harm", sizeof(verb));
            Debug.Log("xx-- fail to harm");
        }

        for (i = 0; i < N_ELEMENTS(melee_hit_types); i++) {
            const char *dmg_text = "";

            if (msg_type != melee_hit_types[i].msg_type)
                continue;

            if (OPT(p, show_damage))
                dmg_text = format(" (%d)", dmg);

            if (melee_hit_types[i].text)
                msgt(msg_type, "You %s %s%s. %s", verb, m_name, dmg_text,
                        melee_hit_types[i].text);
            else
                msgt(msg_type, "You %s %s%s.", verb, m_name, dmg_text);
        }

        /* Pre-damage side effects */
        blow_side_effects(p, mon);

        /* Damage, check for hp drain, fear and death */
        drain = MIN(mon->hp, dmg);
        stop = mon_take_hit(mon, p, dmg, fear, NULL);

        /* Small chance of bloodlust side-effects */
        if (p->timed[TMD_BLOODLUST] && one_in_(50)) {
            msg("You feel something give way!");
            player_over_exert(p, PY_EXERT_CON, 20, 0);
        }

        if (!stop) {
            if (p->timed[TMD_ATT_VAMP] && monster_is_living(mon)) {
                effect_simple(EF_HEAL_HP, source_player(), format("%d", drain),
                            0, 0, 0, 0, 0, NULL);
            }
        }

        if (stop)
            (*fear) = false;

        /* Post-damage effects */
        if (blow_after_effects(grid, dmg, splash, fear, do_quake))
            stop = true;

        return stop;
    }
}
