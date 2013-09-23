using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PrototypeModel
{
    public class Lattice
    {
        private int _xCoord, _yCoord;
        private double _macroDensity;
        private double[] _microDensity, _microVelocity, _weights, _microEqDensity;
        private bool _IsBoundary;

        public Lattice(int x,int y,bool IsBoundary)
        {
            _xCoord = x;
            _yCoord = y;
            _macroDensity = MacroDensity(_xCoord, _yCoord);
            _microDensity = MicroDensity();
            _weights = Weights();
            _microEqDensity = MicroEqDensity();
            _IsBoundary = IsBoundary;
        }

        public bool IsBoundary()
        {
            return _IsBoundary;
        }

        public double GetMacroDensity()
        {
            return _microDensity.Sum();
        }

        public Point Coordinates()
        {
            return new Point(_xCoord,_yCoord);
        }

        public double[] f()
        {
            return _microDensity;
        }

        public double[] fEq()
        {
            return _microEqDensity;
        }

        public void NewF(double[] nF)
        {
            _microDensity = nF;
        }

        private double MacroDensity(int x, int y)
        {
            //return Math.Sqrt(x^2+y^2);
            return 1;
        }

        private double[] MicroDensity()
        {

            double[] tmp = {1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0};

            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = _macroDensity*tmp[i];
            }

            return tmp;
        }

        private double[] MicroEqDensity()
        {
            double[] tmp = new double[9];

            for (int i = 0; i < 8; i++)
            {
                tmp[i] = _weights[i]*(1 + 3 + 9/2 - 3/2);
            }
            return tmp;
        }

        private double[] Weights()
        {
            double[] tmp = {4/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/36.0, 1/36.0, 1/36.0, 1/36.0};
            return tmp;
        }
    }
}
