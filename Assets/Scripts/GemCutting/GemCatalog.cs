using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using GemCutting;

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

    public class AidennMask : VoxelGrid
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

    public class Amanti : VoxelGrid
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

    public class Amunet : VoxelGrid
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

    public class Appolonia : VoxelGrid
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

    public class Bastet : VoxelGrid
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

    public class Bridge : VoxelGrid
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

    public class CatsEye : VoxelGrid
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

    public class Choronzon : VoxelGrid
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

    public class CornerHeart : VoxelGrid
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

    public class Crater : VoxelGrid
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

    public class Dalessi : VoxelGrid
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

    public class DragynsEye : VoxelGrid
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

    public class Eyelet : VoxelGrid
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

    public class FullAidennMask : VoxelGrid
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

    public class FullEye : VoxelGrid
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

    public class HexasPlate : VoxelGrid
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

    public class HilesChevrons : VoxelGrid
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

    public class KhufusShip : VoxelGrid
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

    public class KingsTomb : VoxelGrid
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

    public class Lens : VoxelGrid
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

    public class LookingGlass : VoxelGrid
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

    public class Lotus : VoxelGrid
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

    public class MoorishArch : VoxelGrid
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

    public class OsirisEye : VoxelGrid
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

    public class Prism : VoxelGrid
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

    public class QueensTomb : VoxelGrid
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

    public class RasLantern : VoxelGrid
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

    public class Reflection : VoxelGrid
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

    public class Refractor : VoxelGrid
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
    public class Ring : VoxelGrid
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

    public class ShenOfHorus : VoxelGrid
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

    public class Solarkin : VoxelGrid
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

    public class Sprocket : VoxelGrid
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

    public class Suspension : VoxelGrid
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

    public class Symmetry : VoxelGrid
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
    public class TheFox : VoxelGrid
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

    public class TheShark : VoxelGrid
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

    public class Thistle : VoxelGrid
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
    public class ThothsKnot : VoxelGrid
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
    public class TikiMask : VoxelGrid
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

    public class Tombstone : VoxelGrid
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

    public class Triclops : VoxelGrid
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

    public class Unity : VoxelGrid
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

    public class TEST_GEM : VoxelGrid
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