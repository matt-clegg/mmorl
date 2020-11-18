using System;
using Toolbox;

namespace MMORL.Shared.Pathfinding
{
    public static class Heuristics
    {
        private static readonly double TwoSqrt = Math.Sqrt(2);

        public static float ManhattanDistance(Point2D a, Point2D b)
        {
            return ManhattanDistance(a.X, a.Y, b.X, b.Y);
        }

        public static float ManhattanDistance(int xa, int ya, int xb, int yb)
        {
            return Math.Abs(xa - xb) + Math.Abs(ya - yb);
        }

        public static float ChebyshevDistance(int xa, int ya, int xb, int yb)
        {
            int dx = Math.Abs(xb - xa);
            int dy = Math.Abs(yb - ya);
            return (dx + dy) - Math.Min(dx, dy);
        }

        public static float OctileDistance(int xa, int ya, int xb, int yb)
        {
            //int dx = Math.Abs( xb - xa );
            //int dy = Math.Abs( yb - ya );
            //return (float) (1 * ( dx + dy ) + ( TwoSqrt - 2 * 1 ) * Math.Min( dx, dy ));
            int dx = Math.Abs(xb - xa);
            int dy = Math.Abs(yb - ya);

            if (dx > dy)
            {
                return 14 * dy + 10 * (dx - dy);
            }

            return 14 * dx + 10 * (dy - dx);
        }

        public static float EuclideanDistance(Point2D a, Point2D b)
        {
            return EuclideanDistance(a.X, a.Y, b.X, b.Y);
        }

        public static float EuclideanDistance(int xa, int ya, int xb, int yb)
        {
            int dx = Math.Abs(xa - xb);
            int dy = Math.Abs(ya - yb);
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static float EuclideanDistanceSquared(Point2D a, Point2D b)
        {
            return EuclideanDistanceSquared(a.X, a.Y, b.X, b.Y);
        }

        public static float EuclideanDistanceSquared(int xa, int ya, int xb, int yb)
        {
            int dx = Math.Abs(xa - xb);
            int dy = Math.Abs(ya - yb);
            return dx * dx + dy * dy;
        }
    }
}
