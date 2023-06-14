using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Matchmaker.Models;

namespace Matchplay.Server
{
    public class ServerGameManager : IDisposable
    {
        public bool StartedServices => m_StartedServices;

        //MatchplayNetworkServer m_NetworkServer;
        MatchplayBackfiller m_Backfiller;
        string connectionString => $"{m_ServerIP}:{m_ServerPort}";
        string m_ServerIP = "0.0.0.0";
        int m_ServerPort = 7777;                                    // Server port: This is the port where the actual game data is sent and received.Players connect to this port when they join a game. In your case, this is represented by m_ServerPort.
        //int m_QueryPort = 7787;                                     // Query port: This is the port that is used for sending and receiving meta-information about the server, such as what game is being played, how many players are currently connected, etc.This information is often used by server browsers and matchmaking systems to determine which servers are available and what their current status is. In your case, this is represented by m_QueryPort.
        const int k_MultiplayServiceTimeout = 20000;
        bool m_StartedServices;
        MultiplayAllocationService m_MultiplayAllocationService;
        MultiplayServerQueryService m_MultiplayServerQueryService;
        string m_ServerName = "Matchplay Server";

        ushort maxPlayers = 20;

        public ServerGameManager()
        {
            m_MultiplayAllocationService = new MultiplayAllocationService();
            m_MultiplayServerQueryService = new MultiplayServerQueryService();
        }

        /// <summary>
        /// Attempts to initialize the server with services on Multiplay.
        /// </summary>
        public async Task StartGameServerAsync()
        {
            Debug.Log($"Starting server.");

            // The server should respond to query requests irrespective of the server being allocated.
            // Hence, start the handler as soon as we can.
            await m_MultiplayServerQueryService.BeginServerQueryHandler();
            try
            {
                var matchmakerPayload = await GetMatchmakerPayload(k_MultiplayServiceTimeout);

                if (matchmakerPayload != null)
                {
                    Debug.Log($"Got payload: {matchmakerPayload}");
                    //startingGameInfo = PickGameInfo(matchmakerPayload);

                    MatchStartedServerQuery();
                    await StartBackfill(matchmakerPayload);
                    m_StartedServices = true;
                }
                else
                {
                    Debug.LogWarning("Getting the Matchmaker Payload timed out, starting with defaults.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Something went wrong trying to set up the Services:\n{ex} ");
            }
        }

        async Task<MatchmakingResults> GetMatchmakerPayload(int timeout)
        {
            if (m_MultiplayAllocationService == null)
            {
                Debug.Log("m_MultiplayAllocationService is null");
                return null;
            }                

            //Try to get the matchmaker allocation payload from the multiplay services, and init the services if we do.
            var matchmakerPayloadTask = m_MultiplayAllocationService.SubscribeAndAwaitMatchmakerAllocation();

            //If we don't get the payload by the timeout, we stop trying.
            if (await Task.WhenAny(matchmakerPayloadTask, Task.Delay(timeout)) == matchmakerPayloadTask)
            {
                return matchmakerPayloadTask.Result;
            }

            return null;
        }

        private void MatchStartedServerQuery()
        {
            //Create a unique name for the server to show that we are joining the same one

            m_MultiplayServerQueryService.SetServerName(m_ServerName);
            m_MultiplayServerQueryService.SetMaxPlayers(maxPlayers);
            m_MultiplayServerQueryService.SetBuildID("0");
        }

        // TODO
        // See MatchplayBackfiller.cs. Matchmaking not being properly handled. Fusion handling players joining and leaving.
        async Task StartBackfill(MatchmakingResults payload)
        {
            m_Backfiller = new MatchplayBackfiller(connectionString, payload.QueueName, payload.MatchProperties,
                maxPlayers: maxPlayers);

            if (m_Backfiller.NeedsPlayers())
            {
                await m_Backfiller.BeginBackfilling();
            }
        }

        public async Task ShutdownServer()
        {
            Debug.Log($"Shutting down server.");
            await m_Backfiller.StopBackfill();
            Dispose();
            Application.Quit();
        }

        public void Dispose()
        {
            m_Backfiller?.Dispose();
            m_MultiplayAllocationService?.Dispose();
        }
    }
}
