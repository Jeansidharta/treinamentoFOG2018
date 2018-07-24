using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHero : Creature {
   public static GameObject prefab;

   const string _name = "Sir Godfrey";
   const string _teamName = "Humans";
   const int _maxHealth = 1000;
   const int _maxActionPoints = 5;
   const int _baseDodge = 15;
   const int _defenseHeal = 75;
   const int _defenseResistance = 30;
   const int _attackDamage = 100;
   const int _attackRange = 1;
   static string[] _skillsNames = new string[3] { "Fortaleza", "Encurralar", "Últimos Recursos" };
   static string[] _skillsDescriptions = new string[3] { @"Fortaleza: (CD = 7) (AP = tudo que tiver, min 1) (alcance = infinito)
Fortaleza irá criar um novo terreno de fortaleza do estilo “Forte de madeira” em um terreno selecionado, o forte de madeira é similar a uma fortaleza comum, mas o bônus é 30 ao invés do 50 normal. Fortalezas de madeira podem apodrecer, um processo que a destruirá em 15 turnos.
", @"Encurralar: (CD = 5) (AP = 2) (alcance = 2)
Ao usar encurralar o jogador escolhe uma unidade inimiga próxima, até dois aliados próximos irão causar seu dano de ataque básico nessa unidade, esses “ataques” não irão contar como ataque de verdade para essas unidades, que poderão atacar de verdade depois se desejarem.
", @"Últimos Recursos: (CD = 5) (AP = 3) (alcance = 1)
Defensor dos povos concede a todas as unidades aliadas ao seu redor defesa equivalente a 150% da porcentagem vida perdida dessas unidades.
" };

    const int _areaHealingAmmount = 15;
   const int _areaHealingRange = 1;

   public HumanHero(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge, _name, _teamName, _skillsNames, _skillsDescriptions) {

   }

   public override bool defend(){
      if(!base.defend())
         return false;
      Surroundings surroundings = terrain.expandByDistance(_areaHealingRange);
      for(int aux = 1; aux < surroundings.creatures.Count; aux ++){
         surroundings.creatures[aux].second.changeHealth(_areaHealingAmmount);
      }
      return true;
   }
}
