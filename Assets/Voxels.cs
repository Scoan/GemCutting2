using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using ProceduralNoiseProject;
using MarchingCubesProject;

namespace Voxels
{
    [Flags]
    public enum AxisFlags
    {
        None = 0,
        posX = 1,
        posY = 2,
        posZ = 4,
        negX = 8,
        negY = 16,
        negZ = 32,
    }

    public class VoxelGrid
    {
        public Vector3Int extents;
        public float[] values;
        public float defaultValue;  // Newly initialized voxels default to this value.

        public VoxelGrid(Vector3Int _extents, float _defaultValue = -1)
        {
            extents = _extents;
            defaultValue = _defaultValue;
            values = Enumerable.Repeat(defaultValue, extents.x * extents.y * extents.z).ToArray();
        }
        public VoxelGrid(int _width, int _height, int _length, float _defaultValue = -1) : this(new Vector3Int(_width, _height, _length), _defaultValue) { }

        public int CoordToIdx(Vector3Int coord)
        {
            ///Converts a given (x,y,z) coordinate in the voxel grid to an index.
            // Catch out-of-range errors, return an invalid index.
            if (coord.x < 0 || coord.y < 0 || coord.z < 0)
            {
                return -1;
            }
            if (coord.x >= extents.x || coord.y >= extents.y || coord.z >= extents.z)
            {
                return -1;
            }
            return coord.x + coord.y * extents.x + coord.z * extents.x * extents.y;
        }
        // TODO: Deprecate?
        public int CoordToIdx(int x, int y, int z)
        {
            return CoordToIdx(new Vector3Int(x, y, z));
        }

        public Vector3Int IdxToCoord(int idx)
        {
            ///Converts a given index to (x,y,z) coordinates in the voxel grid.
            // TODO: Catch out-of-range errors
            int z = idx / (extents.x * extents.y);
            int zRem = (int)(idx % (extents.x * extents.y));
            int y = zRem / extents.x;
            int x = (int)(zRem % extents.x);
            //Debug.Log(string.Format("idx {0} returned {1}, {2}, {3} for grid of dimensions {4}, {5}, {6]", idx, pos[0], pos[1], pos[2], width, height, length));
            return new Vector3Int(x, y, z);
        }

        public float GetVal(int idx)
        {
            /// Returns the value at the specified index. If out of range, the default value is returned.
            if (idx < 0 || idx >= values.Length)
                return defaultValue;
            return values[idx];
        }

        public float GetVal(Vector3Int coordinate)
        {
            /// Returns the value at the specified position. If out of range, the default value is returned.
            return GetVal(CoordToIdx(coordinate));
        }
        // TODO: Deprecate?
        public float GetVal(int x, int y, int z) { return GetVal(new Vector3Int(x, y, z)); }

        public void SetVal(int idx, float val)
        {
            /// Sets the value at the specified index. If out of range, the value is ignored.
            if (idx < 0 || idx >= values.Length)
                return;
            values[idx] = val;
        }

        public void SetVal(Vector3Int coordinate, float val)
        {
            /// Sets the value at the specified position. If out of range, the value is ignored.
            SetVal(CoordToIdx(coordinate), val);
        }
        public void SetVal(int x, int y, int z, float val) { SetVal(new Vector3Int(x, y, z), val); }

        public void Shift(Vector3Int shiftVector, bool resize = false)
        {
            /// Offsets voxels in the supplied direction.
            /// If resize is false, values pushed off-grid will be discarded.
            /// TODO: If resize is true, resize the grid to accomodate interesting (non -1) values being pushed off grid?
            /// TODO: Support multiple resize behaviors? No resize (discard lost values) / resize / "loop" values pushed off back to other side?
            /// 
            // TODO: Can we do this in place? Would need to shift defaultValue in on sides where we've shifted away from the grid edge.
            float[] newValues = Enumerable.Repeat(defaultValue, extents.x * extents.y * extents.z).ToArray();
            // Fill newVoxels with offset values!
            for (int idx = 0; idx < values.Length; idx++)
            {
                int newIdx = CoordToIdx(IdxToCoord(idx) + shiftVector);

                // If we're moving off the grid, discard the data. But it better be dead data!
                // TODO: Take resize flag into account
                if ((newIdx < 0 || newIdx >= values.Length))
                {
                    if (values[idx] != defaultValue)
                    {
                        Debug.LogError(string.Format("Tried to push real data {0} from idx {1} to idx {2}, off the grid. BAD!", values[idx], idx, newIdx));
                    }
                    else
                        continue;
                }
                newValues[newIdx] = values[idx];
            }

            // Replace old voxels with new
            values = newValues;
        }


