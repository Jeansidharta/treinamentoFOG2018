using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surroundings {
   public Dictionary<string, Pair<int, Terrain>> terrains;
   public Dictionary<string, Pair<int, Creature>> creatures;
   public Surroundings() {
      terrains = new Dictionary<string, Pair<int, Terrain>>();
   }
}
