using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Entity, IInteractable
{
    public enum NPC_Behavior
    {
        STATIC,
        RANDOM,
        CUSTOM
    }
    
    public NPC_Behavior behavior;

    [System.Serializable]
    public class NPC_Command
    {
        public EntityCommand command;
        public int value = 1;
    }

    public float baseWaitTimeBetweenActions = 2.0f;

    public Dialogue dialogue;

    public NPC_Command[] customCommands;
    public bool customLoop;

    

    public override void Start()
    {
        base.Start();
        if (behavior.Equals(NPC_Behavior.RANDOM))
        {
            StartCoroutine(RandomMovement());
        } else if(behavior.Equals(NPC_Behavior.CUSTOM))
        {
            StartCoroutine(CustomMovement());
        }
    }

    private IEnumerator RandomMovement()
    {
        while(true)
        {
            int r = Random.Range(0, 3);

            MoveInDirection((Direction)r);
            while (isMoving)
            {
                yield return null;
            }
            yield return new WaitForSeconds(baseWaitTimeBetweenActions);
        }
    }

    private IEnumerator CustomMovement()
    {
        do
        {
            for (int i = 0; i < customCommands.Length; i++)
            {
                NPC_Command npcCommand = customCommands[i];

                ExecuteCommand(npcCommand.command, npcCommand.value);

                while(isMoving)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(baseWaitTimeBetweenActions);
            }
        } while (customLoop);
    }
    
    public void OnInteract()
    {
        StartCoroutine(StartDialogueCoroutine());
    }
    
    private IEnumerator StartDialogueCoroutine()
    {
        Direction d = direction;
        FacePlayer();
        StopMovement();

        pokemonGameManager.StartDialogue(dialogue);

        while(DialogueManager.instance.IsDialogueDisplayed())
        {
            yield return null;
        }
        FaceDirection(d);
        StartMovement();

    }

}
