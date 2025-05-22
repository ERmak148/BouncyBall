using System.Collections.Generic;

namespace BouncyBall
{
    public class EventsHandler
    {
        public void OnWaitingForPlayers()
        {
            foreach (KeyValuePair<ushort, Ball> kv1 in new Dictionary<ushort, Ball>(BallRegistry.GetAllBalls))
            {
                BallRegistry.UnregisterBall(kv1.Key);
            }
        }
    }
}