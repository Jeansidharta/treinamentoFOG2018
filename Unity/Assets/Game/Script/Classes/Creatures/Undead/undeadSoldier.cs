using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadSoldier : Creature {
   public static GameObject prefab;


   const string _name = "Soldier";
   const string _teamName = "Undeads";
   static string[] _skillsNames = new string[1] {"Toque amaldiçoado"};
   static string[] _skillsDescriptions = new string[1] { @"Toque amaldiçoado: (CD  = 5) (AP = 1) (Alcance = 1)
Por um turno, unidade inimiga selecionada recebe 5% mais de dano de todas as fontes. Uma unidade pode receber o toque amaldiçoado de múltiplos guerreiros esqueléticos diferentes. Se a unidade morrer naquele turno, crie um guerreiro esquelético com vida máxima 1.
"};

   const int _maxHealth = 250;
   const int _maxActionPoints = 3;
   const int _baseDodge = 0;
   const int _defenseHeal = 30;
   const int _defenseResistance = 20;
   const int _attackDamage = 40;
   const int _attackRange = 1;

   public UndeadSoldier(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge, _name, _teamName, _skillsNames, _skillsDescriptions) {

   }
}
