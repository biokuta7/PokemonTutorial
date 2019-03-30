using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Direction direction;

    public float movespeed = .5f;
    float t = 0;
    bool moving = false;
    bool running = false;
    bool swimming = false;
    Vector3 targetPosition;
    Vector3 currentPosition;

    SpriteRenderer spriteRenderer;

    public LayerMask collidableLayerMask;
    public LayerMask nonCollidableLayerMask;

    public enum Direction
    {
        NORTH,
        SOUTH,
        EAST,
        WEST,
        NONE
    }

    bool leftFoot = false;

    [Header("Neutral, Left Stride, Right Stride")]
    public Sprite[] northSprites;
    public Sprite[] eastSprites;
    public Sprite[] southSprites;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!moving)
        {
            GatherInput();
        }
        Movement();
        Animation();
    }

    private void GatherInput()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        running = Input.GetButton("Run");

        direction = Direction.NONE;

        //Movement Made
        if (x != 0 || y != 0)
        {
            t = 0;
            leftFoot = !leftFoot;

            if (x > 0) { direction = Direction.EAST; }
            else if (x < 0) { direction = Direction.WEST; }
            else if (y > 0) { direction = Direction.NORTH; }
            else if (y < 0) { direction = Direction.SOUTH; }


        }
        RaycastHit2D collisionHit = Physics2D.Raycast(transform.position, Direction2Vector(direction), 1.0f, collidableLayerMask.value);
        RaycastHit2D nonCollisionHit = Physics2D.Raycast(transform.position, Direction2Vector(direction), 1.0f, nonCollidableLayerMask.value);

        /*
        if (collisionHit.collider != null)
            Debug.Log(collisionHit.collider.gameObject.name);
        if (nonCollisionHit.collider != null)
            Debug.Log(nonCollisionHit.collider.gameObject.name);
            */
        currentPosition = transform.position;
        
        if (direction != Direction.NONE && nonCollisionHit.collider != null && nonCollisionHit.distance < 0.3f && nonCollisionHit.collider.gameObject.CompareTag("Grass"))
        {
            MapSettings.instance.OnGrassShake();
            //Debug.Log("Trigger Battle Check");
            ParticleSystemPooler.instance.SpawnParticleSystem("ShakingGrass", currentPosition);
            //Instantiate(grassParticles, currentPosition, Quaternion.identity);
        }
        
        targetPosition = transform.position +
            ((collisionHit.collider != null && collisionHit.distance < 1.0f) ?
            Vector3.zero : Direction2Vector(direction));
        

    }

    private void Animation()
    {

        int index = 0;

        if (t < .8f)
        {
            index = leftFoot ? 1 : 2;
        }

        if (running && moving) { index += 3; }

        switch (direction)
        {
            case Direction.NORTH:
                spriteRenderer.sprite = northSprites[index];
                break;
            case Direction.SOUTH:
                spriteRenderer.sprite = southSprites[index];
                break;
            case Direction.EAST:
                spriteRenderer.flipX = false;
                spriteRenderer.sprite = eastSprites[index];
                break;
            case Direction.WEST:
                spriteRenderer.flipX = true;
                spriteRenderer.sprite = eastSprites[index];
                break;
            default:
                break;
        }
    }

    private void Movement()
    {
        moving = t < 1;//(transform.position != targetPosition);
        transform.position = Vector3.Lerp(currentPosition, targetPosition, t);

        t += Time.deltaTime * movespeed * (running? 2.0f : 1.0f);
        
    }

    private Vector3 Direction2Vector(Direction d)
    {
        switch(d)
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
