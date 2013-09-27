using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PrototypeModel
{
    public class Lattice
    {
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


        private int _xCoord, _yCoord;
        private double _macroDensity;
        private double[] _microDensity, _microDensityAfterTime, _weights, _microEqDensity;
        private bool _IsBoundary,_IsTransition;

        public Lattice(int x,int y,bool IsBoundary,bool IsTransition)
        {
            _xCoord = x;
            _yCoord = y;
            _IsBoundary = IsBoundary;
            _IsTransition = IsTransition;
            _macroDensity = MacroDensity(_xCoord, _yCoord);
            _microDensity = MicroDensity();
            _microDensityAfterTime = MicroDensity();
            _weights = Weights();
            _microEqDensity = MicroEqDensity();
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

        public void NewF(double nF,int i)
        {
            _microDensityAfterTime[i] = nF;
        }

        public void UpdateDensity()
        {
            _microDensity = _microDensityAfterTime;
        }

        private double MacroDensity(int x, int y)
        {
            return 1;
        }

        private double[] MicroDensity()
        {
            if (_xCoord>100 && _xCoord<200 && !_IsBoundary)
            {
                double[] tmp = {1/54.0, 45/54.0, 1/54.0, 1/54.0, 1/54.0, 1/54.0, 1/54.0, 1/54.0, 1/54.0};
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = _macroDensity * tmp[i];
                }

                return tmp;
            }
            else
            {
                double[] tmp = {1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0};
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = _macroDensity * tmp[i];
                }

                return tmp;
            }


        }

        private double[] MicroEqDensity()
        {
            double[] tmp = new double[9];
            double[] Velocity = MacroVelocity();
            if (_IsBoundary && !_IsTransition)
            {
                Velocity[0] = 0;
                Velocity[1] = 0;
            }
            for (int i = 0; i < 9; i++)
            {
                //TODO: КАК ЗАДАВАТЬ СКОРОСТЬ????????
                // omega_i * rho * (1 + 3(e_i,u) + 9*(e_i,u)^2 / 2 - 3*u^2/2), где (e_i,u) - скалярное произведение
                tmp[i] = _weights[i]*GetMacroDensity()*
                         (1 + 3*(_directions[i].X*Velocity[0] + _directions[i].Y*Velocity[1]) +
                          9*(Math.Pow(_directions[i].X*Velocity[0] + _directions[i].Y*Velocity[1], 2))/2 -
                          3*(Math.Pow(Velocity[0], 2) + Math.Pow(Velocity[1], 2))/2);
            }
            return tmp;
        }

        public double[] MacroVelocity()
        {
            double[] velocity = new double[2];
            
            double[] tmp1 = new double[9];
            for (int i = 0; i < 9; i++)
            {
                tmp1[i] = (1/GetMacroDensity())*_microDensity[i]*_directions[i].X;
            }

            velocity[0] = tmp1.Sum();
            for (int i = 0; i < 9; i++)
            {
                tmp1[i] = (1/GetMacroDensity())*_microDensity[i]*_directions[i].Y;
                
                }
            velocity[1] = tmp1.Sum();
            return velocity;
        }

        private double[] Weights()
        {
            double[] tmp = {4/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/36.0, 1/36.0, 1/36.0, 1/36.0};
            return tmp;
        }
    }
}
