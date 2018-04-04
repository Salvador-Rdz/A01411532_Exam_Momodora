using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;
    public BoxCollider2D fallCollider;
    public Vector3 offset;
    public Vector3 resetPoint;
    public float minX;
    public float maxX;
    public float minY;
    private bool atLimits;
    // Use this for initialization
    void Start()
    {
        fallCollider = GetComponent<BoxCollider2D>();
        resetPoint = player.position;
    }

    //Follows the player, limiting the position of the camera and its boundaries
    void Update()
    {
        /* Vector3 newPos = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
         if (newPos.x > minX && newPos.x < maxX && newPos.y>minY)
         {
             transform.position = newPos;
         }*/
        transform.position = new Vector3(
        Mathf.Clamp(player.position.x, minX, maxX),
        Mathf.Clamp(player.position.y, minY, player.position.y),
        -10
        );
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = resetPoint;
            collision.gameObject.GetComponent<PlayerPlatformer2D>().takeDamage(1);
        }
    }
    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(4f);
    }
    IEnumerator finishLevel()
    {
        yield return new WaitForSeconds(4f);
        LevelManager lm = GameObject.FindObjectOfType<LevelManager>();
        lm.LoadStart();
    }
}