        //public float[] GetPlane(VoxelAxis axis, int atVal)
        //{
        //    /// Returns the values for the plane along [axis] at [atVal]
        //    /// 
        //    // How big is the plane along this axis?
        //    int planeDims;
        //    switch (axis)
        //    {
        //        case VoxelAxis.X: case VoxelAxis.NegX:
        //            planeDims = sizeY * sizeZ;
        //            break;
        //        case VoxelAxis.Y: case VoxelAxis.NegY:
        //            planeDims = sizeX * sizeZ;
        //            break;
        //        case VoxelAxis.Z: case VoxelAxis.NegZ:
        //            planeDims = sizeX * sizeY;
        //            break;
        //        default:
        //            Debug.LogError("Unhandled dimension!");
        //            planeDims = 0;
        //            break;
        //    }
        //
        //    float[] vals = new float[planeDims];
        //
        //    for (int y = 0; y < sizeY; y++)
        //    {
        //        for (int z = 0; z < sizeZ; z++)
        //        {
        //            vals[y + z * sizeY] = voxels[PosToIdx(x, y, z)];
        //        }
        //    }
        //    return vals;
        //}

        public float[] GetYZPlane(int x)
        {
            /// Returns the values for the YZ plane at X
            /// 
            float[] vals = new float[extents.y * extents.z];

            for (int y = 0; y < extents.y; y++)
            {
                for (int z = 0; z < extents.z; z++)
                {
                    vals[y + z * extents.y] = values[CoordToIdx(x, y, z)];
                }
            }
            return vals;
        }

        public float[] GetXZPlane(int y)
        {
            /// Returns the values for the XZ plane at Y
            /// 
            float[] vals = new float[extents.x * extents.z];

            for (int x = 0; x < extents.x; x++)
            {
                for (int z = 0; z < extents.z; z++)
                {
                    vals[x + z * extents.x] = values[CoordToIdx(x, y, z)];
                }
            }
            return vals;
        }

        public float[] GetXYPlane(int z)
        {
            /// Returns the values for the XY plane at Z
            /// 
            float[] vals = new float[extents.x * extents.y];

            for (int x = 0; x < extents.x; x++)
            {
                for (int y = 0; y < extents.y; y++)
                {
                    vals[x + y * extents.x] = values[CoordToIdx(x, y, z)];
                }
            }
            return vals;
        }

        public float[] GetXRow(int y, int z)
        {
            /// Returns the values for the X row at Y,Z
            /// 
            float[] vals = new float[extents.x];

            for (int x = 0; x < extents.x; x++)
            {
                vals[x] = GetVal(x, y, z);
            }
            return vals;
        }

        public float[] GetYRow(int x, int z)
        {
            /// Returns the values for the Y row at X,Z
            /// 
            float[] vals = new float[extents.y];

            for (int y = 0; y < extents.y; y++)
            {
                vals[y] = GetVal(x, y, z);
            }
            return vals;
        }

        public float[] GetZRow(int x, int y)
        {
            /// Returns the values for the X row at Y,Z
            /// 
            float[] vals = new float[extents.z];

            for (int z = 0; z < extents.z; z++)
            {
                vals[z] = GetVal(x, y, z);
            }
            return vals;
        }

