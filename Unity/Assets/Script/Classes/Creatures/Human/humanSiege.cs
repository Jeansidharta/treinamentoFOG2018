using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSiege : Creature {
   const int _maxHealth = 300;
   const int _maxActionPoints = 3;
   const int _baseDodge = 0;
   const int _defenseHeal = 35;
   const int _defenseResistance = 10;
   const int _attackDamage = 70;
   const int _attackRange = 3;
   const int _team = 0;

   public HumanSiege(GameObject prefab, int x, int y) : base(prefab, x, y, _maxActionPoints, _team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

   }
}
