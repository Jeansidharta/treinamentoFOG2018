using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature {
   public int health;
   public int maxHealth;
   public int attackDamage;
   public int maxActionPoints;
   public int attackRange;
   public int actionPoints;
   public int defenseHeal;
   public int defenseResistance;
   public int baseDodge;
   public int x, y;
   public int team;
   Terrain currentTile;
   GameObject spriteInstance;

   public static List<Creature> allCreatures = new List<Creature>();

   public Creature(GameObject prefab, int x, int y, int maxActionPoints, int team, int maxHealth = 10, int attackDamage = 1, int attackRange = 1, int defenseHeal = 10, int defenseResistance = 10, int baseDodge = 10) {
      this.x = x;
      this.y = y;
      this.team = team;
      this.maxActionPoints = maxActionPoints;
      this.attackRange = attackRange;
      this.maxHealth = maxHealth;
      this.health = maxHealth;
      this.attackDamage = attackDamage;
      this.defenseHeal = defenseHeal;
      this.defenseResistance = defenseResistance;
      this.baseDodge = baseDodge;

      this.currentTile = Terrain.allTiles[y][x];
      this.currentTile.creature = this;
      this.spriteInstance = MonoBehaviour.Instantiate(prefab);
      this.spriteInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      this.spriteInstance.transform.position = this.currentTile.spriteInstance.transform.position + new Vector3(0, 0, -0.0001f);
      this.newTurn();
      allCreatures.Add(this);
   }

   public void newTurn() {
      this.actionPoints = this.maxActionPoints;
   }

   public void sufferDamage(int damage) {
      this.health -= damage;
      if (this.health <= 0)
         Debug.Log("me dead");
   }

   public void attack(Creature victim) {
      Debug.Log("pew pew");
      victim.sufferDamage(this.attackDamage);
      this.actionPoints = 0;
   }

   public void move(int x, int y, int distance) {
      this.x = x;
      this.y = y;
      this.currentTile.creature = null;
      this.currentTile = Terrain.allTiles[y][x];
      this.currentTile.creature = this;
      spriteInstance.transform.position = this.currentTile.spriteInstance.transform.position + new Vector3(0, 0, -0.0001f);
      this.actionPoints -= distance;
   }

   public Dictionary<string, Pair<int, Terrain>> mouseDown() {
      return currentTile.expand(this.actionPoints);
   }

   public void mouseUp(Dictionary<string, Pair<int, Terrain>> dict) {
      Terrain.collapse(dict);
   }
}
