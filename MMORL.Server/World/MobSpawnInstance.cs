using MMORL.Server.Entities;
using MMORL.Server.Entities.Ai;
using MMORL.Server.Net;
using MMORL.Shared.Util;
using MMORL.Shared.World;
using System;
using System.Linq;

namespace MMORL.Server.World
{
    public class MobSpawnInstance
    {
        private readonly MobSpawnDefinition _definition;
        private readonly ServerWorld _world;
        private readonly GameServer _server;

        private int _count;
        private readonly int _max = 4;
        // TODO: These values should live in the definition
        private readonly float _spawnInterval = 5f;

        private float _time;

        private readonly Random _random = new Random(); // TODO: Move into server;

        public MobSpawnInstance(MobSpawnDefinition definition, ServerWorld world, GameServer server)
        {
            _definition = definition;
            _world = world;
            _server = server;
        }

        public void Update(float delta)
        {
            _time += delta;

            if (_time >= _spawnInterval)
            {
                if (_count < _max)
                {
                    SpawnMob();
                }
                _time -= _spawnInterval;
            }
        }

        private void SpawnMob()
        {

            string toSpawn = _definition.Mobs.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(toSpawn))
            {
                _count++;

                Race race = new Race(toSpawn, toSpawn, GameColor.Blood, Energy.NormalSpeed);
                Mob mob = new Mob(_world.Map.Entities.Count, race, _server);
                new MobAi(mob);

                // TODO: Check for solid tiles.
                int x = _definition.X + _random.Next(_definition.Width);
                int y = _definition.Y + _random.Next(_definition.Height);

                _world.SpawnMob(mob, x, y);
            }
        }

        public static MobSpawnInstance FromDefinition(MobSpawnDefinition definition, ServerWorld world, GameServer server)
        {
            return new MobSpawnInstance(definition, world, server);
        }
    }
}
