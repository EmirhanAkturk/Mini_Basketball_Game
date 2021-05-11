using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPooling : MonoBehaviour
{
    public static BallPooling Instance;

    [Header("Ball")]
    [SerializeField]
    GameObject ballPrefab;

    [SerializeField]
    Transform ballsPoolParent;

    [SerializeField]
    Vector3 spawnPosition;

    private List<GameObject> balls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        // Allocate memory for th list
        MemoryAllocate();

        GenerateBalls();
    }

    private void MemoryAllocate()
    {
        //allocate memory as many as the number of max trail cube.
        balls = new List<GameObject>(5);
    }

    private void GenerateBalls()
    {
        int capacity = balls.Capacity;

        GameObject newBall;

        for (int i = 0; i < capacity; ++i)
        {
            newBall = Instantiate(ballPrefab, spawnPosition, ballPrefab.transform.rotation);
            newBall.name = "newBall" + (i+1);
            newBall.SetActive(false);
            newBall.transform.parent = transform;

            balls.Add(newBall);
        }
    }

    public GameObject GetNewBall()
    {
        if (balls.Count == 0 || balls[0].activeSelf)
        {
            GameObject newBall;

            newBall = Instantiate(ballPrefab, spawnPosition, ballPrefab.transform.rotation);
            newBall.name = "newBall" + (balls.Count + 1);
            newBall.SetActive(true);
            newBall.transform.parent = transform;

            balls.Insert(0, newBall);
        }

        GameObject usingBall = balls[0];

        balls.RemoveAt(0);
        balls.Add(usingBall);

        SetDefaultValues(usingBall);

        IPooledBall pooledBall = usingBall.GetComponent<IPooledBall>();

        if (pooledBall != null)
            pooledBall.OnBallSpawn();

        return usingBall;
    }

    private void SetDefaultValues(GameObject usingBall)
    {
        usingBall.SetActive(true);
        usingBall.transform.position = spawnPosition;
        usingBall.transform.rotation = ballPrefab.transform.rotation;
        usingBall.tag = "Ball";
    }
}
