using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHero : Creature {
   const int _maxHealth = 1000;
   const int _maxActionPoints = 5;
   const int _baseDodge = 15;
   const int _defenseHeal = 75;
   const int _defenseResistance = 30;
   const int _attackDamage = 100;
   const int _attackRange = 1;
   const int _team = 0;

   public HumanHero(GameObject prefab, int x, int y) : base(prefab, x, y, _maxActionPoints, _team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

   }
}
