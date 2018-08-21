using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController {
   static Creature creatureClicked = null;
   public static Surroundings possibilities = null;
   public static GUIController guiController;

   static bool isPreviewHumanArcherTrap = false;
   static bool isPreviewHumanHeroFortress = false;
   static bool isPreviewCorner = false;
   static bool isPreviewCursedTouch = false;
   static bool isPreviewSupress = false;

   public static void previewingHumanArcherTrap(){
      isPreviewHumanArcherTrap = true;
      possibilities.clear();
   }

   public static void previewingCorner(){
      isPreviewCorner = true;
      possibilities.clear();
   }

   public static void previewingHumanHeroFortress(){
      isPreviewHumanHeroFortress = true;
      possibilities.clear();
   }

   public static void previewingCursedTouch(){
      isPreviewCursedTouch = true;
      possibilities.clear();
   }

   public static void previewingSupress(){
      isPreviewSupress = true;
      possibilities.clear();
   }
   
   public static void notPreviewingHumanArcherTrap(){
      isPreviewHumanArcherTrap = false;
   }
   
   public static void notPreviewingSupress(){
      isPreviewSupress = false;
   }
   
   public static void notPreviewingCursedTouch(){
      isPreviewCursedTouch = false;
   }

   public static void notPreviewingCorner(){
      isPreviewCorner = false;
   }
   
   public static void notPreviewingHumanHeroFortress(){
      isPreviewHumanHeroFortress = false;
   }

   private static void selectCreature(Creature creature) {
      if (possibilities != null)
         creatureClicked.mouseUp(possibilities);
      creatureClicked = creature;
      if (creature != null)
         possibilities = creatureClicked.mouseDown();
      else
         possibilities = null;
      guiController.selectCreature(creature);
   }

   public static void clickTerrain(Terrain terrain) {
      if(isPreviewHumanArcherTrap){
         Creature creature = creatureClicked;
         selectCreature(null);
         (creature as HumanArcher).trySetTrap(terrain);
         return;
      }
      if(isPreviewHumanHeroFortress){
         Creature creature = creatureClicked;
         selectCreature(null);
         (creature as HumanHero).trySetFortress(terrain);
         return;
      }
      if(isPreviewCorner){
         Creature creature = creatureClicked;
         selectCreature(null);
         (creature as HumanHero).tryCorner(terrain);
         return;
      }
      if(isPreviewCursedTouch){
         Creature creature = creatureClicked;
         selectCreature(null);
         (creature as UndeadSoldier).tryCursedTouch(terrain);
         return;
      }
      if(isPreviewSupress){
         Creature creature = creatureClicked;
         selectCreature(null);
         (creature as UndeadSiege).trySupress(terrain);
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
         if (possibilities.tryGetTerrain(terrain.x, terrain.y, out t)) {
            if (terrain.creature == null) {
               creatureClicked.move(terrain.x, terrain.y, t.second);
               selectCreature(null);
            }
            else if (terrain.creature == creatureClicked) selectCreature(null);
            else if (terrain.creature.team != creatureClicked.team && t.first <= creatureClicked.attackRange) {
               creatureClicked.attack(terrain.creature);
               selectCreature(creatureClicked);
            }
         }
         else if (terrain.creature != null && terrain.creature.team == TurnController.turn)
            selectCreature(terrain.creature);
         else selectCreature(null);
      }
   }
}