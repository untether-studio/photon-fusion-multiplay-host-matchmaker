using System;
using System.Text;
using Matchplay.Shared.Tools;

namespace Matchplay.Shared
{
    public class MatchplayUser
    {
        public MatchplayUser(string gameQueue)
        {
            var tempId = Guid.NewGuid().ToString();
            Data = new UserData(NameGenerator.GetName(tempId), tempId, 0, new GameInfo(gameQueue));
        }

        public UserData Data { get; }

        public string Name
        {
            get => Data.userName;
            set
            {
                Data.userName = value;
                onNameChanged?.Invoke(Data.userName);
            }
        }

        public Action<string> onNameChanged;

        public string AuthId
        {
            get => Data.userAuthId;
            set => Data.userAuthId = value;
        }

        public override string ToString()
        {
            var userData = new StringBuilder("MatchplayUser: ");
            userData.AppendLine($"- {Data}");
            return userData.ToString();
        }
    }

    /// <summary>
    /// Data to be passed to the network.
    /// </summary>
    [Serializable]
    public class UserData
    {
        public string userName; 
        public string userAuthId; 
        public ulong networkId;
        public GameInfo userGamePreferences; // User game preferences such as map, game mode, etc.

        public UserData(string userName, string userAuthId, ulong networkId, GameInfo userGamePreferences)
        {
            this.userName = userName;
            this.userAuthId = userAuthId;
            this.networkId = networkId;
            this.userGamePreferences = userGamePreferences;
        }
    }

    /// <summary>
    /// Subset of information that sets up the map and gameplay
    /// </summary>
    [Serializable]
    public class GameInfo
    {
        public string GameQueue;       // Name must match a queue name in Multiplay/Matchmaker/Queues.

        public GameInfo(string gameQueue)
        {
            GameQueue = gameQueue;
        }
    }
}