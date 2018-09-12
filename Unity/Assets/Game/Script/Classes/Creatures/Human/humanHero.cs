using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHero : Creature {
   public static GameObject prefab;
   const int _maxHealth = 1000;
   const int _maxActionPoints = 5;
   const int _baseDodge = 15;
   const int _defenseHeal = 75;
   const int _defenseResistance = 30;
   const int _attackDamage = 100;
   const int _attackRange = 1;

   const int _areaHealingAmmount = 15;
   const int _areaHealingRange = 1;

   const int _minFortressAP = -1;
   const int _maxFortressCooldown = 7;
   
   const int _maxCornerCooldown = 7;
   const int _minCornerAP = 2;
   
   const int _maxLastResourcesCooldown = 5;
   const int _minLastResourcesAP = 2;
   List<Pair<Creature, int>> lastResourcesTargets = null;

   public Surroundings cornerSurroundings = null;

   public HumanHero(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {

      skills[0] = new Skill("Fortaleza", "Fortaleza: (CD = 7) (AP = tudo que tiver, min 1) (alcance = infinito)\nFortaleza irá criar um novo terreno de fortaleza do estilo “Forte de madeira” em um terreno selecionado, o forte de madeira é similar a uma fortaleza comum, mas o bônus é 30 ao invés do 50 normal. Fortalezas de madeira podem apodrecer, um processo que a destruirá em 15 turnos.\n", previewFortress, this, _minFortressAP, _maxFortressCooldown);
      
      skills[1] = new Skill("Encurralar", "Encurralar: (CD = 5) (AP = 2) (alcance = 2)\n\nAo usar encurralar o jogador escolhe uma unidade inimiga próxima, até dois aliados próximos irão causar seu dano de ataque básico nessa unidade, esses “ataques” não irão contar como ataque de verdade para essas unidades, que poderão atacar de verdade depois se desejarem.\n", previewCorner, this, _minCornerAP, _maxCornerCooldown);
      
      skills[2] = new Skill("Últimos Recursos", "Últimos Recursos: (CD = 5) (AP = 3) (alcance = 1)\n\nDefensor dos povos concede a todas as unidades aliadas ao seu redor defesa equivalente a 150% da porcentagem vida perdida dessas unidades.\n", lastResource, this, _minLastResourcesAP, _maxLastResourcesCooldown);
   }

   public override void newTeamTurn(){
      base.newTeamTurn();
      if(lastResourcesTargets != null){
         for(int aux = 0; aux < lastResourcesTargets.Count; aux ++){
            Creature creature = lastResourcesTargets[aux].first;
            int ammount = lastResourcesTargets[aux].second;
            creature.defenseResistance -= ammount;
         }
         GameController.console.Log("removing defenses\n");
         lastResourcesTargets = null;
      }
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

   public void previewFortress(){
      if(!skills[0].canUse()) return;
      GameController.overrideClick(trySetFortress);
   }

   public void trySetFortress(Terrain terrain){
      if(terrain.fortress != null){
         GameController.console.Log("already has a fortress here\n");
         return;
      }
      new HumanWoodenFortress(terrain, team);
      if(!skills[0].use()) return;
   }

   public void lastResource(){
      if(!skills[2].use()) return;
      Surroundings surroundings = terrain.expandByDistance(1);
      lastResourcesTargets = new List<Pair<Creature, int>>();
      GameController.console.Log("using last resource\n");
      for(int aux = 0; aux < surroundings.creatures.Count; aux ++){
         Creature creature = surroundings.creatures[aux].second;
         if(creature == this) continue;
         if(creature.team == team){
            int ammount = (int)((float)(creature.maxHealth - creature.health) * 1.5);
            creature.defenseResistance += ammount;
            lastResourcesTargets.Add(new Pair<Creature, int>(creature, ammount));
            GameController.console.Log("giving " + ammount + " defense to + " + creature.getName() + "\n");
         }
      }
   }

   public void previewCorner(){
      if(!skills[1].canUse()) return;
      cornerSurroundings = terrain.expandByDistance(2);
      GameController.overrideClick(tryCorner);
      cornerSurroundings.paint(Color.blue);
   }

   public void tryCorner(Terrain terrain){
      cornerSurroundings.clear();
      if(terrain.creature == null || terrain.creature.team == team){
         GameController.console.Log("invalid target\n");
         return;
      }
      if(!cornerSurroundings.hasTerrain(terrain.x, terrain.y)){
         GameController.console.Log("out of reach\n");
         return;
      }
      if(!skills[1].use(terrain.creature)) return;
      int bestCount = 0;
      int secondBestCount = 0;
      Creature best;
      Creature secondBest;
      for(int aux = 0; aux < cornerSurroundings.creatures.Count; aux ++){
         Creature creature = cornerSurroundings.creatures[aux].second;
         if(creature == this) continue;
         if(creature.team == team){
            if(creature.attackDamage > bestCount){
               bestCount = creature.attackDamage;
               best = creature;
            }
            else if(creature.attackDamage > secondBestCount){
               secondBest = creature;
               secondBestCount = creature.attackDamage;
            }
         }
      }
      int buffer = attackDamage;
      attackDamage = bestCount + secondBestCount;
      terrain.creature.receiveAttack(this);
      attackDamage = buffer;
      GameController.console.Log("used corner\n");
   }
}
