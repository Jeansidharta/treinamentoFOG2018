using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : Terrain {
   const int _walkSpeed = 2;
   public Forest(GameObject prefab, int x, int y) : base(prefab, x, y, _walkSpeed) {
   }
}