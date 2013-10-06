﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PrototypeModel
{
    public class Lattice
    {

        public List<Lattice> neighbours; 

        private Vector<double>[] _directions =
            {
                new Vector<double>(0, 0),
                new Vector<double>(1, 0),
                new Vector<double>(0, -1),
                new Vector<double>(-1, 0),
                new Vector<double>(0, 1),
                new Vector<double>(1, -1),
                new Vector<double>(-1, -1),
                new Vector<double>(-1, 1),
                new Vector<double>(1, 1)
            };


        private Vector<double> _outerForce;
        private int _xCoord, _yCoord;
        private double _macroDensity;
        private double[] _microDensity, _microDensityAfterTime, _weights, _microEqDensity;
        private bool _IsBoundary,_IsTransition;

        public Lattice(int x,int y,bool IsBoundary,bool IsTransition, double force)
        {
            _outerForce = new Vector<double>(0,-force);
            _xCoord = x;
            _yCoord = y;
            _IsBoundary = IsBoundary;
            _IsTransition = IsTransition;
            _macroDensity = MacroDensity(_xCoord, _yCoord);
            _microDensity = MicroDensity();
            _microDensityAfterTime = MicroDensity();
            _weights = Weights();
            _microEqDensity = MicroEqDensity();
            neighbours = new List<Lattice>(9);
        }

        public Point SpeedToDraw()
        {
            return new Point(_xCoord,_yCoord+Speed());
        }

        public int Speed()
        {
            double spd = Math.Pow(Math.Pow(MacroVelocity().X, 2) + Math.Pow(MacroVelocity().Y, 2),0.5);
            return (int) (40 * spd/_outerForce.Module());
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

        public double[] Force()
        {
            double[] tmp = new double[_directions.Length];

            for (int i = 0; i < tmp.Length; i++)
            {
                if (_yCoord<500)
                {
                    Random rnd = new Random();
                    
                    double someRndY = rnd.NextDouble();
                    Vector<double> NewForce = new Vector<double>( _outerForce.X , _outerForce.Y);
                    tmp[i] = _weights[i] * (NewForce * _directions[i]);
                }
                else
                {
                    Vector<double> NewForce = new Vector<double>(_outerForce.X, _outerForce.Y);
                    tmp[i] = _weights[i] * (NewForce * _directions[i]);
                }

            }

            return tmp;
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
            if (_xCoord>200 && _xCoord<220 && _yCoord>200 && _yCoord<800)
            {
                //double[] tmp = {1/54.0, 20/54.0, 26/54.0, 1/54.0, 1/54.0, 1/54.0, 1/54.0, 1/54.0, 1/54.0};
                double[] tmp = {1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0};
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
            double[] tmp = new double[_directions.Length];
            Vector<double> Velocity = MacroVelocity();
            if (_IsBoundary && !_IsTransition)
            {
                Velocity.X = 0;
                Velocity.Y = 0;
            }
            for (int i = 0; i < _directions.Length; i++)
            {
                // omega_i * rho * (1 + 3(e_i,u) + 9*(e_i,u)^2 / 2 - 3*u^2/2), где (e_i,u) - скалярное произведение
                tmp[i] = _weights[i]*GetMacroDensity()*
                         (1 + 3*(_directions[i]*Velocity) +
                          9*(Math.Pow(_directions[i]*Velocity, 2))/2 - 3*(Velocity*Velocity)/2);
            }
            return tmp;
        }

        public Vector<double> MacroVelocity()
        {
            Vector<double> velocity = new Vector<double>();
            
            double[] tmp1 = new double[_directions.Length];
            for (int i = 1; i < _directions.Length; i++)
            {
                tmp1[i] = (1/GetMacroDensity())*_microDensity[i]*_directions[i].X;
            }

            velocity.X = tmp1.Sum();
            for (int i = 1; i < _directions.Length; i++)
            {
                tmp1[i] = (1/GetMacroDensity())*_microDensity[i]*_directions[i].Y;
                
                }
            velocity.Y = tmp1.Sum();
            return velocity;
        }

        private double[] Weights()
        {
            double[] tmp = {4/9.0, 1/9.0, 1/9.0, 1/9.0, 1/9.0, 1/36.0, 1/36.0, 1/36.0, 1/36.0};
            return tmp;
        }
    }
}
