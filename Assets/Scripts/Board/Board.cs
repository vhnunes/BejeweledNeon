using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BJW
{
    public class Board
    {
        // TODO: IMPROVE GAME MANAGER REFERENCE TO CALL ROUTINES
        
        #region Variables

        private bool _canStartWitchMatches = false;
        
        private BoardState _boardState = BoardState.Playing;
        private int _rowSize, _collumSize;

        private GemData[] _gemsDataAvaliable;
        private Gem[] _gemsInGame;
        private List<GemMatch> _gemMatches = new List<GemMatch>();
        
        private float _gemMatchAnimTime = 0.5f;
        private float _gemSwitchTime = 0.5f;
        private float _boardAfterMatchDelay = 1f;

        #region Properties

        public BoardState boardState => _boardState;
        public Gem[] gemsInGame => _gemsInGame;

        #endregion

        #endregion

        #region Components

        private GameManager _gameManager = null;

        #endregion

        #region Methods
        
        // Constructor
        public Board(int rowSize, int collumSize, GemData[] gemsDataToUseInGame, bool canStartWithMatches = false)
        {
            _canStartWitchMatches = canStartWithMatches;
            
            _rowSize = rowSize;
            _collumSize = collumSize;

            _gemsDataAvaliable = gemsDataToUseInGame;
            _gemsInGame = new Gem[rowSize * collumSize];
        }
        public void ChangeBoardState(BoardState newState)
        {
            _boardState = newState;
        }

        public void OnStart()
        {
            _gameManager = GameManager.instance;
            _gameManager.OnGameRestart += () => ChangeBoardState(BoardState.Playing);
            _gameManager.OnGameRestart += ResetAllGems;
            InitializeGems();
            PlaceGemsOnBoard();
            TryGameOver();
        }
        
        public void SetTimings(float matchAnimTime, float switchTime, float boardMatchDelay)
        {
            _gemMatchAnimTime = matchAnimTime;
            _gemSwitchTime = switchTime;
            _boardAfterMatchDelay = boardMatchDelay;
        }

        private bool TryGameOver()
        {
            var movementsLeft = PossibleMovementsToMakeMatch();
            if (movementsLeft == 0)
            {
                ChangeBoardState(BoardState.Waiting);
                
                GameManager.instance.GameOver();
                
                return true;
            }
                
            
            return false;
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
            
            var firstGemMatch = MatchInGem(firstGem, firstGem.boardPosition);
            var secondGemMatch = MatchInGem(secondGem, secondGem.boardPosition);

            var isSwitchLegal = firstGemMatch.IsMatch() || secondGemMatch.IsMatch();
            
            // TODO: Better way to use start coroutine without need for singleton? Can have a refence in this class...
            GameManager.instance.StartCoroutine(isSwitchLegal
                ? OnLegalGemSwitchRoutine(firstGemMatch, secondGemMatch)
                : OnIlegalSwitchRoutine(firstGem, secondGem));
        }

        public void SetAllGemsSpeeds(float speed)
        {
            foreach (var gem in gemsInGame)
            {
                gem.SetViewSpeed(speed);
            }
        }

        private IEnumerator OnLegalGemSwitchRoutine(GemMatch firstGemMatch, GemMatch secondGemMatch)
        {
            GemMatch allGemsMatch = new GemMatch();
            foreach (var gem in firstGemMatch.gems)
            {
                allGemsMatch.AddGem(gem);
            }
            foreach (var gem in secondGemMatch.gems)
            {
                allGemsMatch.AddGem(gem);
            }

            var mainRoutine = GameManager.instance.StartCoroutine(MatchGemRoutine(allGemsMatch));
            yield return mainRoutine;
            yield return new WaitForSeconds(_boardAfterMatchDelay);
            
            // Do all other matches that can have in board.
            var boardMatches = GetAllMatchsInBoard();
            while (boardMatches.Length > 0)
            {
                // Do first other match im board.
                var firstOtherMatch = boardMatches[0];
                var otherFirstRoutine =  GameManager.instance.StartCoroutine(MatchGemRoutine(firstOtherMatch));
                
                // Start all other matches at the same time if have.
                if (boardMatches.Length > 1)
                {
                    for (int i = 1; i < boardMatches.Length; i++)
                    {
                        var match = boardMatches[i];
                        GameManager.instance.StartCoroutine(MatchGemRoutine(match));
                    }
                }

                // Wait for any match to stop, (all matches have the same time)
                yield return otherFirstRoutine;
                yield return new WaitForSeconds(_boardAfterMatchDelay);
                boardMatches = GetAllMatchsInBoard();
            }
            
            ChangeBoardState(BoardState.Playing);
            TryGameOver();

        }
        private IEnumerator OnIlegalSwitchRoutine(Gem firstGem, Gem secondGem)
        {
            var halfSwitchTime = _gemSwitchTime * 0.5f;
            
            yield return new WaitForSeconds(halfSwitchTime);
            SwitchGems(firstGem, secondGem);
            yield return new WaitForSeconds(halfSwitchTime);
            ChangeBoardState(BoardState.Playing);
        }

        private void InitializeGems()
        {
            for (int i = 0; i < _gemsInGame.Length; i++)
            {
                var sortGemData = GetRandomGemData();
                _gemsInGame[i] = new Gem(sortGemData);
            }
        }

        private void ResetAllGems()
        {
            foreach (var gem in gemsInGame)
            {
                gem.TransformIntoNewGem(GetRandomGemData());
            }

            if (!_canStartWitchMatches)
            {
                while (GetAllMatchsInBoard().Length > 0)
                {
                    foreach (var gem in gemsInGame)
                    {
                        gem.TransformIntoNewGem(GetRandomGemData());
                    }
                }
            }

            TryGameOver();
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

            if (!_canStartWitchMatches)
            {
                var matches = GetAllMatchsInBoard();
                while (matches.Length > 0)
                {
                    var firstGemOnMatch = matches[0].gems[0];
                
                    SetGemToAnother(firstGemOnMatch, true);
                    matches = GetAllMatchsInBoard();
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
        
        private void SetGemToAnother(Gem gem, GemData newGemData)
        {
            gem.TransformIntoNewGem(newGemData);
        }
        private void SetGemToAnother(Gem gem, bool randomNewData)
        {
            var sortGemData = GetRandomGemData();
            gem.TransformIntoNewGem(sortGemData);
        }

        private GemData GetRandomGemData()
        {
            return _gemsDataAvaliable[Random.Range(0, _gemsDataAvaliable.Length)];
        }
        
        private void SendGemToTop(Gem gem)
        {
            // Send a gem to top of his collum while not replacing others.
            var topPosition = new Vector2(gem.boardPosition.x, _collumSize - 1);
            var gemOnTop = GetGemInPosition(topPosition);
            
            while (gemOnTop != null && gemOnTop != gem)
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

        public int PossibleMovementsToMakeMatch()
        {
            int result = 0;
            foreach (var gem in gemsInGame)
            {
                if (CanMakeAnyMatch(gem))
                    result++;
            }
            
            Debug.Log($"Possible match movements in game: {result}");
            return result;
        }
        private bool CanMakeAnyMatch(Gem gem)
        {
            // Check if this gem can make any match in the baord.

            // Check in all positions in each diretion for possible matches if switch to this position.

            // Check Right
            var canMakeAtRight = MatchInGem(gem, gem.boardPosition + Vector2.right, false).IsMatch();
            if (canMakeAtRight) return true;

            // Check Left
            var canMakeAtLeft = MatchInGem(gem, gem.boardPosition - Vector2.right, false).IsMatch();;
            if (canMakeAtLeft) return true;
            
            // Check Up
            var canMakeAtUp = MatchInGem(gem, gem.boardPosition + Vector2.up, false).IsMatch();;
            if (canMakeAtUp) return true;

            // Check Down
            var canMakeAtDown = MatchInGem(gem, gem.boardPosition - Vector2.up, false).IsMatch();;
            if (canMakeAtDown) return true;

            return false;
        }

        private void DoMatch(GemMatch match)
        {
            bool isMatchLegal = match.IsMatch();
            if (!isMatchLegal) return;
            
            foreach (var hGem in match.gems)
            {
                hGem.OnMatchEnd();
                hGem.TransformIntoNewGem(GetRandomGemData());
                FallAllGemsFromPosition(hGem.boardPosition + Vector2.up);
                SendGemToTop(hGem);
            }
        }
        private IEnumerator MatchGemRoutine(GemMatch match)
        {
            // Pre Match
            foreach (var gem in match.gems)
            {
                gem.OnMatchStart();
            }
            yield return new WaitForSeconds(_gemMatchAnimTime);
            
            // On Match
            DoMatch(match);
        }
        private GemMatch[] GetAllMatchsInBoard()
        {
            // The ideia is to star checkin in first position of board, after
            // i will ignore in next check the 2 next horizontal an 2 next vertical positions from this
            // checked position. Simply because a match always will require at least 3 equals pieces in his axis,
            // dont needto check for all pieces in board.

            List<GemMatch> boardMatchs = new List<GemMatch>();

            // TODO: Isolate that on board initialization
            Vector2[] positionsInBoard = new Vector2[_rowSize * _collumSize];
            int positionIndex = 0;
            for (int row = 0; row < _rowSize; row++)
            {
                for (int collum = 0; collum < _collumSize; collum++)
                {
                    var position = new Vector2(row, collum);
                    positionsInBoard[positionIndex] = position;
                    positionIndex++;
                }
            }

            List<Vector2> positionsToCheck = new List<Vector2>();
            List<Vector2> positionsToNotCheck = new List<Vector2>();
            
            var lastPositionCheck = new Vector2();
            positionsToCheck.Add(lastPositionCheck);
            positionsToNotCheck.Add(lastPositionCheck);
            
            while (lastPositionCheck.y < _collumSize)
            {
                // Ad positions to not check based on last position.
                for (int x = 1; x < 3; x++)
                {
                    var position = lastPositionCheck + new Vector2(x, 0);
                    if (position.x >= _rowSize)
                        break;
                        
                    if (!positionsToNotCheck.Contains(position))
                        positionsToNotCheck.Add(position);
                }
                
                for (int y = 1; y < 3; y++)
                {
                    var position = lastPositionCheck + new Vector2(0, y);
                    if (position.y >= _collumSize)
                        break;
                    
                    if (!positionsToNotCheck.Contains(position))
                        positionsToNotCheck.Add(position);
                }
                
                // Ad next position to check and set it as last
                // if next position is a position to uncheck, go to next
                var nextPosition = lastPositionCheck;
                while (positionsToNotCheck.Contains(nextPosition))
                {
                    bool brk = false;
                    
                    while (nextPosition.x < _rowSize-1)
                    {
                        nextPosition.x += 1;
                        if (!positionsToNotCheck.Contains(nextPosition))
                        {
                            brk = true;
                            break;
                        }
                    }
                    
                    // If while pass without break.
                    if (!brk)
                    {
                        nextPosition.x = 0;
                        nextPosition.y += 1;
                    }
                        
                }
                
                positionsToCheck.Add(nextPosition);
                positionsToNotCheck.Add(nextPosition);
                lastPositionCheck = nextPosition;
            }
            positionsToCheck.Remove(positionsToCheck[positionsToCheck.Count - 1]);

            // Check for match in all positions to check
            foreach (var position in positionsToCheck)
            {
                var gem = GetGemInPosition(position);
                var match = MatchInGem(gem, gem.boardPosition);
                if (match.IsMatch())
                    boardMatchs.Add(match);
            }
            
            return boardMatchs.ToArray();
        }
        private GemMatch MatchInGem(Gem gem, Vector2 gemPosition, bool canCheckSelf = true)
        {
            GemMatch horizontalMatch = HorizontalMatchOfGem(gem, gemPosition, canCheckSelf);
            horizontalMatch.AddGem(gem);
            
            GemMatch verticalMatch = VerticalMatchOfGem(gem, gemPosition, canCheckSelf);
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
        private GemMatch HorizontalMatchOfGem(Gem gem, Vector2 gemPosition, bool canCheckSelf = true)
        {
            GemMatch match = new GemMatch();
            
            Vector2 nextGemPosition;
            Gem nextGem = null;

            // Right
            nextGemPosition = gemPosition + Vector2.right;
            while (CanCheckPosition(nextGemPosition))
            {
                nextGem = GetGemInPosition(nextGemPosition);
                bool isCompatible = nextGem.IsCompatibleWith(gem);
                if (!canCheckSelf && nextGem == gem)
                    isCompatible = false;
                
                if (isCompatible)
                {
                    match.AddGem(nextGem);
                    nextGemPosition += Vector2.right;
                }
                
                else
                    break;
            }
            
            // Left
            nextGemPosition = gemPosition + Vector2.left;
            while (CanCheckPosition(nextGemPosition))
            {
                nextGem = GetGemInPosition(nextGemPosition);
                bool isCompatible = nextGem.IsCompatibleWith(gem);
                if (!canCheckSelf && nextGem == gem)
                    isCompatible = false;
                
                if (isCompatible)
                {
                    match.AddGem(nextGem);
                    nextGemPosition += Vector2.left;
                }

                else
                    break;
            }
            
            return match;
        }
        private GemMatch VerticalMatchOfGem(Gem gem, Vector2 gemPosition, bool canCheckSelf = true)
        {
            GemMatch match = new GemMatch();

            Vector2 nextGemPosition;
            Gem nextGem = null;

            // Up
            nextGemPosition = gemPosition + Vector2.up;
            while (CanCheckPosition(nextGemPosition))
            {
                nextGem = GetGemInPosition(nextGemPosition);
                bool isCompatible = nextGem.IsCompatibleWith(gem);
                if (!canCheckSelf && nextGem == gem)
                    isCompatible = false;
                
                if (isCompatible)
                {
                    match.AddGem(nextGem);
                    nextGemPosition += Vector2.up;
                }

                else
                    break;
            }
            
            // Down
            nextGemPosition = gemPosition + Vector2.down;
            while (CanCheckPosition(nextGemPosition))
            {
                nextGem = GetGemInPosition(nextGemPosition);
                bool isCompatible = nextGem.IsCompatibleWith(gem);
                if (!canCheckSelf && nextGem == gem)
                    isCompatible = false;
                
                if (isCompatible)
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