using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHero : Creature {
   public static GameObject prefab;

   const string _name = "Sir Godfrey";
   const string _teamName = "Humans";
   const int _maxHealth = 1000;
   const int _maxActionPoints = 5;
   const int _baseDodge = 15;
   const int _defenseHeal = 75;
   const int _defenseResistance = 30;
   const int _attackDamage = 100;
   const int _attackRange = 1;
   static string[] _skillsNames = new string[3] { "Fortaleza", "Encurralar", "Últimos Recursos" };
   static string[] _skillsDescriptions = new string[3] { @"Fortaleza: (CD = 7) (AP = tudo que tiver, min 1) (alcance = infinito)
Fortaleza irá criar um novo terreno de fortaleza do estilo “Forte de madeira” em um terreno selecionado, o forte de madeira é similar a uma fortaleza comum, mas o bônus é 30 ao invés do 50 normal. Fortalezas de madeira podem apodrecer, um processo que a destruirá em 15 turnos.
", @"Encurralar: (CD = 5) (AP = 2) (alcance = 2)
Ao usar encurralar o jogador escolhe uma unidade inimiga próxima, até dois aliados próximos irão causar seu dano de ataque básico nessa unidade, esses “ataques” não irão contar como ataque de verdade para essas unidades, que poderão atacar de verdade depois se desejarem.
", @"Últimos Recursos: (CD = 5) (AP = 3) (alcance = 1)
Defensor dos povos concede a todas as unidades aliadas ao seu redor defesa equivalente a 150% da porcentagem vida perdida dessas unidades.
" };

   const int _areaHealingAmmount = 15;
   const int _areaHealingRange = 1;

   const int _maxFortressCooldown = 7;
   private int fortressCooldown = 0;
   
   const int _maxCornerCooldown = 7;
   const int _minCornerAP = 2;
   private int cornerCooldown = 0;
   
   const int _maxLastResourcesCooldown = 5;
   const int _minLastResourcesAP = 2;
   private int lastResourcesCooldown = 0;
   List<Pair<Creature, int>> lastResourcesTargets = null;

   public Surroundings cornerSurroundings = null;

   public HumanHero(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge, _name, _teamName, _skillsNames, _skillsDescriptions) {

   }

   public override void newTeamTurn(){
      base.newTeamTurn();
      if(fortressCooldown > 0) fortressCooldown --;
      if(lastResourcesCooldown > 0) lastResourcesCooldown--;
      if(cornerCooldown > 0) cornerCooldown --;
      if(lastResourcesTargets != null){
         for(int aux = 0; aux < lastResourcesTargets.Count; aux ++){
            Creature creature = lastResourcesTargets[aux].first;
            int ammount = lastResourcesTargets[aux].second;
            creature.defenseResistance -= ammount;
         }
         Debug.Log("removing defenses");
         lastResourcesTargets = null;
      }
   }

   public override bool defend(){
      if(!base.defend())
         return false;
      Surroundings surroundings = terrain.expandByDistance(_areaHealingRange);
      for(int aux = 1; aux < surroundings.creatures.Count; aux ++){
         surroundings.creatures[aux].second.changeHealth(_areaHealingAmmount);
      }
      return true;
   }

   public void previewFortress(){
      if(fortressCooldown > 0){
         Debug.Log("wait " + fortressCooldown + " turns to use this");
         return;
      }
      if(actionPoints < 1){
         Debug.Log("not enough action points");
         return;
      }
      GameController.previewingHumanHeroFortress();
   }

   public void trySetFortress(Terrain terrain){
      if(terrain.fortress != null){
         Debug.Log("already has a fortress here");
         return;
      }
      new HumanWoodenFortress(terrain, team);
      fortressCooldown = _maxFortressCooldown;
      actionPoints = 0;
      GameController.notPreviewingHumanHeroFortress();
   }

   public void lastResource(){
      if(lastResourcesCooldown > 0){
         Debug.Log("wait " + lastResourcesCooldown + " rounds");
         return;
      }
      if(!useActionPoints(_minLastResourcesAP)){
         Debug.Log("not enough action points");
         return;
      }
      lastResourcesCooldown = _maxLastResourcesCooldown;
      Surroundings surroundings = terrain.expandByDistance(1);
      lastResourcesTargets = new List<Pair<Creature, int>>();
      for(int aux = 0; aux < surroundings.creatures.Count; aux ++){
         Creature creature = surroundings.creatures[aux].second;
         if(creature == this) continue;
         if(creature.team == team){
            int ammount = (int)((float)(creature.maxHealth - creature.health) * 1.5);
            creature.defenseResistance += ammount;
            lastResourcesTargets.Add(new Pair<Creature, int>(creature, ammount));
            Debug.Log("giving " + ammount + " defense to creature");
         }
      }
   }

   public void previewCorner(){
      if(fortressCooldown > 0){
         Debug.Log("wait " + fortressCooldown + " turns to use this");
         return;
      }
      if(!useActionPoints(_minCornerAP)){
         Debug.Log("not enough action points");
         return;
      }
      cornerSurroundings = terrain.expandByDistance(2);
      GameController.previewingCorner();
      cornerSurroundings.paint(Color.blue);
   }

   public void tryCorner(Terrain terrain){
      if(terrain.creature == null || terrain.creature.team == team){
         Debug.Log("invalid target");
         cornerSurroundings.clear();
         GameController.notPreviewingCorner();
         return;
      }
      if(!cornerSurroundings.hasTerrain(terrain.x, terrain.y)){
         Debug.Log("out of reach");
         cornerSurroundings.clear();
         GameController.notPreviewingCorner();
         return;
      }
      int bestCount = 0;
      int secondBestCount = 0;
      Creature best = null;
      Creature secondBest = null;
      for(int aux = 0; aux < cornerSurroundings.creatures.Count; aux ++){
         Creature creature = cornerSurroundings.creatures[aux].second;
         if(creature == this) continue;
         if(creature.team == team){
            if(creature.attackDamage > bestCount){
               bestCount = creature.attackDamage;
               best = creature;
            }
            else if(creature.attackDamage > secondBestCount){
               secondBest = creature;
               secondBestCount = creature.attackDamage;
            }
         }
      }
      int buffer = attackDamage;
      attackDamage = bestCount + secondBestCount;
      terrain.creature.receiveAttack(this);
      attackDamage = buffer;
      Debug.Log("used last resource");
      GameController.notPreviewingCorner();
   }
}
