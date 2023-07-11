using UnityEngine;

namespace Research_Tree.Engine
{
    [CreateAssetMenu(fileName = "EngineModule", menuName = "Data/Create Engine Module")]
    public class EngineModuleData : ShipModuleData
    {
        /// <summary>
        /// Max Movement Speed
        /// </summary>
        public float engineMaxSpeed;
        
        /// <summary>
        /// Max Rotation Speed
        /// </summary>
        public float traverseMaxSpeed;
    }
}