using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Entity
{
    
    bool swimming = false;
    bool running = false;
    public static PlayerController instance;

    public EntitySpriteData walkingSpriteData;
    public EntitySpriteData runningSpriteData;

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

        Tick();

    }

    public void SetRunning(bool r)
    {
        running = r;
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
        

        if(Input.GetButtonDown("Fire1"))
        {
            Interact();
        }
        
    }

    private void Tick()
    {

        if (running && isMoving)
        {
            entitySpriteData = runningSpriteData;
            movespeed = 2;
            InitSprites();
        }
        else
        {
            entitySpriteData = walkingSpriteData;
            movespeed = 1;
            InitSprites();
        }
        
    }

    public override bool CheckCurrentTile()
    {

        base.CheckCurrentTile();

        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(.1f, .1f), 0, nonCollidableLayerMask.value);
        if (collider != null)
        {

            Portal p = collider.GetComponent<Portal>();

            if(p != null)
            {
                pokemonGameManager.TraversePortal(p);
            }

        }

        return false;
    }

    private void Interact()
    {
        Collider2D c = CheckForthTile();
        
        if (c != null)
        {
            IInteractable interactable = c.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
    }

}
