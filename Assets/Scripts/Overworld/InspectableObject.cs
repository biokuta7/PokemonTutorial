using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectableObject : MonoBehaviour, IInteractable
{

    public Dialogue dialogue;
    
    public void OnInteract()
    {

        PokemonGameManager.instance.StartDialogue(dialogue);
    }
}
