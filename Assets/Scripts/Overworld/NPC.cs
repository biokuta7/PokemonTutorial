using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Entity
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

}
