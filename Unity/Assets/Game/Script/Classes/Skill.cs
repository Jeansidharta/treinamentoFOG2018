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

   private consoledisplayer console = GameObject.FindGameObjectWithTag("Console").GetComponent<consoledisplayer>();

   public delegate void SkillFunction();
   public SkillFunction function;

   public Skill(string name, string description, SkillFunction function, Creature caster, int minAp, int maxCooldown){
      this.name = name;
      this.description = description;
      this.function = function;
      this.caster = caster;
      this.minAP = minAp;
      this.maxCooldown = maxCooldown;
   }

   public bool canUse(){
      if(caster.actionPoints < minAP || (minAP == -1 && caster.actionPoints == 0)){
         console.Log("not enough action points for " + name + "\n");
         return false;
      }
      if(cooldown > 0){
         console.Log("You must wait " + cooldown + " turns to use " + name + "\n");
         return false;
      }
      return true;
   }

   public bool use(Creature victim = null){
      if(!canUse()) return false;
      if(victim is UndeadKnight && (victim as UndeadKnight).isImmaterial){
         GameController.console.Log("cannot target immaterial undead knight\n");
         return false;
      }
      GameController.guiController.reload();
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
