using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSoldier : Creature {
   const int _maxHealth = 400;
   const int _maxActionPoints = 3;
   const int _baseDodge = 10;
   const int _defenseHeal = 40;
   const int _defenseResistance = 30;
   const int _attackDamage = 40;
   const int _attackRange = 1;
   const int _team = 0;

   public HumanSoldier(GameObject prefab, int x, int y) : base(prefab, x, y, _maxActionPoints, _team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

   }
}
