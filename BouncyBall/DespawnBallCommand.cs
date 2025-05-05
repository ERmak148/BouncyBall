using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.Permissions.Extensions;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BouncyBall
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]

    public class DespawnBallCommand : ICommand, IUsageProvider
    {
        public string Command { get; } = "despawnball";

        public string[] Aliases { get; } = { };

        public string Description { get; } = "Despawns the registered ball";

        public bool SanitizeResponse { get; } = false;

        public string[] Usage { get; } = { "BallId/*" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("er.bouncyball"))
            {
                response = "You do not have permissions to use this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: despawnball BallId/*";
                return false;
            }

            if (arguments.At(0) == "*")
            {
                foreach (KeyValuePair<ushort, Ball> kv1 in new Dictionary<ushort, Ball>(BallRegistry.GetAllBalls))
                {
                    BallRegistry.UnregisterBall(kv1.Key);
            
                    NetworkServer.Destroy(kv1.Value.LightSource.gameObject);
                    NetworkServer.Destroy(kv1.Value.gameObject);
                    Object.Destroy(kv1.Value);
                }
                response = "All balls successfully despawned";
                return true;
            }
            
            if (!ushort.TryParse(arguments.At(0), out ushort id))
            {
                response = "BallId must be a ushort number (Max: 65535)";
                return false;
            }

            Ball ball = BallRegistry.GetBallById(id);

            if (ball == null)
            {
                response = "Ball with this id does not exists. Use 'listballs' command for check balls list";
                return false;
            }
            
            BallRegistry.UnregisterBall(id);
            
            NetworkServer.Destroy(ball.LightSource.gameObject);
            NetworkServer.Destroy(ball.gameObject);
            Object.Destroy(ball);
            
            response = "Ball successfully despawned";
            return true;
        }
    }
}