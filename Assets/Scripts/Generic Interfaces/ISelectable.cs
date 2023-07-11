using Generic_Interfaces;

namespace Units
{
    public interface ISelectable
    {
        public IBaseUnit OnSelect();
        public void OnDeselect();

        public void OnAction();
    }
}