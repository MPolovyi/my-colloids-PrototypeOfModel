using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PrototypeModel
{
    class World
    {
        private int _gridSize = 5;
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
        private Bitmap WorldMap;

        public World()
        {
            _grid = new Lattice[_gridSize, _gridSize];
            for (int x = 0; x < _gridSize; x++)
            {
                for (int y = 0; y < _gridSize; y++)
                {
                    _grid[x, y] = new Lattice(x*1202/_gridSize, y*759/_gridSize, false);
                }
            }
        }

        public Bitmap Live()
        {
            Generate();
            WorldMap = DrawBitmap(_grid);
            return WorldMap;
        }

        private void Generate()
        {
            for (int x = 1; x < _gridSize-1; x++)
            {
                for (int y = 1; y < _gridSize-1; y++)
                {
                    var nDens = Stream(x, y);
                    Collide(ref nDens, x, y);
                    _grid[x, y].NewF(nDens);
                }
            }
        }

        private void Collide(ref double[] nDens, int x, int y)
        {
            for (int i = 1; i < 8; i++)
            {
                nDens[i] = (_grid[x - _directions[i, 0], y - _directions[i, 1]].f()[i] -
                            _grid[x - _directions[i, 0], y - _directions[i, 1]].fEq()[i]) / 0.55;
            }
        }

        private double[] Stream(int x, int y)
        {
            double[] nDens = new double[8];
            for (int i = 1; i < 8; i++)
            {
                nDens[i] = _grid[x-_directions[i,0], y-_directions[i,1]].f()[i];
            }
            return nDens;
        }

        private static Bitmap DrawBitmap(Lattice[,] lattices)
        {
            Bitmap bmp = new Bitmap(1202, 759);
            Graphics canvas = Graphics.FromImage(bmp);

            Font timesFont = new Font("Times New Roman", 10.0f);
            Brush BlackBrush = new SolidBrush(Color.Black);
            foreach (var lattice in lattices)
            {
                string str = lattice.GetMacroDensity().ToString();
                canvas.DrawString(str, timesFont, BlackBrush, lattice.Coordinates().X, lattice.Coordinates().Y); 
                Pen pen = new Pen(Color.Aqua);
            }
            return bmp;
        }
    }
}
