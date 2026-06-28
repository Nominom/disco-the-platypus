using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private InputAction r;
    private float restartTime = 1f;
    public float restartTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        r = InputSystem.actions.FindAction("Rewind");
    }

    // Update is called once per frame
    void Update()
    {
        if (r.IsPressed())
        {
            restartTimer += Time.deltaTime;
            if (restartTimer >= restartTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            restartTimer = 0f;
        }
    }
}
