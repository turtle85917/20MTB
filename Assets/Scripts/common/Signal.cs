using UnityEngine;
using UnityEngine.SceneManagement;

public class Signal : MonoBehaviour
{
    public void ChangeMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
