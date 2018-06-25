using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadArcher : Creature {
   const int _maxHealth = 300;
   const int _maxActionPoints = 3;
   const int _baseDodge = 15;
   const int _defenseHeal = 30;
   const int _defenseResistance = 20;
   const int _attackDamage = 30;
   const int _attackRange = 2;
   const int _team = 1;

   public UndeadArcher(GameObject prefab, int x, int y) : base(prefab, x, y, _maxActionPoints, _team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

   }
}
