using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalManager : MonoBehaviour
{
    public void ChangeMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
