using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;

public class SendPointsFusion : NetworkBehaviour, INetworkRunnerCallbacks
{
    // This class is different to the class on the client, but needs to have the same name.

    private void Start()
    {
        Runner.AddCallbacks(this);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // If player that left is the input authority of this object, destory this gameobject
        if (player == Object.InputAuthority)
        {
            Runner.RemoveCallbacks(this);
            Destroy(gameObject);
        }

    }

    #region Interface methods
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {

    }
    #endregion
}
