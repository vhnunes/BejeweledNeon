using UnityEngine;
using UnityEngine.UI;

namespace BJW
{
    [RequireComponent(typeof(Text))]
    public class ScoreView : MonoBehaviour
    {
        #region Variables
        
        private enum ViewType
        {
            Score, HighScore
        }
        
        [SerializeField] private ViewType _viewType;
        
        #endregion

        #region Components

        private Text _myText => this.gameObject.GetComponent<Text>();
        
        private GameManager _gm;
        private Score _score;

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
            _score = _gm.score;
            
            SetOnScoreManager();
        }
        private void SetOnScoreManager()
        {
            if (_viewType == ViewType.Score)
            {
                _score.ReceiveScoreView(this);
            }
            
            else
            {
                _score.ReceiveHighScoreView(this);
            }
        }

        public void UpdateView()
        {
            _myText.text = _viewType == ViewType.Score ? _score.currentScore.ToString() : _score.highScore.ToString();
        }

        #endregion
    }
}