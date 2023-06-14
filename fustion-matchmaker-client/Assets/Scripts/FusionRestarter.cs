using Fusion;
using System.Collections;
using UnityEngine;

public class FusionRestarter : MonoBehaviour
{
    [SerializeField] RunnerSpawner runnerSpawnerPrefab;

    public static FusionRestarter Instance => fusionRestarter;
    static FusionRestarter fusionRestarter;

    private void Awake()
    {
        if (fusionRestarter == null)
            fusionRestarter = this;
        else
            Destroy(gameObject);
    }

    public void Restart()
    {
        var runner = FindObjectOfType<NetworkRunner>();
        if (runner != null)
        {
            Destroy(runner.gameObject);
        }
        
        StartCoroutine(DelayInstantiate());
    }

    // coroutine that is delayed 30 seconds
    IEnumerator DelayInstantiate()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("Spawning new runner");
        Instantiate(runnerSpawnerPrefab);
    }
}
