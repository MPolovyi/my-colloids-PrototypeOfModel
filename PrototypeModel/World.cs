using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PrototypeModel
{
    class World
    {
        private int _gridSize = 20;
        private List<List<Lattice>> _grid;

        private string[] _map = {"++++++++++++++++++++",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "+      ++++++      +",
                                 "+      ++++++      +",
                                 "+      ++++++      +",
                                 "+      ++++++      +",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "+                  +",
                                 "++++++++++++++++++++"
                                 };

        private Point[] _directions =
            {
                new Point(0, 0),
                new Point(1, 0),
                new Point(0, -1),
                new Point(-1, 0),
                new Point(0, 1),
                new Point(1, -1),
                new Point(-1, -1),
                new Point(-1, 1),
                new Point(1, 1)
            };
        
        private Bitmap WorldMap;

        public World()
        {
            _grid = new List<List<Lattice>>(_gridSize);
            for (int x = 0; x < _gridSize; x++)
            {
                List<Lattice> tempList = new List<Lattice>(_gridSize);
                for (int y = 0; y < _gridSize; y++)
                {
                    if (_map[x][y]==' ')
                    {
                        tempList.Add(new Lattice(x*1202/_gridSize, y*759/_gridSize, false));
                    }
                    else
                    {
                        tempList.Add(new Lattice(x*1202/_gridSize, y*759/_gridSize, true));
                    }
                }
                _grid.Add(tempList);
            }
        }

        public Bitmap Live(int iter)
        {
            Generate();
            WorldMap = DrawBitmap(_grid,iter);
            return WorldMap;
        }

        private void Generate()
        {
            for (int x = 0; x < _gridSize; x++)
            {
                for (int y = 0; y < _gridSize; y++)
                {
                    if (!_grid[x][y].IsBoundary())
                    {
                        List<Lattice> localLatticeBlock = GetNeighbours(x, y);
                        StreamAndCollide(localLatticeBlock,x,y); 
                    }
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
                    neighbours.Add(_grid[xId+direction.X][yId+direction.Y]);
                }
                catch (Exception)
                {
                    neighbours.Add(null);
                }
            }
            return neighbours;
        } 

        private void StreamAndCollide(List<Lattice> latticeBlock,int x,int y)
        {
            for (int i = 0; i < 9; i++)
            {
                if (!latticeBlock[i].IsBoundary())
                {
                    double collision = (latticeBlock[0].f()[i] - latticeBlock[0].fEq()[i]) / 0.55;
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
                    double collision = (latticeBlock[0].f()[j] - latticeBlock[0].fEq()[j]) / 0.55;
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

        private static Bitmap DrawBitmap(List<List<Lattice>> lattices, int iter)
        {
            Bitmap bmp = new Bitmap(1202, 759);
            Graphics canvas = Graphics.FromImage(bmp);

            foreach (var latticeString in lattices)
            {
                foreach (var lattice in latticeString)
                {
                    Pen penBlack = new Pen(Color.Black,5);
                    Pen penBlue = new Pen(Color.Blue,5);
                    Pen penRed = new Pen(Color.Red);
                    //float dens = 8*(float)lattice.GetMacroDensity();
                    //canvas.DrawString(str, timesFont, BlackBrush, lattice.Coordinates().X, lattice.Coordinates().Y);

                    if (lattice.IsBoundary())
                    {
                        canvas.DrawEllipse(penBlack, (float) lattice.Coordinates().X - 1,
                                           (float) lattice.Coordinates().Y - 1, 2, 2);
                    }
                    else
                    {
                        canvas.DrawEllipse(penBlue, (float) lattice.Coordinates().X - 1,
                                           (float) lattice.Coordinates().Y - 1, 2, 2);
                    }
                    canvas.DrawLine(penRed, (float)lattice.Coordinates().X, (float)lattice.Coordinates().Y,
                                    (float) (lattice.Coordinates().X + 100*lattice.MacroVelocity()[0]),
                                    (float) (lattice.Coordinates().Y + 100*lattice.MacroVelocity()[1]));
                }

                canvas.DrawString(iter.ToString(), new Font("Arial",10), new SolidBrush(Color.Black), 10, 10);
            }
            return bmp;
        }
    }
}
