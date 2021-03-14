using System.Collections.Generic;
using UnityEngine;

namespace BJW
{
    public class Board
    {
        #region Variables

        private BoardState _boardState = BoardState.Playing;
        private int _rowSize, _collumSize;

        private GemData[] _gemsDataAvaliable;
        private Gem[] _gemsInGame;

        #region Properties

        public BoardState boardState => _boardState;
        public Gem[] gemsInGame => _gemsInGame;

        #endregion

        #endregion

        #region Methods
        
        // Constructor
        public Board(int rowSize, int collumSize, GemData[] gemsDataToUseInGame)
        {
            _rowSize = rowSize;
            _collumSize = collumSize;

            _gemsDataAvaliable = gemsDataToUseInGame;
            _gemsInGame = new Gem[rowSize * collumSize];

            InitializeGems();
            PlaceGemsOnBoard();
        }

        public void ChangeBoardState(BoardState newState)
        {
            _boardState = newState;
        }
        
        #region Gems

        private void InitializeGems()
        {
            for (int i = 0; i < _gemsInGame.Length; i++)
            {
                var sortGemData = _gemsDataAvaliable[Random.Range(0, _gemsDataAvaliable.Length)];
                _gemsInGame[i] = new Gem(sortGemData);
            }
        }
        private void PlaceGemsOnBoard()
        {
            int gemIndex = 0;
            
            for (int row = 0; row < _rowSize; row++)
            {
                for (int collum = 0; collum < _collumSize; collum++)
                {
                    var gem = _gemsInGame[gemIndex];
                    var position = new Vector2(row, collum);
                    gem.SetBoardPosition(position);
                    gemIndex++;
                }
            }
        }

        #endregion

        #endregion
        
    }
}