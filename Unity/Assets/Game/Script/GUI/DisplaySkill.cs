using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplaySkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public Text obTexto;
    public Text nomeSkill;
    public GameObject descricaoOb;
    public Creature creature = null;
    [SerializeField] GameObject panel;

    public Skill.SkillFunction skillFunction;

    private bool Display;

    public void Setup(string strNome, string strDescricao)
    {
        obTexto.color = Color.clear;
        nomeSkill.text = strDescricao;
        obTexto.text = strDescricao;
        nomeSkill.color = Color.black;
        panel.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData){ //apply skill
      if(creature.isUndeadSiegeSupressed){
         Debug.Log("creature is supressed, cant use hability");
         return;
      }
      if(skillFunction != null) skillFunction();
      else Debug.Log("skillFunction was null");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Display = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Display = false;
    }

    private void Update()
    {
        if (Display)
        {
            descricaoOb.SetActive(true);
            panel.SetActive(true);
        }
        else
        {
            descricaoOb.SetActive(false);
            panel.SetActive(false);
        }
    }
}
