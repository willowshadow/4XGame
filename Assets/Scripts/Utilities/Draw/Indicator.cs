using DG.Tweening;
using UnityEngine;

namespace Utilities.Draw
{
    public class Indicator : MonoBehaviour
    {
        public Vector3 scale;

        private Tween _currentTween;
        
        public void Activate()
        {
            if (_currentTween != null)
            {
                _currentTween.Kill();
                _currentTween = null;
            }
            _currentTween=transform.DOScale(scale, 0.3f).OnComplete((() =>
            {
                
                _currentTween = null;
                
                _currentTween=transform.DOPunchScale(Vector3.one / 3, 0.45f);
            }));
        }
        
        public void Deactivate()
        {
            if (_currentTween != null)
            {
                _currentTween.Kill();
                _currentTween = null;
            }
            _currentTween=transform.DOScale(0, 0.3f);
        }
    }
}