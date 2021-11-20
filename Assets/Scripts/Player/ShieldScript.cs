using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public Transform playerTarget;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
        transform.position = newPosition;
    }
}
