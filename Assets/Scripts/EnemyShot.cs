using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour {

    public Transform target;
    public float bulletSpeed = 5f;
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            print("Deflected!");
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 dir = target.position - transform.position;

        float distanceThisFrame = bulletSpeed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.Rotate(Vector3.forward * -90*0.4f);
    }
    public void Seek(Transform _target)
    {
        target = _target;
    }
    public int getDamage()
    {
        return damage;
    }
}
