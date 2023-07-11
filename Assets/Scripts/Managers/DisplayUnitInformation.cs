using Generic_Interfaces;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class DisplayUnitInformation : MonoBehaviour
    {
        private IBaseUnit _selectedUnit;

        
        [BoxGroup("Display Unit Information")]public GameObject unitInformationPanel;
        [BoxGroup("Display Unit Information")]public Image unitImage;
        [BoxGroup("Display Unit Information")]public TextMeshProUGUI unitHp;
        [BoxGroup("Display Unit Information")]public Slider unitHpBar;
        [BoxGroup("Display Unit Information")]public TextMeshProUGUI unitName;
    
        public void UpdateSelectedUnit(IBaseUnit newUnit)
        {
            _selectedUnit = newUnit;
            DisplayInformation();
        }

        public void RemoveSelection()
        {
            unitInformationPanel.SetActive(false);
        }

        private void DisplayInformation()
        {
            if (_selectedUnit == null)
            {
                Debug.Log("No unit selected");
                return;
            }

            Debug.Log($"Selected Unit: {_selectedUnit.unitName}");
            Debug.Log($"Selected Unit HP: {_selectedUnit.hp}");
            // Add more logging for other unit properties
            
            unitInformationPanel.SetActive(true);
            
            unitImage.sprite = _selectedUnit.sprite;
            
            unitHp.SetText(_selectedUnit.hp+"/"+_selectedUnit.maxHp);
            unitHpBar.maxValue = _selectedUnit.maxHp;
            unitHpBar.value = _selectedUnit.hp;
            
            unitName.SetText(_selectedUnit.unitName);
            
        }
        
    }
}

