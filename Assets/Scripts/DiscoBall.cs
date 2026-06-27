using System;
using UnityEngine;

public class DiscoBall : MonoBehaviour
{
    public float RotateSpeed = 20f;
    public float HeightOffsetAtStart = 10f;
    public float DropSpeed = 1f;

    private bool dropping = false;
    private float _targetHeight;
    
    public void StartDrop(float targetHeight)
    {
        dropping = true;
        _targetHeight = targetHeight;
        transform.position = new Vector3(transform.position.x, targetHeight + HeightOffsetAtStart, transform.position.z);
    }
    
    public void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, RotateSpeed * Time.deltaTime);
        if (dropping)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - DropSpeed * Time.deltaTime, transform.position.z);
            if (transform.position.y < _targetHeight)
                dropping = false;
        }
    }
}
