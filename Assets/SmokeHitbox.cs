using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeHitbox : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

        Physics.IgnoreLayerCollision(8, 9);
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
