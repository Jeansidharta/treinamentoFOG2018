using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadSiege : Creature {
   public static GameObject prefab;
   const int _maxHealth = 500;
   const int _maxActionPoints = 2;
   const int _baseDodge = 0;
   const int _defenseHeal = 50;
   const int _defenseResistance = 30;
   const int _attackDamage = 20;
   const int _attackRange = 3;
   
   const int _maxMountCooldown = 0;
   const int _minMountAP = 2;
   public bool isMounted = false;

   const int _maxSupressCooldown = 4;
   const int _minSupressAP = 2;
   private Surroundings supressSurroundings = null;
   private Creature supressTarget = null;

   const int _maxReviveCooldown = 6;
   const int _minReviveAP = 2;
   private Surroundings reviveSurroundings = null;

   public UndeadSiege(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {
      skills[0] = new Skill("Montar", "Montar (AP = 2):\n\nPara uma unidade de catapulta ser capaz de atirar ela precisa primeiro se montar o que custa AP para fazer. Depois de montada a catapulta pode atacar normalmente desde que ela não se movimente, se a catapulta se mover ela tem que se montar novamente.\n", toggleMount, this, _minMountAP, _maxMountCooldown);
      
      skills[1] = new Skill("Renascer", "Renascer: (CD = 6) (AP = 2) (alcance = 1)\n\nIgual ao Renascer do herói, apenas com um CD menor e custo de AP\n", previewRevive, this, _minReviveAP, _maxReviveCooldown);
      
      skills[2] = new Skill("Suprimir", "Suprimir: (CD = 4) (AP = 2) (Alcance = 2)\n\nSelecione uma unidade inimiga que não for o Herói, essa unidade não poderá se mover ou usar habilidades especiais no próximo turno do oponente. Ele poderá, no entanto, ainda se defender e atacar.\n", previewSupress, this, _minSupressAP, _maxSupressCooldown);
   }

   public void toggleMount() {
      if(!skills[0].use()) return;
      isMounted = !isMounted;
      if (isMounted)
         GameController.console.Log("Mounted\n");
      else
         GameController.console.Log("Unmounted\n");
   }

   public override void move(int x, int y, int distance) {
      if (!isMounted)
         base.move(x, y, distance);
      else
         GameController.console.Log("Cant move while mounted\n");
   }

   public override void attack(Creature victim) {
      if (!isMounted) {
         GameController.console.Log("Must mount to attack\n");
         return;
      }
      base.attack(victim);
   }

   public override void newTeamTurn(){
      base.newTeamTurn();
      if(supressTarget != null){
         supressTarget.isUndeadSiegeSupressed = false;
         supressTarget = null;
      }
   }

   public void previewSupress(){
      if(!skills[2].canUse()) return;
      supressSurroundings = terrain.expandByDistance(2);
      GameController.overrideClick(trySupress);
      supressSurroundings.paint(Color.blue);
   }

   public void trySupress(Terrain terrain){
      supressSurroundings.clear();
      if(terrain.creature == null || terrain.creature.team == team){
         GameController.console.Log("invalid target\n");
         return;
      }
      if(!supressSurroundings.hasTerrain(terrain.x, terrain.y)){
         GameController.console.Log("out of range\n");
         return;
      }
      if(terrain.creature is HumanHero || terrain.creature is UndeadHero){
         GameController.console.Log("Cannot select hero\n");
         return;
      }
      if(!skills[2].use(terrain.creature)) return;
      terrain.creature.isUndeadSiegeSupressed = true;
      supressTarget = terrain.creature;
      GameController.console.Log("supressing creature\n");
   }

   public void previewRevive(){
      if(!skills[1].canUse()) return;
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
      skills[1].use();
      UndeadSoldier soldier = new UndeadSoldier(terrain.x, terrain.y, team);
      soldier.actionPoints = 0;
   }
}
