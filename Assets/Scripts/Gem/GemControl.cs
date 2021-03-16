using System;
using UnityEngine;

namespace BJW
{
    public class GemControl
    {
        #region Components

        private Board _board = null;
        private Gem _firstSelectedGem = null;
        private Gem _secondSelectedGem = null;

        #endregion
        
        #region Methods
        
        // Constructor
        public GemControl(Board gameBoard)
        {
            _board = gameBoard;
            SetControlOnAllGems();
        }

        private void SetControlOnAllGems()
        {
            foreach (var gem in _board.gemsInGame)
            {
                gem.OnClick = () => OnGemClick(gem);
            }
        }
        private void OnGemClick(Gem gem)
        {
            if (!CanInteractWithGem()) return;

            if (_board.boardState == BoardState.Playing)
            {
                _board.ChangeBoardState(BoardState.GemSelected);
                _firstSelectedGem = gem;
                gem.OnSelected();
            }
            
            else if (_board.boardState == BoardState.GemSelected)
            {
                _secondSelectedGem = gem;

                if (CanSwitchGems())
                {
                    _board.SwitchGems(_firstSelectedGem, _secondSelectedGem);
                    _board.OnSwitchGems(_firstSelectedGem, _secondSelectedGem);
                    
                    UnselectAllGems();
                }

                else
                {
                    UnselectAllGems();
                    // TODO: Improve board state changes(?)
                    _board.ChangeBoardState(BoardState.Playing);
                }
                    
            }
        }

        private void UnselectAllGems()
        {
            _firstSelectedGem.OnUnselected();
            _secondSelectedGem.OnUnselected();
            
            _firstSelectedGem = null;
            _secondSelectedGem = null;
        }

        private bool CanSwitchGems()
        {
            var isOnRangeToSwitch = IsOnRangeToSwitch();

            if (isOnRangeToSwitch)
            {
                return true;
            }
                
            
            return false;
        }

        private bool IsOnRangeToSwitch()
        {
            var firstGemPosition = _firstSelectedGem.boardPosition;
            var secondGemPosition = _secondSelectedGem.boardPosition;

            var xDistance = (int) Mathf.Abs(secondGemPosition.x - firstGemPosition.x);
            var yDistance = (int) Math.Abs(secondGemPosition.y - firstGemPosition.y);

            if (xDistance == 1 && yDistance == 0)
            {
                return true;
            }
                
            
            else if (yDistance == 1 && xDistance == 0)
            {
                return true;
            }
            
            return false;
        }
        
        private bool CanInteractWithGem()
        {
            if (_board.boardState == BoardState.Waiting)
                return false;
            
            
            // True if no negation conditions above
            return true;
        }
        

        #endregion
    }
}