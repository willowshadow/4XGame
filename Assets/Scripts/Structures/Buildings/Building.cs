using System.Collections;
using UnityEngine;

namespace Assets._Scripts.Buildings
{
    public class Building : MonoBehaviour
    {
        public BuildingType BuildingType { get; private set; }

    }
}
public enum BuildingType
{
    Defence,
    UnitProduction,
    Resource,
    Command
}