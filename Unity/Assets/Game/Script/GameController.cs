using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
   static Creature creatureClicked = null;
   public static Surroundings possibilities = null;
   public static Surroundings attackPossibilities = null;
   public static GUIController guiController;
   
   public delegate void SkillFunction(Terrain terrain);
   public static SkillFunction overrideFunction;

   public static consoledisplayer console;

   static bool isOverriding = false;

    private void Update(){
        guiController.console.display();
    }

    public static void overrideClick(SkillFunction func){
      isOverriding = true;
      selectCreature(null);
      overrideFunction = func;
   }

   public static void newTurn(int turn){
      selectCreature(null);
   }

   private static void clearAll(){
      if(possibilities != null)
         possibilities.clear();
      if(attackPossibilities != null)
         attackPossibilities.clear();
   }

   private static void selectCreature(Creature creature) {
      if (possibilities != null){
         clearAll();
      }
      creatureClicked = creature;
      if (creature != null){
         GameObject.FindGameObjectWithTag("SoundController").GetComponent<Sound_controller>().playClick();
         int ap = creature.actionPoints;
         int range = creature.attackRange;
         if(creature is HumanKnight && creature.terrain is Plains && creature.actionPoints > 0)
            ap++;
         if(creature is HumanSiege){
            if((creature as HumanSiege).isMounted)
               ap = 0;
            else
               range = 0;
         }
         if(creature is UndeadSiege){
            if((creature as UndeadSiege).isMounted)
               ap = 0;
            else
               range = 0;
         }
         possibilities = creature.terrain.expandByAP(ap);
         possibilities.paint(Color.red);
         attackPossibilities = creature.terrain.expandByDistance(range);
         for(int aux = 0; aux < attackPossibilities.creatures.Count; aux ++){
            Creature targetCreature = attackPossibilities.creatures[aux].second;
            if(targetCreature.team != creature.team){
               targetCreature.terrain.setColor(Color.blue);
            }
         }
      }
      else{
         possibilities = null;
         attackPossibilities = null;
      }
      guiController.selectCreature(creature);
   }

   public static void clickTerrain(Terrain terrain) {
      if(creatureClicked is UndeadKnight && terrain is Mountain){
         selectCreature(null);
         return;
      }
      if(isOverriding){
         isOverriding = false;
         overrideFunction(terrain);
         return;
      }
      if (creatureClicked == null) {
         if (terrain.creature != null) {
            if (terrain.creature.team == TurnController.turn) {
               selectCreature(terrain.creature);
            }
         }
      }
      else {
         Trio<int, int, Terrain> t;
         if(terrain.creature == creatureClicked){
            selectCreature(null);
            return;
         }
         if(attackPossibilities.hasCreature(terrain.creature)){
            if(terrain.creature.team != creatureClicked.team){
               creatureClicked.attack(terrain.creature);
               selectCreature(creatureClicked);
            }
            else{
               selectCreature(terrain.creature);
            }
         }
         else if (possibilities.tryGetTerrain(terrain.x, terrain.y, out t)) {
            if (terrain.creature == null) {
               if(creatureClicked is HumanKnight && t.first == 1)
                  t.second++;
               creatureClicked.move(terrain.x, terrain.y, t.second);
               selectCreature(null);
            }
            else if (terrain.creature == creatureClicked) selectCreature(null);
            else if (terrain.creature.team == creatureClicked.team)
               selectCreature(terrain.creature);
         }
         else if (terrain.creature != null && terrain.creature.team == TurnController.turn)
            selectCreature(terrain.creature);
         else selectCreature(null);
      }
   }
}