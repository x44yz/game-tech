using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Minecraft
{
    public class Unit : MonoBehaviour
    {   
        public int hp;
        public int attack;
        public int defence;


        // public void Attack(mtUnit target)
        // {
        //     // https://rpg.fandom.com/wiki/Damage_Formula
        //     int damage = (int)(100.0f / (100.0f + target.defence) * attack);
        //     target.TakeDamage(damage);
        // }
        
        // public void TakeDamage(int damage)
        // {
        //     hp -= damage;
        //     hp = Mathf.Max(hp, 0);
        // }
    }
}

