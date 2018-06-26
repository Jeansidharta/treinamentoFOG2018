﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
   static Creature creatureClicked = null;
   static Surroundings possibilities = null;

   static bool trapPreview = false;

   private static void selectCreature(Creature creature) {
      if (possibilities != null)
         creatureClicked.mouseUp(possibilities);
      creatureClicked = creature;
      if (creature != null)
         possibilities = creatureClicked.mouseDown();
      else
         possibilities = null;
   }

   private static void placeTrap(Terrain terrain){
      if(possibilities.hasTerrain(terrain.x, terrain.y)){
         if(terrain.creature == null && terrain.trap == null){
            if(!(terrain is Mountain)){
               (creatureClicked as HumanArcher).setTrap(terrain.x, terrain.y);
            }
            else Debug.Log("Cant place trap in mountain");
         }
         else Debug.Log("Cant place trap over another creature or trap");
      }
      else Debug.Log("position out of range");

      trapPreview = false;
      selectCreature(null);
   }

   public static void clickTerrain(Terrain terrain) {
      if(trapPreview){
         placeTrap(terrain);
         return ;
      }
      if (creatureClicked == null) { //if no creatures was selected
         if (terrain.creature != null) { //and f terrain has creature
            if (terrain.creature.team == TurnController.turn) { //and if creatures is owned by the player
               selectCreature(terrain.creature); //select it
            }
         }
      }
      else { //howeever, if there was a creature selected
         Trio<int, int, Terrain> t;
         if (possibilities.tryGetTerrain(terrain.x, terrain.y, out t)) { //and the clicked terrain was in range
            if (terrain.creature == null) { //if the terrain didnt contain a creature, move
               creatureClicked.move(terrain.x, terrain.y, t.second);
               selectCreature(null);
            }
            else if (terrain.creature == creatureClicked) selectCreature(null); //if user clicked at the already selected creature, unselect it
            else if (terrain.creature.team != creatureClicked.team && t.first <= creatureClicked.attackRange) { //if the creature was of the oposite team and was in range for attack
               creatureClicked.attack(terrain.creature); //attack target
               selectCreature(creatureClicked); //unselect creature;
            }
         }
         else if (terrain.creature != null && terrain.creature.team == TurnController.turn) //if the clicked terrain wasnt in range and had a creature owned by the player, select it
            selectCreature(terrain.creature);
         else selectCreature(null); //if nothing else, unselect the creature.
      }
   }

   private void Update() {
      if(Input.GetKeyDown(KeyCode.Alpha0)){
         if(creatureClicked != null)
            creatureClicked.defend();
      }
      else if (Input.GetKeyDown(KeyCode.Alpha1)) {
         if (creatureClicked is HumanKnight) {
            (creatureClicked as HumanKnight).assault();
         }
         else if (creatureClicked is HumanSiege) {
            (creatureClicked as HumanSiege).toggleMount();
            selectCreature(creatureClicked);
         }
         else if (creatureClicked is HumanArcher) {
            possibilities.clear();
            possibilities = (creatureClicked as HumanArcher).previewTrap();
            if(possibilities == null){
               selectCreature(null);
            }
            else{
               possibilities.paint(new Color(255, 255, 0));
               trapPreview = true;
            }
         }
         else if (creatureClicked is HumanSoldier) {
            (creatureClicked as HumanSoldier).raiseShields();
            selectCreature(creatureClicked);
         }
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2)) {
         if (creatureClicked is HumanSiege) {
            (creatureClicked as HumanSiege).habilityPush();
            selectCreature(creatureClicked);
         }
      }
   }
}