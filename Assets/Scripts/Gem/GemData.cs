using UnityEngine;

namespace BJW
{
    /// <summary>
    /// Contrains all the data information about a gem, like his visuals, color and type.
    /// </summary>
    [CreateAssetMenu(fileName = "Gem Data", menuName = "BJW/Gem Data", order = 0)]
    public class GemData : ScriptableObject
    {
        #region Variables

        [SerializeField] private GemType _gemType;
        [SerializeField] private GameObject _gemPrefab;
        [SerializeField] private Sprite _gemSprite = null;
        [SerializeField] private Color _gemColor = new Color();
        
        #region Properties

        public GemType gemType => _gemType;
        public GameObject gemPrefab => _gemPrefab;
        public Sprite gemSprite => _gemSprite;
        public Color gemColor => _gemColor;

        #endregion

        #endregion
    }
}