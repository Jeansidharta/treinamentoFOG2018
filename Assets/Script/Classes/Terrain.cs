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

   private void expandRecursive(int level, Dictionary<string, Terrain> dictionary) {
      if (level < 0) return;
      Terrain t;
      if (!dictionary.TryGetValue(this.x + "," + this.y, out t))
         dictionary.Add(this.x + "," + this.y, this);
      this.setColorRed();
      for (int aux = 0; aux < this.neighboursCount; aux++) {
         if (this.neighbours[aux].movePointsRequired != -1) {
            neighbours[aux].expandRecursive(level - movePointsRequired, dictionary);
         }
      }
   }

   public Dictionary<string, Terrain> expand(int level) {
      Dictionary<string, Terrain> dictionary = new Dictionary<string, Terrain>();
      this.expandRecursive(level, dictionary);
      return dictionary;
   }

   public static void collapse(Dictionary<string, Terrain> dict) {
      foreach(var item in dict){
         item.Value.setColorWhite();
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
}