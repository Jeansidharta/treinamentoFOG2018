using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadHero : Creature {
   const int _maxHealth = 900;
   const int _maxActionPoints = 5;
   const int _baseDodge = 15;
   const int _defenseHeal = 50;
   const int _defenseResistance = 30;
   const int _attackDamage = 75;
   const int _attackRange = 2;
   const int _team = 1;

   public UndeadHero(GameObject prefab, int x, int y) : base(prefab, x, y, _maxActionPoints, _team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

   }
}
