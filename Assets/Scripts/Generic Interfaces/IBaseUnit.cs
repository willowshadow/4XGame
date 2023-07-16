using UnityEngine;

namespace Generic_Interfaces
{
    public interface IBaseUnit
    {
        public string unitName { get; set; }
        public int hp { get; set; }
        public int maxHp { get; set; }
        
        public int armor { get; set; }
        
        public int maxArmor { get; set; }
        
        public int shield { get; set; }
        
        public int maxShield { get; set; }
        
        public Sprite sprite { get; set; }
    }
}