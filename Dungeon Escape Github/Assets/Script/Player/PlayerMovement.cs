using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask platformlayerMask;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private BoxCollider2D boxCollider2d;

    private float dirX = 0f;
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float jumpforce = 10f;
    public bool onMovingPlatform = false;
    
    public Vector2 startpos;
    public int startinghealth = 2;
    public int health = 2;
    public bool active = true;
    public bool respawning = false;
    public float TimetoRespawn = 2f;
    public float currentRespawnTime = 0f;

    private enum MovementState { idle, running, jumping, attack} 

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource Death;

    // public float min_X = -8.74f,max_X = 21.7f;
    // private bool Out_of_Bounds;

    
    // void CheckBounds()
    // {
    //     Vector2 temp = transform.position;

    //     if(temp.x > max_X)
    //     temp.x = max_X;

    //     if(temp.x < min_X)
    //     temp.x = min_X;

    //     transform.position = temp;

    // }

    // Start is called before the first frame update
    private void Start()
    {
        startpos = transform.position;
        active = true;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(respawning)
        {
            currentRespawnTime += Time.deltaTime;
            if(currentRespawnTime >= TimetoRespawn)
            {
                currentRespawnTime = 0f;
                respawning = false;
                RespawnPlayer();
            }

        }

        if(!active)
        {
            return;
        }

        //Horizontal run
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movespeed,rb.velocity.y);

        //Jump
         if(IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpforce);
            jumpSoundEffect.Play();
        } 

        UpdateAnimationUpdate();

      //D  CheckBounds();
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 1f,platformlayerMask);
        return raycastHit2d.collider != null;
    }

    //Animation
    private void UpdateAnimationUpdate()
    {
        MovementState state;

          if(dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
          //  boxCollider2d.offset = new Vector2(-0.17f,boxCollider2d.offset.y);
        }
        else if(dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
          //  boxCollider2d.offset = new Vector2(0.2f,boxCollider2d.offset.y);
        }
        else
        {
            state = MovementState.idle;
        }

        if(rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }

        anim.SetInteger("state",(int)state);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MovingPlatform")
        {
            transform.parent = collision.gameObject.transform;
            onMovingPlatform = true; 
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
            onMovingPlatform = false;
        }
    }

    public void playerHit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Death.Play();
            playerDefeated();
        }
    }

    public void playerDefeated()
    {
        active = false;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        respawning = true;
    }

    public void RespawnPlayer()
    {
        active = true;
        rb.isKinematic = false;
        health = startinghealth;
        transform.position = startpos;
    }
}
