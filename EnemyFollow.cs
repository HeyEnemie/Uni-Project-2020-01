using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float distanciaVisao = 8f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = 10;
    public Transform groundDetR, groundDetL, groundCheck;
    public LayerMask theGround;

    private Transform target;
    private Vector2 targetPos;
    private SpriteRenderer spriteRenderer;
    static bool onTheGround = false;
    private float oldPosition, difference;
    private float vSpeed;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        oldPosition = transform.position.x;
        vSpeed = 0f;
    }

    void Update()
    {
        Vector3 chaseDir = target.position - transform.position;
        chaseDir.y = 0;
        float distance = chaseDir.magnitude; 

        onTheGround = Physics2D.Linecast(transform.position, groundCheck.position, theGround);

        if (distance <= distanciaVisao)
        {
            this.spriteRenderer.flipX = target.transform.position.x < this.transform.position.x;

            targetPos = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            difference = transform.position.x - oldPosition;

            if (difference > 0) 
            {
                RaycastHit2D groundInfoR = Physics2D.Raycast(groundDetR.position, Vector2.down, 5f);

                if (groundInfoR.collider == false)
                {
                    // aplly jump speed
                    vSpeed = jumpSpeed;
                    // apply gravity
                    vSpeed -= gravity * Time.deltaTime;
                    // calculate horizontal velocity vector                                
                    chaseDir = chaseDir.normalized * moveSpeed;
                    // include vertical speed
                    chaseDir.y += vSpeed;
                    //move the enemy                      
                    transform.Translate(chaseDir * Time.deltaTime);
                }
            }
            else if(difference < 0)
            {
                RaycastHit2D groundInfoL = Physics2D.Raycast(groundDetL.position, Vector2.down, 5f);

                if (groundInfoL.collider == false)
                {
                    vSpeed = jumpSpeed; 
                    vSpeed -= gravity * Time.deltaTime; 
                                                        
                    chaseDir = chaseDir.normalized * moveSpeed;
                    chaseDir.y += vSpeed; 
                                          
                    transform.Translate(chaseDir * Time.deltaTime);
                }
                
            }

            oldPosition = transform.position.x;

        }

    }
}
