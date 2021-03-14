using System;
using BJW;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Components

    [SerializeField] private BoardManager _boardManager = new BoardManager();
    
    private GemControl _gemControl = null;

    #endregion

    #region MonoBehaviour

    private void OnDrawGizmos()
    {
        _boardManager.OnGizmos();
    }

    private void Start()
    {
        InitializeBoardManager();
        
        InitializeGemControl();
    }

    #endregion

    #region Methods

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