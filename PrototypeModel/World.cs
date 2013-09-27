using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrototypeModel
{
    class World
    {
        private Point _gridSize;
        private List<List<Lattice>> _grid;

        private string[] _map =
            {
                "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                ">                                                                                                  >",
                "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"
            };

/*                   \
 *                    \ 
 *   |-----------------  X
 *   |                /
 *   |               /
 *   |   6   2   5   
 *   |    \  |  /
 *   |   3 - 0 - 1
 *   |    /  |  \
 *   |   7   4   8
 *\  |  /
 * \ | /
 *  \|/
 * 
 *   Y
 * 
 * 
 */

        private Point[] _directions =
            {
                new Point(0, 0),
                new Point(-1, 0),
                new Point(0, 1),
                new Point(1, 0),
                new Point(0, -1),
                new Point(-1, 1),
                new Point(1, 1),
                new Point(1, -1),
                new Point(-1, -1)
            };
        
        private Bitmap WorldMap;

        public World()
        {
            _gridSize.X = _map[0].Length;
            _gridSize.Y = _map.Length;
            _grid = new List<List<Lattice>>(_gridSize.X);
            for (int x = 0; x < _gridSize.X; x++)
            {
                List<Lattice> tempList = new List<Lattice>(_gridSize.Y);
                for (int y = 0; y < _gridSize.Y; y++)
                {
                    if (_map[y][x] == ' ')
                    {
                        tempList.Add(new Lattice(x*1202/_gridSize.X, y*640/_gridSize.Y, false, false));
                    }
                    else if (_map[y][x] == '>')
                    {
                        tempList.Add(new Lattice(x*1202/_gridSize.X, y*640/_gridSize.Y, false, true));
                    }
                    else
                    {
                        tempList.Add(new Lattice(x*1202/_gridSize.X, y*640/_gridSize.Y, true, false));
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
            for (int x = 0; x < _gridSize.X; x++)
            {
                for (int y = 0; y < _gridSize.Y; y++)
                {
                    if (!_grid[x][y].IsBoundary() || _map[y][x]=='>')
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
            int lenX = _gridSize.X - 1;
            int lenY = _gridSize.Y - 1;
            List<Lattice> neighbours = new List<Lattice>(9);
            foreach (var direction in _directions)
            {
                try
                {
                    neighbours.Add(_grid[xId - direction.X][yId + direction.Y]);
                }
                catch (Exception)
                {
                    if (xId == 0)
                    {
                        neighbours.Add(_grid[xId - direction.X + lenX][yId + direction.Y]);
                    }
                    else if (xId == (lenX))
                    {
                        neighbours.Add(_grid[xId - direction.X - lenX][yId + direction.Y]);
                    }
                    else if (yId == 0)
                    {
                        neighbours.Add(_grid[xId - direction.X][yId + direction.Y + lenY]);
                    }
                    else if (yId == (lenY))
                    {
                        neighbours.Add(_grid[xId - direction.X][yId + direction.Y - lenY]);
                    }
                    else
                    {
                        neighbours.Add(null);
                    }
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
                else if (_map[y][x]=='+')
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
