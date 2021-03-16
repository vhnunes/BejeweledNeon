using System.Collections;
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
        
        public void SwitchGems(Gem firstGem, Gem secondGem)
        {
            var firstGemPosition = firstGem.boardPosition;
            var secondGemPosition = secondGem.boardPosition;
            
            firstGem.SetBoardPosition(secondGemPosition);
            secondGem.SetBoardPosition(firstGemPosition);
        }
        public void OnSwitchGems(Gem firstGem, Gem secondGem)
        {
            ChangeBoardState(BoardState.Waiting);
            
            var firstGemMatch = MatchInGem(firstGem);
            var secondGemMatch = MatchInGem(secondGem);

            var isSwitchLegal = firstGemMatch.IsMatch() || secondGemMatch.IsMatch();
            
            // TODO: Better way to use start coroutine without need for singleton? Can have a refence in this class...
            GameManager.instance.StartCoroutine(isSwitchLegal
                ? OnLegalGemSwitchRoutine(1f, firstGemMatch, secondGemMatch)
                : OnIlegalSwitchRoutine(1f, firstGem, secondGem));
        }

        private IEnumerator OnLegalGemSwitchRoutine(float timeToWait, GemMatch firstGem, GemMatch secondGem)
        {
            // TODO: On match occures, check all board for other matchs ocurring to make them.
            
            // Pre Match
            

            yield return new WaitForSeconds(timeToWait);
            // On Match
            DoMatch(firstGem);
            DoMatch(secondGem);
            
            yield return new WaitForSeconds(timeToWait);
            // After Match
            
            ChangeBoardState(BoardState.Playing);

        }
        private IEnumerator OnIlegalSwitchRoutine(float timeToWait, Gem firstGem, Gem secondGem)
        {
            yield return new WaitForSeconds(timeToWait);
            SwitchGems(firstGem, secondGem);
            yield return new WaitForSeconds(timeToWait);
            ChangeBoardState(BoardState.Playing);
        }

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
        private void FallAllGemsFromPosition(Vector2 boardPosition, int amount = 1)
        {
            // All gems from this position to up will fall

            List<Gem> gemsToFall = new List<Gem>();
            
            var currentBoardPosition = boardPosition;
            
            while (currentBoardPosition.y < _collumSize)
            {
                var gem = GetGemInPosition(currentBoardPosition);
                if (gem != null) gemsToFall.Add(gem);
                
                currentBoardPosition.y += 1;
            }

            foreach (var gem in gemsToFall)
            {
                gem.SetBoardPosition(gem.boardPosition + Vector2.down * amount);
            }
        }

        private void SendGemToTop(Gem gem)
        {
            // Send a gem to top of his collum while not replacing others.
            var topPosition = new Vector2(gem.boardPosition.x, _collumSize - 1);
            var gemOnTop = GetGemInPosition(topPosition);
            
            while (gemOnTop != null)
            {
                topPosition.y -= 1;
                gemOnTop = GetGemInPosition(topPosition);
            }
            
            gem.SetBoardPosition(topPosition);
        }
        private Gem GetGemInPosition(Vector2 boardPosition)
        {
            foreach (var gem in _gemsInGame)
            {
                if (gem.boardPosition == boardPosition)
                    return gem;
            }
            
            return null;
        }

        #endregion

        #region Match
        
        private void DoMatch(GemMatch match)
        {
            bool isMatchLegal = match.IsMatch();

            if (isMatchLegal)
            {
                foreach (var hGem in match.gems)
                {
                    hGem.OnMatchEnd();
                    FallAllGemsFromPosition(hGem.boardPosition + Vector2.up);
                    SendGemToTop(hGem);
                }
            }
        }
        private GemMatch MatchInGem(Gem gem)
        {
            GemMatch horizontalMatch = HorizontalMatchOfGem(gem);
            horizontalMatch.AddGem(gem);
            
            GemMatch verticalMatch = VerticalMatchOfGem(gem);
            verticalMatch.AddGem(gem);

            GemMatch definitiveMatch = new GemMatch();

            if (horizontalMatch.IsMatch())
            {
                foreach (var _gem in horizontalMatch.gems)
                {
                    definitiveMatch.AddGem(_gem);
                }
            }

            if (verticalMatch.IsMatch())
            {
                foreach (var _gem in verticalMatch.gems)
                {
                    definitiveMatch.AddGem(_gem);
                }
            }

            return definitiveMatch;
        }
        private GemMatch HorizontalMatchOfGem(Gem gem)
        {
            GemMatch match = new GemMatch();
            
            Vector2 nextGemPosition;
            Gem nextGem = null;

            // Right
            nextGemPosition = gem.boardPosition + Vector2.right;
            while (CanCheckPosition(nextGemPosition))
            {
                nextGem = GetGemInPosition(nextGemPosition);
                
                if (nextGem.IsCompatibleWith(gem))
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
                
                if (nextGem.IsCompatibleWith(gem))
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

                if (nextGem.IsCompatibleWith(gem))
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

                if (nextGem.IsCompatibleWith(gem))
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

        #endregion
        
    }
}