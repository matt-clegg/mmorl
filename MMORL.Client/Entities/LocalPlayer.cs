using MMORL.Shared.Entities;
using MMORL.Shared.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMORL.Client.Entities
{
    public class LocalPlayer : Entity
    {
        public LocalPlayer(int id, string name, string sprite, GameColor color) : base(id, name, sprite, color)
        {
        }
    }
}
