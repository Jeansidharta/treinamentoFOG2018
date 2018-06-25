using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadSiege : Creature {
   const int _maxHealth = 500;
   const int _maxActionPoints = 2;
   const int _baseDodge = 0;
   const int _defenseHeal = 50;
   const int _defenseResistance = 30;
   const int _attackDamage = 20;
   const int _attackRange = 1;
   const int _team = 1;

   public UndeadSiege(GameObject prefab, int x, int y) : base(prefab, x, y, _maxActionPoints, _team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

   }
}
