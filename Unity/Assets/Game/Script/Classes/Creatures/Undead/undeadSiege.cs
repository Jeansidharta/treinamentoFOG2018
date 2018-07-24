using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadSiege : Creature {
   public static GameObject prefab;

   const string _name = " Siege";
   const string _teamName = "Undeads";
   static string[] _skillsNames = new string[2] {"Renascer", "Suprimir"};
   static string[] _skillsDescriptions = new string[2] { @"Renascer: (CD = 6) (AP = 2) (alcance = 1)
Igual ao Renascer do herói, apenas com um CD menor e custo de AP
", @"Suprimir: (CD = 4) (AP = 2) (Alcance = 2)
Selecione uma unidade inimiga que não for o Herói, essa unidade não poderá se mover ou usar habilidades especiais no próximo turno do oponente. Ele poderá, no entanto, ainda se defender e atacar.
"};

    const int _maxHealth = 500;
   const int _maxActionPoints = 2;
   const int _baseDodge = 0;
   const int _defenseHeal = 50;
   const int _defenseResistance = 30;
   const int _attackDamage = 20;
   const int _attackRange = 1;

   public UndeadSiege(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge, _name, _teamName, _skillsNames, _skillsDescriptions) {

   }
}
