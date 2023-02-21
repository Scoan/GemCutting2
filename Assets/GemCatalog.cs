using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Voxels;

namespace GemStoneCatalog
{
    /// <summary>
    ///  Catalog'd Gems
    /// </summary>
    /// 

    //public class GemPreset
    //{
    //    private static Vector3Int __extents;
    //    private static float[] __voxels = new float[] { };
    //
    //    public static VoxelGrid VoxelsFromPreset()
    //    {
    //        VoxelGrid grid = new VoxelGrid(__extents);
    //        grid.values = __voxels;
    //        return grid;
    //    }
    //}

    //public class AidennMask_ : GemPreset
    //{
    //    private static Vector3Int __extents = new Vector3Int(7, 3, 1);
    //    private static float[] __voxels = new float[] {-1, 1,  1, 1,  1, 1, -1,
    //                                                    1, 1, -1, 1, -1, 1,  1,
    //                                                   -1, 1,  1, 1,  1, 1, -1};
    //}

    // TODO: Make sure extents are updated!

    public class AidennMask : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(7, 3, 1);
        public static float[] __voxels = new float[] {-1, 1,  1, 1,  1, 1, -1,
                                                       1, 1, -1, 1, -1, 1,  1,
                                                      -1, 1,  1, 1,  1, 1, -1};

