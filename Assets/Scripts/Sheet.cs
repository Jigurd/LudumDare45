using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheet : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerStay2D(Collider2D col)
    {
        //Debug.Log("Hello!");
        if (col.gameObject.CompareTag("Player"))
        {
            //Debug.Log("poop");
            col.gameObject.transform.parent = transform;
        }
    }
}
