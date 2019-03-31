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
        MOVEDOWN,
        MOVELEFT,
        MOVERIGHT,
        MOVEUP
    }

    Direction direction;

    int index = 0;

    public int movespeed = 1;

    protected bool running = false;
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
            case EntityCommand.MOVEDOWN:
                MoveDown(value); break;
            case EntityCommand.MOVELEFT:
                MoveLeft(value); break;
            case EntityCommand.MOVERIGHT:
                MoveRight(value); break;
            case EntityCommand.MOVEUP:
                MoveUp(value); break;
            default:
                break;
        }
    }
    

    public void FaceUp() { FaceDirection(Direction.NORTH); }
    public void FaceDown() { FaceDirection(Direction.SOUTH); }
    public void FaceLeft() { FaceDirection(Direction.WEST); }
    public void FaceRight() { FaceDirection(Direction.EAST); }

    public void MoveUp(int times = 1) { MoveInDirection(Direction.NORTH, times); }
    public void MoveDown(int times = 1) { MoveInDirection(Direction.SOUTH, times); }
    public void MoveLeft(int times = 1) { MoveInDirection(Direction.WEST, times); }
    public void MoveRight(int times = 1) { MoveInDirection(Direction.EAST, times); }

    public void StopMovement()
    {
        stopped = true;
    }

    public void FaceDirection(Direction d)
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
                    stopped = false;

                    yield return null;
                }
            } else
            {
                if (CheckForthTile() || stopped)
                {
                    stopped = false;
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

    public virtual bool CheckForthTile()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction2Vector(direction), 1.0f, collidableLayerMask.value);

        Collider2D collider = hit.collider;

        if (collider != null)
        {
            if (hit.distance < 1.0f)
            {
                return true;
            }
        }

        return false;

    }

    private void Animation()
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
