using System;
using System.Linq;
using ProceduralNoiseProject;
using UnityEngine;

namespace GemCutting
{
    public class VoxelGrid
    {
        private static readonly float TOLERANCE = .001f;
        
        public Vector3Int m_extents;
        public float[] m_values;
        public readonly float m_defaultValue;  // Newly initialized voxels default to this value.

        public VoxelGrid(Vector3Int extents, float defaultValue = -1)
        {
            m_extents = extents;
            m_defaultValue = defaultValue;
            m_values = Enumerable.Repeat(m_defaultValue, m_extents.x * m_extents.y * m_extents.z).ToArray();
        }

        public int CoordToIdx(Vector3Int coord)
        {
            // Converts a given (x,y,z) coordinate in the voxel grid to an index.
            // Catch out-of-range errors, return an invalid index.
            if (coord.x < 0 || coord.y < 0 || coord.z < 0)
            {
                return -1;
            }
            if (coord.x >= m_extents.x || coord.y >= m_extents.y || coord.z >= m_extents.z)
            {
                return -1;
            }
            return coord.x + coord.y * m_extents.x + coord.z * m_extents.x * m_extents.y;
        }

        public Vector3Int IdxToCoord(int idx)
        {
            // Converts a given index to (x,y,z) coordinates in the voxel grid.
            // TODO: Catch out-of-range errors
            int z = idx / (m_extents.x * m_extents.y);
            int zRem = idx % (m_extents.x * m_extents.y);
            int y = zRem / m_extents.x;
            int x = zRem % m_extents.x;
            return new Vector3Int(x, y, z);
        }

        private float GetVal(int idx)
        {
            // Returns the value at the specified index. If out of range, the default value is returned.
            if (idx < 0 || idx >= m_values.Length)
                return m_defaultValue;
            return m_values[idx];
        }

        private float GetVal(Vector3Int coordinate)
        {
            // Returns the value at the specified position. If out of range, the default value is returned.
            return GetVal(CoordToIdx(coordinate));
        }

        private void SetVal(int idx, float val)
        {
            // Sets the value at the specified index. If out of range, the value is ignored.
            if (idx < 0 || idx >= m_values.Length)
                return;
            m_values[idx] = val;
        }

        private void SetVal(Vector3Int coordinate, float val)
        {
            // Sets the value at the specified position. If out of range, the value is ignored.
            SetVal(CoordToIdx(coordinate), val);
        }

        private void Shift(Vector3Int shiftVector, bool resize = false)
        {
            // Offsets voxels in the supplied direction.
            // If resize is false, values pushed off-grid will be discarded.
            // TODO: If resize is true, resize the grid to accomodate interesting (non -1) values being pushed off grid?
            // TODO: Support multiple resize behaviors? No resize (discard lost values) / resize / "loop" values pushed off back to other side?
            // 
            // TODO: Can we do this in place? Would need to shift defaultValue in on sides where we've shifted away from the grid edge.
            float[] newValues = Enumerable.Repeat(m_defaultValue, m_extents.x * m_extents.y * m_extents.z).ToArray();
            // Fill newVoxels with offset values!
            for (int idx = 0; idx < m_values.Length; idx++)
            {
                int newIdx = CoordToIdx(IdxToCoord(idx) + shiftVector);

                // If we're moving off the grid, discard the data. But it better be dead data!
                // TODO: Take resize flag into account
                if ((newIdx < 0 || newIdx >= m_values.Length))
                {
                    if (Math.Abs(m_values[idx] - m_defaultValue) > TOLERANCE)
                    {
                        Debug.LogError(
                            $"Tried to push real data {m_values[idx]} from idx {idx} to idx {newIdx}, off the grid. BAD!");
                    }
                    else
                        continue;
                }
                newValues[newIdx] = m_values[idx];
            }

            // Replace old voxels with new
            m_values = newValues;
        }

        private float[] GetYZPlane(int x)
        {
            // Returns the values for the YZ plane at X
            // 
            float[] vals = new float[m_extents.y * m_extents.z];

            for (int y = 0; y < m_extents.y; y++)
            {
                for (int z = 0; z < m_extents.z; z++)
                {
                    vals[y + z * m_extents.y] = m_values[CoordToIdx(new Vector3Int(x, y, z))];
                }
            }
            return vals;
        }

        private float[] GetXZPlane(int y)
        {
            // Returns the values for the XZ plane at Y
            // 
            float[] vals = new float[m_extents.x * m_extents.z];

            for (int x = 0; x < m_extents.x; x++)
            {
                for (int z = 0; z < m_extents.z; z++)
                {
                    vals[x + z * m_extents.x] = m_values[CoordToIdx(new Vector3Int(x, y, z))];
                }
            }
            return vals;
        }

