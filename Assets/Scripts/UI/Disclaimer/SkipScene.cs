using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipScene : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Mouse0))
		{
			SceneManager.LoadScene("MenuScene");
		}
    }
}
