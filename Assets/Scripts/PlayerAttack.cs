using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public int damage = 5;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public int getDmg()
    {
        return damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.GetComponent<PatrolEnemy>())collision.gameObject.GetComponent<PatrolEnemy>().TakeDamage(damage);
            if(collision.gameObject.GetComponent<FloatingShooterEnemy>())collision.gameObject.GetComponent<FloatingShooterEnemy>().TakeDamage(damage);
            if (collision.gameObject.GetComponent<Magnolia>()) collision.gameObject.GetComponent<Magnolia>().TakeDamage(damage);
            //StartCoroutine(FreezeHit());
        }
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
}
