using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadHero : Creature {
   public static GameObject prefab;

   const string _name = "Zarasputim";
   const string _teamName = "Undeads";
   static string[] _skillsNames = new string[3] { "Renascer", "Possessão", "Investida macabra" };
   static string[] _skillsDescriptions = new string[3] { @"Renascer: (CD = 7) (AP = 3) (Alcance = 1)
Ao usar renascer Zarasputin invoca uma unidade do tipo “Guerreiro esquelético” da classe infantaria ao seu lado. Essa unidade inicia com 0 de AP, retornando depois para o seu nível padrão.
", @"Possessão: (CD = 7) (Ap = 3) (Alcance = 3)
Ao usar possessão o jogador selecionará um inimigo a 3 de distância ou menos e poderá move-lo para qualquer lugar a 3 de distância ou menos de onde essa unidade está.
", @"Investida macabra: (CD = 10) (AP = 5) (Alcance = nulo)
Por um turno dobre a quantidade de AP de todas as suas outras unidades.
" };

    const int _maxHealth = 900;
   const int _maxActionPoints = 5;
   const int _baseDodge = 15;
   const int _defenseHeal = 50;
   const int _defenseResistance = 30;
   const int _attackDamage = 75;
   const int _attackRange = 2;

   public UndeadHero(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge, _name, _teamName, _skillsNames, _skillsDescriptions) {

   }
}
