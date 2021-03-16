using System;
using BJW;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    #region Components

    [SerializeField] private BoardManager _boardManager = new BoardManager();
    
    private GemControl _gemControl = null;

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