using MMORL.Shared.Entities;
using MMORL.Shared.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMORL.Client.Entities
{
    public class Player : Creature
    {
        public Player(int id, string name, string sprite, GameColor color, int speed) : base(id, name, sprite, color, speed)
        {
        }
    }
}