        // TODO: Use Unity's Bounds class instead?
        void GetBounds(out int xMin, out int yMin, out int zMin, out int xMax, out int yMax, out int zMax)
        {
            /// Returns the bounding box where the voxel grid begins to have interesting (non default) values.
            /// 
            // TODO: Provide uninteresting value as an argument? In case we have to call this after changing that value.

            // Step through each plane until we hit a significant value.
            // We could get the min and max in one loop, but it's probably quicker to step in from each side and stop when we find the min/max respectively.
            // TODO: Might be able to step through a single axis and get all mins/maxes though (as we step through we'd track each axes.. but we'd have to step thru ENTIRE grid)

            // x
            xMin = 0;
            for (int x = 0; x < extents.x; x++)
            {
                float[] vals = GetYZPlane(x);
                if (vals.Any(val => val != defaultValue))
                {
                    xMin = x;
                    break;
                }
            }
            xMax = extents.x - 1;
            for (int x = extents.x - 1; x >= 0; x--)
            {
                float[] vals = GetYZPlane(x);
                if (vals.Any(val => val != defaultValue))
                {
                    xMax = x;
                    break;
                }
            }

            // y
            yMin = 0;
            for (int y = 0; y < extents.y; y++)
            {
                float[] vals = GetXZPlane(y);
                if (vals.Any(val => val != defaultValue))
                {
                    yMin = y;
                    break;
                }
            }
            yMax = extents.y - 1;
            for (int y = extents.y - 1; y >= 0; y--)
            {
                float[] vals = GetXZPlane(y);
                if (vals.Any(val => val != defaultValue))
                {
                    yMax = y;
                    break;
                }
            }

            // z
            zMin = 0;
            for (int z = 0; z < extents.z; z++)
            {
                float[] vals = GetXYPlane(z);
                if (vals.Any(val => val != defaultValue))
                {
                    zMin = z;
                    break;
                }
            }
            zMax = extents.z - 1;
            for (int z = extents.z - 1; z >= 0; z--)
            {
                float[] vals = GetXYPlane(z);
                if (vals.Any(val => val != defaultValue))
                {
                    zMax = z;
                    break;
                }
            }
        }

        public void CopyFrom(VoxelGrid srcGrid, int xOffset = 0, int yOffset = 0, int zOffset = 0, bool expandThis = false)
        {
            ///Copies the specified grid's voxels into this grid (with an optional offset).
            /// TODO: If expandThis is true, expands this grid to accomodate all incoming values. Else, only copies values that fit into this grid.
            if (expandThis)
            {
                // TODO: Expand grid to accomodate srcGrid's size
            }

            for (int x = 0; x < extents.x; x++)
            {
                for (int y = 0; y < extents.y; y++)
                {
                    for (int z = 0; z < extents.z; z++)
                    {
                        //voxels[PosToIdx(x, y, z)] = srcGrid.voxels[PosToIdx(x, y, z)];
                        SetVal(x + xOffset, y + yOffset, z + zOffset, srcGrid.GetVal(x, y, z));
                    }
                }
            }
        }

        void Justify()
        {
            ///Justifies interesting voxels to the origin of the grid.
            ///

            // Get bounds
            int xMin, yMin, zMin, xMax, yMax, zMax;
            GetBounds(out xMin, out yMin, out zMin, out xMax, out yMax, out zMax);

            // Shift voxels such that the bounds aligns with the grid edge
            Shift(new Vector3Int(-(xMin), -(yMin), -(zMin)));
        }

        public void Resize(Vector3Int newExtents)
        {
            ///Resizes the grid. Old values outside the new grid are discarded, new values outside the old grid are initialized to default.
            ///
            // TODO: A wholesale recreation and copy seems expensive. Can we resize in place?
            VoxelGrid newVoxels = new VoxelGrid(newExtents, defaultValue);
            newVoxels.CopyFrom(this);
            // Replace own contents with resized contents.
            extents = newVoxels.extents;
            values = newVoxels.values;
        }

        public void Trim()
        {
            ///Trims the voxel grid to the minimum size necessary to hold its interesting (non default) values.
            ///

            // Align with lower corner, resize grid to match the bounds size
            Justify();

            int xMin, yMin, zMin, xMax, yMax, zMax;
            GetBounds(out xMin, out yMin, out zMin, out xMax, out yMax, out zMax);
            Resize(new Vector3Int(xMax + 1, yMax + 1, zMax + 1));
        }

