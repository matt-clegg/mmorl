using MMORL.Shared.Entities;
using MMORL.Shared.World;
using System;

namespace MMORL.Shared
{
    public class GameWorld
    {
        public Map Map { get; private set; }

        public event EventHandler<Entity> EntityAddedEvent;

        public GameWorld(int chunkSize)
        {
            Map = new Map(chunkSize);
        }

        public GameWorld(Map map)
        {
            Map = map;
        }

        public virtual void Update(float delta)
        {
            Map.CleanupEntities();

            foreach (Entity entity in Map.Entities)
            {
                entity.Update(delta);
            }
        }

        public virtual void AddEntity(Entity entity, int x, int y)
        {
            Map.Add(entity, x, y);
            EntityAddedEvent?.Invoke(this, entity);
        }

        public virtual void RemoveEntity(int entityId)
        {
            Map.Remove(entityId);
        }
    }
}
