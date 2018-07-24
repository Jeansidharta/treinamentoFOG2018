using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameController : MonoBehaviour {

    /*-------GUI Scripting----------*/
    [SerializeField] Text text_name;
    [SerializeField] Text text_team;
    [SerializeField] Text text_HP;
    [SerializeField] Text text_AP;
    [SerializeField] Text text_DE;
    [SerializeField] Text text_AT;
    [SerializeField] Text text_EV;
    [SerializeField] Image unitImgObject;
    [SerializeField] Sprite blankImg;
    [SerializeField] Sprite Harcher;
    [SerializeField] Sprite HHero;
    [SerializeField] Sprite HKnight;
    [SerializeField] Sprite HSiege;
    [SerializeField] Sprite HSoldier;
    [SerializeField] Sprite UDarcher;
    [SerializeField] Sprite UDHero;
    [SerializeField] Sprite UDKnight;
    [SerializeField] Sprite UDSiege;
    [SerializeField] Sprite UDSoldier;

    private GameObject[] panel;
    private GameObject[] skillGO;
    private GameObject[] skillN;
    private GameObject[] skillD;

    /*-------Start Function----------*/
    private void Start()
    {
        unitImgObject.GetComponent<Image>().sprite = blankImg;
        panel = GameObject.FindGameObjectsWithTag("DescriptionPanel");
        skillGO = GameObject.FindGameObjectsWithTag("SkillObject").OrderBy(go => go.name).ToArray(); 
        skillN = GameObject.FindGameObjectsWithTag("SkillText").OrderBy(go => go.name).ToArray();
        skillD = GameObject.FindGameObjectsWithTag("SkillDescription").OrderBy(go => go.name).ToArray(); 
    }

    /*-----------------------------*/

    static Creature creatureClicked = null;
   static Surroundings possibilities = null;

   static bool trapPreview = false;

   private static void selectCreature(Creature creature) {
      if (possibilities != null)
         creatureClicked.mouseUp(possibilities);
      creatureClicked = creature;
      if (creature != null)
         possibilities = creatureClicked.mouseDown();
      else
         possibilities = null;
   }

   private static void placeTrap(Terrain terrain){
      if(possibilities.hasTerrain(terrain.x, terrain.y)){
         if(terrain.creature == null && terrain.trap == null){
            if(!(terrain is Mountain)){
               (creatureClicked as HumanArcher).setTrap(terrain.x, terrain.y);
            }
            else Debug.Log("Cant place trap in mountain");
         }
         else Debug.Log("Cant place trap over another creature or trap");
      }
      else Debug.Log("position out of range");

      trapPreview = false;
      selectCreature(null);
   }

   public static void clickTerrain(Terrain terrain) {
      if(trapPreview){
         placeTrap(terrain);
         return ;
      }
      if (creatureClicked == null) { //if no creatures was selected
         if (terrain.creature != null) { //and f terrain has creature
            if (terrain.creature.team == TurnController.turn) { //and if creatures is owned by the player
               selectCreature(terrain.creature); //select it
            }
         }
      }
      else { //howeever, if there was a creature selected
         Trio<int, int, Terrain> t;
         if (possibilities.tryGetTerrain(terrain.x, terrain.y, out t)) { //and the clicked terrain was in range
            if (terrain.creature == null) { //if the terrain didnt contain a creature, move
               creatureClicked.move(terrain.x, terrain.y, t.second);
               selectCreature(null);
            }
            else if (terrain.creature == creatureClicked) selectCreature(null); //if user clicked at the already selected creature, unselect it
            else if (terrain.creature.team != creatureClicked.team && t.first <= creatureClicked.attackRange) { //if the creature was of the oposite team and was in range for attack
               creatureClicked.attack(terrain.creature); //attack target
               selectCreature(creatureClicked); //unselect creature;
            }
         }
         else if (terrain.creature != null && terrain.creature.team == TurnController.turn) //if the clicked terrain wasnt in range and had a creature owned by the player, select it
            selectCreature(terrain.creature);
         else selectCreature(null); //if nothing else, unselect the creature.
      }
   }

   private void Update() {
      if(Input.GetKeyDown(KeyCode.Alpha0)){
         if(creatureClicked != null)
            creatureClicked.defend();
      }
      else if (Input.GetKeyDown(KeyCode.Alpha1)) {
         if (creatureClicked is HumanKnight) {
            (creatureClicked as HumanKnight).assault();
         }
         else if (creatureClicked is HumanSiege) {
            (creatureClicked as HumanSiege).toggleMount();
            selectCreature(creatureClicked);
         }
         else if (creatureClicked is HumanArcher) {
            possibilities.clear();
            possibilities = (creatureClicked as HumanArcher).previewTrap();
            if(possibilities == null){
               selectCreature(null);
            }
            else{
               possibilities.paint(new Color(255, 255, 0));
               trapPreview = true;
            }
         }
         else if (creatureClicked is HumanSoldier) {
            (creatureClicked as HumanSoldier).raiseShields();
            selectCreature(creatureClicked);
         }
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2)) {
         if (creatureClicked is HumanSiege) {
            (creatureClicked as HumanSiege).habilityPush();
            selectCreature(creatureClicked);
         }
      }

      /*-------GUI Scripting----------*/
        if (creatureClicked != null)
        {
            //Assigning Selected creature's value to diplay on GUI
            text_name.text = "Name: " + creatureClicked.name;
            text_team.text = "Team: " + creatureClicked.teamName;
            text_HP.text = "HP: " + creatureClicked.health.ToString();
            text_AP.text = "AP: " + creatureClicked.actionPoints.ToString();
            text_DE.text = "DE: " + creatureClicked.defenseResistance.ToString();
            text_AT.text = "AT: " + creatureClicked.attackDamage.ToString();

            //Checking number of skills and displaying their names and description on GUI
            if(creatureClicked.skills_names.Length >= 3)
            {
                for (int i = 0; i < skillGO.Length; i++) skillGO[i].SetActive(true);
                for (int i = 0; i < 3; i++) skillN[i].GetComponent<Text>().text = creatureClicked.skills_names[i];
                for (int i = 0; i < 3; i++) skillD[i].GetComponent<Text>().text = creatureClicked.skill_description[i];
               

            }
            else if (creatureClicked.skills_names.Length == 2)
            {
                skillGO[0].SetActive(true);
                skillGO[1].SetActive(true);
                skillGO[2].SetActive(false);
                for (int i = 0; i < 2; i++) skillN[i].GetComponent<Text>().text = creatureClicked.skills_names[i];
                for (int i = 0; i < 2; i++) skillD[i].GetComponent<Text>().text = creatureClicked.skill_description[i];
                skillN[2].GetComponent<Text>().text = null;
                skillD[2].GetComponent<Text>().text = null;
            }
            else //1 Skill only
            {
                for (int i = 0; i < skillGO.Length; i++)
                {
                    if(i == 0) skillGO[i].SetActive(true);
                    else skillGO[i].SetActive(false);
                }
                skillN[0].GetComponent<Text>().text = creatureClicked.skills_names[0];
                for (int i = 1; i < 3; i++) skillN[i].GetComponent<Text>().text = null;
                for (int i = 1; i < 3; i++) skillD[i].GetComponent<Text>().text = null;
                skillD[0].GetComponent<Text>().text = creatureClicked.skill_description[0];
            }
            //text_EV.text = "EV: " + creatureClicked.evasion.ToString();

            //Assigning Selected creature's image file to display on GUI
            //Humans Team
            if (creatureClicked is HumanArcher)
            {
                unitImgObject.GetComponent<Image>().sprite = Harcher;
            }
            else if (creatureClicked is HumanHero)
            {
                unitImgObject.GetComponent<Image>().sprite = HHero;
            }
            else if (creatureClicked is HumanKnight)
            {
                unitImgObject.GetComponent<Image>().sprite = HKnight;
            }
            else if (creatureClicked is HumanSiege)
            {
                unitImgObject.GetComponent<Image>().sprite = HSiege;
            }
            else if (creatureClicked is HumanSoldier)
            {
                unitImgObject.GetComponent<Image>().sprite = HSoldier;
            }
            //Undead Team
            else if(creatureClicked is UndeadArcher)
            {
                unitImgObject.GetComponent<Image>().sprite = UDarcher;
            }
            else if (creatureClicked is UndeadHero)
            {
                unitImgObject.GetComponent<Image>().sprite = UDHero;
            }
            else if (creatureClicked is UndeadKnight)
            {
                unitImgObject.GetComponent<Image>().sprite = UDKnight;
            }
            else if (creatureClicked is UndeadSiege)
            {
                unitImgObject.GetComponent<Image>().sprite = UDSiege;
            }
            else if (creatureClicked is UndeadSoldier)
            {
                unitImgObject.GetComponent<Image>().sprite = UDSoldier;
            }
        }
        else
        {
            text_name.text = "Name: ";
            text_team.text = "Team: ";
            text_HP.text = "HP: ";
            text_AP.text = "AP: ";
            text_DE.text = "DE: ";
            text_AT.text = "AT: ";
            text_EV.text = "EV: ";
            for (int i = 0; i < skillGO.Length; i++) skillGO[i].SetActive(false);
            for (int i = 0; i < panel.Length; i++) panel[i].SetActive(false);
            

            unitImgObject.GetComponent<Image>().sprite = blankImg;
        }
        /*-----------------------------*/
    }

}