using UnityEngine;
using UnityEngine.SceneManagement;

// Put this on an empty object in the Win scene, then hook a UI Button's
// OnClick event to it (see the README, Part 6).
public class SceneButtons : MonoBehaviour
{
    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
