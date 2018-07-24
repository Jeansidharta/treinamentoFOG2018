using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplaySkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text obTexto;
    public Text nomeSkill;
    public GameObject descricaoOb;
    [SerializeField] GameObject panel;

    private bool Display;

    public void Setup(string strNome, string strDescricao)
    {
        obTexto.color = Color.clear;
        nomeSkill.text = strDescricao;
        obTexto.text = strDescricao;
        nomeSkill.color = Color.black;
        panel.SetActive(false);
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
            obTexto.color = Color.white;
        }
        else
        {
            descricaoOb.SetActive(false);
            panel.SetActive(false);
            obTexto.color = Color.clear;
        }
    }
}
