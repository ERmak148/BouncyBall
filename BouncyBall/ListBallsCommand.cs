using System;
using System.Collections.Generic;
using System.Text;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace BouncyBall
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]

    public class ListBallsCommand : ICommand, IUsageProvider
    {
        public string Command { get; } = "listballs";

        public string[] Aliases { get; } = { };

        public string Description { get; } = "Lists all registered balls";

        public bool SanitizeResponse { get; } = false;

        public string[] Usage { get; } = { };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("er.bouncyball"))
            {
                response = "You do not have permissions to use this command";
                return false;
            }
            Log.Info("1");
            Dictionary<ushort, Ball> balls = BallRegistry.GetAllBalls;

            if (balls.Count < 1)
            {
                response = "No balls registered";
                return true;
            }
            Log.Info("2");
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<ushort, Ball> kv in balls)
            {
                Log.Info("3");
                Vector3 ballCoords = kv.Value.gameObject.transform.position;
                Log.Info("4");
                Room room = Room.Get(ballCoords);
                string roomName = "Unknown";
                string zoneName = "Unknown";
                Log.Info("5");
                if (room != null)
                {
                    Log.Info("6");
                    roomName = room.Type.ToString();
                    zoneName = room.Zone.ToString();
                }
                
                builder.AppendLine($"{kv.Key} - {ballCoords.ToPreciseString()} ({roomName}, {zoneName})");
            }
            
            response = $"Balls list:\n{builder.ToString()}";
            return true;
        }
    }
}