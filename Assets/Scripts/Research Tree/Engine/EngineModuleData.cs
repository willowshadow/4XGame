using UnityEngine;

namespace Research_Tree.Engine
{
    [CreateAssetMenu(fileName = "EngineModule", menuName = "Data/Create Engine Module")]
    public class EngineModuleData : ShipModuleData
    {
        /// <summary>
        /// Max Movement Speed
        /// </summary>
        public float engineHorsePower;
    }
}