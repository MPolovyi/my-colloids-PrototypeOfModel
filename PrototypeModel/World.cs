using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PrototypeModel
{
    class World
    {
        private int _gridSize = 10;
        private List<List<Lattice>> _grid; 
        //private Lattice[,] _grid;
        private Point[] _directions =   {new Point(0, 0),
                                         new Point(-1,0),
                                         new Point(0,1),
                                         new Point(1,0),
                                         new Point(0,-1),
                                         new Point(-1,1),
                                         new Point(1,1),
                                         new Point(-1,1),
                                         new Point(-1,-1)};
        private Bitmap WorldMap;

        public World()
        {
            _grid = new List<List<Lattice>>(_gridSize);
            for (int x = 0; x < _gridSize; x++)
            {
                List<Lattice> tempList = new List<Lattice>(_gridSize);
                for (int y = 0; y < _gridSize; y++)
                {
                    tempList.Add(new Lattice(x*1202/_gridSize, y*759/_gridSize, false));
                }
                _grid.Add(tempList);
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
                    List<Lattice> localLatticeBlock = GetNeighbours(x, y);
                    StreamAndCollide(localLatticeBlock);
                }
            }

            foreach (List<Lattice> lattices in _grid)
            {
                foreach (Lattice lattice in lattices)
                {
                    lattice.UpdateDensity();
                }
            }
        }

        private List<Lattice> GetNeighbours(int xId, int yId)
        {
            List<Lattice> neighbours = new List<Lattice>(9);
            foreach (var direction in _directions)
            {
                try
                {
                    neighbours.Add(_grid[xId-direction.X][yId-direction.Y]);
                }
                catch (Exception)
                {
                    neighbours.Add(null);
                }
            }
            return neighbours;
        } 

        private void StreamAndCollide(List<Lattice> latticeBlock)
        {
            for (int i = 0; i < 9; i++)
            {
                if (!latticeBlock[i].IsBoundary())
                {
                    double collision = (latticeBlock[0].f()[i] - latticeBlock[0].fEq()[i]) / 0.80;
                    double NewFi = latticeBlock[0].f()[i] - collision;
                    latticeBlock[i].NewF(NewFi,i);
                }
                else
                {
                    int j;
                    if (i==1 || i==2 || i==5 || i==6)
                    {
                        j = i + 2;
                    }
                    else
                    {
                        j = i - 2;
                    }
                    double collision = (latticeBlock[0].f()[j] - latticeBlock[0].fEq()[j]) / 0.80;
                    double NewFi = latticeBlock[0].f()[j] - collision;
                    latticeBlock[0].NewF(NewFi,j);
                }
            }
        }

        private void Collide(List<Lattice> latticeBlock)
        {
            double[] nDens = new double[8];
            for (int i = 1; i < 9; i++)
            {
                if (latticeBlock[i] != null)
                {
                    double collision = (latticeBlock[0].f()[i] - latticeBlock[0].fEq()[i])/0.55;
                    double NewFi = latticeBlock[i].f()[i] - collision;

                }
            }
        }

        private static Bitmap DrawBitmap(List<List<Lattice>> lattices)
        {
            Bitmap bmp = new Bitmap(1202, 759);
            Graphics canvas = Graphics.FromImage(bmp);

            Font timesFont = new Font("Times New Roman", 5.0f);
            timesFont = new Font(timesFont,FontStyle.Regular);
            Brush BlackBrush = new SolidBrush(Color.Black);
            foreach (var latticeString in lattices)
            {
                foreach (var lattice in latticeString)
                {
                    Pen pen = new Pen(BlackBrush);
                    float dens = 8*(float)lattice.GetMacroDensity();
                    //canvas.DrawString(str, timesFont, BlackBrush, lattice.Coordinates().X, lattice.Coordinates().Y);
                    canvas.DrawEllipse(pen, (float)lattice.Coordinates().X, (float)lattice.Coordinates().Y, dens, dens);
                    
                }
            }
            return bmp;
        }
    }
}
