using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    Vector3 playerLastPosition;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist;
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDur;

    void Start()
    {
        playerLastPosition = player.transform.position;
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {   
        if (!currentChunk)
        {
            return;
        }

        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);

        CheckAndSpawnChunk(directionName);

        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
            CheckAndSpawnChunk("Left");
            CheckAndSpawnChunk("Right");
            CheckAndSpawnChunk("RightUp");
            CheckAndSpawnChunk("LeftUp");
        }
        else if (directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
            CheckAndSpawnChunk("Left");
            CheckAndSpawnChunk("Right");
            CheckAndSpawnChunk("LeftDown");
            CheckAndSpawnChunk("RightDown");
        }
        else if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
            CheckAndSpawnChunk("Down");
            CheckAndSpawnChunk("Up");
            CheckAndSpawnChunk("RightUp");
            CheckAndSpawnChunk("RightDown");
        }
        else if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
            CheckAndSpawnChunk("Down");
            CheckAndSpawnChunk("Up");
            CheckAndSpawnChunk("LeftUp");
            CheckAndSpawnChunk("LeftDown");
        }
    }

    void CheckAndSpawnChunk(string dirName)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(dirName).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(dirName).position);
        }
    }

    string GetDirectionName(Vector3 dir)
    {
        dir = dir.normalized;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.y > 0.5f)
            {
                return dir.x > 0 ? "RightUp" : "LeftUp";
            }
            else if (dir.y < -0.5f)
            {
                return dir.x > 0 ? "RightDown" : "LeftDown";
            }
            else
            {
                return dir.x > 0 ? "Right" : "Left";
            }
        } 
        else
        {
            if (dir.x > 0.5f)
            {
                return dir.y > 0 ? "RightUp" : "RightDown";
            }
            else if (dir.x < -0.5f)
            {
                return dir.y > 0 ? "LeftUp" : "LeftDown";
            }
            else
            {
                return dir.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDur;
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            } 
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