        private float[] GetXYPlane(int z)
        {
            // Returns the values for the XY plane at Z
            // 
            float[] vals = new float[m_extents.x * m_extents.y];

            for (int x = 0; x < m_extents.x; x++)
            {
                for (int y = 0; y < m_extents.y; y++)
                {
                    vals[x + y * m_extents.x] = m_values[CoordToIdx(new Vector3Int(x, y, z))];
                }
            }
            return vals;
        }

        public float[] GetXRow(int y, int z)
        {
            // Returns the values for the X row at Y,Z
            // 
            float[] vals = new float[m_extents.x];

            for (int x = 0; x < m_extents.x; x++)
            {
                vals[x] = GetVal(new Vector3Int(x, y, z));
            }
            return vals;
        }

        public float[] GetYRow(int x, int z)
        {
            // Returns the values for the Y row at X,Z
            // 
            float[] vals = new float[m_extents.y];

            for (int y = 0; y < m_extents.y; y++)
            {
                vals[y] = GetVal(new Vector3Int(x, y, z));
            }
            return vals;
        }

        public float[] GetZRow(int x, int y)
        {
            // Returns the values for the X row at Y,Z
            // 
            float[] vals = new float[m_extents.z];

            for (int z = 0; z < m_extents.z; z++)
            {
                vals[z] = GetVal(new Vector3Int(x, y, z));
            }
            return vals;
        }

        // TODO: Use Unity's Bounds class instead?
        private void GetBounds(out int xMin, out int yMin, out int zMin, out int xMax, out int yMax, out int zMax)
        {
            // Returns the bounding box where the voxel grid begins to have interesting (non default) values.
            // 
            // TODO: Provide uninteresting value as an argument? In case we have to call this after changing that value.

            // Step through each plane until we hit a significant value.
            // We could get the min and max in one loop, but it's probably quicker to step in from each side and stop when we find the min/max respectively.
            // TODO: Might be able to step through a single axis and get all mins/maxes though (as we step through we'd track each axes.. but we'd have to step thru ENTIRE grid)

            // x
            xMin = 0;
            for (int x = 0; x < m_extents.x; x++)
            {
                float[] vals = GetYZPlane(x);
                if (vals.Any(val => Math.Abs(val - m_defaultValue) > TOLERANCE))
                {
                    xMin = x;
                    break;
                }
            }
            xMax = m_extents.x - 1;
            for (int x = m_extents.x - 1; x >= 0; x--)
            {
                float[] vals = GetYZPlane(x);
                if (vals.Any(val => Math.Abs(val - m_defaultValue) > TOLERANCE))
                {
                    xMax = x;
                    break;
                }
            }

            // y
            yMin = 0;
            for (int y = 0; y < m_extents.y; y++)
            {
                float[] vals = GetXZPlane(y);
                if (vals.Any(val => Math.Abs(val - m_defaultValue) > TOLERANCE))
                {
                    yMin = y;
                    break;
                }
            }
            yMax = m_extents.y - 1;
            for (int y = m_extents.y - 1; y >= 0; y--)
            {
                float[] vals = GetXZPlane(y);
                if (vals.Any(val => Math.Abs(val - m_defaultValue) > TOLERANCE))
                {
                    yMax = y;
                    break;
                }
            }

            // z
            zMin = 0;
            for (int z = 0; z < m_extents.z; z++)
            {
                float[] vals = GetXYPlane(z);
                if (vals.Any(val => Math.Abs(val - m_defaultValue) > TOLERANCE))
                {
                    zMin = z;
                    break;
                }
            }
            zMax = m_extents.z - 1;
            for (int z = m_extents.z - 1; z >= 0; z--)
            {
                float[] vals = GetXYPlane(z);
                if (vals.Any(val => Math.Abs(val - m_defaultValue) > TOLERANCE))
                {
                    zMax = z;
                    break;
                }
            }
        }

        public void CopyFrom(VoxelGrid srcGrid, int xOffset = 0, int yOffset = 0, int zOffset = 0, bool expandThis = false)
        {
            // Copies the specified grid's voxels into this grid (with an optional offset).
            // TODO: If expandThis is true, expands this grid to accomodate all incoming values. Else, only copies values that fit into this grid.
            if (expandThis)
            {
                // TODO: Expand grid to accomodate srcGrid's size
            }

            for (int x = 0; x < m_extents.x; x++)
            {
                for (int y = 0; y < m_extents.y; y++)
                {
                    for (int z = 0; z < m_extents.z; z++)
                    {
                        //voxels[PosToIdx(x, y, z)] = srcGrid.voxels[PosToIdx(x, y, z)];
                        SetVal(new Vector3Int(x + xOffset, y + yOffset, z + zOffset), 
                            srcGrid.GetVal(new Vector3Int(x, y, z)));
                    }
                }
            }
        }

