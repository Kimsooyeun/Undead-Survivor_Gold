using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spwanPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;

    float timer;
    private void Awake()
    {
        spwanPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }
    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        if(timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy =  GameManager.instance.poolManager.Get(0);
        enemy.transform.position = spwanPoint[Random.Range(1,spwanPoint.Length)].transform.position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
