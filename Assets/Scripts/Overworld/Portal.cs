using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public Portal target;
    public Direction facingDirectionExit;

    public void OnEntered()
    {

    }

    public void OnExited()
    {

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.transform.position);
        Gizmos.DrawWireCube(transform.position, Vector3.one);

    }

    

}
