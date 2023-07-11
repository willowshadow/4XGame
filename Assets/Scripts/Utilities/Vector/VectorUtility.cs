using UnityEngine;

namespace Utilities.Vector
{
    public class VectorUtility
    {
        public static Vector3 LerpCurrent(Vector3 value, Vector3 newValue, float multiplier)
        {
            return Vector3.Lerp(value,newValue,multiplier*Time.deltaTime);
        }
    }
}