using UnityEngine;
using UnityEngine.SceneManagement;  

public class SceneSwitchManager : MonoBehaviour
{
    public static SceneSwitchManager Instance;

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

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
