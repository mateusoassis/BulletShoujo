using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private bool startUpdate;
    public Transform target;
    public GameObject myPlayer;

    private void LateUpdate()
    {
        Vector3 newPosition = new Vector3(myPlayer.transform.position.x, 0, myPlayer.transform.position.z);
        transform.position = newPosition;
    }
}
