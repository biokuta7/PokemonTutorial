using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Entity
{
    
    bool swimming = false;
    
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    public override void Update()
    {
        base.Update();

        if (!isMoving && pokemonGameManager.gameState.Equals(PokemonGameManager.GameState.OVERWORLD))
        {
            GatherInput();
        }
    }

    private void GatherInput()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        running = Input.GetButton("Run");

        if (x > 0)
        { MoveRight(); }
        else if (x < 0)
        { MoveLeft(); }
        else if (y > 0)
        { MoveUp(); }
        else if (y < 0)
        { MoveDown(); }
        
    }

    public override bool CheckCurrentTile()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(.1f, .1f), 0, nonCollidableLayerMask.value);
        if (collider != null)
        {
            if (collider.CompareTag("Grass"))
            {
                if (MapSettings.instance.OnGrassShake())
                {
                    StopMovement();
                }
                ParticleSystemPooler.instance.SpawnParticleSystem("ShakingGrass", transform.position);
                return true;
            }

            Portal p = collider.GetComponent<Portal>();

            if(p != null)
            {
                pokemonGameManager.TraversePortal(p);
            }

        }

        return false;
    }


}
