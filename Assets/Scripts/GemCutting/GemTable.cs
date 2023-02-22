using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace GemCutting
{
    public class GemTable : MonoBehaviour {
    
        private static readonly Vector3[] ValidHorizontalAngles = { 
            Vector3.left,
            (Vector3.left + Vector3.forward).normalized,
            Vector3.forward,
            (Vector3.forward + Vector3.right).normalized,
            Vector3.right,
            (Vector3.right + Vector3.back).normalized,
            Vector3.back,
            (Vector3.back + Vector3.left).normalized};

        private static readonly Vector3[] ValidSlantedAngles = {   
            (Vector3.left + Vector3.up                   ).normalized,
            (Vector3.left + Vector3.forward + Vector3.up ).normalized,
            (Vector3.forward + Vector3.up                ).normalized,
            (Vector3.forward + Vector3.right + Vector3.up).normalized,
            (Vector3.right + Vector3.up                  ).normalized,
            (Vector3.right + Vector3.back + Vector3.up   ).normalized,
            (Vector3.back + Vector3.up                   ).normalized,
            (Vector3.back + Vector3.left + Vector3.up    ).normalized};

        [SerializeField] private GemStone m_gem;

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

        void FixedUpdate () {
            if (m_gem != null && m_postRotateAction != null)
            {
                //ProcessRotation();
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
        }

        private void TriggerRotation()
        {
            // TODO: Figure out what new cutting plane position will be, tween it as well. Or just UpdateSlicePreview in OnComplete
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
            int horizAngleIndex = Array.IndexOf(ValidHorizontalAngles, m_curCutAngle);
            int slantedAngleIndex = Array.IndexOf(ValidSlantedAngles, m_curCutAngle);
            if (yaw != 0)
            {
                if (horizAngleIndex != -1)
                {
                    int newIndex = (horizAngleIndex + (int)Mathf.Sign(yaw)) % ValidHorizontalAngles.Length;
                    newIndex = (newIndex == -1) ? ValidHorizontalAngles.Length - 1 : newIndex;
                    m_curCutAngle = ValidHorizontalAngles[newIndex];
                }
                else if (slantedAngleIndex != -1)
                {
                    int newIndex = (slantedAngleIndex + (int)Mathf.Sign(yaw)) % ValidSlantedAngles.Length;
                    newIndex = (newIndex == -1) ? ValidSlantedAngles.Length - 1 : newIndex;
                    m_curCutAngle = ValidSlantedAngles[newIndex];
                }
            }
            else if (pitch != 0)
            {
                if (horizAngleIndex != -1 && pitch == 1)
                {
                    m_curCutAngle = ValidSlantedAngles[horizAngleIndex];
                }
                else if (slantedAngleIndex != -1)
                {
                    if (pitch == -1)
                    {
                        m_curCutAngle = ValidHorizontalAngles[slantedAngleIndex];
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
                    float[] dots = ValidSlantedAngles
                        .Select(value => Vector3.Dot(Camera.main!.transform.forward, value))
                        .ToArray();
                    int minIndex = Array.IndexOf(dots, dots.Min());
                    m_curCutAngle = ValidSlantedAngles[minIndex];
                }
            }
            // Update slice preview
            m_gem.UpdateSlicePreview(m_curCutAngle);
        }
    }
}

