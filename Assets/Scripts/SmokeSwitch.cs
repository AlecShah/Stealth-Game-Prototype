using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeSwitch : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isSmokeOn;

    public ParticleSystem Smoke;

    private Collider smokeCollider;

    void Start()
    {
        isSmokeOn = false;
        smokeCollider = Smoke.gameObject.GetComponentInChildren<Collider>();
    }


    public void ChangeSmokeState()
    {


        smokeCollider.enabled = !smokeCollider.enabled;
        if (isSmokeOn)
        {
            isSmokeOn = false;
            
        }
        else
        {
            isSmokeOn = true;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (isSmokeOn && !Smoke.isEmitting)
        {
            Smoke.Play();
        }
        else if (!isSmokeOn && Smoke.isPlaying)
        {
            Smoke.Stop();
        }

    }
}
