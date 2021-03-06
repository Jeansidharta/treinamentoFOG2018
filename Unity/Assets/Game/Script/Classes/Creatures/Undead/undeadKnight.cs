﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadKnight : Creature {
   public static GameObject prefab;
   const int _maxHealth = 250;
   const int _maxActionPoints = 5;
   const int _baseDodge = 20;
   const int _defenseHeal = 20;
   const int _defenseResistance = 20;
   const int _attackDamage = 60;
   const int _attackRange = 1;

   const int _minImaterialAP = 0;
   const int _maxImaterialCooldown = 4;

   public bool isImmaterial = false;

    public UndeadKnight(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {
      skills[0] = new Skill("Imaterial", "Imaterial: (Cd = 4) (AP = 0) (Alcance = nulo)\n\nDurante o próximo turno do inimigo o cavaleiro espectral não poderá receber dano ou ser alvo de qualquer habilidade.\n", imaterial, this, _minImaterialAP, _maxImaterialCooldown);
   }

   public override void attack(Creature victim) {
      if(victim is UndeadKnight && (victim as UndeadKnight).isImmaterial){
         GameController.console.Log("Cannot attack immaterial undead knight\n");
         return;
      }
      if(hasAttacked){
         GameController.console.Log("a horse can only attack once\n");
         return;
      }
      if(actionPoints > 0){
         int ap = this.actionPoints - 1;
         base.attack(victim);
         this.actionPoints = ap;
      }
      else{
         GameController.console.Log("not enough action points\n");
      }
   }

   public void imaterial(){
      if(!skills[0].use()) return;
      GameController.console.Log("Using immaterial\n");
      buffs.add_buff("Imaterial");
      isImmaterial = true;
   }

   public override void newTeamTurn(){
      base.newTeamTurn();
      if(isImmaterial){
         isImmaterial = false;
         buffs.remove_buff("Imaterial");
      }
   }
}
