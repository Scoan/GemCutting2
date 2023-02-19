using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Linq.Expressions;
using GemStoneCatalog;

public class GemTable : MonoBehaviour {

    public GemStone Gem;

    private bool m_inputEnabled = true;

    private Quaternion m_targetRotation = Quaternion.identity;
    private Func<GemStone, bool> m_postRotateAction = null;

    private Vector3 m_curCutAngle = Vector3.left;

	// Use this for initialization
	void Start () {
        Gem.UpdateSlicePreview(m_curCutAngle);
    }

    private void Update()
    {
        if (Gem == null)
            return;
        if (m_inputEnabled)
            ProcessInput();
    }

    void FixedUpdate () {
        if (Gem == null)
            return;
        if (m_postRotateAction != null)
        {
            ProcessRotation();
            return;
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
            m_targetRotation = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
            m_postRotateAction = (gem) => { gem.voxels.RotateY(); gem.UpdateMesh(); Gem.UpdateSlicePreview(m_curCutAngle); return true; };
        }
        else if (Input.GetKey(KeyCode.X))
        {
            m_inputEnabled = false;
            m_targetRotation = Quaternion.AngleAxis(-90, new Vector3(0, 1, 0));
            m_postRotateAction = (gem) => { gem.voxels.RotateY(ccw: true); gem.UpdateMesh(); Gem.UpdateSlicePreview(m_curCutAngle); return true; };
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            m_inputEnabled = false;
            m_targetRotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
            m_postRotateAction = (gem) => { gem.voxels.RotateZ(ccw: true); gem.UpdateMesh(); Gem.UpdateSlicePreview(m_curCutAngle); return true; };
        }
        else if (Input.GetKey(KeyCode.E))
        {
            m_inputEnabled = false;
            m_targetRotation = Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
            m_postRotateAction = (gem) => { gem.voxels.RotateZ(); gem.UpdateMesh(); Gem.UpdateSlicePreview(m_curCutAngle); return true; };
        }
        else if (Input.GetKey(KeyCode.R))
        {
            m_inputEnabled = false;
            m_targetRotation = Quaternion.AngleAxis(90, new Vector3(1, 0, 0));
            m_postRotateAction = (gem) => { gem.voxels.RotateX(ccw: true); gem.UpdateMesh(); Gem.UpdateSlicePreview(m_curCutAngle); return true; };
        }
        else if (Input.GetKey(KeyCode.F))
        {
            m_inputEnabled = false;
            m_targetRotation = Quaternion.AngleAxis(-90, new Vector3(1, 0, 0));
            m_postRotateAction = (gem) => { gem.voxels.RotateX(); gem.UpdateMesh(); Gem.UpdateSlicePreview(m_curCutAngle); return true; };
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

    private void ProcessRotation()
    {
        Gem.transform.rotation = Quaternion.RotateTowards(Gem.transform.rotation, m_targetRotation, 5f);

        // If rotation is complete and we have a post-rotate action, execute it
        if (Quaternion.Angle(Gem.transform.rotation, m_targetRotation) < .01)
        {
            Gem.transform.rotation = Quaternion.identity;
            m_postRotateAction(Gem);
            m_postRotateAction = null;
            // Clear rotation
            m_targetRotation = Quaternion.identity;
            m_inputEnabled = true;
        }
    }

    public void SliceGem()
    {
        Vector3Int planePos = Gem.voxels.GetCoordinateFurthestAlongAngle(m_curCutAngle);
        Vector3 planeWorldPos = Gem.CoordToWorldPos(planePos.x, planePos.y, planePos.z);
        Gem.voxels.ClearPointsBeyondPlane(planePos, m_curCutAngle);
        Gem.voxels.Trim();
        Gem.UpdateMesh();
        Gem.UpdateSlicePreview(m_curCutAngle);
    }

    private static Vector3[] m_validHorizontalAngles = new[] { Vector3.left,
                                                              (Vector3.left + Vector3.forward).normalized,
                                                               Vector3.forward,
                                                              (Vector3.forward + Vector3.right).normalized,
                                                               Vector3.right,
                                                              (Vector3.right + Vector3.back).normalized,
                                                               Vector3.back,
                                                              (Vector3.back + Vector3.left).normalized};

    private static Vector3[] m_validSlantedAngles = new[] {   (Vector3.left + Vector3.up                   ).normalized,
                                                              (Vector3.left + Vector3.forward + Vector3.up ).normalized,
                                                              (Vector3.forward + Vector3.up                ).normalized,
                                                              (Vector3.forward + Vector3.right + Vector3.up).normalized,
                                                              (Vector3.right + Vector3.up                  ).normalized,
                                                              (Vector3.right + Vector3.back + Vector3.up   ).normalized,
                                                              (Vector3.back + Vector3.up                   ).normalized,
                                                              (Vector3.back + Vector3.left + Vector3.up    ).normalized};

    private void RotateCutAngle(int yaw, int pitch)
    {
        //
        int horizAngleIndex = System.Array.IndexOf(m_validHorizontalAngles, m_curCutAngle);
        int slantedAngleIndex = System.Array.IndexOf(m_validSlantedAngles, m_curCutAngle);
        if (yaw != 0)
        {
            if (horizAngleIndex != -1)
            {
                int newIndex = (horizAngleIndex + (int)Mathf.Sign(yaw)) % m_validHorizontalAngles.Length;
                newIndex = (newIndex == -1) ? m_validHorizontalAngles.Length - 1 : newIndex;
                m_curCutAngle = m_validHorizontalAngles[newIndex];
            }
            else if (slantedAngleIndex != -1)
            {
                int newIndex = (slantedAngleIndex + (int)Mathf.Sign(yaw)) % m_validSlantedAngles.Length;
                newIndex = (newIndex == -1) ? m_validSlantedAngles.Length - 1 : newIndex;
                m_curCutAngle = m_validSlantedAngles[newIndex];
            }
        }
        else if (pitch != 0)
        {
            if (horizAngleIndex != -1 && pitch == 1)
            {
                m_curCutAngle = m_validSlantedAngles[horizAngleIndex];
            }
            else if (slantedAngleIndex != -1)
            {
                if (pitch == -1)
                {
                    m_curCutAngle = m_validHorizontalAngles[slantedAngleIndex];
                }
                else if (pitch == 1)
                {
                    m_curCutAngle = Vector3.up;
                }
            }
            // We're vertical, need to tilt down towards camera
            else if (pitch == -1 && m_curCutAngle == Vector3.up)
            {
                // TODO: Instead of tilting down towards camera, just tilt down towards the last side we were on. Feels better.
                // TODO: Find slanted angle closest to -camera.forward and use that
                var dots = from value in m_validSlantedAngles select Vector3.Dot(Camera.main.transform.forward, value);
                float[] dotsArr = dots.ToArray();
                int minIndex = Array.IndexOf(dotsArr, dotsArr.Min());
                m_curCutAngle = m_validSlantedAngles[minIndex];
            }
        }
        // Update slice preview
        Gem.UpdateSlicePreview(m_curCutAngle);

    }

    //public void UpdateSlicePreview()
    //{
    //    Vector3Int closestToPosZ = Gem.voxels.GetCoordinateFurthestAlongAngle(m_curCutAngle);
    //    Debug.Log(string.Format("Closest is {0}", closestToPosZ));
    //    //Gem.voxels.ClearPointsBeyondPlane(closestToPosZ, m_curCutAngle * 100);
    //}

    //public void SawFace()
    //{
    //    ///Saw off the front XY plane
    //    ///
    //    if (Gem == null)
    //        return;
    //
    //    // Find the first plane with values. That's the one we need to saw!
    //    int firstPlane = 0;
    //    for (int z = 0; z < Gem.SizeZ; z++)
    //    {
    //
    //        if (Gem.voxels.GetXYPlane(z).Any(val => val != Gem.voxels.defaultValue))
    //        {
    //            firstPlane = z; 
    //            //Debug.Log(string.Format("sawing at Z = {0}", z));
    //            //Debug.Log(string.Format("sawing at pos = {0}", Gem.CoordToWorldPos(0,0,z)));
    //            break;
    //        }
    //    }
    //
    //    // Clear the plane!
    //    for (int x = 0; x < Gem.SizeX; x++)
    //    {
    //        for (int y = 0; y < Gem.SizeY; y++)
    //        {
    //            Gem.voxels.SetVal(x, y, firstPlane, Gem.voxels.defaultValue);
    //        }
    //    };
    //}
    //
    //public void SawEdge()
    //{
    //    ///Saw off the top right most diagonal
    //    ///
    //    // Need to find the first Z-row with a significant value to remove, and remove that whole diagonal.
    //    // A search like this:
    //    //   4   2    1
    //    //     ┘    ┘  
    //    //   7   5    3
    //    //          ┘
    //    //   9   8    6
    //    if (Gem == null)
    //        return;
    //
    //    bool valueFound = false;
    //    int curDiagonal = 0;
    //    while (!valueFound && curDiagonal < (Gem.SizeX + Gem.SizeY))
    //    {
    //        int x;
    //        int y;
    //        // If we're still scanning across the top, the first coordinate is at the top
    //        if (curDiagonal < Gem.SizeX)
    //        {
    //            y = Gem.SizeY - 1;
    //            x = (Gem.SizeX - 1) - curDiagonal;
    //        }
    //        // Otherwise, the first coordinate is down the left side
    //        else
    //        {
    //            y = (Gem.SizeY - 1) - (curDiagonal - Gem.SizeX);
    //            x = 0;
    //        }
    //
    //        // Step through this diagonal.
    //        while (x < Gem.SizeX && y >= 0)
    //        {
    //            float[] curRow = Gem.voxels.GetZRow(x, y);
    //            // If we find a value, start clearing out the diagonal. Note we've found a value so we end our search after this diagonal.
    //            if (curRow.Any(val => val != Gem.voxels.defaultValue))
    //            {
    //                // Clear out this diagonal and be done with the sawing.
    //                valueFound = true;
    //                for (int z = 0; z < Gem.SizeZ; z++)
    //                {
    //                    Gem.voxels.SetVal(x, y, z, Gem.voxels.defaultValue);
    //                }
    //            }
    //
    //            // Step down to next row in diagonal
    //            x += 1;
    //            y -= 1;
    //        }
    //        curDiagonal += 1;
    //    }
    //}
    //
    //
    //public void SawCorner()
    //{
    //    /// Saw off the back right topmost corner
    //    /// 
    //    // Need to find the first corner slice with a significant value to remove, and remove that corner
    //    //
    //    // Start by searching along the top back row, then come to the front, then down the front left side
    //    //
    //
    //    if (Gem == null)
    //        return;
    //
    //    bool valueFound = false;
    //    int curSlice = 0;
    //    while (!valueFound && curSlice < (Gem.SizeX + Gem.SizeZ + Gem.SizeY - 2))
    //    {
    //        // Starting coordinates for a given slice
    //        int x;
    //        int y;
    //        int z;
    //
    //        // If we're still scanning across the top, the first coordinate is along the top back
    //        if (curSlice < Gem.SizeX)
    //        {
    //            x = (Gem.SizeX - 1) - curSlice;
    //            y = Gem.SizeY - 1;
    //            z = Gem.SizeZ - 1;
    //        }
    //        // If we're still scanning towards the front, the first coordinate is along the top left edge
    //        else if (curSlice < (Gem.SizeZ + Gem.SizeX - 1))
    //        {
    //            x = 0;
    //            y = Gem.SizeY - 1;
    //            z = (Gem.SizeZ - 1) - (curSlice - (Gem.SizeX - 1));
    //        }
    //        // Otherwise, the first coordinate is down the front left edge
    //        else
    //        {
    //            x = 0;
    //            y = (Gem.SizeY - 1) - (curSlice - (Gem.SizeX + Gem.SizeZ - 2));
    //            z = 0;
    //        }
    //
    //        // Step through this diagonal.
    //        // Add to X & Z
    //        while (x < Gem.SizeX && y >= 0)
    //        {
    //            // Check the current XZ line through the grid for values
    //            int curX = x;
    //            int curY = y;
    //            int curZ = z;
    //            while (curX < Gem.SizeX && curZ >= 0)
    //            {
    //                float val = Gem.voxels.GetVal(curX, curY, curZ);
    //                if (val != Gem.voxels.defaultValue)
    //                {
    //                    valueFound = true;
    //                    Gem.voxels.SetVal(curX, curY, curZ, Gem.voxels.defaultValue);
    //                }
    //                curX += 1;
    //                curZ -= 1;
    //            }
    //
    //            // Step down to next row in diagonal
    //            // If we're along the back edge, this is (x+1, y-1). Otherwise, this is (z+1, y-1).
    //            if (z == Gem.SizeZ - 1)
    //            {
    //                x += 1;
    //                y -= 1;
    //            }
    //            else
    //            {
    //                z += 1;
    //                y -= 1;
    //            }
    //            
    //        }
    //        curSlice += 1;
    //    }
    //}

    //public Vector3 DetermineCutAngle()
    //{
    //    // TODO: if using a controller, use stick angle
    //    //       if using mouse+kb, use mouse position or numpad selection
    //    //       Investigate other options (eg. hotkeys)
    //
    //    Vector2 mouseXY = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - new Vector2(Screen.width/2.0f, Screen.height/2.0f);
    //    // Normalize position against a circle that fits within the viewport
    //    mouseXY /= Mathf.Min(Screen.width, Screen.height) * .5f;
    //
    //    // We need to resolve the mouse position to either "down the camera" or N, E, S, W, NE, NW, SE, SW.
    //    Vector3 effectiveDirection = Vector3.zero;
    //    
    //    // Mouse is too far from center, ignore position
    //    if (mouseXY.magnitude > .75f)
    //        effectiveDirection = Vector3.zero;
    //    // Mouse is near center, so forward
    //    else if (mouseXY.magnitude < .25f)
    //        effectiveDirection = Vector3.forward;
    //    else
    //    {
    //        mouseXY = mouseXY.normalized;
    //        // Dot the mouse pos against N, NE, E, E. We can figure out which cardinal or semi-cardinal we're in.
    //        // Each direction has a 45% arc, so if dot(direction, mousepos) > cos(22.5*) (0.9238) then we're in that octant.
    //        float dotThreshold = 0.9238f;
    //
    //        // Cardinals
    //        if (Vector2.Dot(mouseXY, Vector2.up) >= dotThreshold)
    //            effectiveDirection = Vector3.up;
    //        else if (Vector2.Dot(mouseXY, Vector2.down) >= dotThreshold)
    //            effectiveDirection = Vector3.down;
    //        else if (Vector2.Dot(mouseXY, Vector2.left) >= dotThreshold)
    //            effectiveDirection = Vector3.left;
    //        else if (Vector2.Dot(mouseXY, Vector2.right) >= dotThreshold)
    //            effectiveDirection = Vector3.right;
    //        // Diagonals. TODO: cache dirs at compile time? Normalization ain't cheap
    //        else if (Vector2.Dot(mouseXY, (Vector2.up + Vector2.left).normalized) > dotThreshold)
    //            effectiveDirection = (Vector3.up + Vector3.left).normalized;
    //        else if (Vector2.Dot(mouseXY, (Vector2.up + Vector2.right).normalized) > dotThreshold)
    //            effectiveDirection = (Vector3.up + Vector3.right).normalized;
    //        else if (Vector2.Dot(mouseXY, (Vector2.down + Vector2.left).normalized) > dotThreshold)
    //            effectiveDirection = (Vector3.down + Vector3.left).normalized;
    //        else if (Vector2.Dot(mouseXY, (Vector2.down + Vector2.right).normalized) > dotThreshold)
    //            effectiveDirection = (Vector3.down + Vector3.right).normalized;
    //    }
    //
    //    //Debug.Log(string.Format("Mouse pos is {0}", effectiveDirection));
    //
    //
    //
    //
    //    return effectiveDirection;
    //
    //}
}

