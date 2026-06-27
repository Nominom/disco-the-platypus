using System;
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
        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = beatHitType.ToString();

        switch (beatHitType)
        {
            case BeatHitType.Miss:
                break;
            case BeatHitType.Good:
                text.color = Color.green;
                break;
            case BeatHitType.Nice:
                text.color = Color.blue;
                break;
            case BeatHitType.Perfect:
                text.color = Color.yellow;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(beatHitType), beatHitType, null);
        }
    }
}