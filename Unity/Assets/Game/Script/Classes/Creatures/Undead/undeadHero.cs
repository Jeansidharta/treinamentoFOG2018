using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadHero : Creature {
   public static GameObject prefab;
   const int _maxHealth = 900;
   const int _maxActionPoints = 5;
   const int _baseDodge = 15;
   const int _defenseHeal = 50;
   const int _defenseResistance = 30;
   const int _attackDamage = 75;
   const int _attackRange = 2;

   const int _maxReviveCooldown = 7;
   const int _minReviveAP = 3;
   private Surroundings reviveSurroundings = null;

   const int _maxPossessCooldown = 7;
   const int _minPossessAP = 3;
   private Surroundings possessSurroundings = null;

   const int _maxOnslaughtCooldown = 10;
   const int _minOnslaughtAP = 5;

    public UndeadHero(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {
      skills[0] = new Skill("Renascer", "Renascer: (CD = 7) (AP = 3) (Alcance = 1)\n\nAo usar renascer Zarasputin invoca uma unidade do tipo “Guerreiro esquelético” da classe infantaria ao seu lado. Essa unidade inicia com 0 de AP, retornando depois para o seu nível padrão.\n", previewRevive, this, _minReviveAP, _maxReviveCooldown);
      
      skills[1] = new Skill("Possessão", "Possessão: (CD = 7) (Ap = 3) (Alcance = 3)\n\nAo usar possessão o jogador selecionará um inimigo a 3 de distância ou menos e poderá move-lo para qualquer lugar a 3 de distância ou menos de onde essa unidade está.\n", previewPossess, this, _minPossessAP, _maxPossessCooldown);
      
      skills[2] = new Skill("Investida macabra", "Investida macabra: (CD = 10) (AP = 5) (Alcance = nulo)\n\nPor um turno dobre a quantidade de AP de todas as suas outras unidades.\n", onslaught, this, _minOnslaughtAP, _maxOnslaughtCooldown);
   }

   public void previewRevive(){
      if(!skills[0].canUse()) return;
      reviveSurroundings = terrain.expandByDistance(1);
      GameController.overrideClick(tryRevive);
      reviveSurroundings.paint(Color.blue);
   }

   public void tryRevive(Terrain terrain){
      reviveSurroundings.clear();
      if(terrain.creature != null || terrain is Mountain){
         GameController.console.Log("invalid target\n");
         return;
      }
      if(!reviveSurroundings.hasTerrain(terrain.x, terrain.y)){
         GameController.console.Log("out of range\n");
         return;
      }
      if(!skills[0].use()) return;
      UndeadSoldier soldier = new UndeadSoldier(terrain.x, terrain.y, team);
      soldier.actionPoints = 0;
   }

   public void previewPossess(){
      if(!skills[1].canUse()) return;
      possessSurroundings = terrain.expandByDistance(3);
      GameController.overrideClick(tryPossess);
      possessSurroundings.paint(Color.blue);
   }

   public void tryPossess(Terrain terrain){
      possessSurroundings.clear();

      if(!possessSurroundings.hasTerrain(terrain.x, terrain.y)){
         Debug.Log("out of range");
         return;
      }
      if(terrain.creature == null){
         GameController.console.Log("invalid target\n");
         return;
      }
      if(terrain.creature is UndeadKnight && (terrain.creature as UndeadKnight).isImmaterial){
         GameController.console.Log("Cannot use habilities on immaterial undead knight");
         return;
      }
      GameController.overrideClick(tryPossess2);
      possessSurroundings = terrain.expandByDistance(3);
      possessSurroundings.paint(Color.blue);
   }

   public void tryPossess2(Terrain terrain){
      possessSurroundings.clear();
      if(!possessSurroundings.hasTerrain(terrain.x, terrain.y)){
         GameController.console.Log("out of range\n");
         return;
      }
      if(terrain.creature != null){
         GameController.console.Log("invalid target\n");
         return;
      }
      if(!skills[1].use(terrain.creature)) return;
      possessSurroundings.origin.creature.setPos(terrain.x, terrain.y);
      possessSurroundings = null;
   }

   public void onslaught(){
      if(!skills[2].use()) return;
      for(int aux = 0; aux < Creature.allCreatures.Count; aux ++){
         Creature creature = Creature.allCreatures[aux];
         if(creature.team == team){
            creature.actionPoints += creature.maxActionPoints;
         }
      }
   }
}