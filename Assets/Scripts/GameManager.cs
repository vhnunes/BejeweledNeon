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

    #region Game Variables / Data
    
    [Header("Game Datas")]
    [SerializeField] private GemCollectionData _gameGemCollection = null;
    
    [Header("Gem")]
    [SerializeField] private float _gemMoveSpeed = 10f;
    [SerializeField] private float _gemMatchAnimTime = 0.5f;
    [SerializeField] private float _gemSwitchTime = 0.5f;
    
    [Header("Board")]
    [SerializeField] private float _boardDelayAfterMatch = 1f;
    
    #region Properties

    // Game Data
    public GemCollectionData gemeGemCollection => _gameGemCollection;
    
    // Gem Variables
    public float gemMoveSpeed => _gemMoveSpeed;
    public float gemMatchAnimTime => _gemMatchAnimTime;
    public float gemSwitchTime => _gemSwitchTime;
    
    // Board Variables
    public float boardDelayAfterMatch => _boardDelayAfterMatch;

    #endregion
    
    #endregion
    
    #region Game Classes

    [Header("Game Classes")]
    [SerializeField] private Board _board = new Board();
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
        _board.OnDrawGizmos();
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        InitializeBoard();
        InitializeScoreManager();
        InitializeUIManager();
        InitializeGemControl();
        
        InvokeRepeating(nameof(InvokeUpdate), .1f, .1f);
    }

    private void InvokeUpdate()
    {
        _board.OnUpdate();
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
    private void InitializeBoard()
    {
        _board.OnStart();
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
        _gemControl = new GemControl(_board);
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