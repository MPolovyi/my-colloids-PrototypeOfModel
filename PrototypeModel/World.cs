﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrototypeModel
{
    internal class World
    {
        private Point _gridSize;
        private List<List<Lattice>> _grid;
        private double _scale;


        private string[] _map =
            {
                "+++++>>>>>>>>>>>>>>>>+++++",
                "+   +                +   +",
                "+   +                +   +",
                "+   +                +   +",
                "+   +                +   +",
                "+   +                +   +",
                "+   +                +   +",
                "+   +                +   +",
                "+   +                +   +",
                "++++++ + + + + + + + +++++",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                ">                        >",
                "+>>>>>>>>>>>>>>>>>>>>>>>>+"
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
                new Point(1, 0),
                new Point(0, -1),
                new Point(-1, 0),
                new Point(0, 1),
                new Point(1, -1),
                new Point(-1,-1),
                new Point(-1, 1),
                new Point(1, 1)
            };

        private Bitmap WorldMap;

        public World(int heights, int width, double force, double scale)
        {
            if ((int)scale == 20)
            {
                _scale = 40/force;
            }
            else
            {
                _scale = scale;
            }
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
                        tempList.Add(new Lattice(x*width/_gridSize.X, y*heights/_gridSize.Y, false, false, force));
                    }
                    else if (_map[y][x] == '>')
                    {
                        tempList.Add(new Lattice(x*width/_gridSize.X, y*heights/_gridSize.Y, false, true,force));
                    }
                    else
                    {
                        tempList.Add(new Lattice(x*width/_gridSize.X, y*heights/_gridSize.Y, true, false,force));
                    }
                }
                _grid.Add(tempList);
            }

            for (int x = 0; x < _gridSize.X; x++)
            {
                for (int y = 0; y < _gridSize.Y; y++)
                {

                    _grid[x][y].neighbours = GetNeighbours(x, y);
                }
            }
        }

        public Bitmap Live(int iter)
        {
            Generate();
            WorldMap = DrawBitmap(_grid, iter);
            return WorldMap;
        }

        private void Generate()
        {
            for (int x = 0; x < _gridSize.X; x++)
            {
                for (int y = 0; y < _gridSize.Y; y++)
                {
                    StreamAndCollide(_grid[x][y].neighbours, x, y);
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
            int lenX = _gridSize.X-1;
            int lenY = _gridSize.Y-1;
            List<Lattice> neighbours = new List<Lattice>(9);
            foreach (var direction in _directions)
            {
                try
                {
                    neighbours.Add(_grid[xId + direction.X][yId + direction.Y]);
                }
                catch (Exception)
                {
                    if (_map[yId][xId]=='>')
                    {
                        if (xId == 0)
                        {
                            neighbours.Add(_grid[xId + lenX][yId + direction.Y]);
                        }
                        if (xId == (lenX))
                        {
                            neighbours.Add(_grid[xId - lenX][yId + direction.Y]);
                        }
                        if (yId == 0)
                        {
                            neighbours.Add(_grid[xId + direction.X][yId + lenY]);
                        }
                        if (yId == (lenY))
                        {
                            neighbours.Add(_grid[xId + direction.X][yId - lenY]);
                        }
                    }
                    else
                    {
                        neighbours.Add(null);
                    }
                }

            }
            return neighbours;
        }

        private void StreamAndCollide(List<Lattice> latticeBlock, int x, int y)
        {
            for (int i = 0; i < _directions.Length; i++)
            {
                try
                {
                    if (!latticeBlock[i].IsBoundary())
                    {
                        double collision = (latticeBlock[0].f()[i] - latticeBlock[0].fEq()[i] + latticeBlock[0].Force()[i])/5;
                        double NewFi = latticeBlock[0].f()[i] - collision;
                        latticeBlock[i].NewF(NewFi, i);
                    }
                    else if (latticeBlock[i].IsBoundary())
                    {
                        int j=0;
                        if (i == 1 || i == 2 || i == 5 || i == 6)
                        {
                            j = i + 2;
                        }
                        else if (i!=0)
                        {
                            j = i - 2;
                        }
                        double collision = (latticeBlock[0].f()[i] - latticeBlock[0].fEq()[i] + latticeBlock[0].Force()[i]) /5;
                        double NewFi = latticeBlock[0].f()[i] -collision;
                        latticeBlock[i].NewF(NewFi, j);
                    }
                }
                catch (Exception){}
            }
        }

        private Bitmap DrawBitmap(List<List<Lattice>> lattices, int iter)
        {
            Bitmap bmp = new Bitmap(1202, 759);
            Graphics canvas = Graphics.FromImage(bmp);
            Pen penBlack = new Pen(Color.Black, 5);
            Pen penBlue = new Pen(Color.Blue, 3);
            Pen penRed = new Pen(Color.Red);
            Pen penGreen = new Pen(Color.Green);

            foreach (var latticeString in lattices)
            {
                foreach (var lattice in latticeString)
                {
                    //float dens = 8*(float)lattice.GetMacroDensity();
                    //canvas.DrawString(str, timesFont, BlackBrush, lattice.Coordinates().X, lattice.Coordinates().Y);

                    if (lattice.IsBoundary())
                    {
                        canvas.DrawEllipse(penBlack, (float) lattice.Coordinates().X - 1,
                                           (float) lattice.Coordinates().Y - 1, 1, 1);
                    }
                    else
                    {
                        canvas.DrawEllipse(penBlue, (float) lattice.Coordinates().X - 1,
                                           (float) lattice.Coordinates().Y - 1, 1, 1);
                    }

                    //for (int i = 1; i < _directions.Length; i++)
                    //{
                    //    canvas.DrawLine(penGreen, (float)lattice.Coordinates().X, (float)lattice.Coordinates().Y,
                    //                    (float)(lattice.Coordinates().X + 100 * lattice.f()[i] * _directions[i].X),
                    //                    (float)(lattice.Coordinates().Y + 100 * lattice.f()[i] * _directions[i].Y));
                    //}

                    canvas.DrawLine(penRed, (float) lattice.Coordinates().X, (float) lattice.Coordinates().Y,
                                    (float) (lattice.Coordinates().X + _scale*lattice.MacroVelocity().X),
                                    (float) (lattice.Coordinates().Y + _scale*lattice.MacroVelocity().Y));

                }

                
                List<Point> y2Velosity = new List<Point>();
                for (int i = 0; i < _grid.Count; i++)
                {
                    y2Velosity.Add(_grid[i][10].SpeedToDraw());
                }

                for (int i = 1; i < y2Velosity.Count; i++)
                {
                    canvas.DrawLine(new Pen(Color.Aqua), y2Velosity[i - 1], y2Velosity[i]);
                }
                
                canvas.DrawString((iter+1).ToString(), new Font("Arial", 10), new SolidBrush(Color.Black), 10, 10);
            }
            return bmp;
        }

        public Bitmap InitialCondition()
        {
            return DrawBitmap(_grid, 0);
        }
    }
}
