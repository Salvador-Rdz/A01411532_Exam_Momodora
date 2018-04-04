using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingShooterEnemy : MonoBehaviour {
    public GameObject[] Drops;
    public int hp = 100;
    public int damage = 1;
    public bool Flips = false;
    public bool followScreen = false;
    public float direction = 1;
    public enum BossActions
    {
        Idle,
        Attacking,
        Teleporting,
    }
    public GameObject Projectile;
    public Transform target;
    public Camera cam;
    public Vector2 TeleportA;
    public Vector2 TeleportB;
    public Vector2 TeleportC;
    public Vector2 TeleportD;
    public Vector2 TeleportE;
    public BossHPBar hpBar;
    private Animator an;
    private SpriteRenderer sp;
    private BoxCollider2D col;
    private BossActions eCurState = BossActions.Idle;
    public float actionSpeed;
    public float bulletSpeed = 5f;
    // Use this for initialization
    void Start () {
        sp = gameObject.GetComponent<SpriteRenderer>();
        an = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<BoxCollider2D>();
        hpBar.setTotalHP(hp);
        if (followScreen) cam = FindObjectOfType<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        switch (eCurState)
        {
            case BossActions.Idle:
                //print("Rethink...");
                StartCoroutine(Idle());
                break;
            case BossActions.Attacking:
                StopCoroutine(Attack());
                
                StartCoroutine(Attack());
                eCurState = BossActions.Idle;
                break;
            case BossActions.Teleporting:
                StopAllCoroutines();
                Teleport();
                break;
        }
	}
    void Teleport()
    {
        StartCoroutine(flashHit());
        StartCoroutine(Invulnerability());
        an.Play("Idle");
        switch (Random.Range(0, 4))
        {
            case 0:
                if (!followScreen) transform.position = TeleportA;
                else transform.position = new Vector2(cam.transform.position.x + TeleportA.x, cam.transform.position.y + TeleportA.y);
                eCurState = BossActions.Attacking;
                break;
            case 1:
                if (!followScreen) transform.position = TeleportB;
                else transform.position = new Vector2(cam.transform.position.x + TeleportB.x, cam.transform.position.y + TeleportB.y);
                eCurState = BossActions.Attacking;
                break;
            case 2:
                if (!followScreen) transform.position = TeleportC;
                else transform.position = new Vector2(cam.transform.position.x + TeleportC.x, cam.transform.position.y + TeleportC.y);
                eCurState = BossActions.Attacking;
                break;
            case 3:
                if (!followScreen) transform.position = TeleportD;
                else transform.position = new Vector2(cam.transform.position.x + TeleportD.x, cam.transform.position.y + TeleportD.y);
                eCurState = BossActions.Attacking;
                break;
            case 4:
                if (!followScreen) transform.position = TeleportE;
                else transform.position = new Vector2(cam.transform.position.x + TeleportE.x, cam.transform.position.y + TeleportE.y);
                eCurState = BossActions.Attacking;
                break;
        }
    }
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.6f);
        eCurState = BossActions.Idle;
        an.Play("Attack");
        yield return new WaitForSeconds(0.4f);
        Shoot();
    }
    void Shoot()
    {
        GameObject bulletFire = (GameObject)Instantiate(Projectile, transform.position, Quaternion.identity);
        EnemyShot bullet = bulletFire.GetComponent<EnemyShot>();
        bullet.GetComponent<SpriteRenderer>().color = new Color(229, 156, 255, 255);
        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
    IEnumerator Idle()
    {
        yield return new WaitForSeconds(actionSpeed);
        eCurState = (BossActions)Random.Range(0, 3);
    }
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = direction;
        transform.localScale = scale;
    }

    public int getDmg()
    {
        return damage;
    }
    //controler for taking damage and updating hits, its called from the source.
    public void TakeDamage(int damage)
    {
        eCurState = BossActions.Teleporting;
        StartCoroutine(flashHit());
        this.hp -= damage;
        if (hp <= 0)
        {
            Die();
        } 
        else if(hp<15)
        {
            actionSpeed = 1.5f;
        }
        else if(hp <40 && hp>15)
        {
            actionSpeed = 2f;
        }
        else if(hp<75 && hp>40)
        {
            actionSpeed = 2.5f;
        }
        hpBar.TakeDamage(damage);
    }
    public void Die()
    {
        for (int i = 0; i < 15; i++)
        {
            Instantiate(Drops[Random.Range(0, Drops.Length - 1)], new Vector2(transform.position.x, transform.position.y - 0.4f), Quaternion.identity);
        }
        Destroy(gameObject);
    }
    //flashes three times, disabling the renderer to make the sprite invisible.
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
    IEnumerator Invulnerability()
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.3f);
        col.enabled = true;
        
    }
    private void OnDestroy()
    {
        GameObject.FindObjectOfType<EnemyCounter>().enemyDied();
    }
}
