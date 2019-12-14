using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class PauseController : MonoBehaviour
{
  public GameObject pauseArea;
  private List<CharacterStatsHandler> pausedHandlers = new List<CharacterStatsHandler>();
  public void OnClickMainMenu() {
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainmenuScene");
  }

  public void OnClickRestart() {
  // TODO
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainmenuScene");
  }

  public void OnClickResume() {
    Debug.Log("OnResume");
    pauseArea.SetActive(false);
    for (int i = 0; i < pausedHandlers.Count; i++) {
      pausedHandlers[i].ResumeBack();
    }
    pausedHandlers.Clear();
  }

  public void Pause(CharacterStatsHandler statusHandler) {
    pausedHandlers.Add(statusHandler);
    pauseArea.SetActive(true);
  }

  // Start is called before the first frame update
  void Start()
  {
      
  }

  // Update is called once per frame
  void Update()
  {
      
  }

}
