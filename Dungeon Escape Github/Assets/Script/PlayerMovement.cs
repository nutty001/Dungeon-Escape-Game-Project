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
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Horizontal run
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movespeed,rb.velocity.y);

        //Jump
         if(IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpforce);
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
          if(dirX > 0f)
        {
            anim.SetBool("Running",true);
            sprite.flipX = false;
          //  boxCollider2d.offset = new Vector2(-0.17f,boxCollider2d.offset.y);
        }
        else if(dirX < 0f)
        {
            anim.SetBool("Running",true);
            sprite.flipX = true;
          //  boxCollider2d.offset = new Vector2(0.2f,boxCollider2d.offset.y);
        }
        else
        {
            anim.SetBool("Running",false);
        }
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
}
