using MMORL.Shared;
using MMORL.Shared.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMORL.Server.World
{
    public class ServerWorld : GameWorld
    {
        private readonly float _turnTime;
        private float _time;

        public ServerWorld(Map map, float turnTime) : base(map) 
        {
            _turnTime = turnTime;
        }

        public override void Update(float delta)
        {
            _time += delta;

            if(_time >= _turnTime)
            {
                _time -= _time;
                DoTurn();
            }

            base.Update(delta);
        }

        private void DoTurn()
        {
        }
    }
}
