using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadSiege : Creature {
   public static GameObject prefab;

   const string _name = " Siege";
   const string _teamName = "Undeads";
   static string[] _skillsNames = new string[2] {"Renascer", "Suprimir"};
   static string[] _skillsDescriptions = new string[2] { @"Renascer: (CD = 6) (AP = 2) (alcance = 1)
Igual ao Renascer do herói, apenas com um CD menor e custo de AP
", @"Suprimir: (CD = 4) (AP = 2) (Alcance = 2)
Selecione uma unidade inimiga que não for o Herói, essa unidade não poderá se mover ou usar habilidades especiais no próximo turno do oponente. Ele poderá, no entanto, ainda se defender e atacar.
"};

    const int _maxHealth = 500;
   const int _maxActionPoints = 2;
   const int _baseDodge = 0;
   const int _defenseHeal = 50;
   const int _defenseResistance = 30;
   const int _attackDamage = 20;
   const int _attackRange = 1;
   
   const int _mountAPCost = 2;
   private bool isMounted = false;

   const int _maxSupressCooldown = 4;
   const int _minSupressAP = 2;
   private int supressCooldown = 0;
   private Surroundings supressSurroundings = null;
   private Creature supressTarget = null;

   public UndeadSiege(int x, int y, int team) : base(prefab, x, y, _maxActionPoints, team, _maxHealth, _attackDamage, _attackRange, _defenseHeal, _defenseResistance, _baseDodge, _name, _teamName, _skillsNames, _skillsDescriptions) {

   }

   public void toggleMount() {
      if (!useActionPoints(_mountAPCost)) {
         Debug.Log("Not enough action points");
         return;
      }
      isMounted = !isMounted;
      if (isMounted)
         Debug.Log("Mounted");
      else
         Debug.Log("Unmounted");
   }

   public override void attack(Creature victim) {
      if (!isMounted) {
         Debug.Log("Must mount to attack");
         return;
      }
      base.attack(victim);
   }

   public override void newTeamTurn(){
      base.newTeamTurn();
      if(supressCooldown > 0) supressCooldown--;
      if(supressTarget != null){
         supressTarget.isUndeadSiegeSupressed = false;
         supressTarget = null;
      }
   }

   public void previewSupress(){
      if(supressCooldown > 0){
         Debug.Log("wait " + supressCooldown + " turns");
         return;
      }
      if(!useActionPoints(_minSupressAP)){
         Debug.Log("not enough action points");
         return;
      }
      supressSurroundings = terrain.expandByDistance(2);
      GameController.previewingSupress();
      supressSurroundings.paint(Color.blue);
   }

   public void trySupress(Terrain terrain){
      GameController.notPreviewingSupress();
      supressSurroundings.clear();
      if(terrain.creature == null || terrain.creature.team == team){
         Debug.Log("invalid target");
         return;
      }
      if(!supressSurroundings.hasTerrain(terrain.x, terrain.y)){
         Debug.Log("out of range");
         return;
      }
      supressCooldown = _maxSupressCooldown;
      terrain.creature.isUndeadSiegeSupressed = true;
      supressTarget = terrain.creature;
      Debug.Log("supressing creature");
   }
}
