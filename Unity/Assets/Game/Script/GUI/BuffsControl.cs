using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsControl : MonoBehaviour {

    [SerializeField] Sprite[] img;
    [SerializeField] Sprite[] venom;
    [SerializeField] Sprite backBuffs2;
    [SerializeField] Sprite backBuffs;
    [SerializeField] Sprite backDefense;
    [SerializeField] Sprite backTerrain;

    [SerializeField] GameObject prefab;

    private int venomstack = 0;
    private float r = 4.5f;
    private GameObject[][] buffs = new GameObject[4][];

    private bool hasStarted = false;

    private void Start()
    {
       if(!hasStarted){
         buffs[0] = new GameObject[1] { null };
         buffs[1] = new GameObject[12];
         for (int i = 0; i < buffs.Length; i++) buffs[1][i] = null;
         buffs[2] = new GameObject[1] { null };
         buffs[3] = new GameObject[3] { null, null, null };
         hasStarted = true;
       }
    }

    private int apply_venom()
    {
        venomstack++;
        buffs[1][8].GetComponentsInChildren<SpriteRenderer>()[0].sprite = venom[venomstack-1];
        return venomstack;
    }
    
    //Buff[0] == Azul, buff secundario
    //Buff[1] == Verde, buff primario
    //Buff[2] == Cinza, buff defesa
    //Buff[3] == Vermelho, buff terreno

    /* Buff[0]: montado: 1
     * Buff[1]: Instinto de liderança, Ultimos recursos, Levantar escudo, Arco longo, Senhor das Planicies, Investida macabra,
     * Senhor do enxame, Toque amaldiçoado, Municao venenosa, Imaterial, ataque obelisco, suprimir : 12
     * Buff[2]: defender: 1
     * Buff[3]: fortaleza, floresta, rio: 3
     */

    private Vector3 calculate_pos(int pos)
    {
        float theta = 0;
        int numbuff = 1;
        int i = 0;
        while (i < buffs[pos].Length)
            if(buffs[pos][i++] != null) numbuff++;
        switch (pos)
        {
            case 0:
                theta = Mathf.PI/2 + ((Mathf.PI / 12) * numbuff);
                break;
            case 1:
                theta = ((Mathf.PI / 12) * numbuff);
                break;
            case 2:
                theta = 3 * (Mathf.PI/2) + ((Mathf.PI / 12) * numbuff);
                break;
            case 3:
                theta = Mathf.PI + ((Mathf.PI / 12) * numbuff);
                break;
        }
        float xoff = Mathf.Cos(theta) * r;
        float yoff = Mathf.Sin(theta) * r;
        Vector3 v = new Vector3(xoff, yoff, 0);
        return v;
    }

    /* recebe nome do buff a ser adicionado */
    public void add_buff(string adc)
    {
       if(!hasStarted) this.Start();
        if(adc == "Montado" && buffs[0][0] == null)
        {
            Vector3 position = calculate_pos(0);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[4];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs2;

            buffs[0][0] = newOb;
        }
        else if (adc == "Defender" && buffs[2][0] == null)
        {
            Vector3 position = calculate_pos(2);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[5];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backDefense;

            buffs[2][0] = newOb;
        }
        else if (adc == "Fortaleza" && buffs[3][0] == null)
        {
            Vector3 position = calculate_pos(3);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[8];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backTerrain;

            buffs[3][0] = newOb;
        }
        else if (adc == "Floresta" && buffs[3][1] == null)
        {
            Vector3 position = calculate_pos(3);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[7];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backTerrain;

            buffs[3][1] = newOb;
        }
        else if (adc == "Rio" && buffs[3][2] == null)
        {
            Vector3 position = calculate_pos(3);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            // Empty: newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[7];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs2;

            buffs[3][2] = newOb;
        }
        else if (adc == "Instinto de Lideranca" && buffs[1][0] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[9];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][0] = newOb;
        }
        else if (adc == "Ultimos Recursos" && buffs[1][1] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[15];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][1] = newOb;
        }
        else if (adc == "Levantar Escudo" && buffs[1][2] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[1];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][2] = newOb;
        }
        else if (adc == "Arco Longo" && buffs[1][3] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[0];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][3] = newOb;
        }
        else if (adc == "Senhor das Planicies" && buffs[1][4] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[12];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][4] = newOb;
        }
        else if (adc == "Investida Macabra" && buffs[1][5] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[10];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][5] = newOb;
        }
        else if (adc == "Senhor do enxame" && buffs[1][6] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[13];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][6] = newOb;
        }
        else if (adc == "Toque Amaldicoado" && buffs[1][7] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[14];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][7] = newOb;
        }
        else if (adc == "Municao Venenosa" && venomstack <= 3)
        {
            if(venomstack == 0)
            {
                Vector3 position = calculate_pos(1);
                GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
                newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

                buffs[1][8] = newOb;
            }
            apply_venom();
        }
        else if (adc == "Imaterial" && buffs[1][9] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[17];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][9] = newOb;
        }
        else if (adc == "Ataque Obelisco" && buffs[1][10] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[16];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][10] = newOb;
        }
        else if (adc == "Suprimido" && buffs[1][11] == null)
        {
            Vector3 position = calculate_pos(1);
            GameObject newOb = Instantiate(prefab, gameObject.transform.localPosition + position, Quaternion.identity, gameObject.transform);
            newOb.GetComponentsInChildren<SpriteRenderer>()[0].sprite = img[3];
            newOb.GetComponentsInChildren<SpriteRenderer>()[1].sprite = backBuffs;

            buffs[1][11] = newOb;
        }

    }

    /* recebe nome do buff a ser removido */
    public void remove_buff(string rmv)
    {
        if (rmv == "Montado" && buffs[0][0] != null)
        {
            Destroy(buffs[0][0]);
            buffs[0][0] = null;
        }
        else if (rmv == "Defender" && buffs[2][0] != null)
        {
            Destroy(buffs[2][0]);
            buffs[2][0] = null;
        }
        else if (rmv == "Fortaleza" && buffs[3][0] != null)
        {
            Destroy(buffs[3][0]);
            buffs[3][0] = null;
        }
        else if (rmv == "Floresta" && buffs[3][1] != null)
        {
            Destroy(buffs[3][1]);
            buffs[3][1] = null;
        }
        else if (rmv == "Rio" && buffs[3][2] != null)
        {
            Destroy(buffs[3][2]);
            buffs[3][2] = null;
        }
        else if (rmv == "Instinto de Lideranca" && buffs[1][0] != null)
        {
            Destroy(buffs[1][0]);
            buffs[1][0] = null;
        }
        else if (rmv == "Ultimos Recursos" && buffs[1][1] != null)
        {
            Destroy(buffs[1][1]);
            buffs[1][1] = null;
        }
        else if (rmv == "Levantar Escudo" && buffs[1][2] != null)
        {
            Destroy(buffs[1][2]);
            buffs[1][2] = null;
        }
        else if (rmv == "Arco Longo" && buffs[1][3] != null)
        {
            Destroy(buffs[1][3]);
            buffs[1][3] = null;
        }
        else if (rmv == "Senhor das Planicies" && buffs[1][4] != null)
        {
            Destroy(buffs[1][4]);
            buffs[1][4] = null;
        }
        else if (rmv == "Investida Macabra" && buffs[1][5] != null)
        {
            Destroy(buffs[1][5]);
            buffs[1][5] = null;
        }
        else if (rmv == "Senhor do enxame" && buffs[1][6] != null)
        {
            Destroy(buffs[1][6]);
            buffs[1][6] = null;
        }
        else if (rmv == "Toque Amaldicoado" && buffs[1][7] != null)
        {
            Destroy(buffs[1][7]);
            buffs[1][7] = null;
        }
        else if (rmv == "Municao Venenosa" && buffs[1][8] != null)
        {
            venomstack = 0;
            Destroy(buffs[1][8]);
            buffs[1][8] = null;
        }
        else if (rmv == "Imaterial" && buffs[1][9] != null)
        {
            Destroy(buffs[1][9]);
            buffs[1][9] = null;
        }
        else if (rmv == "Ataque Obelisco" && buffs[1][10] != null)
        {
            Destroy(buffs[1][10]);
            buffs[1][10] = null;
        }
        else if (rmv == "Suprimido" && buffs[1][11] != null)
        {
            Destroy(buffs[1][11]);
            buffs[1][11] = null;
        }

    }

}
