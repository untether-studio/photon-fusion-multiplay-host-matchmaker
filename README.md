Demo showing how a Fusion client can auto load a Multiplay server with a Fusion headless Linux build.

The demo is a slimmed down version of Unity's [Matchplay sample](https://github.com/Unity-Technologies/com.unity.services.samples.matchplay). Note that Fusion is instead of Netcode.

The repo contains a headless Linux server project and a client project. The Linux server project will not work in the Editor, it has to be built and uploaded to Unity's Multiplay servers. See [Matchplay sample](https://github.com/Unity-Technologies/com.unity.services.samples.matchplay) for instructions on how to get setup. 

Make sure the server and client both share the same Fusion app ID. 

When a client first starts it tries to connect to Fusion. If it can't find a Fusion server it requests Multiplay to start a server. When the server loads it auto starts a Fusion server and waits for the client to connect.

If there are no active players, the server waits for a time period before it quits the Unity application. This makes the Multiplay server shutdown.

TODO
- The code base is a butchered mess. It needs to be tidied up and further simplified. 
- Choose how to manage players leaving/joining with either Fusion or Matchmaker. 