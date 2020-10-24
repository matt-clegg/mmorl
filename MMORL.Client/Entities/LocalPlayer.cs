using Microsoft.Xna.Framework.Input;
using MMORL.Client.Util;
using MMORL.Shared.Entities;
using MMORL.Shared.Util;
using System.Collections.Generic;
using Toolbox;

namespace MMORL.Client.Entities
{
    public class LocalPlayer : Entity
    {
        private readonly List<Point2D> _movementQueue = new List<Point2D>();
        public IReadOnlyCollection<Point2D> MovementQueue => _movementQueue.AsReadOnly();

        private int _lastDx;
        private int _lastDy;

        public LocalPlayer(int id, string name, string sprite, GameColor color) : base(id, name, sprite, color)
        {
        }

        public void Input(Keys key)
        {
            if (Controls.North.IsPressed(key)) QueueMove(0, -1);
            else if (Controls.South.IsPressed(key)) QueueMove(0, 1);
            else if (Controls.East.IsPressed(key)) QueueMove(1, 0);
            else if (Controls.West.IsPressed(key)) QueueMove(-1, 0);
        }

        private void QueueMove(int dx, int dy)
        {
            Point2D next = new Point2D(X, Y);

            if (_movementQueue.Count != 0)
            {
                next = _movementQueue[_movementQueue.Count - 1];

                bool dirChange = _lastDx != dx || _lastDy != dy;

                System.Console.WriteLine("lastx " + _lastDx + " , lasty " + _lastDy + " | dx " + dx + " , dx " + dy + " | dirchange: " + dirChange);
                
                // TODO: This works but is buggy.
                // If you have queued ONE move, then try and move in a different direction, the one move will be cleared.
                if ((dx != 0 && _lastDx == dx * -1) || (dy != 0 && _lastDy == dy * -1) || dirChange)
                {

                    if (_movementQueue.Count >= 2)
                    {
                        Point2D last = _movementQueue[_movementQueue.Count - 1];
                        Point2D lastLast = _movementQueue[_movementQueue.Count - 2];
                        int toRemoveDx = lastLast.X - last.X;
                        int toRemoveDy = lastLast.Y - last.Y;

                        System.Console.WriteLine("to remove dir " + toRemoveDx + "," + toRemoveDy);

                        if ((dx != 0 && toRemoveDx == dx) || (dy != 0 && toRemoveDy == dy) || dirChange)
                        {
                            _movementQueue.RemoveAt(_movementQueue.Count - 1);
                            System.Console.WriteLine("removing last move");
                            return;
                        }
                    }
                    else
                    {
                        _movementQueue.RemoveAt(_movementQueue.Count - 1);
                        System.Console.WriteLine("removing last move");
                        return;
                    }
                }
            }

            _lastDx = dx;
            _lastDy = dy;
            _movementQueue.Add(next + new Point2D(dx, dy));
        }
    }
}
