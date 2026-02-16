using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
  public void ScenenChanger(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}
