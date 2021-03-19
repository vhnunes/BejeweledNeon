using UnityEngine;

namespace BJW
{
    [CreateAssetMenu(fileName = "Gem Collection", menuName = "BJW/Gem Collection", order = 0)]
    public class GemCollectionData : ScriptableObject
    {
        /// <summary>
        /// A collection of gem data is a set of gems to be used in a game, for example: You can have a lot of
        /// collections to make different types of bejeweled games, with different gems, specials and so on.
        /// </summary>
        
        [SerializeField] private GemData[] _gemDatas;
        public GemData[] gemDatas => _gemDatas;
    }
}