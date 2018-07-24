using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadKnight : Creature {
   public static GameObject prefab;

   const string _name = "Knight";
   const string _teamName = "Undeads";
   static string[] _skillsNames = new string[1] {"Imaterial"};
   static string[] _skillsDescriptions = new string[1] { @"Imaterial: (Cd = 4) (AP = 0) (Alcance = nulo)
Durante o próximo turno do inimigo o cavaleiro espectral não poderá receber dano ou ser alvo de qualquer habilidade.
" };

    const int _maxHealth = 250;
   const int _maxActionPoints = 5;
   const int _baseDodge = 20;
   const int _defenseHeal = 20;
   const int _defenseResistance = 20;
   const int _attackDamage = 60;
   const int _attackRange = 1;

   public UndeadKnight(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge, _name, _teamName, _skillsNames, _skillsDescriptions) {

   }
}
