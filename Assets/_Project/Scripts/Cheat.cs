using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)){
            SceneManager.LoadScene("Prototyping_Parkour");
        }

        if(Input.GetKeyDown(KeyCode.N)){
            Application.LoadLevel("Prototyping_Parkour");
        }
    }
}