        public AidennMask() : base(__extents)
        {
            m_values = __voxels;
        }
    }

    public class Amanti : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 3);
        public static float[] __voxels = new float[] {-1, 1,-1,
                                                      -1, 1,-1,
                                                      -1, 1,-1,
                                                               -1, 1,-1,
                                                                1, 1, 1,
                                                               -1, 1,-1,
                                                                        -1, 1,-1,
                                                                        -1, 1,-1,
                                                                        -1, 1,-1,
                                                      };

        public Amanti() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Amunet : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 5, 6);
        public static float[] __voxels = new float[] {-1,-1,-1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                      -1,-1, 1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                                                    -1,-1, 1,-1,-1,
                                                                                    -1, 1, 1, 1,-1,
                                                                                     1, 1, 1, 1, 1,
                                                                                    -1, 1, 1, 1,-1,
                                                                                    -1,-1, 1,-1,-1,

                                                                                    -1,-1, 1,-1,-1,
                                                                                    -1, 1, 1, 1,-1,
                                                                                     1, 1, 1, 1, 1,
                                                                                    -1, 1, 1, 1,-1,
                                                                                    -1,-1, 1,-1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                      -1,-1, 1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                      -1,-1,-1,-1,-1,

                                                      };


        public Amunet() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Appolonia : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 6, 1);
        public static float[] __voxels = new float[] {-1, 1, -1, 1, -1,
                                                       1, 1,  1, 1,  1,
                                                       1, 1,  1, 1,  1,
                                                       1, 1,  1, 1,  1,
                                                      -1, 1,  1, 1, -1,
                                                      -1,-1,  1,-1, -1};

        public Appolonia() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Bastet : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 5, 1);
        public static float[] __voxels = new float[] {1,-1, 1,
                                                      1,-1, 1,
                                                      1, 1, 1,
                                                      1, 1, 1,
                                                     -1, 1,-1};

        public Bastet() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Bridge : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(6, 3, 1);
        public static float[] __voxels = new float[] {1, 1, 1, 1, 1, 1,
                                                     -1,-1, 1, 1,-1,-1,
                                                      1, 1, 1, 1, 1, 1};

        public Bridge() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class CatsEye : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 5, 2);
        public static float[] __voxels = new float[] {-1, 1, 1, 1,-1,
                                                       1, 1, 1, 1, 1,
                                                       1, 1,-1, 1, 1,
                                                       1, 1, 1, 1, 1,
                                                      -1, 1, 1, 1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1, 1,-1, 1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1,-1,-1,-1,-1,};

        public CatsEye() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, false, true);
        }
    }

    public class Choronzon : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 5, 3);
        public static float[] __voxels = new float[] {-1,-1, 1,-1,-1,
                                                      -1, 1, 1, 1,-1,
                                                      -1, 1, 1, 1,-1,
                                                      -1, 1, 1, 1,-1,
                                                      -1,-1, 1,-1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                      1, 1, 1, 1, 1,
                                                                      1, 1, 1, 1, 1,
                                                                      1, 1, 1, 1, 1,
                                                                     -1, 1, 1, 1,-1,
                                                      -1,-1, 1,-1,-1,
                                                      -1, 1, 1, 1,-1,
                                                      -1, 1, 1, 1,-1,
                                                      -1, 1, 1, 1,-1,
                                                      -1,-1, 1,-1,-1,
                                                     };

        public Choronzon() : base(__extents)
        {
            m_values = __voxels;
        }
    }

    public class CornerHeart : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(2, 2, 2);
        public static float[] __voxels = new float[] {1,-1,
                                                     -1, 1,
                                                             -1, 1,
                                                              1,-1};

        public CornerHeart() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Crater : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 2);
        public static float[] __voxels = new float[] {1,-1, 1,
                                                     -1,-1,-1,
                                                      1,-1, 1,
                                                              -1, 1,-1,
                                                               1, 1, 1,
                                                              -1, 1,-1

        };

        public Crater() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Dalessi : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(4, 3, 4);
        public static float[] __voxels = new float[] {-1, 1,-1,-1,
                                                       1, 1, 1,-1,
                                                       1, 1, 1, 1,
                                                                  -1,-1, 1,-1,
                                                                  -1, 1, 1, 1,
                                                                   1, 1, 1, 1,
                                                                              -1,-1,-1, 1,
                                                                              -1,-1, 1, 1,
                                                                              -1, 1, 1, 1,
                                                                                          -1,-1,-1,-1,
                                                                                          -1,-1,-1, 1,
                                                                                          -1,-1, 1, 1,

        };

        public Dalessi() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class DragynsEye : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(4, 4, 4);
        public static float[] __voxels = new float[] {1, 1, 1, 1,
                                                     -1, 1,-1, 1,
                                                     -1,-1, 1, 1,
                                                     -1,-1,-1, 1,
                                                                 -1, 1, 1, 1,
                                                                 -1,-1,-1,-1,
                                                                 -1,-1,-1, 1,
                                                                 -1,-1,-1,-1,
                                                                             -1,-1, 1, 1,
                                                                             -1,-1,-1, 1,
                                                                             -1,-1,-1,-1,
                                                                             -1,-1,-1,-1,
                                                                                         -1,-1,-1, 1,
                                                                                         -1,-1,-1,-1,
                                                                                         -1,-1,-1,-1,
                                                                                         -1,-1,-1,-1,};

        public DragynsEye() : base(__extents)
        {
            m_values = __voxels;
        }
    }

    public class Eyelet : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 2);
        public static float[] __voxels = new float[] {1, 1, 1,
                                                      1,-1, 1,
                                                      1, 1, 1,
                                                              1, 1, 1,
                                                              1,-1, 1,
                                                              1, 1, 1,
                                                                      };

        public Eyelet() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class FullAidennMask : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 3, 2);
        public static float[] __voxels = new float[] {1, 1, 1, 1, 1,
                                                      1,-1, 1,-1, 1,
                                                     -1, 1,-1, 1,-1,
                                                                    1, 1, 1, 1, 1,
                                                                    1,-1, 1,-1, 1,
                                                                   -1, 1, 1, 1,-1,
                                                                    };

        public FullAidennMask() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class FullEye : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 3);
        public static float[] __voxels = new float[] {1, 1, 1,
                                                      1,-1, 1,
                                                      1, 1, 1,
                                                              1, 1, 1,
                                                              1,-1, 1,
                                                              1, 1, 1,
                                                                      1, 1, 1,
                                                                      1,-1, 1,
                                                                      1, 1, 1,
                                                                              };

        public FullEye() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class HexasPlate : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 3);
        public static float[] __voxels = new float[] {-1, 1,-1,
                                                       1, 1,-1,
                                                      -1,-1,-1,
                                                               -1,-1, 1,
                                                               -1,-1, 1,
                                                                1, 1,-1,
                                                                        -1,-1,-1,
                                                                        -1,-1, 1,
                                                                        -1, 1,-1,



        };

        public HexasPlate() : base(__extents)
        {
            m_values = __voxels;
            Mirror(true, true, true);
        }
    }

    public class HilesChevrons : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 5, 3);
        public static float[] __voxels = new float[] {-1, 1, 1,-1,-1,
                                                       1, 1, 1, 1,-1,
                                                       1, 1, 1, 1, 1,
                                                      -1, 1, 1, 1, 1,
                                                      -1,-1, 1, 1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                                     -1, 1, 1,-1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1,-1, 1, 1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,
                                                                                    -1,-1, 1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,




        };

        public HilesChevrons() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, true);
        }
    }

    public class KhufusShip : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 5);
        public static float[] __voxels = new float[] {-1,-1,-1,
                                                      -1, 1,-1,
                                                      -1,-1,-1,
                                                               -1, 1,-1,
                                                                1, 1, 1,
                                                               -1, 1,-1,
                                                                         1, 1, 1,
                                                                         1, 1, 1,
                                                                        -1, 1,-1,
                                                                                  1, 1, 1,
                                                                                  1, 1, 1,
                                                                                 -1, 1,-1,
                                                                                          -1, 1,-1,
                                                                                           1, 1, 1,
                                                                                          -1,-1,-1,
};

        public KhufusShip() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class KingsTomb : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(7, 7, 4);
        public static float[] __voxels = new float[] {-1,-1,-1, 1,-1,-1,-1,
                                                      -1,-1, 1, 1, 1,-1,-1,
                                                      -1, 1, 1, 1, 1, 1,-1,
                                                       1, 1, 1,-1, 1, 1, 1,
                                                      -1, 1, 1, 1, 1, 1,-1,
                                                      -1,-1, 1, 1, 1,-1,-1,
                                                      -1,-1,-1, 1,-1,-1,-1,
                                                                           -1,-1,-1,-1,-1,-1,-1,
                                                                           -1,-1,-1, 1,-1,-1,-1,
                                                                           -1,-1, 1, 1, 1,-1,-1,
                                                                           -1, 1, 1, 1, 1, 1,-1,
                                                                           -1,-1, 1, 1, 1,-1,-1,
                                                                           -1,-1,-1, 1,-1,-1,-1,
                                                                           -1,-1,-1,-1,-1,-1,-1,
                                                                                                -1,-1,-1,-1,-1,-1,-1,
                                                                                                -1,-1,-1,-1,-1,-1,-1,
                                                                                                -1,-1,-1, 1,-1,-1,-1,
                                                                                                -1,-1, 1, 1, 1,-1,-1,
                                                                                                -1,-1,-1, 1,-1,-1,-1,
                                                                                                -1,-1,-1,-1,-1,-1,-1,
                                                                                                -1,-1,-1,-1,-1,-1,-1,
                                                                                                                     -1,-1,-1,-1,-1,-1,-1,
                                                                                                                     -1,-1,-1,-1,-1,-1,-1,
                                                                                                                     -1,-1,-1,-1,-1,-1,-1,
                                                                                                                     -1,-1,-1, 1,-1,-1,-1,
                                                                                                                     -1,-1,-1,-1,-1,-1,-1,
                                                                                                                     -1,-1,-1,-1,-1,-1,-1,
                                                                                                                     -1,-1,-1,-1,-1,-1,-1,



        };

        public KingsTomb() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Lens : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(4, 4, 1);
        public static float[] __voxels = new float[] {-1, 1, 1,-1,
                                                       1, 1, 1, 1,
                                                       1, 1, 1, 1,
                                                      -1, 1, 1,-1
        };

        public Lens() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class LookingGlass : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(4, 6, 1);
        public static float[] __voxels = new float[] {1, 1, 1, 1,
                                                      1, 1, 1, 1,
                                                      1, 1, 1, 1,
                                                      1, 1, 1, 1,
                                                      1, 1, 1, 1,
                                                      1, 1, 1, 1,
        };

        public LookingGlass() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Lotus : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 4, 3);
        public static float[] __voxels = new float[] {1,-1, 1,
                                                      1, 1, 1,
                                                     -1, 1,-1,
                                                     -1,-1,-1,
                                                              -1,-1,-1,
                                                               1,-1, 1,
                                                               1, 1, 1,
                                                              -1, 1,-1,
                                                                       1,-1, 1,
                                                                       1, 1, 1,
                                                                      -1, 1,-1,
                                                                      -1,-1,-1,


        };

        public Lotus() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class MoorishArch : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 5, 5);
        public static float[] __voxels = new float[] {-1,-1,-1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                       1,-1,-1,-1,-1,
                                                       1,-1,-1,-1,-1,
                                                                      -1,-1,-1,-1,-1,
                                                                      -1, 1,-1,-1,-1,
                                                                      -1, 1,-1,-1,-1,
                                                                      -1, 1,-1,-1,-1,
                                                                      -1, 1,-1,-1,-1,
                                                                                     -1,-1, 1,-1,-1,
                                                                                     -1,-1, 1,-1,-1,
                                                                                     -1,-1, 1,-1,-1,
                                                                                     -1,-1,-1,-1,-1,
                                                                                     -1,-1,-1,-1,-1,
                                                                                                    -1,-1,-1,-1,-1,
                                                                                                    -1,-1,-1, 1,-1,
                                                                                                    -1,-1,-1, 1,-1,
                                                                                                    -1,-1,-1, 1,-1,
                                                                                                    -1,-1,-1, 1,-1,
                                                                                                                    -1,-1,-1,-1,-1,
                                                                                                                    -1,-1,-1,-1,-1,
                                                                                                                    -1,-1,-1,-1,-1,
                                                                                                                    -1,-1,-1,-1, 1,
                                                                                                                    -1,-1,-1,-1, 1,

        };

        public MoorishArch() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class OsirisEye : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 3, 2);
        public static float[] __voxels = new float[] {-1, 1, 1, 1,-1,
                                                       1,-1,-1,-1, 1,
                                                      -1, 1, 1, 1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                                     -1, 1,-1, 1,-1,
                                                                     -1,-1,-1,-1,-1, };

        public OsirisEye() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Prism : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 3);
        public static float[] __voxels = new float[] {-1,-1, 1,
                                                      -1,-1, 1,
                                                      -1,-1, 1,
                                                               -1, 1, 1,
                                                               -1, 1, 1,
                                                               -1, 1, 1,
                                                                        1, 1, 1,
                                                                        1, 1, 1,
                                                                        1, 1, 1,
        };

        public Prism() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class QueensTomb : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 5, 3);
        public static float[] __voxels = new float[] {-1,-1, 1,-1,-1,
                                                      -1, 1, 1, 1,-1,
                                                       1, 1,-1, 1, 1,
                                                      -1, 1, 1, 1,-1,
                                                      -1,-1, 1,-1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                                     -1,-1, 1,-1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1,-1, 1,-1,-1,
                                                                     -1,-1,-1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,
                                                                                    -1,-1, 1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,

        };

        public QueensTomb() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class RasLantern : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 6, 3);
        public static float[] __voxels = new float[] {-1,-1,-1,
                                                      -1, 1,-1,
                                                       1,-1, 1,
                                                       1,-1, 1,
                                                      -1, 1,-1,
                                                      -1,-1,-1,
                                                               -1, 1,-1,
                                                                1, 1, 1,
                                                                1,-1, 1,
                                                                1, 1, 1,
                                                                1, 1, 1,
                                                               -1, 1,-1,
                                                                        -1,-1,-1,
                                                                        -1, 1,-1,
                                                                         1, 1, 1,
                                                                         1, 1, 1,
                                                                        -1, 1,-1,
                                                                        -1,-1,-1,
        };

        public RasLantern() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Reflection : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 2);
        public static float[] __voxels = new float[] {1, 1, 1,
                                                     -1, 1,-1,
                                                      1, 1, 1,
                                                              1, 1, 1,
                                                             -1, 1,-1,
                                                              1, 1, 1,
                                                                      };

        public Reflection() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Refractor : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(4, 3, 3);
        public static float[] __voxels = new float[] {1,-1,-1,-1,
                                                     -1, 1,-1,-1,
                                                     -1,-1, 1,-1,
                                                                 1, 1,-1,-1,
                                                                -1, 1, 1,-1,
                                                                -1,-1, 1, 1,
                                                                             1,-1,-1,-1,
                                                                            -1, 1,-1,-1,
                                                                            -1,-1, 1,-1,


        };

        public Refractor() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    // Underside of ring looks slightly different. WHY???
    public class Ring : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 3, 3);
        public static float[] __voxels = new float[] {-1, 1, 1, 1,-1,
                                                      -1,-1,-1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                                      1, 1,-1, 1, 1,
                                                                     -1, 1,-1, 1,-1,
                                                                     -1,-1, 1,-1,-1,
                                                                                    -1, 1, 1, 1,-1,
                                                                                    -1,-1,-1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,
                                                                                                    };

        public Ring() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class ShenOfHorus : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(6, 5, 3);
        public static float[] __voxels = new float[] {-1,-1,-1,-1,-1,-1,
                                                      -1,-1, 1, 1,-1,-1,
                                                      -1, 1,-1,-1, 1,-1,
                                                      -1,-1, 1, 1,-1,-1,
                                                      -1,-1,-1,-1,-1,-1,
                                                                        -1,-1, 1, 1,-1,-1,
                                                                        -1, 1, 1, 1, 1,-1,
                                                                         1, 1,-1,-1, 1, 1,
                                                                        -1, 1, 1, 1, 1,-1,
                                                                        -1,-1, 1, 1,-1,-1,
                                                                                          -1,-1,-1,-1,-1,-1,
                                                                                          -1,-1, 1, 1,-1,-1,
                                                                                          -1, 1,-1,-1, 1,-1,
                                                                                          -1,-1, 1, 1,-1,-1,
                                                                                          -1,-1,-1,-1,-1,-1,
                                                                                                            };

        public ShenOfHorus() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Solarkin : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(7, 4, 2);
        public static float[] __voxels = new float[] {-1,-1, 1, 1, 1,-1,-1,
                                                      -1, 1, 1,-1, 1, 1,-1,
                                                       1, 1,-1,-1,-1, 1, 1,
                                                      -1, 1, 1, 1, 1, 1,-1,
                                                                            -1,-1,-1,-1,-1,-1,-1,
                                                                            -1,-1, 1, 1, 1,-1,-1,
                                                                            -1, 1, 1, 1, 1, 1,-1,
                                                                            -1,-1,-1,-1,-1,-1,-1,};

        public Solarkin() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Sprocket : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(4, 4, 4);
        public static float[] __voxels = new float[] {-1,-1,-1,-1,
                                                      -1,-1,-1,-1,
                                                      -1, 1,-1,-1,
                                                      -1,-1,-1,-1,
                                                                  -1,-1,-1,-1,
                                                                   1, 1, 1,-1,
                                                                  -1, 1, 1,-1,
                                                                  -1,-1, 1,-1,
                                                                              -1, 1,-1,-1,
                                                                              -1, 1, 1,-1,
                                                                              -1, 1, 1, 1,
                                                                              -1,-1,-1,-1,
                                                                                          -1,-1,-1,-1,
                                                                                          -1,-1, 1,-1,
                                                                                          -1,-1,-1,-1,
                                                                                          -1,-1,-1,-1,};

        public Sprocket() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
            RotateY();
            RotateY();
            RotateY();
        }
    }

    public class Suspension : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 3);
        public static float[] __voxels = new float[] {1,-1,-1,
                                                     -1, 1,-1,
                                                     -1,-1, 1,
                                                              -1, 1,-1,
                                                               1,-1, 1,
                                                              -1,-1,-1,
                                                                        -1,-1, 1,
                                                                        -1, 1,-1,
                                                                         1,-1,-1,};

        public Suspension() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Symmetry : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 3);
        public static float[] __voxels = new float[] {-1, 1, -1,
                                                       1, 1,  1,
                                                      -1, 1, -1,
                                                                 1, 1,  1,
                                                                 1, 1,  1,
                                                                 1, 1,  1,
                                                                          -1, 1, -1,
                                                                           1, 1,  1,
                                                                          -1, 1, -1,
                                                                                    };

        public Symmetry() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    //TODO: Weird asymmetry at front. WHY???
    public class TheFox : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 4, 4);
        public static float[] __voxels = new float[] {-1,-1,-1,
                                                      -1,-1,-1,
                                                      -1,-1,-1,
                                                      -1, 1,-1,
                                                               -1,-1,-1,
                                                               -1,-1,-1,
                                                                1,-1, 1,
                                                               -1, 1,-1,
                                                                        -1,-1,-1,
                                                                         1, 1, 1,
                                                                         1,-1, 1,
                                                                        -1, 1,-1,
                                                                                  1,-1, 1,
                                                                                  1,-1, 1,
                                                                                 -1,-1,-1,
                                                                                 -1,-1,-1,

        };

        public TheFox() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
            RotateZ();
        }
    }

    public class TheShark : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 3, 3);
        public static float[] __voxels = new float[] {-1,-1, 1,
                                                      -1,-1,-1,
                                                       1,-1,-1,
                                                               -1, 1,-1,
                                                                1,-1, 1,
                                                               -1, 1,-1,
                                                                        1,-1,-1,
                                                                       -1,-1,-1,
                                                                       -1,-1, 1,
                                                                              };

        public TheShark() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Thistle : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 4, 3);
        public static float[] __voxels = new float[] {-1,-1,-1,
                                                      -1, 1,-1,
                                                      -1, 1,-1,
                                                      -1,-1,-1,
                                                               -1, 1,-1,
                                                                1, 1, 1,
                                                                1, 1, 1,
                                                               -1, 1,-1,
                                                                        -1,-1,-1,
                                                                        -1, 1,-1,
                                                                        -1, 1,-1,
                                                                        -1,-1,-1,
                                                                                 };

        public Thistle() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    // TODO: Weird asymmetry around holes. WHY???
    public class ThothsKnot : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(3, 4, 3);
        public static float[] __voxels = new float[] {-1, 1,-1,
                                                       1, 1, 1,
                                                       1,-1, 1,
                                                      -1,-1,-1,
                                                               -1, 1,-1,
                                                               -1,-1,-1,
                                                                1,-1, 1,
                                                                1, 1, 1,
                                                                        -1, 1,-1,
                                                                         1, 1, 1,
                                                                         1,-1, 1,
                                                                        -1,-1,-1,
                                                                                 };

        public ThothsKnot() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    // Nose and eyes are wrong. Top of mouth is wrong. WHY???
    public class TikiMask : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 7, 3);
        public static float[] __voxels = new float[] {-1,-1,-1,-1,-1,
                                                      -1,-1, 1,-1,-1,
                                                      -1,-1, 1,-1,-1,
                                                      -1,-1, 1,-1,-1,
                                                      -1,-1, 1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                      -1,-1,-1,-1,-1,
                                                                     -1,-1, 1,-1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1,-1, 1,-1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1,-1, 1,-1,-1,
                                                                                    -1,-1, 1,-1,-1,
                                                                                    -1, 1, 1, 1,-1,
                                                                                     1, 1, 1, 1, 1,
                                                                                     1,-1, 1,-1, 1,
                                                                                    -1, 1, 1, 1,-1,
                                                                                    -1,-1, 1,-1,-1,
                                                                                    -1,-1,-1,-1,-1,

        };

        public TikiMask() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Tombstone : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(7, 6, 1);
        public static float[] __voxels = new float[] {-1,-1, 1, 1, 1,-1,-1,
                                                      -1, 1, 1, 1, 1, 1,-1,
                                                       1, 1, 1,-1, 1, 1, 1,
                                                       1,-1,-1,-1,-1,-1, 1,
                                                       1, 1, 1,-1, 1, 1, 1,
                                                      -1, 1, 1,-1, 1, 1,-1 };

        public Tombstone() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Triclops : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(7, 3, 1);
        public static float[] __voxels = new float[] {-1, 1, 1, 1, 1, 1,-1,
                                                       1,-1, 1,-1, 1,-1, 1,
                                                      -1, 1, 1, 1, 1, 1,-1,};

        public Triclops() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class Unity : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 5, 3);
        public static float[] __voxels = new float[] {-1,-1, 1,-1,-1,
                                                      -1, 1, 1, 1,-1,
                                                       1, 1,-1, 1, 1,
                                                      -1, 1, 1, 1,-1,
                                                      -1,-1, 1,-1,-1,
                                                                     -1,-1, 1,-1,-1,
                                                                     -1, 1, 1, 1,-1,
                                                                      1, 1,-1, 1, 1,
                                                                     -1, 1, 1, 1,-1,
                                                                     -1,-1, 1,-1,-1,
                                                                                    -1,-1, 1,-1,-1,
                                                                                    -1, 1, 1, 1,-1,
                                                                                     1, 1,-1, 1, 1,
                                                                                    -1, 1, 1, 1,-1,
                                                                                    -1,-1, 1,-1,-1,
                                                                                                    };

        public Unity() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public class TEST_GEM : Voxels.VoxelGrid
    {
        private static Vector3Int __extents = new Vector3Int(5, 4, 4);
        public static float[] __voxels = new float[] { -1, -1,  1, -1, -1,
                                                       -1, -1,  1, -1, -1,
                                                       -1, -1, -1, -1, -1,
                                                       -1, -1, -1, -1, -1,
                                                                          -1, -1,  1, -1, -1,
                                                                          -1, -1,  1, -1, -1,
                                                                          -1, -1,  1, -1, -1,
                                                                          -1, -1, -1, -1, -1,
                                                                                             -1,  1,  1,  1, -1,
                                                                                             -1, -1,  1, -1, -1,
                                                                                             -1,  1,  1,  1, -1,
                                                                                             -1, -1,  1, -1, -1,
                                                                                                                 1,  1,  1,  1,  1,
                                                                                                                 1, -1,  1, -1,  1,
                                                                                                                -1,  1,  1,  1, -1,
                                                                                                                -1, -1,  1, -1, -1,


        };

        public TEST_GEM() : base(__extents)
        {
            m_values = __voxels;
            Mirror(false, true, false);
        }
    }

    public enum GemTypes {  CUSTOM,
                            AidennMask,
                            Amanti,
                            Amunet,
                            Appolonia,
                            Bastet,
                            Bridge,
                            CatsEye,
                            Choronzon,
                            CornerHeart,
                            Crater,
                            Dalessi,
                            DragynsEye,
                            Eyelet,
                            FullAidennMask,
                            FullEye,
                            HexasPlate,
                            HilesChevrons,
                            KhufusShip,
                            KingsTomb,
                            Lens,
                            LookingGlass,
                            Lotus,
                            MoorishArch,
                            OsirisEye,
                            Prism,
                            QueensTomb,
                            RasLantern,
                            Reflection,
                            Refractor,
                            Ring,
                            ShenOfHorus,
                            Solarkin,
                            Sprocket,
                            Suspension,
                            Symmetry,
                            TheFox,
                            TheShark,
                            Thistle,
                            ThothsKnot,
                            TikiMask,
                            Tombstone,
                            Triclops,
                            Unity};

}