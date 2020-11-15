using MMORL.Server.Actions;
using MMORL.Server.Net;

namespace MMORL.Server.Entities.Ai
{
    public class MobAi
    {
        protected Mob Mob { get; }

        public MobAi(Mob mob)
        {
            Mob = mob;
            Mob.Ai = this;
        }

        public virtual BaseAction DecideNextAction()
        {
            
            if (GameServer.Random.NextDouble() < 0.25)
            {
                int x = 0;
                int y = 0;
                if (GameServer.Random.NextDouble() < 0.5)
                {
                    x = GameServer.Random.Next(-1, 2);
                }
                else
                {
                    y = GameServer.Random.Next(-1, 2);
                }

                if (x != 0 || y != 0)
                {
                    int nextX = x + Mob.X;
                    int nextY = y + Mob.Y;

                    if (Mob.Map.GetTile(nextX, nextY).IsSolid)
                    {
                        return null;
                    }

                    return new MoveAction(nextX, nextY);
                }

                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
