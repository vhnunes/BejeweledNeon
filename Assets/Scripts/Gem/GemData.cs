using UnityEngine;

namespace BJW
{
    [CreateAssetMenu(fileName = "Gem Data", menuName = "BJW/Gem Data", order = 0)]
    public class GemData : ScriptableObject
    {
        #region Variables

        [SerializeField] private GemType _gemType;
        [SerializeField] private Sprite _gemSprite = null;
        [SerializeField] private Color _gemColor = new Color();
        
        #region Properties

        public GemType gemType => _gemType;
        public Sprite gemSprite => _gemSprite;
        public Color gemColor => _gemColor;

        #endregion

        #endregion
    }
}