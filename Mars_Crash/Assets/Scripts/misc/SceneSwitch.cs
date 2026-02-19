using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{ 
  public static SceneSwitch Instance;

  private void Awake()
    {
      if (Instance != null)
      {
        Destroy(gameObject);
        return;
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);
    }

  public void SceneChanger(string sceneName)
    {
      SceneManager.LoadScene(sceneName);
    }
}