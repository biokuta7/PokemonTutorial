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
    public EntitySpriteData swimmingSpriteData;


    public Entity swimmingEntity;

    public LayerMask normalCollidableLayerMask;
    public LayerMask swimmingCollidableLayerMask;

    private void Awake()
    {
        instance = this;
    }

    public override void Start()
    {
        base.Start();
        swimmingEntity.SetCollision(false);
        swimmingEntity.gameObject.SetActive(false);
        collidableLayerMask = normalCollidableLayerMask;
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
        } else if(Mathf.Abs(x) > .1f || Mathf.Abs(y) > .1f)
        {
            InteractCollision();
        }
        


    }

    private void Tick()
    {
        
        if (swimming)
        {
            entitySpriteData = swimmingSpriteData;
            movespeed = 2;
            swimmingEntity.FaceDirection(direction);
            InitSprites();

        } else if (running && isMoving)
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

            //Check for grass encounter
            if (collider.CompareTag("Grass"))
            {
                if (MapSettings.instance.TryGrassEncounter())
                {
                    StopMovement();
                }
                return true;
            }

            Portal p = collider.GetComponent<Portal>();

            if(p != null)
            {
                pokemonGameManager.TraversePortal(p);
                return true;
            }


        }

        //Check for water encounter

        if(swimming)
        {
            if(MapSettings.instance.TryWaterEncounter())
            {
                StopMovement();
                return true;
            }
        }

        //Check for random encounter

        if(MapSettings.instance.TryRandomEncounter())
        {
            StopMovement();
            return true;
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

            if (c.CompareTag("Water"))
            {
                StartCoroutine(StartSwimming());
            }

            

        }
        
    }

    private void InteractCollision()
    {
        Collider2D c = CheckForthTile();

        if(c != null)
        {
            if (c.CompareTag("ReEnterLand"))
            {
                StartCoroutine(StopSwimming());
            }

            if (c.CompareTag("JumpableCliffSouth") && direction.Equals(Direction.SOUTH))
            {
                StartCoroutine(HopJumpableCliff());
            }

        }

    }

    IEnumerator HopJumpableCliff()
    {

        SetCollision(false);
        pokemonGameManager.gameState = PokemonGameManager.GameState.CUTSCENE;
        running = false;
        yield return new WaitForSeconds(.1f);
        
        MoveForward(2, true);

        while (isMoving)
        {
            yield return null;
        }
        
        SetCollision(true);
        pokemonGameManager.gameState = PokemonGameManager.GameState.OVERWORLD;

    }

    IEnumerator StartSwimming()
    {
        
        pokemonGameManager.gameState = PokemonGameManager.GameState.CUTSCENE;
        SetCollision(false);
        running = false;
        pokemonGameManager.StartDialogue(new Dialogue("The water is a deep blue. Would you like to swim?"));

        while (DialogueManager.instance.IsDialogueDisplayed())
        {
            yield return null;
        }

        MoveForward(1,true);
        
        while(isMoving)
        {
            yield return null;
        }

        swimmingEntity.gameObject.SetActive(true);

        SetCollision(true);
        collidableLayerMask = swimmingCollidableLayerMask;
        swimming = true;

        InitSprites();

        pokemonGameManager.gameState = PokemonGameManager.GameState.OVERWORLD;



    }

    IEnumerator StopSwimming()
    {
        
        SetCollision(false);
        pokemonGameManager.gameState = PokemonGameManager.GameState.CUTSCENE;
        running = false;
        yield return new WaitForSeconds(.1f);

        swimmingEntity.gameObject.SetActive(false);
        collidableLayerMask = normalCollidableLayerMask;
        swimming = false;

        MoveForward(1, true);

        while (isMoving)
        {
            yield return null;
        }


        SetCollision(true);
        
        pokemonGameManager.gameState = PokemonGameManager.GameState.OVERWORLD;



    }

}
