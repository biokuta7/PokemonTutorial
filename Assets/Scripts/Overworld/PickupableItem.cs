using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour, IInteractable
{

    public ItemData item;

    public void OnInteract()
    {

        string[] dString = { Player.instance.name + " found one " + item.name + "!", Player.instance.name + " put the " + item.name + " in the " + item.itemType + " pocket." };

        Dialogue d = new Dialogue(dString);

        PokemonGameManager.instance.StartDialogue(d);

        gameObject.SetActive(false);

    }
    
}
