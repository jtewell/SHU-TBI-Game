using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableTable : InteractableNPC
{

    public GameObject[] tableItems;

    // Start is called before the first frame update
    public override void Start()
    {
        //Go through all items and make sure their interaction component is shut off
        TurnOffItems();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayDialogue()
    {
        TurnOnItems();
        base.PlayDialogue();
    }

    public void TurnOnItems()
    {
        foreach (var item in tableItems)
        {
            item.transform.GetComponent<Interactable>().ForceEnableInteraction();
        }
    }

    public void TurnOffItems()
    {
        foreach (var item in tableItems)
        {
            item.transform.GetComponent<Interactable>().ForceDisableInteraction();
        }
    }
}
