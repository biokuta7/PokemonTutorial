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

    

    public override void Start()
    {
        base.Start();
        if (behavior.Equals(NPC_Behavior.RANDOM))
        {
            StartCoroutine(RandomMovement());
        }
    }

    private IEnumerator RandomMovement()
    {
        while(true)
        {
            int r = Random.Range(0, 3);

            MoveInDirection((Direction)r);

            yield return new WaitForSeconds(3.0f);
        }
    }
    

}
