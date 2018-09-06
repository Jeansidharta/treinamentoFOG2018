using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Creature {
   public int health;
   public int maxHealth;
   public int attackDamage;
   public int maxActionPoints;
   public int attackRange;
   public int actionPoints;
   public int defenseHeal;
   public int defenseResistance;
   public int dodge;

   public healthBar hp_bar;

   public Skill[] skills = new Skill[3];

   public int snareDuration = 0;

   public bool hasAttacked = false;
   public bool isDefending = false;
   public bool wasDefenseHealApplied = false;

   public bool isUndeadSiegeSupressed = false;

   public int x, y;
   public int team;
   public Terrain terrain;
   protected GameObject spriteInstance;

   public static List<Creature> allCreatures = new List<Creature>();

   public Creature(GameObject prefab, int x, int y, int maxActionPoints, int team, int maxHealth = 10, int attackDamage = 1, int attackRange = 1, int defenseHeal = 10, int defenseResistance = 10, int dodge = 10) {
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
      this.dodge = dodge;

      this.terrain = Terrain.allTiles[y][x];
      this.terrain.creature = this;
      this.spriteInstance = MonoBehaviour.Instantiate(prefab);
      this.spriteInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      this.spriteInstance.transform.position = this.terrain.spriteInstance.transform.position + new Vector3(0, 0, -0.0001f);

      var hpTransform = spriteInstance.GetComponentsInChildren<Transform>();
      Debug.Log(hpTransform.Length);
      var hpScale = hpTransform[1].localScale;
      hpTransform[1].localScale = new Vector3(hpScale.x * Terrain._terrainSize * 1.3f, hpScale.y * Terrain._terrainSize * 1.3f, hpScale.z * Terrain._terrainSize * 1.3f);

      this.hp_bar = this.spriteInstance.GetComponent<healthBar>();
      

        if (terrain is Fortress)
         this.defenseResistance += (terrain as Fortress).defenseBonus;

      else if(terrain is Forest)
         this.dodge += (terrain as Forest).dodgeBonus;

      this.actionPoints = maxActionPoints;
      allCreatures.Add(this);

      for(int aux = 0; aux < this.skills.Length; aux ++){
         this.skills[aux] = null;
      }
   }

   public virtual string getRaceName(){
      if(this is HumanArcher || this is HumanHero || this is HumanKnight || this is HumanSiege || this is HumanSoldier)
         return "Human";
      if(this is UndeadArcher || this is UndeadHero || this is UndeadKnight || this is UndeadSiege || this is UndeadSoldier)
         return "Undead";
      return "unknown";
   }

   public virtual string getName(){
      if(this is HumanHero)
         return "Sir Godfrey";
      if(this is HumanArcher)
         return "Imperial archer";
      if(this is HumanSiege)
         return "Trebuchet";
      if(this is HumanKnight)
         return "Royal knight";
      if(this is HumanSoldier)
         return "Squire";

      if(this is UndeadSoldier)
         return "Skeleton warrior";
      if(this is UndeadArcher)
         return "Vile shooters";
      if(this is UndeadKnight)
         return "Spectral knights";
      if(this is UndeadSiege)
         return "Obelisk";
      if(this is UndeadHero)
         return "Zarasputim";

      return "unknown";
   }

   public virtual bool defend() {
      if(hasAttacked){
         Debug.Log("Cant defend in a turn that attacked");
         return false;
      }
      if(isDefending){
         Debug.Log("im already defending");
         return false;
      }
      Debug.Log("defending");
      isDefending = true;
      wasDefenseHealApplied = false;
      return true;
   }

   public virtual void changeHealth(int ammount) {
      health += ammount;
      hp_bar.updateHealthBar(health, maxHealth);
      if (health > maxHealth) health = maxHealth;
      if (health <= 0) die();
   }

   public virtual void die() {
      Surroundings surroundings = terrain.expandByDistance(1);
      for(int aux = 0; aux < surroundings.creatures.Count; aux ++){
         Creature creature = surroundings.creatures[aux].second;
         if(creature is UndeadSoldier){
            if(creature.health < creature.maxHealth){
               creature.health += 70;
            }
            else creature.health += 50;
            Debug.Log("undead soldier absorbed me");
         }
      }
      terrain.creature = null;
      allCreatures.Remove(this);
      MonoBehaviour.Destroy(spriteInstance);
      if(this is HumanHero || this is UndeadHero){
         GameController.guiController.gameOver(this.team, this.team);
      }
   }

   public virtual void newTeamTurn() {
      this.actionPoints = this.maxActionPoints;
      if(snareDuration > 0){
         actionPoints = 0;
         snareDuration--;
      }
      hasAttacked = false;
      isDefending = false;

      for(int aux = 0; aux < skills.Length && skills[aux] != null; aux ++){
         skills[aux].newTeamTurn();
      }
   }

   public virtual void newTurn(int turnNumber) {
      if (turnNumber == this.team) newTeamTurn();
      if (isDefending && !wasDefenseHealApplied) {
         changeHealth(defenseHeal);
         Debug.Log("Healing " + defenseHeal + ". health is now " + health);
         wasDefenseHealApplied = true;
      }
   }

   public virtual void receiveAttack(Creature attacker){
      int damage = attacker.attackDamage;
      int myDefense = defenseResistance;

      if(Random.Range(0, 100) <= dodge){
         Debug.Log("But the attack was dodged!");
         return;
      }
      if(terrain is Fortress || terrain.fortress != null){
         if(
            attacker is HumanSiege ||
            attacker is UndeadSiege ||
            (attacker.terrain is Fortress || attacker.terrain.fortress != null)
         ){
            Debug.Log("fortress bonus denied");
            if(terrain is Fortress)
               myDefense -= (terrain as Fortress).defenseBonus;
            else
               myDefense -= terrain.fortress.additionalDefense;
         }
      }

      if(myDefense < 0) myDefense = 0;
      damage = (int)(damage * (1.0f - myDefense/100.0f));
      if(damage < 0) damage = 0;
      changeHealth(-damage);
      Debug.Log(damage + " went through the total of " + myDefense + " defense. " + health + " Hp left");
   }

   public virtual void attack(Creature victim) {
      if(isDefending){
         Debug.Log("i was defending, but since i attacked, im cancelling my defense");
         isDefending = false;
      }
      hasAttacked = true;
      Debug.Log("dealt " + attackDamage + " damage");
      victim.receiveAttack(this);
      this.actionPoints = 0;
   }

   public void setPos(int x, int y) {
      Terrain lastTerrain = terrain;
      this.x = x;
      this.y = y;
      this.terrain.creature = null;
      this.terrain = Terrain.allTiles[y][x];
      this.terrain.creature = this;
      spriteInstance.transform.position = this.terrain.spriteInstance.transform.position + new Vector3(0, 0, -0.0001f);
      if(!(lastTerrain is Fortress) && terrain is Fortress){
         Debug.Log("Stepping in a fortress");
         this.defenseResistance += (terrain as Fortress).defenseBonus;
      }
      else if(lastTerrain is Fortress && !(terrain is Fortress)){
         Debug.Log("Stepping out of fortress");
         defenseResistance -= (lastTerrain as Fortress).defenseBonus;
      }
      if(!(lastTerrain is Forest) && terrain is Forest){
         Debug.Log("Stepping in forest");
         dodge += (terrain as Forest).dodgeBonus;
      }
      else if(lastTerrain is Forest && !(terrain is Forest)){
         Debug.Log("Stepping out of forest");
         dodge -= (lastTerrain as Forest).dodgeBonus;
      }
      if(lastTerrain.fortress == null && terrain.fortress != null && terrain.fortress.team == team){
         Debug.Log("Stepping in a wooden fortress");
         defenseResistance += terrain.fortress.additionalDefense;
      }
      else if(lastTerrain.fortress != null && terrain.fortress == null && lastTerrain.fortress.team == team){
         Debug.Log("Stepping out of wooden fortress");
         defenseResistance -= lastTerrain.fortress.additionalDefense;
      }
   }

   public bool useActionPoints(int ammount) {
      if (actionPoints < ammount) return false;
      actionPoints -= ammount;
      return true;
   }

   public virtual void move(int x, int y, int distance) {
      setPos(x, y);
      if(!useActionPoints(distance)){
         Debug.Log("not enough points for walking");
      }
      List<Trap> trapsInRange = Trap.TrapsInRange(x, y);
      foreach(Trap trap in trapsInRange){
         trap.activate(this);
      }
   }

    public virtual void playAttackSound(GameObject soundctrl)
    {
        spriteInstance.GetComponent<AudioSource>().volume = soundctrl.GetComponent<Sound_controller>().mastervol * soundctrl.GetComponent<Sound_controller>().soundfxvol;
        spriteInstance.GetComponent<AudioSource>().Play();
    }

    //public virtual void playAttackAnimation() { }
}
