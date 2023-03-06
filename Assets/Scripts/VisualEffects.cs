using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// This script attaches to ‘VisualEffect’ objects. It destroys or deactivates them after the defined time

public class VisualEffects : MonoBehaviour {

    // the time after object will be destroyed
    [SerializeField] public float destructionTime;

    private void OnEnable()
    {
        StartCoroutine(Destruction()); //launch the timer of destruction
    }

    IEnumerator Destruction() // Wait the specified time for the explosion to play, then destroy or deactivate the explosion particle. 
    {
        yield return new WaitForSeconds(destructionTime); 
        Destroy(gameObject);
    }
}
