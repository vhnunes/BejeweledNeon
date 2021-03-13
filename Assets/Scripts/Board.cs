using System.Collections.Generic;
using UnityEngine;

namespace BJW
{
    public class Board
    {
        #region Variables

        private int _rowSize, _collumSize;

        private Gem[] _gems;
        private Dictionary<Gem, Vector2> _gemsPosition = new Dictionary<Gem, Vector2>();

        #endregion

        #region Methods
        
        // Constructor
        public Board(int rowSize, int collumSize)
        {
            _rowSize = rowSize;
            _collumSize = collumSize;
            _gems = new Gem[rowSize * collumSize];
            
            InitializeGems();
            PlaceGemsOnBoard();
        }

        #region Board Control

        #endregion

        #region Gems Control

        private void InitializeGems()
        {
            for (int i = 0; i < _gems.Length; i++)
            {
                _gems[i] = new Gem();
            }
        }
        private void PlaceGemsOnBoard()
        {
            int gemIndex = 0;
            
            for (int row = 0; row < _rowSize; row++)
            {
                for (int collum = 0; collum < _collumSize; collum++)
                {
                    var gem = _gems[gemIndex];
                    var position = new Vector2(row, collum);
                    
                    SetGemPosition(gem, position);
                    gemIndex++;
                }
            }
        }
        private void SetGemPosition(Gem gem, Vector2 position)
        {
            var alreadyHas = _gemsPosition.ContainsKey(gem);

            if (alreadyHas)
            {
                _gemsPosition.Add(gem, position);
            }

            else
            {
                _gemsPosition[gem] = position;
            }
        }

        #endregion
        

        #endregion
        
    }
}