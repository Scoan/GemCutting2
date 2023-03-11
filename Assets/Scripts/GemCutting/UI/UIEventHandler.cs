using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GemCutting.UI
{
    public class UIEventHandler : MonoBehaviour
    {
        [SerializeField] private GemTable m_gemTable;

        [SerializeField] private TMP_Dropdown m_dropdown;

        public void Awake()
        {
            if (m_dropdown)
            {
                m_dropdown.onValueChanged.AddListener(OnCatalogDropdownChanged);
                PopulateDropdown();
            }
        }

        public void OnDestroy()
        {
            if (m_dropdown)
            {
                m_dropdown.onValueChanged.RemoveListener(OnCatalogDropdownChanged);
            }
        }

        private void PopulateDropdown()
        {
            
            m_dropdown.ClearOptions();
            m_dropdown.AddOptions(Enum.GetNames(typeof(GemCatalogType))
                .Select(option => new TMP_Dropdown.OptionData(option))
                .ToList());
            m_dropdown.value = 0;
        }

        public void OnCatalogDropdownChanged(int val)
        {
            m_gemTable.SetCatalogGem((GemCatalogType)val);
        }
        
        public void OnNewGemButtonPressed()
        {
            m_gemTable.GenerateRandomGem();
        }
        
        public void OnSaveGemButtonPressed()
        {
            m_gemTable.SaveGem();
        }
        
        public void OnLoadGemButtonPressed()
        {
            m_gemTable.LoadGem();
        }
        
        public void OnCompareButtonPressed()
        {
            Debug.LogWarning(m_gemTable.CompareGem());
        }
    }
}