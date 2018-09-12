using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSoldier : Creature {
   public static GameObject prefab;
   const int _maxHealth = 400;
   const int _maxActionPoints = 3;
   const int _baseDodge = 10;
   const int _defenseHeal = 40;
   const int _defenseResistance = 30;
   const int _attackDamage = 40;
   const int _attackRange = 1;

   const int _maxRaiseShieldsCooldown = 3;
   const int _minRaiseShieldsAP = 3;

   private bool areShieldsRaised = false;

   public HumanSoldier(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

      skills[0] = new Skill("Levantar Escudos", "Levantar escudos: (CD = 3) (AD = 3) (Alcance = nulo)\n\nPor um turno, a resistência em defender é triplicada enquanto a cura é duplicada.\n", raiseShields, this, _minRaiseShieldsAP, _maxRaiseShieldsCooldown);

   }

   public override void newTeamTurn(){
      if(areShieldsRaised){
         defenseHeal /= 2;
         defenseResistance /= 3;
      }
      base.newTeamTurn();
   }

   public void raiseShields(){
      if(!skills[0].use()) return;
      GameController.console.Log("raising shields, also automatically defending\n");
      areShieldsRaised = true;
      defenseHeal *= 2;
      defenseResistance *= 3;
      defend();
      return;
   }
}
