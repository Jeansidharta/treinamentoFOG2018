﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSiege : Creature {
   public static GameObject prefab;
   const int _maxHealth = 300;
   const int _maxActionPoints = 3;
   const int _baseDodge = 0;
   const int _defenseHeal = 35;
   const int _defenseResistance = 10;
   const int _attackDamage = 70;
   const int _attackRange = 3;

   const int _maxInvertCD = 4;
   const int _minInvertAP = 3;

   const int _maxPushCD = 6;
   const int _minPushAP = 2;

   const int _minMountAP = 2;
   const int _maxMountCooldown = 0;

   const int _splashDamage = 50;
   const int _pushAmmount = 3;
   const int _pushRange = 1;

   public bool isMounted = false;
   private bool isInverted = false;

   public HumanSiege(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {
      skills[0] = new Skill("Montar", "Montar (AP = 2):\n\nPara uma unidade de catapulta ser capaz de atirar ela precisa primeiro se montar o que custa AP para fazer. Depois de montada a catapulta pode atacar normalmente desde que ela não se movimente, se a catapulta se mover ela tem que se montar novamente.\n", toggleMount, this, _minMountAP, _maxMountCooldown);
      
      skills[1] = new Skill("Inversor", "Inversor: (CD = 4) (AP = 3) (Alcance = nulo)\n\nInverte o dano em área, ao invés de causar 70 ao alvo principal e 50 aos secundários, o trebuchet causará 50 ao alvo principal e 70 aos secundários.\n", habilityInvert, this, _minInvertAP, _maxInvertCD);
      
      skills[2] = new Skill("Sistema anti-colisão", "Sistema anti-colisão: (CD = 6) (AP = 2) (Alcance = 1)\n\nRepele um inimigo que está diretamente ao lado do trebuchet em 3 hexágonos para longe da maneira mais reta possível.\n", habilityPush, this, _minPushAP, _maxPushCD);
   }

   public override void attack(Creature victim) {
      if(victim is UndeadKnight && (victim as UndeadKnight).isImmaterial){
         GameController.console.Log("Cannot attack immaterial undead knight\n");
         return;
      }
      if (!isMounted) {
         GameController.console.Log("Must mount to attack\n");
         return;
      }
      if (isInverted) {
         attackDamage = _splashDamage;
         base.attack(victim);
         attackDamage = _attackDamage;
      }
      else {
         base.attack(victim);
         attackDamage = _splashDamage;
      }

      List<Pair<int, int>> neighbours = victim.terrain.getNeighboursCoords();
      for (int aux = 0; aux < neighbours.Count; aux++) {
         var coord = neighbours[aux];
         var creature = Terrain.allTiles[coord.second][coord.first].creature;

         if (creature != null && creature.team != this.team){
            actionPoints = 1;
            base.attack(creature);
         }
      }
      this.attackDamage = _attackDamage;
   }

   public void toggleMount() {
      if(!skills[0].use()) return;
      isMounted = !isMounted;
      if (isMounted)
         GameController.console.Log("Mounted\n");
      else
         GameController.console.Log("Unmounted\n");
   }

   public void habilityInvert() {
      if(!skills[1].use()) return;
      GameController.console.Log("inverting damage\n");
      isInverted = true;
   }

   private void habilityPushAux(Creature creature) {
      Terrain creatureTerrain = creature.terrain;
      Pair<int, int> pair = terrain.getDirectionToTerrain(creatureTerrain);
      for(int aux = 0; aux < _pushAmmount; aux ++){
         Terrain neighbour = creatureTerrain.getNeighbourFromDirection(pair.first, pair.second);
         if(neighbour == null || neighbour.creature != null || neighbour is Mountain)
            break;
         creatureTerrain = neighbour;
         if(creatureTerrain.trap != null) break;
      }
      creature.move(creatureTerrain.x, creatureTerrain.y, 0);
   }

   public void habilityPush() {
      if(!skills[2].use()) return;
      GameController.console.Log("Pushed!\n");

      Surroundings surroundings = terrain.expandByDistance(_pushRange);
      for(int aux = 1; aux < surroundings.creatures.Count; aux ++){
         habilityPushAux(surroundings.creatures[aux].second);
      }
      return;
   }

   public override void move(int x, int y, int distance) {
      if (!isMounted)
         base.move(x, y, distance);
      else
         GameController.console.Log("Cant move while mounted\n");
   }
}