        public void RotateX(bool ccw = false)
        {
            ///TODO: Rotates the contents of this grid 90* about the X axis.
            ///
            // A swizzle is our friend
            VoxelGrid newVoxels = new VoxelGrid(extents.x, extents.z, extents.y);
            if (ccw)
            {
                for (int x = 0; x < extents.x; x++)
                {
                    for (int y = 0; y < extents.y; y++)
                    {
                        for (int z = 0; z < extents.z; z++)
                        {
                            newVoxels.SetVal(x, (newVoxels.extents.y - 1) - z, y, GetVal(x, y, z));
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < extents.x; x++)
                {
                    for (int y = 0; y < extents.y; y++)
                    {
                        for (int z = 0; z < extents.z; z++)
                        {
                            newVoxels.SetVal(x, z, (newVoxels.extents.z - 1) - y, GetVal(x, y, z));
                        }
                    }
                }
            }

            extents = newVoxels.extents;
            values = newVoxels.values;
        }

        public void RotateY(bool ccw = false)
        {
            ///TODO: Rotates the contents of this grid 90* about the Y axis.
            ///
            // A swizzle is our friend
            VoxelGrid newVoxels = new VoxelGrid(extents.z, extents.y, extents.x);
            if (ccw)
            {
                for (int x = 0; x < extents.x; x++)
                {
                    for (int y = 0; y < extents.y; y++)
                    {
                        for (int z = 0; z < extents.z; z++)
                        {
                            newVoxels.SetVal((newVoxels.extents.x - 1) - z, y, x, GetVal(x, y, z));
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < extents.x; x++)
                {
                    for (int y = 0; y < extents.y; y++)
                    {
                        for (int z = 0; z < extents.z; z++)
                        {
                            newVoxels.SetVal(z, y, (newVoxels.extents.z - 1) - x, GetVal(x, y, z));
                        }
                    }
                }
            }

            extents = newVoxels.extents;
            values = newVoxels.values;
        }

        public void RotateZ(bool ccw = false)
        {
            ///TODO: Rotates the contents of this grid 90* about the Z axis.
            ///
            // A swizzle is our friend
            VoxelGrid newVoxels = new VoxelGrid(extents.y, extents.x, extents.z);
            if (ccw)
            {
                for (int x = 0; x < extents.x; x++)
                {
                    for (int y = 0; y < extents.y; y++)
                    {
                        for (int z = 0; z < extents.z; z++)
                        {
                            newVoxels.SetVal((newVoxels.extents.x - 1) - y, x, z, GetVal(x, y, z));
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < extents.x; x++)
                {
                    for (int y = 0; y < extents.y; y++)
                    {
                        for (int z = 0; z < extents.z; z++)
                        {
                            newVoxels.SetVal(y, (newVoxels.extents.y - 1) - x, z, GetVal(x, y, z));
                        }
                    }
                }
            }

            extents = newVoxels.extents;
            values = newVoxels.values;
        }

        public void Mirror(bool mirrorX, bool mirrorY, bool mirrorZ)
        {
            ///Mirrors voxels across the specified direction(s).
            ///
            VoxelGrid newVoxels = new VoxelGrid(extents, defaultValue);
            for (int x = 0; x < extents.x; x++)
            {
                for (int y = 0; y < extents.y; y++)
                {
                    for (int z = 0; z < extents.z; z++)
                    {
                        int newX = mirrorX ? (extents.x - 1) - x : x;
                        int newY = mirrorY ? (extents.y - 1) - y : y;
                        int newZ = mirrorZ ? (extents.z - 1) - z : z;
                        newVoxels.SetVal(newX, newY, newZ, GetVal(x, y, z));
                    }
                }
            }
            values = newVoxels.values;
        }

        public void GenerateFromSeed(int seed, bool threshold=true, float bias=0)
        {
            /// Randomly generate values from a seed.
            /// 
            // Ensure deterministic randomness and save current random state to restore.
            UnityEngine.Random.State oldState = UnityEngine.Random.state;
            UnityEngine.Random.InitState(seed);
            // TODO: Expose perlin/fractal params?
            INoise perlin = new PerlinNoise(seed, .1f);
            FractalNoise fractal = new FractalNoise(perlin, 1, 1.2f);

            // TODO: Erode the fractal? Try and get a bubblier/veinier look? To test:
            // Make a big (60x60x60) volume, remove the threshold op, and set the scale factor to 50 to see the noise on a more detailed level for tweaking

            //Fill voxels with values.
            for (int x = 0; x < extents.x; x++)
            {
                for (int y = 0; y < extents.y; y++)
                {
                    for (int z = 0; z < extents.z; z++)
                    {
                        // Threshold'd perlin noise. TODO: Expose scale?
                        float scaleFactor = .3f;
                        float fx = x / scaleFactor;
                        float fy = y / scaleFactor;
                        float fz = z / scaleFactor;

                        float val = fractal.Sample3D(fx, fy, fz);
                        if (threshold)
                        {
                            SetVal(x, y, z, val + bias > 0 ? 1 : -1);
                        }
                        else
                        {
                            SetVal(x, y, z, val);
                        }
                    }
                }
            }
            // Restore random state
            UnityEngine.Random.state = oldState;
        }


        //public Vector3 GetBoundsIntersection(Vector3 planeNormal)
        //{
        //    Bounds testBounds = new Bounds((Vector3)extents * .5f, extents);
        //    //testBounds.min = (Vector3)extents * -.5f;
        //    //testBounds.max = (Vector3)extents * .5f;
        //    Ray theRay = new Ray((Vector3)extents * .5f, planeNormal.normalized);
        //    float distToIntersection = 0;
        //    if (testBounds.IntersectRay(theRay, out distToIntersection))
        //    {
        //        return planeNormal * distToIntersection;
        //    }
        //    Debug.LogError("Shouldn't get here! Failed to intersect against a VoxelGrid's bounds.");
        //    return Vector3.zero;
        //}


        public Vector3Int GetCoordinateClosestToPlane(Vector3 planePos, Vector3 planeNormal)
        {
            /// Returns the closest coordinate with an interesting value to the given plane.
            float closestDist = Mathf.Infinity;
            Vector3Int closestCoord = Vector3Int.zero;

            for (int idx=0; idx < values.Length; idx++)
            {
                if (values[idx] == defaultValue)
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

        public Vector3Int GetCoordinateFurthestAlongAngle(Vector3 planeNormal)
        {
            /// Returns the furthest coordinate with an interesting value from a plane cutting through the center of the voxel grid.
            float furthestDist = Mathf.NegativeInfinity;
            Vector3Int furthestCoord = Vector3Int.zero;

            for (int idx = 0; idx < values.Length; idx++)
            {
                if (values[idx] == defaultValue)
                    continue;

                Vector3Int curCoord = IdxToCoord(idx);
                float curDist = MyMath.DistanceToPlane(curCoord, (Vector3)extents / 2.0f, planeNormal);
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
            for (int idx = 0; idx < values.Length; idx++)
            {
                // Don't process pts which are already clear
                if (values[idx] == defaultValue)
                    continue;
                // Clear points w/ a positive or 0 distance from plane
                if (MyMath.DistanceToPlane(IdxToCoord(idx), planePos, planeNormal) >= 0 - 1e-3)
                {
                    SetVal(idx, defaultValue);
                }
            }
        }

        /*
        public Vector3Int GetFurthestValue(AxisFlags flags)
        {
            int xMin, yMin, zMin, xMax, yMax, zMax;
            GetBounds(out xMin, out yMin, out zMin, out xMax, out yMax, out zMax);
            switch (flags)
            {
                // Planar checks
                case AxisFlags.posX:
                case AxisFlags.posY:
                case AxisFlags.posZ:
                    return new Vector3Int(xMax, yMax, zMax);
                case AxisFlags.negX:
                case AxisFlags.negY:
                case AxisFlags.negZ:
                    GetBounds(out xMin, out yMin, out zMin, out xMax, out yMax, out zMax);
                    return new Vector3Int(xMin, yMin, zMin);

                // TODO: Edge checks (2 axes)
                // TODO: Corner checks (3 axes)
                // TODO: Handle bad cases (eg. 4+ axes, or pos AND neg X, etc.)

                default:
                    return new Vector3Int(0, 0, 0);
            }
        }
        */


        /*
        public void SawFace()
        {
            ///Saw off the front XY plane
            ///
            if (Gem == null)
                return;

            // Find the first plane with values. That's the one we need to saw!
            int firstPlane = 0;
            for (int z = 0; z < Gem.SizeZ; z++)
            {

                if (Gem.voxels.GetXYPlane(z).Any(val => val != Gem.voxels.defaultValue))
                {
                    firstPlane = z;
                    //Debug.Log(string.Format("sawing at Z = {0}", z));
                    //Debug.Log(string.Format("sawing at pos = {0}", Gem.CoordToWorldPos(0,0,z)));
                    break;
                }
            }

            // Clear the plane!
            for (int x = 0; x < Gem.SizeX; x++)
            {
                for (int y = 0; y < Gem.SizeY; y++)
                {
                    Gem.voxels.SetVal(x, y, firstPlane, Gem.voxels.defaultValue);
                }
            };
        }

        public void SawEdge()
        {
            ///Saw off the top right most diagonal
            ///
            // Need to find the first Z-row with a significant value to remove, and remove that whole diagonal.
            // A search like this:
            //   4   2    1
            //     ┘    ┘  
            //   7   5    3
            //          ┘
            //   9   8    6
            if (Gem == null)
                return;

            bool valueFound = false;
            int curDiagonal = 0;
            while (!valueFound && curDiagonal < (Gem.SizeX + Gem.SizeY))
            {
                int x;
                int y;
                // If we're still scanning across the top, the first coordinate is at the top
                if (curDiagonal < Gem.SizeX)
                {
                    y = Gem.SizeY - 1;
                    x = (Gem.SizeX - 1) - curDiagonal;
                }
                // Otherwise, the first coordinate is down the left side
                else
                {
                    y = (Gem.SizeY - 1) - (curDiagonal - Gem.SizeX);
                    x = 0;
                }

                // Step through this diagonal.
                while (x < Gem.SizeX && y >= 0)
                {
                    float[] curRow = Gem.voxels.GetZRow(x, y);
                    // If we find a value, start clearing out the diagonal. Note we've found a value so we end our search after this diagonal.
                    if (curRow.Any(val => val != Gem.voxels.defaultValue))
                    {
                        // Clear out this diagonal and be done with the sawing.
                        valueFound = true;
                        for (int z = 0; z < Gem.SizeZ; z++)
                        {
                            Gem.voxels.SetVal(x, y, z, Gem.voxels.defaultValue);
                        }
                    }

                    // Step down to next row in diagonal
                    x += 1;
                    y -= 1;
                }
                curDiagonal += 1;
            }
        }


        public void SawCorner()
        {
            /// Saw off the back right topmost corner
            /// 
            // Need to find the first corner slice with a significant value to remove, and remove that corner
            //
            // Start by searching along the top back row, then come to the front, then down the front left side
            //

            if (Gem == null)
                return;

            bool valueFound = false;
            int curSlice = 0;
            while (!valueFound && curSlice < (Gem.SizeX + Gem.SizeZ + Gem.SizeY - 2))
            {
                // Starting coordinates for a given slice
                int x;
                int y;
                int z;

                // If we're still scanning across the top, the first coordinate is along the top back
                if (curSlice < Gem.SizeX)
                {
                    x = (Gem.SizeX - 1) - curSlice;
                    y = Gem.SizeY - 1;
                    z = Gem.SizeZ - 1;
                }
                // If we're still scanning towards the front, the first coordinate is along the top left edge
                else if (curSlice < (Gem.SizeZ + Gem.SizeX - 1))
                {
                    x = 0;
                    y = Gem.SizeY - 1;
                    z = (Gem.SizeZ - 1) - (curSlice - (Gem.SizeX - 1));
                }
                // Otherwise, the first coordinate is down the front left edge
                else
                {
                    x = 0;
                    y = (Gem.SizeY - 1) - (curSlice - (Gem.SizeX + Gem.SizeZ - 2));
                    z = 0;
                }

                // Step through this diagonal.
                // Add to X & Z
                while (x < Gem.SizeX && y >= 0)
                {
                    // Check the current XZ line through the grid for values
                    int curX = x;
                    int curY = y;
                    int curZ = z;
                    while (curX < Gem.SizeX && curZ >= 0)
                    {
                        float val = Gem.voxels.GetVal(curX, curY, curZ);
                        if (val != Gem.voxels.defaultValue)
                        {
                            valueFound = true;
                            Gem.voxels.SetVal(curX, curY, curZ, Gem.voxels.defaultValue);
                        }
                        curX += 1;
                        curZ -= 1;
                    }

                    // Step down to next row in diagonal
                    // If we're along the back edge, this is (x+1, y-1). Otherwise, this is (z+1, y-1).
                    if (z == Gem.SizeZ - 1)
                    {
                        x += 1;
                        y -= 1;
                    }
                    else
                    {
                        z += 1;
                        y -= 1;
                    }

                }
                curSlice += 1;
            }
        }

        */




    }

}