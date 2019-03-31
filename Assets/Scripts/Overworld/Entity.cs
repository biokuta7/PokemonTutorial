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

public class Entity : MonoBehaviour
{

    public enum EntityCommands
    {
        LOOKDOWN,
        LOOKLEFT,
        LOOKRIGHT,
        LOOKUP,
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

    bool leftFoot = false;

    [Header("Neutral, Left Stride, Right Stride")]
    public Sprite[] northSprites;
    public Sprite[] eastSprites;
    public Sprite[] southSprites;

    private Sprite[] spritesInUse;

    SpriteRenderer spriteRenderer;

    public LayerMask collidableLayerMask;
    public LayerMask nonCollidableLayerMask;

    protected PokemonGameManager pokemonGameManager;

    public virtual void Start()
    {

        Debug.Log("Fired");

        spriteRenderer = GetComponent<SpriteRenderer>();
        pokemonGameManager = PokemonGameManager.instance;
        FaceDown();

    }

    public virtual void Update()
    {
        Animation();
        
    }

    

    public void FaceDirection(Direction d)
    {
        direction = d;
        index = 0;
        switch (direction)
        {
            case Direction.NORTH:
                spritesInUse = northSprites;
                break;
            case Direction.SOUTH:
                spritesInUse = southSprites;
                break;
            case Direction.EAST:
                spriteRenderer.flipX = false;
                spritesInUse = eastSprites;
                break;
            case Direction.WEST:
                spriteRenderer.flipX = true;
                spritesInUse = eastSprites;
                break;
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

    public void MoveInDirection(Direction d, int times = 1)
    {
        //StopAllCoroutines();
        StartCoroutine(MoveInDirectionCoroutine(d, times));
    }

    public IEnumerator MoveInDirectionCoroutine(Direction d, int times = 1)
    {
        isMoving = true;
        FaceDirection(d);

        for (int t = 0; t < times; t++)
        {

            if(CheckForthTile() || stopped) {
                stopped = false;
                break;
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

                transform.position = startingPosition + Vector3.Lerp(Vector3.zero, targetPositionOffset, i / 16f);
                yield return new WaitForEndOfFrame();
            }

            transform.position = startingPosition + targetPositionOffset;


            CheckCurrentTile();

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
