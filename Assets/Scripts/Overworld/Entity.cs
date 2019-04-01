using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    NORTH,
    SOUTH,
    EAST,
    WEST,
    NONE
}

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{

    public enum EntityCommand
    {
        FACEDOWN,
        FACELEFT,
        FACERIGHT,
        FACEUP,
        FACEPLAYER,
        MOVEDOWN,
        MOVELEFT,
        MOVERIGHT,
        MOVEUP,
        MOVEFORWARD,
        MOVETOWARDSPLAYER
    }

    protected Direction direction;

    int index = 0;

    public int movespeed = 1;
    
    bool stopped = false;

    protected bool isMoving = false;

    [Header("If stopped, don't increment movement to next.")]
    public bool deterministicMovement = true;

    bool leftFoot = false;

    public EntitySpriteData entitySpriteData;

    private Sprite[] spritesInUse;

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    public LayerMask collidableLayerMask;
    public LayerMask nonCollidableLayerMask;

    protected PokemonGameManager pokemonGameManager;

    public virtual void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        pokemonGameManager = PokemonGameManager.instance;
        FaceDown();

    }

    public virtual void Update()
    {
        Animation();
    }

    
    public void ExecuteCommand(EntityCommand command, int value = 1)
    {
        switch(command)
        {
            case EntityCommand.FACEDOWN:
                FaceDown(); break;
            case EntityCommand.FACELEFT:
                FaceLeft(); break;
            case EntityCommand.FACERIGHT:
                FaceRight(); break;
            case EntityCommand.FACEUP:
                FaceUp(); break;
            case EntityCommand.FACEPLAYER:
                FacePlayer(); break;
            case EntityCommand.MOVEDOWN:
                MoveDown(value); break;
            case EntityCommand.MOVELEFT:
                MoveLeft(value); break;
            case EntityCommand.MOVERIGHT:
                MoveRight(value); break;
            case EntityCommand.MOVEUP:
                MoveUp(value); break;
            case EntityCommand.MOVEFORWARD:
                MoveForward(value); break;
            case EntityCommand.MOVETOWARDSPLAYER:
                MoveTowardsPlayer(value); break;
            default:
                break;
        }
    }
    

    public void FaceUp() { FaceDirection(Direction.NORTH); }
    public void FaceDown() { FaceDirection(Direction.SOUTH); }
    public void FaceLeft() { FaceDirection(Direction.WEST); }
    public void FaceRight() { FaceDirection(Direction.EAST); }
    public void FacePlayer()
    {
        Vector3 difference = PlayerController.instance.transform.position - transform.position;
        
        bool x = Mathf.Abs(difference.x) > Mathf.Abs(difference.y);

        bool negative = (x ? Mathf.Sign(difference.x) : Mathf.Sign(difference.y)) < 0;

        if(x)
        {
            if(negative) {
                FaceLeft();
            } else {
                FaceRight();
            }
        } else
        {
            if (negative) {
                FaceDown();
            } else {
                FaceUp();
            }
        }

    }

    public void MoveUp(int times = 1) { MoveInDirection(Direction.NORTH, times); }
    public void MoveDown(int times = 1) { MoveInDirection(Direction.SOUTH, times); }
    public void MoveLeft(int times = 1) { MoveInDirection(Direction.WEST, times); }
    public void MoveRight(int times = 1) { MoveInDirection(Direction.EAST, times); }
    public void MoveForward(int times = 1) { MoveInDirection(direction, times); }
    public void MoveTowardsPlayer(int times = 1) { FacePlayer(); MoveForward(times); }

    public void StopMovement()
    {
        stopped = true;
    }

    public void StartMovement()
    {
        stopped = false;
    }

    public void FaceDirection(Direction d)
    {

        if (!stopped)
        {
            direction = d;
            index = 0;
            switch (direction)
            {
                case Direction.NORTH:
                    spritesInUse = entitySpriteData.northSprites;
                    break;
                case Direction.SOUTH:
                    spritesInUse = entitySpriteData.southSprites;
                    break;
                case Direction.EAST:
                    spriteRenderer.flipX = false;
                    spritesInUse = entitySpriteData.eastSprites;
                    break;
                case Direction.WEST:
                    spriteRenderer.flipX = true;
                    spritesInUse = entitySpriteData.eastSprites;
                    break;
                default:
                    break;
            }
        }
    }


    public void InitSprites()
    { FaceDirection(direction); }

    public void MoveInDirection(Direction d, int times = 1)
    {
        //StopAllCoroutines();
        StartCoroutine(MoveInDirectionCoroutine(d, times));
    }

    public IEnumerator MoveInDirectionCoroutine(Direction d, int times = 1)
    {
        isMoving = true;
        FaceDirection(d);
        int t = 0;

        while (t < times)
        {

            if (deterministicMovement)
            {
                while (CheckForthTile() || stopped)
                {
                    yield return null;
                }
            } else
            {
                if (CheckForthTile() || stopped)
                {
                    break;
                }
            }

            leftFoot = !leftFoot;

            Vector3 startingPosition = transform.position;
            Vector3 targetPositionOffset = Direction2Vector(direction);

            

            for (int i = 0; i < 16; i+=movespeed)
            {

                if(i>4 && i<12)
                {
                    index = leftFoot ? 1 : 2;
                } else
                {
                    index = 0;
                }

                Vector3 offset = Vector3.Lerp(Vector3.zero, targetPositionOffset, i / 16f);
                Vector3 inverseOffset = Vector3.Lerp(targetPositionOffset, Vector3.zero, i / 16f);

                transform.position = startingPosition + offset;

                boxCollider.offset = inverseOffset;

                yield return new WaitForEndOfFrame();
            }

            transform.position = startingPosition + targetPositionOffset;
            boxCollider.offset = Vector2.zero;

            CheckCurrentTile();

            t++;

            yield return null;
        }

        isMoving = false;
    }

    public virtual bool CheckCurrentTile()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(.1f, .1f), 0, nonCollidableLayerMask.value);
        if (collider != null)
        {
            if(collider.CompareTag("Grass"))
            {
                if(MapSettings.instance.OnGrassShake())
                {
                    StopMovement();
                }
                ParticleSystemPooler.instance.SpawnParticleSystem("ShakingGrass", transform.position);
                return true;
            }
        }

        return false;
    }

    public virtual Collider2D CheckForthTile()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction2Vector(direction), 1.0f, collidableLayerMask.value);

        Collider2D collider = hit.collider;

        if (collider != null)
        {
            if (hit.distance < 1.0f)
            {
                return collider;
            }
        }

        return null;

    }

    protected void Animation()
    {
        spriteRenderer.sprite = spritesInUse[index];
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y);
    }

    public static Vector3 Direction2Vector(Direction d)
    {
        switch (d)
        {
            case Direction.NORTH:
                return Vector3.up;
            case Direction.SOUTH:
                return Vector3.down;
            case Direction.EAST:
                return Vector3.right;
            case Direction.WEST:
                return Vector3.left;
            default:
                return Vector3.zero;
        }
    }

}
