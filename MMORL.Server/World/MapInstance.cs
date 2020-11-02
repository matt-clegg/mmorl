using MMORL.Shared.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMORL.Server.World
{
    public class MapInstance
    {
        public Map Map { get; }

        public MapInstance(Map map)
        {
            Map = map;
        }
    }
}
