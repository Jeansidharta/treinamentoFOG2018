using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class gameController {
   static Creature creatureClicked = null;
   static Dictionary<string, Pair<int, Terrain>> possibilities = null;
   static int turn = 0;

   private static void selectCreature(Creature creature) {
      if (possibilities != null)
         creatureClicked.mouseUp(possibilities);
      creatureClicked = creature;
      if (creature != null)
         possibilities = creatureClicked.mouseDown();
      else
         possibilities = null;
   }

   public static void clickTerrain(Terrain terrain) {
      if (creatureClicked == null) {
         if (terrain.creature != null) {
            selectCreature(terrain.creature);
         }
      }
      else {
         Pair<int, Terrain> t;
         if (possibilities.TryGetValue(terrain.x + "," + terrain.y, out t)) {
            if (terrain.creature == null) {
               creatureClicked.move(terrain.x, terrain.y, t.first);
               selectCreature(null);
            }
            else if (terrain.creature == creatureClicked) selectCreature(null);
            else if (terrain.creature.team != creatureClicked.team && t.first <= creatureClicked.attackRange) {
               creatureClicked.attack(terrain.creature);
               selectCreature(creatureClicked);
            }
         }
         else if (terrain.creature != null) selectCreature(terrain.creature);
         else selectCreature(null);
      }
   }
}