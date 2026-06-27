using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiscoBallSpawner : MonoBehaviour
{
    public GameObject DiscoBallPrefab;
    public int SpawnThreshold = 2;

    public float SpawnXMin = -10;
    public float SpawnXMax = 10;
    public float SpawnZMin = 0;
    public float SpawnZMax = 20;
    public float SpawnHeightMin = 2;
    public float SpawnHeightMax = 5;

    public float SpawnCooldown = 0.5f;
    private float SpawnCooldownTimer = 0;
    public int maxBalls = 5;
    private int ballsDropped;

    public List<GameObject> balls = new List<GameObject>();


    private void Update()
    {
        if (SpawnCooldownTimer < SpawnCooldown)
        {
            SpawnCooldownTimer += Time.deltaTime;
        }

        // if (SongManager.Instance.Combo >= SpawnThreshold && SpawnCooldownTimer >= SpawnCooldown)
        // {
        //     SpawnCooldownTimer = 0;
        //     GameObject disco = Instantiate(DiscoBallPrefab, transform);
        //     balls.Add(disco);
        //     disco.transform.position = new Vector3(
        //         Random.Range(SpawnXMin, SpawnXMax),
        //         Random.Range(SpawnHeightMin, SpawnHeightMax),
        //         Random.Range(SpawnZMin, SpawnZMax));
        //
        //     disco.transform.rotation = Random.rotation;
        //
        //     disco.GetComponent<DiscoBall>().StartDrop(disco.transform.position.y);
        //     ballsDropped++;
        //     Debug.Log("BALL DROPADOPAD: " + ballsDropped);
        // }

        if (SongManager.Instance.Combo >= SpawnThreshold && SpawnCooldownTimer >= SpawnCooldown)
        {
            for (var i = 0; i < balls.Count; i++)
            {
                balls[i].SetActive(true);
            }
        }

        else if (SongManager.Instance.Combo < SpawnThreshold)
        {
            for (var i = 0; i < balls.Count; i++)
            {
                balls[i].SetActive(false);
            }
        }
    }
}