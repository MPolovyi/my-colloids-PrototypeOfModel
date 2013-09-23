using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeModel
{
    class World
    {
        private Lattice[,] _grid;
        private int[,] _directions = {{0, 0},
                                         {-1,0},
                                         {0,1},
                                         {1,0},
                                         {0,-1},
                                         {-1,1},
                                         {1,1},
                                         {-1,1},
                                         {-1,-1}};

        public World()
        {
            _grid = new Lattice[10,10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    _grid[x, y] = new Lattice(x*10, y*10, false);
                }
            }
        }

        public Lattice[,] Live(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                try
                {
                    Generate();
                }
                catch (Exception e)
                {
                    
                }
            }

            return _grid;
        }

        private void Generate()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var nDens = Stream(x, y);
                    Collide(ref nDens, x, y);
                    _grid[x, y].NewF(nDens);
                }
            }
        }

        private void Collide(ref double[] nDens, int x, int y)
        {
            for (int i = 0; i < 9; i++)
            {
                nDens[i] = (_grid[x - _directions[i, 0], y - _directions[i, 1]].f()[i] -
                            _grid[x - _directions[i, 0], y - _directions[i, 1]].fEq()[i])/0.55;
            }
        }

        private double[] Stream(int x, int y)
        {
            double[] nDens = new double[8];
            for (int i = 0; i < 9; i++)
            {
                nDens[i] = _grid[x-_directions[i,0], y-_directions[i,1]].f()[i];
            }
            return nDens;
        }
    }
}
