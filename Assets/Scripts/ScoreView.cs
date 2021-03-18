using System;
using UnityEngine;
using UnityEngine.UI;

namespace BJW
{
    [RequireComponent(typeof(Text))]
    public class ScoreView : MonoBehaviour
    {
        #region Variables

        private const string _textFormat = "000000";
        private enum ViewType
        {
            Score, HighScore
        }
        
        [SerializeField] private ViewType _viewType;
        
        #endregion

        #region Components

        private Text _myText => this.gameObject.GetComponent<Text>();
        
        private GameManager _gm;
        private ScoreManager _scoreManager;

        #endregion
        
        #region MonoBehaviour

        private void Start()
        {
            Initialize();
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            _gm = GameManager.instance;
            if (_gm == null)
            {
                Debug.LogError($"Cannot made score view of {this.gameObject.name} work because there is no game manager.");
                return;
            }
            _scoreManager = _gm.scoreManager;
            
            SetOnScoreManager();
        }
        private void SetOnScoreManager()
        {
            if (_viewType == ViewType.Score)
            {
                _scoreManager.ReceiveScoreView(this);
            }
            
            else
            {
                _scoreManager.ReceiveHighScoreView(this);
            }
        }

        public void UpdateView()
        {
            if (_viewType == ViewType.Score)
            {
                _myText.text = _scoreManager.currentScore.ToString(_textFormat);
            }

            else
            {
                
            }_myText.text = _scoreManager.highScore.ToString(_textFormat);
        }

        #endregion
    }
}