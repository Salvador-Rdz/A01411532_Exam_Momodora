using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour {
    public GameObject[] Drops;
    public float hp = 100;
    public float speed = 5f;
    public int damage = 10;
    public bool Flips = false;
    public float direction = 1;
    public float moveRight = 5f;
    private Vector3 patrolA;
    public Vector3 patrolB;
    public static GameObject popupText;
    public static GameObject canvas;
    private bool moveUp = true;
    private SpriteRenderer sp;
    // Use this for initialization
    void Start()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
        patrolA = transform.position;
        patrolB = new Vector2(patrolA.x + moveRight, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolA == transform.position)
        {
            moveUp = true;
            if (Flips)
            {
                direction = -direction;
                Flip();
            }

        }
        if (patrolB == transform.position)
        {
            moveUp = false;
            if (Flips)
            {
                direction = -direction;
                Flip();
            }
        }
        if (moveUp)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolB, speed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolA, speed);
        }
    }

    //Flips the sprite, just used to change direction.
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
        StartCoroutine(flashHit());
        this.hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Instantiate(Drops[Random.Range(0, Drops.Length - 1)],new Vector2(transform.position.x, transform.position.y -0.4f), Quaternion.identity);
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
    private void OnDestroy()
    {
        GameObject.FindObjectOfType<EnemyCounter>().enemyDied();
    }
}
