using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace GemCutting
{
    public class GemTable : MonoBehaviour {
    
        public static readonly Vector3[] cuttingPlaneHorizontalNormals = 
        { 
            Vector3.left,
            (Vector3.left + Vector3.forward).normalized,
            Vector3.forward,
            (Vector3.forward + Vector3.right).normalized,
            Vector3.right,
            (Vector3.right + Vector3.back).normalized,
            Vector3.back,
            (Vector3.back + Vector3.left).normalized,
        };
        
        public static readonly Vector3[] cuttingPlaneSlantedNormals = 
        {   
            (Vector3.left + Vector3.up                   ).normalized,
            (Vector3.left + Vector3.forward + Vector3.up ).normalized,
            (Vector3.forward + Vector3.up                ).normalized,
            (Vector3.forward + Vector3.right + Vector3.up).normalized,
            (Vector3.right + Vector3.up                  ).normalized,
            (Vector3.right + Vector3.back + Vector3.up   ).normalized,
            (Vector3.back + Vector3.up                   ).normalized,
            (Vector3.back + Vector3.left + Vector3.up    ).normalized,
        };

        // In addition to the cutting angles above, these angles should be used when validating a loaded gem
        public static readonly Vector3[] validationAdditionalAngles =
        {
            Vector3.up,
            Vector3.down,
            (Vector3.left + Vector3.down                   ).normalized,
            (Vector3.left + Vector3.forward + Vector3.down ).normalized,
            (Vector3.forward + Vector3.down                ).normalized,
            (Vector3.forward + Vector3.right + Vector3.down).normalized,
            (Vector3.right + Vector3.down                  ).normalized,
            (Vector3.right + Vector3.back + Vector3.down   ).normalized,
            (Vector3.back + Vector3.down                   ).normalized,
            (Vector3.back + Vector3.left + Vector3.down    ).normalized,
        };

        [SerializeField] private GemStone m_gem;
        [SerializeField] private GemStone m_catalogGem;

        // Is input currently allowed? We disable it during a rotation
        private bool m_inputEnabled = true;
        private Quaternion m_targetRotation = Quaternion.identity;
        private Func<GemStone, bool> m_postRotateAction;
        private Vector3 m_curCutAngle = Vector3.left;

        void Awake () {
            if (m_gem != null)
            {
                m_gem.UpdateSlicePreview(m_curCutAngle); 
            }
        }

        private void Update()
        {
            if (m_gem != null)
            {
                if (m_inputEnabled)
                {
                    HandleInput();
                }
                // TODO: Perf of calling this every frame?
                m_gem.UpdateSlicePreview(m_curCutAngle);
            }
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_gem.Slice(m_curCutAngle);
                return;
            }

            // Gem rotation inputs
            if (Input.GetKey(KeyCode.Z))
            {
                m_targetRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * m_gem.transform.rotation;
                TriggerRotation();
            }
            else if (Input.GetKey(KeyCode.X))
            {
                m_targetRotation = Quaternion.AngleAxis(-90, new Vector3(0, 1, 0)) * m_gem.transform.rotation;
                TriggerRotation();
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                m_targetRotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1)) * m_gem.transform.rotation;
                TriggerRotation();
            }
            else if (Input.GetKey(KeyCode.E))
            {
                m_targetRotation = Quaternion.AngleAxis(-90, new Vector3(0, 0, 1)) * m_gem.transform.rotation;
                TriggerRotation();
            }
            else if (Input.GetKey(KeyCode.R))
            {
                m_targetRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0)) * m_gem.transform.rotation;
                TriggerRotation();
            }
            else if (Input.GetKey(KeyCode.F))
            {
                m_targetRotation = Quaternion.AngleAxis(-90, new Vector3(1, 0, 0)) * m_gem.transform.rotation;
                TriggerRotation();
            }

            // Cutter rotation inputs
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RotateCutAngle(1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RotateCutAngle(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RotateCutAngle(0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                RotateCutAngle(0, -1);
            }
            
            else if (Input.GetKeyDown(KeyCode.O))
            {
                GenerateRandomGem();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.LogWarning(CompareGem());
            }
            else if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                PreviousCatalogGem();
            }
            else if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                NextCatalogGem();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                SaveGem();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                LoadGem();
            }
        }

        // TODO: Move these to general controls?
        public void SaveGem()
        {
            if (m_gem)
            {
                m_gem.Save();
            }
        }
        
        public void LoadGem()
        {
            if (m_gem)
            {
                m_gem.Load();
            }
        }

        public bool CompareGem()
        {
            return m_gem.Compare(m_catalogGem);
        }
        
        public void GenerateRandomGem()
        {
            if (m_gem)
            {
                m_gem.VoxelsSource = VoxelsSource.Seed;
                m_gem.Seed = UnityEngine.Random.Range(0, 1000000);
                m_gem.Initialize();
            }
        }

        public void SetCatalogGem(GemCatalogType gemType)
        {
            if (m_catalogGem)
            {
                m_catalogGem.VoxelsSource = VoxelsSource.Catalog;
                m_catalogGem.GemCatalogType = gemType;
                m_catalogGem.Initialize();
            }
        }
        
        private void PreviousCatalogGem()
        {
            if (m_catalogGem)
            {
                int numOptions = Enum.GetValues(typeof(GemCatalogType)).Length;
                GemCatalogType val = (GemCatalogType)((int)(m_catalogGem.GemCatalogType - 1 + numOptions) % numOptions);
                SetCatalogGem(val);
            }
        }
        private void NextCatalogGem()
        {
            if (m_catalogGem)
            {
                int numOptions = Enum.GetValues(typeof(GemCatalogType)).Length;
                GemCatalogType val = (GemCatalogType)((int)(m_catalogGem.GemCatalogType + 1 + numOptions) % numOptions);
                SetCatalogGem(val);
            }
        }

        private void TriggerRotation()
        {
            m_gem.transform.DORotateQuaternion(m_targetRotation, .5f)
                .OnStart(() => m_inputEnabled = false)
                .OnComplete(() =>
                {
                    m_inputEnabled = true;
                    m_gem.UpdateSlicePreview(m_curCutAngle);
                })
                .SetEase(Ease.Linear); 
        }

        private void RotateCutAngle(int yaw, int pitch)
        {
            //
            int horizAngleIndex = Array.IndexOf(cuttingPlaneHorizontalNormals, m_curCutAngle);
            int slantedAngleIndex = Array.IndexOf(cuttingPlaneSlantedNormals, m_curCutAngle);
            if (yaw != 0)
            {
                if (horizAngleIndex != -1)
                {
                    int newIndex = (horizAngleIndex + (int)Mathf.Sign(yaw)) % cuttingPlaneHorizontalNormals.Length;
                    newIndex = (newIndex == -1) ? cuttingPlaneHorizontalNormals.Length - 1 : newIndex;
                    m_curCutAngle = cuttingPlaneHorizontalNormals[newIndex];
                }
                else if (slantedAngleIndex != -1)
                {
                    int newIndex = (slantedAngleIndex + (int)Mathf.Sign(yaw)) % cuttingPlaneSlantedNormals.Length;
                    newIndex = (newIndex == -1) ? cuttingPlaneSlantedNormals.Length - 1 : newIndex;
                    m_curCutAngle = cuttingPlaneSlantedNormals[newIndex];
                }
            }
            else if (pitch != 0)
            {
                if (horizAngleIndex != -1 && pitch == 1)
                {
                    m_curCutAngle = cuttingPlaneSlantedNormals[horizAngleIndex];
                }
                else if (slantedAngleIndex != -1)
                {
                    if (pitch == -1)
                    {
                        m_curCutAngle = cuttingPlaneHorizontalNormals[slantedAngleIndex];
                    }
                    else if (pitch == 1)
                    {
                        m_curCutAngle = Vector3.up;
                    }
                }
                // We're vertical, need to tilt down towards camera
                else if (pitch == -1 && m_curCutAngle == Vector3.up)
                {
                    // TODO: Instead of tilting down towards camera, tilt down towards the last side we were on. Feels better.
                    // Find slanted angle closest to -camera.forward and use that
                    float[] dots = cuttingPlaneSlantedNormals
                        .Select(value => Vector3.Dot(Camera.main!.transform.forward, value))
                        .ToArray();
                    int minIndex = Array.IndexOf(dots, dots.Min());
                    m_curCutAngle = cuttingPlaneSlantedNormals[minIndex];
                }
            }
            // Update slice preview
            m_gem.UpdateSlicePreview(m_curCutAngle);
        }
    }
}

