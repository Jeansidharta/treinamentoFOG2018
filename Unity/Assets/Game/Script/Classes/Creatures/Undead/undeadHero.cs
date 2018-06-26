﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadHero : Creature {
   public static GameObject prefab;
   
   const int _maxHealth = 900;
   const int _maxActionPoints = 5;
   const int _baseDodge = 15;
   const int _defenseHeal = 50;
   const int _defenseResistance = 30;
   const int _attackDamage = 75;
   const int _attackRange = 2;

   public UndeadHero(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

   }
}
