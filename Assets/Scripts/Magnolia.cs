using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnolia : MonoBehaviour {
    public GameObject[] Drops;
    public int hp = 100;
    public int damage = 1;
    public bool Flips = false;
    public bool followScreen = false;
    public float direction = 1;
    public enum BossActions
    {
        Attacking,
        TraverseToRight,
        TraverseToLeft,
        Nothing,
    }
    public GameObject projectile;
    public GameObject smoke;
    public GameObject magnoliaDummy;
    public Transform target;
    public Camera cam;
    public BossHPBar hpBar;
    private Animator an;
    private SpriteRenderer sp;
    private BoxCollider2D col;
    public BossActions eCurState = BossActions.Attacking;
    public float actionSpeed;
    public float bulletSpeed = 5f;
    public Vector2 Left;
    public Vector2 Right;
    // Use this for initialization
    void Start()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
        an = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<BoxCollider2D>();
        hpBar.setTotalHP(hp);
        if (followScreen) cam = FindObjectOfType<Camera>();
        Think();
    }
	// Update is called once per frame
	void Update ()
    {
        if (hp > 0)
        {
            if (eCurState == BossActions.TraverseToRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, Right, 0.25f);
                if (transform.position.x >= Right.x)
                {
                    StopAllCoroutines();
                    eCurState = (BossActions)Random.Range(0, 3);
                    Think();
                }
            }
            if (eCurState == BossActions.TraverseToLeft)
            {
                transform.position = Vector2.MoveTowards(transform.position, Left, 0.25f);
                if (transform.position.x <= Left.x)
                {
                    StopAllCoroutines();
                    eCurState = (BossActions)Random.Range(0, 3);
                    Think();
                }
            }
        }
    }
    void Think()
    {
        if (hp > 0)
        {
            StopAllCoroutines();
            switch (eCurState)
            {
                case BossActions.Attacking:
                    StartCoroutine(Attack());
                    break;
                case BossActions.TraverseToRight:
                    an.Play("Shoot");
                    PuffSmoke();
                    transform.position = Left;
                    StartCoroutine(Fire());
                    break;
                case BossActions.TraverseToLeft:
                    an.Play("Shoot");
                    PuffSmoke();
                    transform.position = Right;
                    StartCoroutine(Fire());
                    break;
            }
        }
    }
    IEnumerator Attack()
    {
        eCurState = BossActions.Nothing;
        Teleport();
        yield return new WaitForSeconds(0.6f);
        an.Play("Attack");
        yield return new WaitForSeconds(actionSpeed);
        eCurState = (BossActions)Random.Range(0, 3);
        Think();
    }
    IEnumerator Fire()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
            //GameObject bulletFire = (GameObject)Instantiate(projectile, transform.position, projectile.transform.rotation);
            Instantiate(projectile, transform.position, projectile.transform.rotation);
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(actionSpeed);   
        eCurState = (BossActions)Random.Range(0, 3);
        Think();
    }
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;
    }
    void Teleport()
    {
        PuffSmoke();
        StartCoroutine(flashHit());
        StartCoroutine(Invulnerability());
        an.Play("Idle");
        Vector2 newPos = new Vector2(target.position.x + getRange(), -3.3f);
        transform.position = newPos;
        PuffSmoke();
        eCurState = BossActions.Attacking;
    }
    float getRange()
    {
        if(Random.Range(0, 100)> 50)
        {
            direction = -1;
            Flip();
            return Random.Range(-3f,-1.5f);
            
        }
        else
        {
            direction = 1;
            Flip();
            return Random.Range(3f, 1.5f);
        }
    }
    public int getDmg()
    {
        return damage;
    }
    //controler for taking damage and updating hits, its called from the source.
    public void TakeDamage(int damage)
    {
        this.hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
        else if (hp < 25)
        {
            actionSpeed = 1f;
        }
        else if (hp < 40 && hp > 25)
        {
            actionSpeed = 2f;
        }
        else if (hp < 75 && hp > 40)
        {
            actionSpeed = 2.5f;
        }
        hpBar.TakeDamage(damage);
        eCurState = (BossActions)Random.Range(0, 2);
        Think();
        StartCoroutine(flashHit());
    }
    IEnumerator flashHit()
    {
        for (int i = 0; i < 3; i++)
        {
            sp.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sp.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Die()
    {
        for (int i = 0; i < 15; i++)
        {
            Instantiate(Drops[Random.Range(0, Drops.Length - 1)], new Vector2(transform.position.x, transform.position.y - 0.4f), Quaternion.identity);
        }
        StopAllCoroutines();
        col.enabled = false;
        StartCoroutine(FreezeHit());
        StartCoroutine(finishLevel());
        an.Play("Death");
    }
    IEnumerator finishLevel()
    {
        yield return new WaitForSeconds(4f);
        LevelManager lm = GameObject.FindObjectOfType<LevelManager>();
        lm.LoadNextLevel();
    }
    IEnumerator Invulnerability()
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.3f);
        col.enabled = true;
    }
    IEnumerator FreezeHit()
    {
        Time.timeScale = 0.01f;
        float pauseEndTime = Time.realtimeSinceStartup + 0.2f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1;
    }
    void PuffSmoke()
    {
        GameObject smokePuff = Instantiate(smoke, transform.position, Quaternion.identity);
        ParticleSystem ps = smokePuff.GetComponent<ParticleSystem>();
        var main = ps.main;
    }
}
