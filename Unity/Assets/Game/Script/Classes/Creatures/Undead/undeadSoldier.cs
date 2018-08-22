using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadSoldier : Creature {
   public static GameObject prefab;
   const int _maxHealth = 250;
   const int _maxActionPoints = 3;
   const int _baseDodge = 0;
   const int _defenseHeal = 30;
   const int _defenseResistance = 20;
   const int _attackDamage = 40;
   const int _attackRange = 1;

   const int _maxCursedTouchCooldown = 5;
   const int _minCursedTouchAP = 1;
   private Surroundings cursedTouchSurroundings = null;
   Creature cursedTouchCreature = null;

   public UndeadSoldier(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge) {
      skills[0] = new Skill("Toque amaldiçoado", "Toque amaldiçoado: (CD  = 5) (AP = 1) (Alcance = 1)\n\nPor um turno, unidade inimiga selecionada recebe 5% mais de dano de todas as fontes. Uma unidade pode receber o toque amaldiçoado de múltiplos guerreiros esqueléticos diferentes. Se a unidade morrer naquele turno, crie um guerreiro esquelético com vida máxima 1.\n", previewCursedTouch, this, _minCursedTouchAP, _maxCursedTouchCooldown);
   }

   public override void newTeamTurn(){
      base.newTeamTurn();
      if(cursedTouchCreature != null){
         cursedTouchCreature.defenseResistance += 5;
         cursedTouchCreature = null;
         Debug.Log("cursed touch removed");
      }
   }

   public void previewCursedTouch(){
      if(!skills[0].canUse()) return;
      cursedTouchSurroundings = terrain.expandByDistance(1);
      GameController.overrideClick(this.tryCursedTouch);
      cursedTouchSurroundings.paint(Color.blue);
   }
   public void tryCursedTouch(Terrain terrain){
      cursedTouchSurroundings.clear();
      if(terrain.creature == null || terrain.creature.team == team){
         Debug.Log("invalid target");
         return;
      }
      if(!cursedTouchSurroundings.hasTerrain(terrain.x, terrain.y)){
         Debug.Log("out of range");
         return;
      }
      skills[0].use();
      terrain.creature.defenseResistance -= 5;
      cursedTouchCreature = terrain.creature;
      Debug.Log("cursed touch applied");
   }
}
