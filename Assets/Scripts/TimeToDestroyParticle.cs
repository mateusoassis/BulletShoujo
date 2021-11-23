using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToDestroyParticle : MonoBehaviour
{
    public float timer;
    void Start()
    {
        StartCoroutine("ParticleTimer");
    }

    // Update is called once per frame
    public IEnumerator ParticleTimer()
    {
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }

}
