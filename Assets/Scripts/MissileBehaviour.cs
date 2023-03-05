using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scavenger Lite
public class MissileBehaviour : MonoBehaviour
{
    public float speed;
    private float topBounds = 20f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        CheckBoundary();
    }

    void CheckBoundary()
    {
        //If missile is outside boundary, then deactivate it
        if (transform.position.y > topBounds)
        {
            gameObject.SetActive(false);
        }
    }
}
