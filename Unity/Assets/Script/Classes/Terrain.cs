using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain {
   Renderer renderer;
   public GameObject spriteInstance;
   public Terrain[] neighbours = new Terrain[6];
   public int neighboursCount = 0;

   public int movePointsRequired;
   public Creature creature = null;
   public int x, y;

   public static Terrain[][] allTiles;
   public static int terrainHeight;
   public static int terrainWidth;

   public Terrain(GameObject prefab, int x, int y, int walkSpeed = 1){
      this.x = x;
      this.y = y;
      spriteInstance = MonoBehaviour.Instantiate(prefab);
      spriteInstance.transform.position = new Vector3(x + (y % 2 == 0 ? 0 : 0.5f), y * 0.9f, 0) * 0.9f;
      renderer = spriteInstance.GetComponent<Renderer>();
      this.spriteInstance.GetComponent<cellControl>().cell = this;
      this.movePointsRequired = walkSpeed;

      //setting up neighbours
      if (y > 0) {
         this.setNeighbour(allTiles[y - 1][x]);
         if (y % 2 == 0) {
            if(x > 0) this.setNeighbour(allTiles[y - 1][x - 1]);
         }
         else {
            if(x < terrainWidth - 1) this.setNeighbour(allTiles[y - 1][x + 1]);
         }
      }
      if(x > 0) this.setNeighbour(allTiles[y][x - 1]);
   }

   public void mouseDown() {
      gameController.clickTerrain(this);
   }

   public void mouseUp() {
     // if (this.creature != null)
         //this.creature.mouseUp();
   }

   private void expandRecursive(int dist, int maxDist, Dictionary<string, Pair<int, Terrain>> dictionary) {
      if (dist > maxDist) return;

      Pair<int, Terrain> t;
      string dictKey = this.x + "," + this.y;
      if (!dictionary.TryGetValue(dictKey, out t))
         dictionary.Add(dictKey, new Pair<int, Terrain>(dist, this));
      else {
         if (t.first <= dist) return;
         else {
            dictionary.Remove(dictKey);
            dictionary.Add(dictKey, new Pair<int, Terrain>(dist, this));
         }
      }

      this.setColorRed();
      for (int aux = 0; aux < this.neighboursCount; aux++) {
         if (this.neighbours[aux].movePointsRequired != -1) {
            neighbours[aux].expandRecursive(dist + movePointsRequired, maxDist, dictionary);
         }
      }
   }

   public Dictionary<string, Pair<int, Terrain>> expand(int maxDist) {
      var dictionary = new Dictionary<string, Pair<int, Terrain>>();
      this.expandRecursive(0, maxDist, dictionary);
      return dictionary;
   }

   public static void collapse(Dictionary<string, Pair<int, Terrain>> dict) {
      foreach(var item in dict){
         item.Value.second.setColorWhite();
      }
   }

   private void pushNeighbour(Terrain neighbour) {
      this.neighbours[this.neighboursCount] = neighbour;
      this.neighboursCount++;
   }

   private void setNeighbour(Terrain neighbour) {
      this.pushNeighbour(neighbour);
      neighbour.pushNeighbour(this);
   }

   public void setColorRed() {
      renderer.material.color = Color.red;
   }

   public void setColorWhite() {
      renderer.material.color = Color.white;
   }

   public void setNeighboursRed() {
      for (int aux = 0; aux < this.neighboursCount; aux++) {
         this.neighbours[aux].setColorRed();
      }
   }
}