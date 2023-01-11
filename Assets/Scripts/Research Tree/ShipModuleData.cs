using Sirenix.OdinInspector;
using UnityEngine;

namespace Research_Tree
{
    
    public class ShipModuleData : ScriptableObject
    {
        [BoxGroup("Description")] public Sprite moduleIcon;
        [BoxGroup("Description")] public string partName;
        [BoxGroup("Description")] public int weightCost;
        [BoxGroup("Description")] public int energyCost;
        [BoxGroup("Description")] public int price;
    }
}