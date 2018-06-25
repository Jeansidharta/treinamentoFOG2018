using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanKnight: Creature {
   const int _maxHealth = 250;
   const int _maxActionPoints = 5;
   const int _baseDodge = 10;
   const int _defenseHeal = 20;
   const int _defenseResistance = 20;
   const int _attackDamage = 70;
   const int _attackRange = 1;
   const int _team = 0;

   public HumanKnight(GameObject prefab, int x, int y) : base(prefab, x, y, _maxActionPoints, _team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

   }
}