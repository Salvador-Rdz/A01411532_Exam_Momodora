using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnoliaDummy : MonoBehaviour {
    private Animator an;
    // Use this for initialization
    void Start () {
        an = gameObject.GetComponent<Animator>();
        an.Play("Death");
    }
    


}
