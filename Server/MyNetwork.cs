using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

/*
	Documentation: https://mirror-networking.com/docs/Components/NetworkManager.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class MyNetwork : NetworkManager
{
    [Scene] public string gameScene;

    private static MyNetwork instance;
    public static MyNetwork getInstance() => instance;

    private bool isTransitioningFromGameScene;

    private string clientOldScene;
    private string clientCurrentScene;

    #region Unity Callbacks

    public override void OnValidate()
    {
        base.OnValidate();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    /// <summary>
    /// Runs on both Server and Client
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

   

    #endregion

    #region Start & Stop

    /// <summary>
    /// called when quitting the application by closing the window / pressing stop in the editor
    /// </summary>
    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }

    #endregion

    #region Scene Management

    /// <summary>
    /// This causes the server to switch scenes and sets the networkSceneName.
    /// <para>Clients that connect to this server will automatically switch to this scene. This is called automatically if onlineScene or offlineScene are set, but it can be called from user code to switch scenes again while the game is in progress. This automatically sets clients to be not-ready. The clients must call NetworkClient.Ready() again to participate in the new scene.</para>
    /// </summary>
    /// <param name="newSceneName"></param>
    public override void ServerChangeScene(string newSceneName)
    {
        base.ServerChangeScene(newSceneName);
    }

    /// <summary>
    /// Called from ServerChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows server to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    public override void OnServerChangeScene(string newSceneName) 
    {
        Debug.Log($"serverchange scene... {newSceneName}");
    }

    /// <summary>
    /// Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().
    /// </summary>
    /// <param name="sceneName">The name of the new scene.</param>
    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.Contains("lobby"))
        {
            if (isTransitioningFromGameScene)
            {
                ServerSceneChangedFromGameToLobby();
                isTransitioningFromGameScene = false;
            }
        }
        else if (sceneName.Contains("game"))
            ServerSceneChangedToGame();
    }

    private void ServerSceneChangedFromGameToLobby()
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            var oldGameObject = conn.identity.gameObject;
            var playerLobby = Instantiate(playerPrefab);
            NetworkServer.ReplacePlayerForConnection(conn, playerLobby);
            NetworkServer.Destroy(oldGameObject);
        }
    }

    /// <summary>
    /// PlayerLobby will be replaced with PlayerGame first, then fire GameStateChanged (START)
    /// </summary>
    private async void ServerSceneChangedToGame()
    {
        ServerContext.GetInstance().UnsubAndSub();

        PlayerManager.GetInstance().ReplacePlayers(PlayerPrefab.GAME);

        var conns = NetworkServer.connections.Values;

        while(conns.Any(x => x == null || !x.isReady))
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"waiting conns to be ready...");
            await Task.Delay(1000);
        }

        var gameStateEvent = GameStateEvent.GetInstance();
        gameStateEvent.eventGameStateChanged += OnGameStateChanged;
        gameStateEvent.eventGameStateChanged?.Invoke(this, new EventGameStateChanged(GameState.START));
    }

    /// <summary>
    /// Called from ClientChangeScene immediately before SceneManager.LoadSceneAsync is executed
    /// <para>This allows client to do work / cleanup / prep before the scene changes.</para>
    /// </summary>
    /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
    /// <param name="sceneOperation">Scene operation that's about to happen</param>
    /// <param name="customHandling">true to indicate that scene loading will be handled through overrides</param>
    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling) 
    {
        var oldSceneName = SceneManager.GetActiveScene().name;
        ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"oldSceneName={oldSceneName}");
        ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"newSceneName={newSceneName}");

        var uiTransition = UITransition.GetInstance();

        if (newSceneName.Contains("lobby")) // offline/game -> lobby
        {
            if (oldSceneName.Contains("lobby")) // offline -> lobby (oldSceneName value is 'lobby', despite actually being 'offline')
                return;
        }
        else if (newSceneName.Contains("game")) // lobby -> game
            uiTransition.PlayLoadingAnimation(true);
    }

    /// <summary>
    /// Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
    /// <para>Scene changes can cause player objects to be destroyed. The default implementation of OnClientSceneChanged in the NetworkManager is to add a player object for the connection if no player object exists.</para>
    /// </summary>
    /// <param name="conn">The network connection that the scene change message arrived on.</param>
    public async override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);

        var newSceneName = SceneManager.GetActiveScene().name;

        ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"newSceneName={newSceneName}");

        var uiTransition = UITransition.GetInstance();

        if (newSceneName.Contains("lobby")) // lobby
        {
            while (GameObject.FindObjectsOfType<PlayerGame>().Any())
            {
                ClientLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Waiting for all PlayerGame gameObjects to be destroyed...");
                await Task.Delay(100); // wait until player game is complete gone
                continue;
            }
        }
        else if (newSceneName.Contains("game")) // lobby -> game
        {
            
        }

        uiTransition.StopAllBoolAnimations(); // regardless of what screen, stopBoolAnimations

    }

    #endregion

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a new client connects.
    /// <para>Unity calls this on the Server when a Client connects to the Server. Use an override to tell the NetworkManager what to do when a client connects to the server.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerConnect(NetworkConnection conn) { }

    /// <summary>
    /// Called on the server when a client is ready.
    /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
    }

    /// <summary>
    /// Called on the server when a client adds a new player with ClientScene.AddPlayer.
    /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
    }

    /// <summary>
    /// Called on the server when a client disconnects.
    /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
    /// </summary>
    /// <param name="connId">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        if (NetworkServer.connections.Count < 3)
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"Insufficient players ({NetworkServer.connections.Count}). Moving players back to lobby");
            MovePlayersBackToLobby();
        }
    }

    #endregion

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        StartCoroutine(OnClientConnectTwo(conn));
    }

    private IEnumerator OnClientConnectTwo(NetworkConnection conn)
    {
        yield return new WaitUntil(() => conn.isReady);

        NetworkClient.AddPlayer();
    }

    /// <summary>
    /// Called on clients when disconnected from a server.
    /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }

    /// <summary>
    /// Called on clients when a servers tells the client it is no longer ready.
    /// <para>This is commonly used when switching scenes.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientNotReady(NetworkConnection conn) { }

    #endregion

    #region Start & Stop Callbacks

    // Since there are multiple versions of StartServer, StartClient and StartHost, to reliably customize
    // their functionality, users would need override all the versions. Instead these callbacks are invoked
    // from all versions, so users only need to implement this one case.

    /// <summary>
    /// This is invoked when a host is started.
    /// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartHost() { }

    /// <summary>
    /// This is invoked when a server is started - including when a host is started.
    /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
    /// <para>Initialize non-MonoBehaviour classes here </para>
    /// </summary>
    public override void OnStartServer()
    {
        instance = this;

        ServerContext serverContext = ServerContext.GetInstance();
        serverContext.AddBean(this);
        serverContext.AddBean(RNG.GetInstance());
        serverContext.AddBean(PlayerManager.GetInstance());
        serverContext.AddBean(LobbySettings.GetInstance());
        serverContext.AddBean(GridManager.GetInstance());
        serverContext.AddBean(Avalanche.GetInstance());
        serverContext.AddBean(DayTimeManager.GetInstance());

        serverContext.AddBean(AvalancheEvents.GetInstance());
        serverContext.AddBean(GameStateEvent.GetInstance());
        serverContext.AddBean(GoalEvents.GetInstance());
        serverContext.AddBean(HazardEvents.GetInstance());
        serverContext.AddBean(HostEvents.GetInstance());
        serverContext.AddBean(MiniGameEvents.GetInstance());
        serverContext.AddBean(CampEvents.GetInstance());
        serverContext.AddBean(DayTimeEvents.GetInstance());
        serverContext.AddBean(FlagEvents.GetInstance());
    }

    private async void OnGameStateChanged(object sender, EventGameStateChanged e)
    {
        if (e.GameState == GameState.WIN)
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"WIN - Auto Redirecting players back to lobby in 5 seconds");
            await Task.Delay(5000);
            MovePlayersBackToLobby();
        }
        else if (e.GameState == GameState.LOSE)
        {
            ServerLog.Log(this, MethodBase.GetCurrentMethod().Name, $"LOSE - Auto Redirecting players back to lobby in 5 seconds");
            await Task.Delay(6000);
            MovePlayersBackToLobby();
        }
    }

    /// <summary>
    /// <para>call PreStartGame() if you want to include countdowns</para>
    /// </summary>
    public void StartGame() => ServerChangeScene(gameScene);

    /// <summary>
    /// <para>Safely move players back to lobby</para>
    /// </summary>
    private void MovePlayersBackToLobby()
    {
        isTransitioningFromGameScene = true;
        ServerChangeScene(onlineScene);

    }

    /// <summary>
    /// This is invoked when the client is started.
    /// </summary>
    public override void OnStartClient() { }

    /// <summary>
    /// This is called when a host is stopped.
    /// </summary>
    public override void OnStopHost() { }

    /// <summary>
    /// This is called when a server is stopped - including when a host is stopped.
    /// </summary>
    public override void OnStopServer() 
    {
        ServerContext.GetInstance().KillAsyncFunctions();
        ServerContext.GetInstance().Unsub();
    }

    /// <summary>
    /// This is called when a client is stopped.
    /// </summary>
    public override void OnStopClient() { }

    #endregion

    /// <summary>
    /// throws an exception if the spawnPrefab's name is not found
    /// </summary>
    public GameObject findSpawnPrefabByName(string name)
    {
        if (name == null || name.Length == 0)
            throw new Exception($"newPrefab name arg is null or empty!");

        GameObject newPrefab = spawnPrefabs.Find(x => x.transform.name == name);

        if (newPrefab == null)
            throw new Exception($"newPrefab {name} not found. Check your spelling and make sure prefab is found under NetworkManager's spawn prefabs");

        return newPrefab;
    }

}
