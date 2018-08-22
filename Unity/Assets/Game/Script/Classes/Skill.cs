using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill{
   public string name;
   public string description;

   public int minAP;
   public int maxCooldown;

   public int cooldown = 0;

   Creature caster;
   
   public delegate void SkillFunction();
   public SkillFunction function;

   public Skill(string name, string description, SkillFunction function, Creature caster, int minAp, int maxCooldown){
      this.name = name;
      this.description = description;
      this.function = function;
      this.caster = caster;
   }

   public bool canUse(){
      if(caster.actionPoints < minAP || (minAP == -1 && caster.actionPoints == 0)){
         Debug.Log("not enough action points for " + name);
         return false;
      }
      if(cooldown > 0){
         Debug.Log("You must wait " + cooldown + " to use " + name);
         return false;
      }
      return true;
   }

   public bool use(){
      if(!canUse()) return false;
      cooldown = maxCooldown;
      if(minAP == -1)
         caster.useActionPoints(caster.actionPoints);
      else
         caster.useActionPoints(minAP);
      return true;
   }

   public void newTeamTurn(){
      if(cooldown > 0) cooldown--;
   }
}
