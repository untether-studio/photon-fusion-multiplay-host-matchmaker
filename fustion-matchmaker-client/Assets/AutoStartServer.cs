using Fusion;
using UnityEngine;
using System.Collections;

public class AutoStartServer : MonoBehaviour
{
    [SerializeField] float timeToWaitForFusion = 10f;

    NetworkRunner runner
    {
        get
        {
            if(runnerCached == null)
                runnerCached = FindObjectOfType<NetworkRunner>();
            return runnerCached;
        }
    }
    NetworkRunner runnerCached;

    MatchMakerStarter matchMakerStarter
    {
        get
        {
            if(matchMakerStarterCached == null)
                matchMakerStarterCached = GetComponent<MatchMakerStarter>();
            return matchMakerStarterCached;
        }
    }
    MatchMakerStarter matchMakerStarterCached;


    private Coroutine countdownCoroutine;

    void Start()
    {
        countdownCoroutine = StartCoroutine(StartCountdown(timeToWaitForFusion));
                
    }

    private void Update()
    {
        if(countdownCoroutine != null)
        {
            if (runner.State == NetworkRunner.States.Running)
            {
                CancelCountdown();
            }
        }
    }

    IEnumerator StartCountdown(float countdownValue)
    {
        while (countdownValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
            countdownValue--;
        }

        if(runner.State != NetworkRunner.States.Running)
        {
            Debug.Log("Starting Multiplay server");

            matchMakerStarter.StartServer();
        }
    }

    // Call this method to cancel the countdown
    public void CancelCountdown()
    {
        if (countdownCoroutine != null)
        {
            //Debug.Log("Countdown cancelled!");
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

}
