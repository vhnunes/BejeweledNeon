using System.Collections.Generic;
using UnityEngine;

namespace BJW
{
    public class Board
    {
        #region Variables

        private int _rowSize, _collumSize;

        private GemData[] _gemsDataAvaliable;
        private Gem[] _gemsInGame;

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

        #region Gems Control

        private void InitializeGems()
        {
            for (int i = 0; i < _gemsInGame.Length; i++)
            {
                var sortGemData = _gemsDataAvaliable[Random.Range(0, _gemsDataAvaliable.Length)];
                
                _gemsInGame[i] = new Gem(sortGemData);    // TODO: Randomize gem data to initialize
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
                    gem.SetPosition(position);
                    gemIndex++;
                }
            }
        }

        #endregion

        #endregion
        
    }
}