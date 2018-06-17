using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class gameController {
   static Creature creatureClicked = null;
   static Dictionary<string, Terrain> possibilities;

   public static void clickTerrain(Terrain terrain) {
      if (creatureClicked == null) {
         if (terrain.creature != null) {
            creatureClicked = terrain.creature;
            possibilities = creatureClicked.mouseDown();
         }
      }
      else {
         creatureClicked.mouseUp(possibilities);
         Terrain t;
         if (terrain.creature == null && possibilities.TryGetValue(terrain.x + "," + terrain.y, out t)) {
            creatureClicked.move(terrain.x, terrain.y);
         }
         creatureClicked = null;
      }
   }
}
