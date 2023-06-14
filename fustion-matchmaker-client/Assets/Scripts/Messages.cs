using Fusion;
using UnityEngine;

public class Messages : NetworkBehaviour
{
    [SerializeField] bool sendMessage;
    [SerializeField] string message = "Hello World";

    public override void FixedUpdateNetwork()
    {
        if (sendMessage)
        {
            //sendMessage = false;
            RPC_Message(message);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Message(string message)
    {
        Debug.Log("RPC_Message: " + message);
        RPC_MessageAll(message);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_MessageAll(string message)
    {
        Debug.Log("RPC_MessageAll: " + message);
    }
}
