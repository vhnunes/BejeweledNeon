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
        private List<GemMatch> _gemMatches = new List<GemMatch>();

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

        private Gem GetGemInPosition(Vector2 boardPosition)
        {
            foreach (var gem in _gemsInGame)
            {
                if (gem.boardPosition == boardPosition)
                    return gem;
            }
            
            // If erro
            Debug.LogError($"Cannot find gem in board on {boardPosition}");
            return null;
        }

        #endregion
        
        public void CheckForMatchsInGem(Gem gem)
        {
            GemMatch horizontalMatch = HorizontalMatcsOfGem(gem);
            horizontalMatch.AddGem(gem);
            
            GemMatch verticalMatch = VerticalMatchOfGem(gem);
            verticalMatch.AddGem(gem);

            if (horizontalMatch.IsMatch())
            {
                Debug.Log("Horizontal MATCH!!!");
                foreach (var _gem in horizontalMatch.gems)
                {
                    _gem.OnMatch();
                }
            }

            if (verticalMatch.IsMatch())
            {
                Debug.Log("Vertical MATCH!!!");
                
                foreach (var _gem in horizontalMatch.gems)
                {
                    _gem.OnMatch();
                }
            }
        }
        private GemMatch HorizontalMatcsOfGem(Gem gem)
        {
            GemMatch match = new GemMatch();
            
            Vector2 nextGemPosition;
            Gem nextGem = null;

            // Right
            nextGemPosition = gem.boardPosition + Vector2.right;
            while (CanCheckPosition(nextGemPosition))
            {
                nextGem = GetGemInPosition(nextGemPosition);

                if (nextGem.gemColor == gem.gemColor)
                {
                    match.AddGem(nextGem);
                    nextGemPosition += Vector2.right;
                }
                
                else
                    break;
            }
            
            // Left
            nextGemPosition = gem.boardPosition + Vector2.left;
            while (CanCheckPosition(nextGemPosition))
            {
                nextGem = GetGemInPosition(nextGemPosition);

                if (nextGem.gemColor == gem.gemColor)
                {
                    match.AddGem(nextGem);
                    nextGemPosition += Vector2.left;
                }

                else
                    break;
            }
            
            return match;
        }
        private GemMatch VerticalMatchOfGem(Gem gem)
        {
            GemMatch match = new GemMatch();

            Vector2 nextGemPosition;
            Gem nextGem = null;

            // Up
            nextGemPosition = gem.boardPosition + Vector2.up;
            while (CanCheckPosition(nextGemPosition))
            {
                nextGem = GetGemInPosition(nextGemPosition);

                if (nextGem.gemColor == gem.gemColor)
                {
                    match.AddGem(nextGem);
                    nextGemPosition += Vector2.up;
                }

                else
                    break;
            }
            
            // Down
            nextGemPosition = gem.boardPosition + Vector2.down;
            while (CanCheckPosition(nextGemPosition))
            {
                nextGem = GetGemInPosition(nextGemPosition);

                if (nextGem.gemColor == gem.gemColor)
                {
                    match.AddGem(nextGem);
                    nextGemPosition += Vector2.down;
                }

                else
                    break;
            }
            
            return match;
        }
        
        private bool CanCheckPosition(Vector2 boardPosition)
        {
            if (boardPosition.x >= _rowSize)
                return false;
            
            else if (boardPosition.x < 0)
                return false;
            
            else if (boardPosition.y < 0)
                return false;
            
            else if (boardPosition.y >= _collumSize)
                return false;
                
            return true;
        }

        #endregion
        
    }
}