using System;
using BJW;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Events

    public delegate void OnGameOverEvents();
    public OnGameOverEvents OnGameOver;

    public delegate void OnGameRestartEvents();
    public OnGameOverEvents OnGameRestart;

    #endregion
    
    #region Components

    [SerializeField] private BoardManager _boardManager = new BoardManager();
    [SerializeField] private ScoreManager _scoreManager = new ScoreManager();
    [SerializeField] private UIManager _uiManager = new UIManager();
    private GemControl _gemControl = null;

    #region Properties

    public ScoreManager scoreManager => _scoreManager;

    #endregion
    
    #endregion

    #region MonoBehaviour

    private void OnDrawGizmos()
    {
        _boardManager.OnGizmos();
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        InitializeBoardManager();
        InitializeScoreManager();
        InitializeUIManager();
        InitializeGemControl();
        
        InvokeRepeating(nameof(InvokeUpdate), .1f, .1f);
    }

    private void InvokeUpdate()
    {
        _boardManager.OnUpdate();
    }

    #endregion

    #region Methods

    #region Initialization

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            
            if (this.transform.parent != null)  // Fix to singleton work if this obj started inside another one in scene.
                this.transform.parent = null;
            
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this);
        }
    }
    private void InitializeBoardManager()
    {
        _boardManager.OnStart();
    }
    private void InitializeScoreManager()
    {
        scoreManager.OnStart();
    }
    private void InitializeUIManager()
    {
        _uiManager.OnStart();
    }
    private void InitializeGemControl()
    {
        _gemControl = new GemControl(_boardManager.board);
    }

    #endregion

    #region CallBacks
    
    // TODO: Centralize callbacks here?
    
    public void GameOver()
    {
        OnGameOver?.Invoke();
        Invoke(nameof(RestartGame), 5f);
    }
    
    private void RestartGame()
    {
        OnGameRestart?.Invoke();
    }

    #endregion

    #endregion
}