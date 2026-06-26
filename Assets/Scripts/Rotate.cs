using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speedX;
    public float speedY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * speedX);
        transform.Rotate(Vector3.right, Time.deltaTime  * speedY);
    }
}
