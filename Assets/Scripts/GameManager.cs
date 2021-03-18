using System;
using BJW;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    #region Components

    [SerializeField] private BoardManager _boardManager = new BoardManager();
    private ScoreManager _scoreManager = new ScoreManager();
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
        
        InitializeGemControl();
    }

    private void Update()
    {
        _boardManager.OnUpdate();
    }

    #endregion

    #region Methods

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

    private void InitializeGemControl()
    {
        _gemControl = new GemControl(_boardManager.board);
    }

    #endregion
}