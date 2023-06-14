using Fusion;
using Matchplay.Server;
using System.Linq;
using UnityEngine;

public class ServerShutdownMultiplay : MonoBehaviour
{
    NetworkRunner runner
    {
        get
        {
            if (runnerCached == null)
                runnerCached = FindObjectOfType<NetworkRunner>();
            return runnerCached;
        }
    }
    NetworkRunner runnerCached;

    // If player count is 0 for X seconds, shutdown
    [SerializeField] float shutdownDelay = 30f;
    float shutdownDelayCached;

    bool shuttingDown = false;

    private void Awake()
    {
        shutdownDelayCached = shutdownDelay;
    }

    private void Update()
    {
        if (runner == null)
            return;

        if (runner.ActivePlayers.Count() == 0)
        {
            if (shuttingDown)
                return;

            shutdownDelay -= Time.deltaTime;
            if (shutdownDelay <= 0)
            {
                Debug.Log("Shutting down server due to inactivity");

                shuttingDown = true;
                ServerShutdown();
            }
        }
        else
        {
            shutdownDelay = shutdownDelayCached;        // Reset the countdown if a play joins again.
        }
    }

    async void ServerShutdown()
    {
        await ServerSingleton.Instance.Manager.ShutdownServer();
    }
}
