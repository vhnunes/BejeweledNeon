using System;
using BJW;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private int _boardRowSize = 8;
    [SerializeField] private int _boardCollumSize = 8;

    #endregion
    
    #region Components

    [SerializeField] private GemData[] _gemDatasToUseInGame = null; //TODO: Scriptable of gems game collection to use.
    
    private Board _board = null;

    #endregion

    #region MonoBehaviour

    private void OnDrawGizmos()
    {
        DrawBoardGizmosPreview();
    }

    private void Start()
    {
        InitializeBoard();
    }

    #endregion

    #region Methods

    private void InitializeBoard()
    {
        _board = new Board(_boardRowSize, _boardCollumSize, _gemDatasToUseInGame);
    }

    #region Gizmos

    private void DrawBoardGizmosPreview()
    {
        var sizeX = 1;
        var sizeY = 1;
        
        var size = new Vector2(sizeX,sizeY);
        
        for (int i = 0; i < _boardRowSize; i++)
        {
            for (int j = 0; j < _boardCollumSize; j++)
            {
                var position = new Vector3(i*size.x, j*size.y, 0);
                
                Gizmos.DrawWireCube(position, size);
            }
        }
    }

    #endregion

    #endregion
}
