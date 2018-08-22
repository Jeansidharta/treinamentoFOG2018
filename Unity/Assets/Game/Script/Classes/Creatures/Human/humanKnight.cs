using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanKnight : Creature {
   public static GameObject prefab;
   const int _maxHealth = 250;
   const int _maxActionPoints = 5;
   const int _baseDodge = 10;
   const int _defenseHeal = 20;
   const int _defenseResistance = 20;
   const int _attackDamage = 70;
   const int _attackRange = 1;

   const int _maxAssaultCooldown = 5;
   const int _minAssaultAP = 0;
   private bool isAssaulting = false;

   public HumanKnight(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {
      skills[0] = new Skill("Investida", "Investida: (CD = 5) (AP = 0) (alcance = nulo)\n\nNo turno quando essa habilidade for ativada o dano do cavaleiro real é aumentado em 70 ptos, no entanto, seu ataque consome todo o seu AP disponível.\n", assault, this, _minAssaultAP, _maxAssaultCooldown);
   }

   public override void attack(Creature victim) {
      if (isAssaulting) {
         isAssaulting = false;
         attackDamage += 70;
         base.attack(victim);
         attackDamage -= 70;
      }
      else {
         int ap = this.actionPoints - 1;
         base.attack(victim);
         this.actionPoints = ap;
      }
   }

   public override void newTurn(int turnNumber) {
      base.newTurn(turnNumber);
      isAssaulting = false;
   }

   public override void move(int x, int y, int distance) {
      if(terrain is Plains) actionPoints++;
      base.move(x, y, distance);
   }

   public void assault() {
      if(!skills[0].use()) return;
      Debug.Log("knight entered assault mode");
      isAssaulting = true;
   }
}