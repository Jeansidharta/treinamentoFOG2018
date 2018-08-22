using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController{
   public static int turn = 0;
   public static void nextTurn() {
      GameController.newTurn(turn);
      turn = 1 - turn;
      foreach (Creature creature in Creature.allCreatures) {
         creature.newTurn(turn);
      }
      
      foreach (Trap trap in Trap.allTraps) {
         trap.newTurn(turn);
      }
      
      foreach (HumanWoodenFortress fortress in HumanWoodenFortress.allFortresses) {
         fortress.newTurn(turn);
      }
      while(Trap.allTraps.Remove(null));
      while(HumanWoodenFortress.allFortresses.Remove(null));
   }
}
