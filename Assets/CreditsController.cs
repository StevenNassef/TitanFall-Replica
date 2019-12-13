using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsController : MonoBehaviour
{
    public Animator creditsRolling;
    // Start is called before the first frame update
    void Start()
    {
        creditsRolling.Play("Text_rolling");
    }

    // Update is called once per frame
    void Update()
    {
        if(creditsRolling.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !creditsRolling.IsInTransition(0)){
            SceneManager.LoadScene("MainmenuScene");
            Debug.Log("Main menu!");
        }
    }
}
