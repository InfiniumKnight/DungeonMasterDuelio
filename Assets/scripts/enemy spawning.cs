using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyspawning : MonoBehaviour
{
    [SerializeField] 
    private GameObject skelebroPrefab;

    [SerializeField]
    private float min = 4;
    private float max = 8;

    private float timeTilNext;
    // Start is called before the first frame update
    void Awake()
    {
        SetTimeTillSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timeTilNext -= Time.deltaTime;

        if (timeTilNext <= 0)
        {
            Instantiate(skelebroPrefab, transform.position, Quaternion.identity);
            SetTimeTillSpawn();
        }
    }

    private void SetTimeTillSpawn()
    {
        timeTilNext = Random.Range(min, max);
    }
}
