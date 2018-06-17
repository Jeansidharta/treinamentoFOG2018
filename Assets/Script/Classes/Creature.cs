using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature {
   int health;
   int maxHealth;
   int attackDamage;
   int moves;
   int x, y;
   Terrain currentTile;
   GameObject spriteInstance;

   static int totalCreatures;

   public Creature(GameObject prefab, int x, int y, int moves, int maxHealth = 10, int attackDamage = 1) {
      this.x = x;
      this.y = y;
      this.moves = moves;
      this.maxHealth = maxHealth;
      this.health = maxHealth;
      this.attackDamage = attackDamage;
      this.currentTile = Terrain.allTiles[y][x];
      this.currentTile.creature = this;
      spriteInstance = MonoBehaviour.Instantiate(prefab);
      spriteInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      spriteInstance.transform.position = this.currentTile.spriteInstance.transform.position + new Vector3(0, 0, -0.0001f);
   }

   private void tryToMove(int x, int y) {
   }

   public void sufferDamage(int damage) {
      this.health -= damage;
      if (this.health <= 0)
         Debug.Log("me dead");
   }

   public void attack(Creature victim) {
      victim.sufferDamage(this.attackDamage);
   }

   public void move(int x, int y) {
      this.x = x;
      this.y = y;
      this.currentTile.creature = null;
      this.currentTile = Terrain.allTiles[y][x];
      this.currentTile.creature = this;
      spriteInstance.transform.position = this.currentTile.spriteInstance.transform.position + new Vector3(0, 0, -0.0001f);
   }

   public Dictionary<string, Terrain> mouseDown() {
      return currentTile.expand(this.moves);
   }

   public void mouseUp(Dictionary<string, Terrain> dict) {
      Terrain.collapse(dict);
   }
}
