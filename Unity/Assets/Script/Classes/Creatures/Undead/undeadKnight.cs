using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadKnight : Creature {
   const int _maxHealth = 250;
   const int _maxActionPoints = 5;
   const int _baseDodge = 20;
   const int _defenseHeal = 20;
   const int _defenseResistance = 20;
   const int _attackDamage = 60;
   const int _attackRange = 1;
   const int _team = 1;

   public UndeadKnight(GameObject prefab, int x, int y) : base(prefab, x, y, _maxActionPoints, _team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

   }
}
