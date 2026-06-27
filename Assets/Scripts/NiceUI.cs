using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NiceUI : MonoBehaviour
{
    public static NiceUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("Many NiceUI in scene!");
        }
    }

    public GameObject NiceText;

    public Transform LeftSpawn;
    public Transform RightSpawn;

    public void SpawnNice(BeatHitType beatHitType)
    {
        GameObject nice = Instantiate(NiceText, transform);
        if (Random.value > 0.5f)
            nice.transform.position = RightSpawn.position;
        else
            nice.transform.position = LeftSpawn.position;
        
        nice.GetComponent<NiceText>().Set(beatHitType);
    }
}
