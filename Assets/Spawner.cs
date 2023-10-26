using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject CharacterPrefab;

    private int spawnCount = 0;
    private bool canSpawn = true;
    public bool startSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("start");
        //startSpawn = false;
        //StartSpawner();

        // gameObject.SetActive(true);


    }

    public void StartSpawner()
    {
        //Debug.Log("entrou");
        //startSpawn = true;
    }

    // public IEnumerator setInactive()
    // {
    //     startSpawn = false;
    //     yield return new WaitForSeconds(.2f);
    // }

    // Update is called once per frame
    void Update()
    {
        // if (startSpawn == true)
        // {
        //     Debug.Log("entrou");
        //     StartCoroutine(SpawnCharacters());
        //     //setInactive();
        //     startSpawn=false;
        // }
        if (startSpawn == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SpawnCharacters());
                startSpawn = false;
            }
        }

    }

    //   public void SpawnCharacters()
    // {
    //     Instantiate(CharacterPrefab, transform.position, Quaternion.identity);
    // }

    IEnumerator SpawnCharacters()
    {
        if (canSpawn)
        {
            while (spawnCount < 10)
            {
                if (canSpawn)
                {
                    Instantiate(CharacterPrefab, transform.position, Quaternion.identity);
                    spawnCount++;
                    if (spawnCount == 10)
                    {
                        canSpawn = false; // Stop spawning when spawnCount reaches 10
                    }
                }

                yield return new WaitForSeconds(2.0f); // Wait for 5 seconds before the next spawn
            }
        }
    }

}
