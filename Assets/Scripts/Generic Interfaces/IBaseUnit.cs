using UnityEngine;

namespace Generic_Interfaces
{
    public interface IBaseUnit
    {
        public string unitName { get; set; }
        public int hp { get; set; }
        public int maxHp { get; set; }
        
        public Sprite sprite { get; set; }
    }
}