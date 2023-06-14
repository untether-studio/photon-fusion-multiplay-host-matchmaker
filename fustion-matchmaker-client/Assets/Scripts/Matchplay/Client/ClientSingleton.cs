using UnityEngine;

namespace Matchplay.Client
{
    public class ClientSingleton : MonoBehaviour
    {
        // Tooltip 
        [Tooltip("Queue name must match a queue name in Multiplay/Matchmaker/Queues.")]
        [SerializeField] string gameQueue; 

        public static ClientSingleton Instance
        {
            get
            {
                if (s_ClientGameManager != null) return s_ClientGameManager;
                s_ClientGameManager = FindObjectOfType<ClientSingleton>();
                if (s_ClientGameManager == null)
                {
                    Debug.LogError("No ClientSingleton in scene, did you run this from the bootStrap scene?");
                    return null;
                }

                return s_ClientGameManager;
            }
        }

        static ClientSingleton s_ClientGameManager;

        public ClientGameManager Manager
        {
            get
            {
                if (m_GameManager != null) return m_GameManager;
                Debug.LogError($"ClientGameManager is missing, did you run StartClient()?", gameObject);
                return null;
            }
        }

        ClientGameManager m_GameManager;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            CreateClient();
        }

        public void CreateClient(string profileName = "default")
        {
            m_GameManager = new ClientGameManager(gameQueue, profileName);
        }

        void OnDestroy()
        {
            Manager?.Dispose();
        }
    }
}