        void Justify()
        {
            // Justifies interesting voxels to the origin of the grid.
            //

            // Get bounds
            GetBounds(out int xMin, out int yMin, out int zMin, 
                out int _, out int _, out int _);

            // Shift voxels such that the bounds aligns with the grid edge
            Shift(new Vector3Int(-(xMin), -(yMin), -(zMin)));
        }

        private void Resize(Vector3Int newExtents)
        {
            // Resizes the grid. Old values outside the new grid are discarded, new values outside the old grid are initialized to default.
            //
            // TODO: A wholesale recreation and copy seems expensive. Can we resize in place?
            VoxelGrid newVoxels = new(newExtents, m_defaultValue);
            newVoxels.CopyFrom(this);
            // Replace own contents with resized contents.
            m_extents = newVoxels.m_extents;
            m_values = newVoxels.m_values;
        }

        public void Trim()
        {
            // Trims the voxel grid to the minimum size necessary to hold its interesting (non default) values.
            //

            // Align with lower corner, resize grid to match the bounds size
            Justify();

            GetBounds(out _, out _, out _, 
                out int xMax, out int yMax, out int zMax);
            Resize(new Vector3Int(xMax + 1, yMax + 1, zMax + 1));
        }

        public void RotateX(bool ccw = false)
        {
            // Rotates the contents of this grid 90* about the X axis.
            //
            // A swizzle is our friend
            VoxelGrid newVoxels = new VoxelGrid(new Vector3Int(m_extents.x, m_extents.z, m_extents.y));
            if (ccw)
            {
                for (int x = 0; x < m_extents.x; x++)
                {
                    for (int y = 0; y < m_extents.y; y++)
                    {
                        for (int z = 0; z < m_extents.z; z++)
                        {
                            newVoxels.SetVal(new Vector3Int(x, (newVoxels.m_extents.y - 1) - z, y), 
                                GetVal(new Vector3Int(x, y, z)));
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < m_extents.x; x++)
                {
                    for (int y = 0; y < m_extents.y; y++)
                    {
                        for (int z = 0; z < m_extents.z; z++)
                        {
                            newVoxels.SetVal(new Vector3Int(x, z, (newVoxels.m_extents.z - 1) - y), 
                                GetVal(new Vector3Int(x, y, z)));
                        }
                    }
                }
            }

            m_extents = newVoxels.m_extents;
            m_values = newVoxels.m_values;
        }

        public void RotateY(bool ccw = false)
        {
            // Rotates the contents of this grid 90* about the Y axis.
            //
            // A swizzle is our friend
            VoxelGrid newVoxels = new VoxelGrid(new Vector3Int(m_extents.z, m_extents.y, m_extents.x));
            if (ccw)
            {
                for (int x = 0; x < m_extents.x; x++)
                {
                    for (int y = 0; y < m_extents.y; y++)
                    {
                        for (int z = 0; z < m_extents.z; z++)
                        {
                            newVoxels.SetVal(new Vector3Int((newVoxels.m_extents.x - 1) - z, y, x), 
                                GetVal(new Vector3Int(x, y, z)));
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < m_extents.x; x++)
                {
                    for (int y = 0; y < m_extents.y; y++)
                    {
                        for (int z = 0; z < m_extents.z; z++)
                        {
                            newVoxels.SetVal(new Vector3Int(z, y, (newVoxels.m_extents.z - 1) - x), 
                                GetVal(new Vector3Int(x, y, z)));
                        }
                    }
                }
            }

            m_extents = newVoxels.m_extents;
            m_values = newVoxels.m_values;
        }

        public void RotateZ(bool ccw = false)
        {
            // Rotates the contents of this grid 90* about the Z axis.
            //
            // A swizzle is our friend
            VoxelGrid newVoxels = new VoxelGrid(new Vector3Int(m_extents.y, m_extents.x, m_extents.z));
            if (ccw)
            {
                for (int x = 0; x < m_extents.x; x++)
                {
                    for (int y = 0; y < m_extents.y; y++)
                    {
                        for (int z = 0; z < m_extents.z; z++)
                        {
                            newVoxels.SetVal(new Vector3Int((newVoxels.m_extents.x - 1) - y, x, z), 
                                GetVal(new Vector3Int(x, y, z)));
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < m_extents.x; x++)
                {
                    for (int y = 0; y < m_extents.y; y++)
                    {
                        for (int z = 0; z < m_extents.z; z++)
                        {
                            newVoxels.SetVal(new Vector3Int(y, (newVoxels.m_extents.y - 1) - x, z), 
                                GetVal(new Vector3Int(x, y, z)));
                        }
                    }
                }
            }

            m_extents = newVoxels.m_extents;
            m_values = newVoxels.m_values;
        }

        protected void Mirror(bool mirrorX, bool mirrorY, bool mirrorZ)
        {
            // Mirrors voxels across the specified direction(s).
            //
            VoxelGrid newVoxels = new VoxelGrid(m_extents, m_defaultValue);
            for (int x = 0; x < m_extents.x; x++)
            {
                for (int y = 0; y < m_extents.y; y++)
                {
                    for (int z = 0; z < m_extents.z; z++)
                    {
                        int newX = mirrorX ? (m_extents.x - 1) - x : x;
                        int newY = mirrorY ? (m_extents.y - 1) - y : y;
                        int newZ = mirrorZ ? (m_extents.z - 1) - z : z;
                        newVoxels.SetVal(new Vector3Int(newX, newY, newZ), 
                            GetVal(new Vector3Int(x, y, z)));
                    }
                }
            }
            m_values = newVoxels.m_values;
        }

        public void GenerateFromSeed(int seed, bool threshold=true, float bias=0)
        {
            // Randomly generate values from a seed.
            // 
            // Ensure deterministic randomness and save current random state to restore.
            UnityEngine.Random.State oldState = UnityEngine.Random.state;
            UnityEngine.Random.InitState(seed);
            // TODO: Expose perlin/fractal params?
            INoise perlin = new PerlinNoise(seed, .1f);
            FractalNoise fractal = new FractalNoise(perlin, 1, 1.2f);

            // TODO: Erode the fractal? Try and get a bubblier/veinier look? To test:
            //       Make a big (60x60x60) volume, remove the threshold op,
            //       and set the scale factor to 50 to see the noise on a more detailed level for tweaking

            //Fill voxels with values.
            for (int x = 0; x < m_extents.x; x++)
            {
                for (int y = 0; y < m_extents.y; y++)
                {
                    for (int z = 0; z < m_extents.z; z++)
                    {
                        // Threshold'd perlin noise. TODO: Expose scale?
                        float scaleFactor = .3f;
                        float fx = x / scaleFactor;
                        float fy = y / scaleFactor;
                        float fz = z / scaleFactor;

                        float val = fractal.Sample3D(fx, fy, fz);
                        if (threshold)
                        {
                            SetVal(new Vector3Int(x, y, z), 
                                val + bias > 0 ? 1 : -1);
                        }
                        else
                        {
                            SetVal(new Vector3Int(x, y, z), 
                                val);
                        }
                    }
                }
            }
            // Restore random state
            UnityEngine.Random.state = oldState;
        }
        
        public Vector3Int GetCoordinateClosestToPlane(Vector3 planePos, Vector3 planeNormal)
        {
            // Returns the closest coordinate with an interesting value to the given plane.
            float closestDist = Mathf.Infinity;
            Vector3Int closestCoord = Vector3Int.zero;

            for (int idx=0; idx < m_values.Length; idx++)
            {
                if (Math.Abs(m_values[idx] - m_defaultValue) < TOLERANCE)
                    continue;

                Vector3Int curCoord = IdxToCoord(idx);
                float curDist = Mathf.Abs(MyMath.DistanceToPlane(curCoord, planePos, planeNormal));
                if (curDist < closestDist)
                {
                    closestDist = curDist;
                    closestCoord = curCoord;
                }
                // Special case: if dist is ~0, we're not getting any closer. Break!
                if (closestDist < 1e-3)
                {
                    break;
                }
            }
            return closestCoord;
        }

        public Vector3Int GetCoordinateFurthestAlongNormal(Vector3 planeNormal)
        {
            // Returns the furthest coordinate with an interesting value from a plane cutting through the center of the voxel grid.
            float furthestDist = Mathf.NegativeInfinity;
            Vector3Int furthestCoord = Vector3Int.zero;

            for (int idx = 0; idx < m_values.Length; idx++)
            {
                if (Math.Abs(m_values[idx] - m_defaultValue) < TOLERANCE)
                    continue;

                Vector3Int curCoord = IdxToCoord(idx);
                float curDist = MyMath.DistanceToPlane(curCoord, (Vector3)m_extents / 2.0f, planeNormal);
                if (curDist > furthestDist)
                {
                    furthestDist = curDist;
                    furthestCoord = curCoord;
                }
            }
            return furthestCoord;
        }

        public void ClearPointsBeyondPlane(Vector3 planePos, Vector3 planeNormal)
        {
            for (int idx = 0; idx < m_values.Length; idx++)
            {
                // Don't process pts which are already clear
                if (Math.Abs(m_values[idx] - m_defaultValue) < TOLERANCE)
                    continue;
                // Clear points w/ a positive or 0 distance from plane
                if (MyMath.DistanceToPlane(IdxToCoord(idx), planePos, planeNormal) >= 0 - 1e-3)
                {
                    SetVal(idx, m_defaultValue);
                }
            }
        }
    }

}