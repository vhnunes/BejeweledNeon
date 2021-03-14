using System;
using BJW;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Components

    [SerializeField] private BoardManager _boardManager = new BoardManager();
    private GemControl _gemControl = new GemControl();

    #endregion

    #region MonoBehaviour

    private void OnDrawGizmos()
    {
        _boardManager.OnGizmos();
    }

    private void Start()
    {
        _boardManager.OnStart();
    }

    #endregion
}