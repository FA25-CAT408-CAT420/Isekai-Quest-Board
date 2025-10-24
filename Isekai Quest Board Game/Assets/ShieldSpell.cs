using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : Spells
{
    public GameObject shieldPrefab;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

    public override void Spell(){
        Instantiate(shieldPrefab);
        base.Spell();
    }

    public override void Cost(){
        baseMPCost = 10f;
    }
}
