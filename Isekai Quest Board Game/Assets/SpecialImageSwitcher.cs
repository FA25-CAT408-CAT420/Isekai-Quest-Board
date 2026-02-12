using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialImageSwitcher : MonoBehaviour
{
    public PlayerMovement pm;
    public PlayerCombat pc;
    public GameManager gm;
    public Image prevImage;
    public Image mainImage;
    public Image postImage;

    //public Image uiImage;
    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pm != null)
        {
            
        }
        else if (pm == null)
        {
            pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }

        if (pc != null)
        {
            
        }
        else if (pc == null)
        {
            pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        }

        if (gm != null)
        {
            if (gm.specials.Count > 0)
            {
                mainImage.sprite = gm.specials[pm.nextSpell].sprite.sprite;
                if (gm.specials.Count > 2)
                {
                    prevImage.sprite = gm.specials[pm.prevSpell].sprite.sprite;
                }
                else if (gm.specials.Count <= 2)
                {
                    prevImage.sprite = gm.specials[pm.overSpell].sprite.sprite;
                }
                
                postImage.sprite = gm.specials[pm.overSpell].sprite.sprite;  
            }
        }
        else if (pm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }
    }
}
