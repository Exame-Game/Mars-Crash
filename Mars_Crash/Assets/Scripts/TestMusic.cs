using UnityEngine;
using UnityEngine.UI;

public class TestMusic : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu", 1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestMusicFade();
        }
    }
    public void TestMusicFade()
    {
        MusicManager.Instance.PlayMusic("Level 1", 1f);
    }

}
