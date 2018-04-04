using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Similar workings to ShantaeController, just revised and cleaned.
public class PlayerPlatformer2D : MonoBehaviour {
    [Range(1, 10)]
    public float jumpForce;
    public Vector3 currentSpeed;
    public int maxHP = 8;
    public int currentHP;
    public int score = 0;
    public Sprite hurtSprite;
    public HealthBar hpBar;
    public ScoreTracker scoreTrack;
    private Rigidbody2D rb;
    public Animator animator;
    public AudioManager adm;
    private int direction = 1;
    [Range(1, 10)]
    public float speed = 10f;
    public bool isHit = false;
    private SpriteRenderer sp;
    private LevelManager lm;
    //Experimental
    public float jumpTime = 2f;
    bool jumpButtonPressed = false;
    public Vector2 jumpVector;
    // Use this for initialization
    void Awake()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = gameObject.GetComponent<SpriteRenderer>();
        lm = GameObject.FindObjectOfType<LevelManager>();
        if (!adm) adm = GameObject.FindObjectOfType<AudioManager>();
        hpBar.setTotalHP(maxHP);
    }
    private void FixedUpdate()
    {
        if (isHit == false)
        {
            currentSpeed = rb.velocity;
            if (!Input.anyKey)
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("isCrouching", false);
                animator.SetBool("isMoving", false);
            }
            else
            {
                if (Input.GetKey(KeyCode.Z))
                {
                    animator.SetBool("isAttacking", true);
                    if (checkIfAttacking())
                    {
                        //AudioManager
                    }
                }
                else
                {
                    animator.SetBool("isAttacking", false);
                }
                if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && animator.GetBool("isGrounded") && !checkIfAttacking())
                {
                    animator.SetBool("isGrounded", false);
                    jump();
                }
                if ((!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)))
                {
                    if (!animator.GetBool("isGrounded") && Input.GetKey(KeyCode.Z))
                    {
                        animator.SetBool("isAttacking", true);
                        animator.SetBool("isMoving", true);
                        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow) && !checkIfAttacking())
                        {
                            move(-1);
                        }
                        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !checkIfAttacking())
                        {
                            move(1);
                        }
                    }
                    else
                    {
                        if (!Input.GetKey(KeyCode.Z) && !animator.GetBool("isAttacking"))
                        {
                            animator.SetBool("isMoving", true);
                            animator.SetBool("isAttacking", false);
                            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow) && !checkIfAttacking())
                            {
                                move(-1);
                            }
                            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !checkIfAttacking())
                            {
                                move(1);
                            }
                        }
                        else
                        {
                            animator.SetBool("isMoving", false);
                            animator.SetBool("isAttacking", true);
                        }
                    }
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    animator.SetBool("isCrouching", false);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    crouch();
                }
            }
        }
    }
    IEnumerator attack()
    {
        while (true)
        {
            animator.SetBool("isAttacking", true);
            yield return new WaitForSeconds(1f);
        }

    }
   
    void jump()
    {
        animator.SetBool("isGrounded", false);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    IEnumerator JumpRoutine()
    {
        rb.velocity = Vector2.zero;
        float timer = 0;

        while (jumpButtonPressed && timer < jumpTime)
        {
            //Calculate how far through the jump we are as a percentage
            //apply the full jump force on the first frame, then apply less force
            //each consecutive frame

            float proportionCompleted = timer / jumpTime;
            Vector2 thisFrameJumpVector = Vector2.Lerp(jumpVector, Vector2.zero, proportionCompleted);
            rb.AddForce(thisFrameJumpVector);
            timer += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isGrounded", true);
    }
    void crouch()
    {
        animator.SetBool("isCrouching", true);
        animator.SetBool("isAttacking", false);
        if (Input.GetKey(KeyCode.Z))
        {
            animator.SetBool("isAttacking", true);
        }
        /* Want to move while crouching?
        else if (Input.GetKey(KeyCode.LeftArrow) && !checkIfAttacking())
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
            direction = -1;
            Flip();
            transform.position += Vector3.left * speed * 0.2f * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !checkIfAttacking())
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
            direction = 1;
            Flip();
            transform.position += Vector3.right * speed * 0.2f * Time.deltaTime;
        }*/
        else
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isMoving", false);
        }
    }
    //left is -1 right is 1
    void move(int dir)
    {
        direction = dir;
        Flip();
        if (dir < 0)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool("isGrounded", true);
        }
        if (collision.gameObject.tag == "Enemy" && isHit == false)
        {
            if(collision.gameObject.GetComponent<PatrolEnemy>()) takeDamage(collision.gameObject.GetComponent<PatrolEnemy>().getDmg());
            if (collision.gameObject.GetComponent<FloatingShooterEnemy>()) takeDamage(collision.gameObject.GetComponent<FloatingShooterEnemy>().getDmg());
            
        }
        if (collision.gameObject.tag == "Spike" && isHit == false)
        {
            takeDamage(1);
            if (direction == 1) rb.velocity = new Vector2(-4.5f, 6f);
            if (direction == -1) rb.velocity = new Vector2(4.5f, 6f);
        }


    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool("isGrounded", true);
        }
        if (collision.gameObject.tag == "Spike" && isHit == false)
        {
            takeDamage(1);
            if (direction == 1) rb.velocity = new Vector2(-4.5f, 6f);
            if (direction == -1) rb.velocity = new Vector2(4.5f, 6f);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool("isGrounded", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HealthPickup")
        {
            if (maxHP > currentHP)
            {
                heal(collision.gameObject.GetComponent<Pickup>().getHeal());
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.tag == "ScorePickup")
        {
            scoreGain(collision.gameObject.GetComponent<Pickup>().getScore());
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Projectile")
        {
            if(collision.gameObject.GetComponent<EnemyShot>()) takeDamage(collision.gameObject.GetComponent<EnemyShot>().getDamage());
            if (collision.gameObject.GetComponent<ArrowDown>()) takeDamage(collision.gameObject.GetComponent<ArrowDown>().getDamage());
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Enemy" && isHit == false)
        {
            if (collision.gameObject.GetComponent<MeeleAttack>()) takeDamage(collision.gameObject.GetComponent<MeeleAttack>().getDmg());
        }
    }
    void heal(int healValue)
    {
        currentHP += healValue;
        hpBar.IncreaseHealth(healValue);
    }
    void scoreGain(int scoreIncrease)
    {
        score += scoreIncrease;
        lm.score = score;
        scoreTrack.IncreaseScore(score);
    }
    //Flips the sprite, just used to change direction.
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;

    }
    //Useful to manage the smoothness inbetween the attacks completing and the character moving.
    bool checkIfAttacking()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("KahoNeutralAttack"))
        {
            return true;
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("shantaeDuckAttack"))
        {
            return true;
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("shantaeJumpAttack"))
        {
            return true;
        }
        return false;
    }
    //controler for taking damage and updating hits, its called from the source.
    public void takeDamage(int damage)
    {
        isHit = true;
        sp.GetComponent<SpriteRenderer>().sprite = hurtSprite;
        StartCoroutine(flashHit());
        StartCoroutine(Invulnerability());
        if (direction == 1) rb.velocity = new Vector2(-1.5f, 1f);
        if (direction == -1) rb.velocity = new Vector2(1.5f, 1f);
        this.currentHP -= damage;
        hpBar.TakeDamage(damage);
    }
    IEnumerator Invulnerability()
    {
        
        animator.enabled = false;
        yield return new WaitForSeconds(0.5f);
        if (currentHP <= 0)
        {
            lm.LoadLevel("GameOver");
        }
        isHit = false;
        animator.enabled = true;
    }
    //flashes three times, disabling the renderer to make the sprite invisible.
    IEnumerator flashHit()
    {
        sp.GetComponent<SpriteRenderer>().sprite = hurtSprite;
        for (int i = 0; i < 3; i++)
        {
            sp.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sp.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
}
