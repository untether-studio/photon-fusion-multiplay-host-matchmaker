using Matchplay.Client;
using System;
using UnityEngine;

public class MatchMakerStarter : MonoBehaviour
{
    ClientGameManager gameManager
    {
        get
        {
            if (gameManagerCached == null)
            {
                gameManagerCached = ClientSingleton.Instance.Manager;
            }
            return gameManagerCached;
        }
    }
    ClientGameManager gameManagerCached;

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 175, 20), "Manually Start A Server"))
        {
            Debug.Log("Start server button");
            StartServer();
        }
    }

    public void StartServer()
    {
#pragma warning disable 4014
        gameManager.MatchmakeAsync(OnMatchMade);
#pragma warning restore 4014
    }

    void OnMatchMade(MatchmakerPollingResult result)
    {
        switch (result)
        {
            case MatchmakerPollingResult.Success:
                //SetMenuState(MainMenuPlayState.Connecting);
                break;
            case MatchmakerPollingResult.TicketCreationError:
                //SetMenuState(MainMenuPlayState.Error,
                    Debug.Log("Matchmaking Error while Creating a ticket.\n Check Console for more details.");
                break;
            case MatchmakerPollingResult.TicketCancellationError:
                //SetMenuState(MainMenuPlayState.Error,
                Debug.Log("Matchmaking Error while Cancelling a ticket.\n Check Console for more details.");
                break;
            case MatchmakerPollingResult.TicketRetrievalError:
                //SetMenuState(MainMenuPlayState.Error,
                    Debug.Log("Matchmaking Error while Retrieving a ticket.\n Check Console for more details.");
                break;
            case MatchmakerPollingResult.MatchAssignmentError:
                //SetMenuState(MainMenuPlayState.Error,
                Debug.Log("Matchmaking Error while Assigning a ticket.\n Check Console for more details.");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }
}

