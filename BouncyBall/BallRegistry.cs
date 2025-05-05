using System;
using System.Collections.Generic;

namespace BouncyBall
{
    public class BallRegistry
    {
        private static Dictionary<ushort, Ball> balls = new Dictionary<ushort, Ball>();

        private static ushort nextId
        {
            get
            {
                int nextValue = balls.Keys.Count + 1;
                if (nextValue > ushort.MaxValue)
                {
                    throw new OverflowException("Cannot generate a new ID. Maximum ushort value (65,535) reached. Um.. Why your server is still alive?");
                }
                return (ushort)nextValue;
            }
        }
        
        public static void RegisterBall(Ball comp)
        {
            balls.Add(nextId, comp);
        }

        public static Ball GetBallById(ushort id)
        {
            if (balls.TryGetValue(id, out Ball ball))
            {
                return ball;
            }

            return null;
        }

        public static Dictionary<ushort, Ball> GetAllBalls => balls;

        public static void UnregisterBall(ushort id)
        {
            balls.Remove(id);
        }
    }
}