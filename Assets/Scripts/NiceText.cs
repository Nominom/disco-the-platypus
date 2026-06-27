using TMPro;
using UnityEngine;

public class NiceText : MonoBehaviour
{
    private float _mult = 0.01f;
    public float ScaleSpeed = 0.1f;
    public float LifeTime = 2f;
    private float _lifeTimer = 0f;
    
    private void Update()
    {
        transform.localScale = Vector3.one * _mult;
        
        _mult += ScaleSpeed * Time.deltaTime;
        
        _lifeTimer += Time.deltaTime;

        if (_lifeTimer >= LifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void Set(BeatHitType beatHitType)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = beatHitType.ToString();
    }
}
