using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f;                         // Delay between each zombie spawn

    public int currentWave = 0;
    public float waveCooldown = 10f;                        // Delay between each wave

    public bool inCooldown;
    public float cooldownCounter = 0f;                      // UI

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;
    public TextMeshProUGUI waveCounterUI;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;
        GlobalReferences.Instance.waveNumber = currentWave;
        StartNewWave();
    }



    private void Update()
    {

        List<Enemy> zombiesToRemove = currentZombiesAlive.Where(zombie => zombie.isDead).ToList();

        currentZombiesAlive.RemoveAll(zombie => zombie.isDead);
        zombiesToRemove.Clear();


        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCooldown());
        }

        if(inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }

        cooldownCounterUI.text = cooldownCounter.ToString("F0");
    }



    private void StartNewWave()
    {
        currentZombiesAlive.Clear();

        currentWave++;
        GlobalReferences.Instance.waveNumber = currentWave;
        waveCounterUI.text = $"Wave {currentWave}";

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            // Randomize spawn position
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Spawn zombie
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            Enemy enemyScript = zombie.GetComponent<Enemy>();

            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);

        }
    }
    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        waveOverUI.gameObject.SetActive(true);


        yield return new WaitForSeconds(waveCooldown);
    
        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);


        currentZombiesPerWave *= 2;         // Doubles the number of zombies per wave

        StartNewWave();
    }
}
