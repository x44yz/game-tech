using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Actor
{
    public RaceCfg race;
    public ClassCfg cls;

    public void Attack(Actor target)
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

        AttackImpl(target);
    }

    public Entity equipped_item_by_slot_name(string slot)
    {
        throw new System.NotImplementedException();
    }

    public void AttackImpl(Actor target)
    {
        /* The weapon used */
        Entity obj = equipped_item_by_slot_name("weapon");

        /* Information about the attack */
        int drain = 0;
        int splash = 0;
        bool do_quake = false;
        bool success = false;

        /* Auto-Recall and track if possible and visible */
        if (monster_is_visible(mon)) {
            monster_race_track(p->upkeep, mon->race);
            health_track(p->upkeep, mon);
        }

        /* Handle player fear (only for invisible monsters) */
        if (player_of_has(p, OF_AFRAID)) {
            equip_learn_flag(p, OF_AFRAID);
            msgt(MSG_AFRAID, "You are too afraid to attack %s!", m_name);
            return false;
        }

        /* Disturb the monster */
        monster_wake(mon, false, 100);
        mon_clear_timed(mon, MON_TMD_HOLD, MON_TMD_FLG_NOTIFY);

        /* See if the player hit */
        success = test_hit(chance_of_melee_hit(p, obj, mon), mon->race->ac);

        /* If a miss, skip this hit */
        if (!success) {
            msgt(MSG_MISS, "You miss %s.", m_name);

            /* Small chance of bloodlust side-effects */
            if (p->timed[TMD_BLOODLUST] && one_in_(50)) {
                msg("You feel strange...");
                player_over_exert(p, PY_EXERT_SCRAMBLE, 20, 20);
            }

            return false;
        }

        if (obj) {
            /* Handle normal weapon */
            weight = obj->weight;
            my_strcpy(verb, "hit", sizeof(verb));
        } else {
            weight = 0;
        }

        /* Best attack from all slays or brands on all non-launcher equipment */
        b = 0;
        s = 0;
        for (j = 2; j < p->body.count; j++) {
            struct object *obj_local = slot_object(p, j);
            if (obj_local)
                improve_attack_modifier(p, obj_local, mon, &b, &s,
                    verb, false);
        }

        /* Get the best attack from all slays or brands - weapon or temporary */
        if (obj) {
            improve_attack_modifier(p, obj, mon, &b, &s, verb, false);
        }
        improve_attack_modifier(p, NULL, mon, &b, &s, verb, false);

        /* Get the damage */
        if (!OPT(p, birth_percent_damage)) {
            dmg = melee_damage(mon, obj, b, s);
            /* For now, exclude criticals on unarmed combat */
            if (obj) dmg = critical_melee(p, mon, weight, obj->to_h,
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
            msg_type = MSG_MISS;
            my_strcpy(verb, "fail to harm", sizeof(verb));
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
