using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadArcher : Creature {
   public static GameObject prefab;

   const string _name = "Archer";
   const string _teamName = "Undeads";
   static string[] _skillsNames = new string[1] {"Tóxico"};
   static string[] _skillsDescriptions = new string[1] { @"Tóxico: (CD = 4) (AP = 0) (Alcance = nulo)
Nesse turno ao invés do ataque conceder apenas 1 contador do veneno (aumentar em 5 qualquer que seja o valor de veneno atual) ele envenenará ao máximo o alvo. Qualquer unidade a 1 de alcance do alvo também recebera veneno, no entanto sendo apenas um 1 contador.
"};

    const int _maxHealth = 300;
   const int _maxActionPoints = 3;
   const int _baseDodge = 15;
   const int _defenseHeal = 30;
   const int _defenseResistance = 20;
   const int _attackDamage = 30;
   const int _attackRange = 2;

   public UndeadArcher(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge, _name, _teamName, _skillsNames, _skillsDescriptions) {

   }
}
