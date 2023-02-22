using System;
using System.Collections.Generic;
using System.Linq;
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
            if (m_gem == null)
            {
                return;
            }

            if (m_inputEnabled)
            {
                ProcessInput();
            }
        }

        void FixedUpdate () {
            if (m_gem != null && m_postRotateAction != null)
            {
                ProcessRotation();
            }
        }

        private void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SliceGem();
                return;
            }

            // Gem rotation inputs
            if (Input.GetKey(KeyCode.Z))
            {
                m_inputEnabled = false;
                //m_targetRotation = m_gem.transform.rotation * Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                m_targetRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                m_postRotateAction = (gem) => { gem.Voxels.RotateY(); gem.UpdateMesh(); m_gem.UpdateSlicePreview(m_curCutAngle); return true; };
            }
            else if (Input.GetKey(KeyCode.X))
            {
                m_inputEnabled = false;
                //m_targetRotation = m_gem.transform.rotation * Quaternion.AngleAxis(-90, new Vector3(0, 1, 0));
                m_targetRotation = Quaternion.AngleAxis(-90, new Vector3(0, 1, 0));
                m_postRotateAction = (gem) => { gem.Voxels.RotateY(ccw: true); gem.UpdateMesh(); m_gem.UpdateSlicePreview(m_curCutAngle); return true; };
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                m_inputEnabled = false;
                //m_targetRotation = m_gem.transform.rotation * Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
                m_targetRotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
                m_postRotateAction = (gem) => { gem.Voxels.RotateZ(ccw: true); gem.UpdateMesh(); m_gem.UpdateSlicePreview(m_curCutAngle); return true; };
            }
            else if (Input.GetKey(KeyCode.E))
            {
                m_inputEnabled = false;
                //m_targetRotation = m_gem.transform.rotation * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
                m_targetRotation = Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
                m_postRotateAction = (gem) => { gem.Voxels.RotateZ(); gem.UpdateMesh(); m_gem.UpdateSlicePreview(m_curCutAngle); return true; };
            }
            else if (Input.GetKey(KeyCode.R))
            {
                m_inputEnabled = false;
                //m_targetRotation = m_gem.transform.rotation * Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
                m_targetRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
                m_postRotateAction = (gem) => { gem.Voxels.RotateX(ccw: true); gem.UpdateMesh(); m_gem.UpdateSlicePreview(m_curCutAngle); return true; };
            }
            else if (Input.GetKey(KeyCode.F))
            {
                m_inputEnabled = false;
                //m_targetRotation = m_gem.transform.rotation * Quaternion.AngleAxis(-90, new Vector3(1, 0, 0));
                m_targetRotation = Quaternion.AngleAxis(-90, new Vector3(1, 0, 0));
                m_postRotateAction = (gem) => { gem.Voxels.RotateX(); gem.UpdateMesh(); m_gem.UpdateSlicePreview(m_curCutAngle); return true; };
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

        // TODO: Turn into a coroutine?
        private void ProcessRotation()
        {
            m_gem.transform.rotation = Quaternion.RotateTowards(m_gem.transform.rotation, m_targetRotation, 5f);

            // If rotation is complete and we have a post-rotate action, execute it
            if (Quaternion.Angle(m_gem.transform.rotation, m_targetRotation) < .001)
            {
                //m_gem.transform.rotation = m_targetRotation;
                m_gem.transform.rotation = Quaternion.identity;
                m_postRotateAction(m_gem);
                m_postRotateAction = null;
                // Clear rotation
                m_targetRotation = Quaternion.identity;
                m_inputEnabled = true;
            }
        }

        private void SliceGem()
        {
            Vector3Int planePos = m_gem.Voxels.GetCoordinateFurthestAlongAngle(m_curCutAngle);
            m_gem.Voxels.ClearPointsBeyondPlane(planePos, m_curCutAngle);
            m_gem.Voxels.Trim();
            m_gem.UpdateMesh();
            m_gem.UpdateSlicePreview(m_curCutAngle);
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
                    // TODO: Use LINQ comprehension
                    IEnumerable<float> dots = 
                        from value in ValidSlantedAngles select 
                            Vector3.Dot(Camera.main!.transform.forward, value);
                    float[] dotsArr = dots.ToArray();
                    int minIndex = Array.IndexOf(dotsArr, dotsArr.Min());
                    m_curCutAngle = ValidSlantedAngles[minIndex];
                }
            }
            // Update slice preview
            m_gem.UpdateSlicePreview(m_curCutAngle);
        }
    }
}

