using Lidgren.Network;
using MMORL.Server.Actions;
using MMORL.Server.Entities;
using MMORL.Server.Net;
using MMORL.Shared;
using MMORL.Shared.Entities;
using MMORL.Shared.Net;
using MMORL.Shared.World;
using System.Collections.Generic;

namespace MMORL.Server.World
{
    public class ServerWorld : GameWorld
    {
        private readonly float _turnTime;
        private float _time;

        private readonly GameServer _server;

        private readonly List<MobSpawnInstance> _spawners = new List<MobSpawnInstance>();

        private readonly List<Player> _players = new List<Player>();

        public ServerWorld(Map map, float turnTime, GameServer server) : base(map)
        {
            _turnTime = turnTime;
            _server = server;

            foreach (MobSpawnDefinition spawnDefinition in map.Spawns)
            {
                _spawners.Add(MobSpawnInstance.FromDefinition(spawnDefinition, this, server));
            }
        }

        public override void Update(float delta)
        {
            _time += delta;

            if (_time >= _turnTime)
            {
                _time -= _time;
                DoTurn();
            }

            foreach (MobSpawnInstance spawn in _spawners)
            {
                spawn.Update(delta);
            }

            base.Update(delta);
        }

        public void SpawnMob(Entity entity, int x, int y)
        {
            AddEntity(entity, x, y);
            SpawnEntityMessage message = new SpawnEntityMessage(entity, x, y, EntityType.Mob);
            _server.SendMessageToAll(message, NetDeliveryMethod.ReliableUnordered);
        }

        public override void AddEntity(Entity entity, int x, int y)
        {
            if (entity is Player player)
            {
                _players.Add(player);
            }
            base.AddEntity(entity, x, y);
        }

        public override void RemoveEntity(int entityId)
        {
            _players.RemoveAll(p => p.Id == entityId);
            base.RemoveEntity(entityId);
        }

        private void DoTurn()
        {
            foreach (Entity entity in Map.Entities)
            {
                entity.ProcessTurn();
            }
        }

        public void QueueMovement(int playerId, int x, int y)
        {
            foreach (Player player in _players)
            {
                if (player.Id == playerId)
                {
                    MoveAction action = new MoveAction(x, y);
                    player.QueueAction(action);
                    return;
                }
            }
        }

        public void ClearMoves(int playerId)
        {
            foreach (Player player in _players)
            {
                if (player.Id == playerId)
                {
                    player.ClearMoves();
                    return;
                }
            }
        }
    }
}
