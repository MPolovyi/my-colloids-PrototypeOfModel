using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototypeModel
{
    public class Vector<Type> where Type : struct, IComparable, IComparable<Type>, IEquatable<Type>
    {
        public Type X { get; set; }
        public Type Y { get; set; }

        public Vector(Type _x, Type _y)
        {
            X = _x;
            Y = _y;
        }

        public Vector()
        {
            X = (dynamic)(double)0;
            Y = (dynamic)(double)0;
        }

        public static Vector<Type> operator +(Vector<Type> vect1, Vector<Type> vect2)
        {
            return new Vector<Type>((dynamic)vect1.X + vect2.X, (dynamic)vect1.Y + vect2.Y);
        }

        public static Type operator *(Vector<Type> vect1, Vector<Type> vect2)
        {
            return (dynamic)vect1.X * vect2.X + (dynamic)vect1.Y * vect2.Y;
        }

        public static Type DotProduct(Vector<Type> vect1, Vector<Type> vect2)
        {
            return (dynamic)vect1.X * vect2.X + (dynamic)vect1.Y * vect2.Y;
        } 

        public double Module()
        {
            return Math.Pow(Math.Pow((dynamic)X, 2) + Math.Pow((dynamic)Y, 2), 0.5);
        }

        public Vector<Type> CrossProduct(Vector<Type> vect1)
        {
            throw new NotImplementedException();
            return new Vector<Type>((dynamic)1, (dynamic)1);
        }

    }
}
