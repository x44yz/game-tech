using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  /**
    * The object flags
    */
    public enum OF {
        OF_NONE,
        OF_SUST_STR,
        OF_SUST_INT,
        OF_SUST_WIS,
        OF_SUST_DEX,
        OF_SUST_CON,
        OF_PROT_FEAR,
        OF_PROT_BLIND,
        OF_PROT_CONF,
        OF_PROT_STUN,
        OF_SLOW_DIGEST,
        OF_FEATHER,
        OF_REGEN,
        OF_TELEPATHY,
        OF_SEE_INVIS,
        OF_FREE_ACT,
        OF_HOLD_LIFE,
        OF_IMPACT,
        OF_BLESSED,
        OF_BURNS_OUT,
        OF_TAKES_FUEL,
        OF_NO_FUEL,
        OF_IMPAIR_HP,
        OF_IMPAIR_MANA,
        OF_AFRAID,
        OF_NO_TELEPORT,
        OF_AGGRAVATE,
        OF_DRAIN_EXP,
        OF_STICKY,
        OF_FRAGILE,
        OF_LIGHT_2,
        OF_LIGHT_3,
        OF_DIG_1,
        OF_DIG_2,
        OF_DIG_3,
        OF_EXPLODE,
        OF_TRAP_IMMUNE,
        OF_THROWING,
        OF_MAX,
    };

public class GObject
{
    public int weight;

	public int dd;		/**< Number of damage dice */
	public int ds;		/**< Number of sides on each damage die */
	public int ac;		/**< Normal AC */
	public int to_a;		/**< Plusses to AC */
	public int to_h;		/**< Plusses to hit */
	public int to_d;		/**< Plusses to damage */
}
