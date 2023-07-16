using DG.Tweening;
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
        [BoxGroup("Display Unit Information")]public TextMeshProUGUI unitName;
        [BoxGroup("Display Unit Information")]public Image unitImage;
        
        [BoxGroup("Display Unit Information")]public TextMeshProUGUI unitHp;
        [BoxGroup("Display Unit Information")]public Slider unitHpBar;
        
        [BoxGroup("Display Unit Information")]public TextMeshProUGUI unitArmor;
        [BoxGroup("Display Unit Information")]public Slider unitArmorBar;
        
        [BoxGroup("Display Unit Information")]public TextMeshProUGUI unitShield;
        [BoxGroup("Display Unit Information")]public Slider unitShieldBar;
    
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
            unitName.SetText(_selectedUnit.unitName);
            unitImage.sprite = _selectedUnit.sprite;
            
            unitHp.SetText(_selectedUnit.hp+"/"+_selectedUnit.maxHp);
            unitHpBar.maxValue = _selectedUnit.maxHp;
            unitHpBar.value = 0;
            unitHpBar.DOValue(_selectedUnit.hp, 1f);
            
            unitArmor.SetText(_selectedUnit.armor+"/"+_selectedUnit.maxArmor);
            unitArmorBar.maxValue = _selectedUnit.maxArmor;
            unitArmorBar.value = 0;
            unitArmorBar.DOValue(_selectedUnit.armor, 1f);
            
            unitShield.SetText(_selectedUnit.shield+"/"+_selectedUnit.maxShield);
            unitShieldBar.maxValue = _selectedUnit.maxShield;
            unitShieldBar.value = 0;
            unitShieldBar.DOValue(_selectedUnit.shield, 1f);
            
            
        }
        
    }
}

