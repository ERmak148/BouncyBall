using System;
using System.Linq;
using AdminToys;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Permissions.Extensions;
using MEC;
using Mirror;
using UnityEngine;
using UserSettings;
using UserSettings.VideoSettings;

namespace BouncyBall
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnBallCommand : ICommand, IUsageProvider
    {
        public string Command { get; } = "spawnball";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Spawn a bouncy ball with custom parameters";
        public bool SanitizeResponse { get; } = false;

        public string[] Usage { get; } = {
            "PlayerId",          // 0
            "SizeX",             // 1
            "SizeY",             // 2
            "SizeZ",             // 3
            "LightIntensity",    // 4
            "LightRange",        // 5
            "Mass",              // 6
            "Drag",              // 7
            "Interpolation",     // 8
            "Bounciness",        // 9
            "DynamicFriction",   // 10
            "StaticFriction",    // 11
            "BounceCombine",     // 12
            "ChangeSpeed",       // 13
            "VerticalDivider",   // 14
            "MaxAngle",          // 15
            "MaxDistance",       // 16
            "KickForceMul",      // 17
            "KickForceUp",       // 18
            "CheckOnlyDistance", // 19
            "DestroyDoors",      // 20
            "DestroyMaxDistance" // 21
        };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("er.bouncyball"))
            {
                response = "You do not have permissions to use this command";
                return false;
            }
            
            if (arguments.Count < Usage.Length)
            {
                response = $"Usage: {Command} " + string.Join(" ", Usage.Select(u => $"<{u}>'"));
                return false;
            }

            string FormatOptions<T>() where T : Enum => string.Join(", ", Enum.GetNames(typeof(T)));

            if (!int.TryParse(arguments.At(0), out int playerId))
            {
                response = $"Invalid PlayerId '{arguments.At(0)}'. Expected an integer.";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float sizeX))
            {
                response = $"Invalid SizeX '{arguments.At(1)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(2), out float sizeY))
            {
                response = $"Invalid SizeY '{arguments.At(2)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(3), out float sizeZ))
            {
                response = $"Invalid SizeZ '{arguments.At(3)}'. Expected a floating-point number.";
                return false;
            }
            var size = new Vector3(sizeX, sizeY, sizeZ);

            if (!float.TryParse(arguments.At(4), out float lightIntensity))
            {
                response = $"Invalid LightIntensity '{arguments.At(4)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(5), out float lightRange))
            {
                response = $"Invalid LightRange '{arguments.At(5)}'. Expected a floating-point number.";
                return false;
            }

            if (!float.TryParse(arguments.At(6), out float mass))
            {
                response = $"Invalid Mass '{arguments.At(6)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(7), out float drag))
            {
                response = $"Invalid Drag '{arguments.At(7)}'. Expected a floating-point number.";
                return false;
            }

            if (!Enum.TryParse(arguments.At(8), true, out RigidbodyInterpolation interpolation)
                || !Enum.IsDefined(typeof(RigidbodyInterpolation), interpolation))
            {
                response = $"Invalid Interpolation '{arguments.At(8)}'. Expected one of: {FormatOptions<RigidbodyInterpolation>()}.";
                return false;
            }

            if (!float.TryParse(arguments.At(9), out float bounciness))
            {
                response = $"Invalid Bounciness '{arguments.At(9)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(10), out float dynamicFric))
            {
                response = $"Invalid DynamicFriction '{arguments.At(10)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(11), out float staticFric))
            {
                response = $"Invalid StaticFriction '{arguments.At(11)}'. Expected a floating-point number.";
                return false;
            }
            
            if (!Enum.TryParse(arguments.At(12), true, out PhysicsMaterialCombine bounceCombine)
                || !Enum.IsDefined(typeof(PhysicsMaterialCombine), bounceCombine))
            {
                response = $"Invalid BounceCombine '{arguments.At(12)}'. Expected one of: {FormatOptions<PhysicsMaterialCombine>()}.";
                return false;
            }

            if (!float.TryParse(arguments.At(13), out float changeSpeed))
            {
                response = $"Invalid ChangeSpeed '{arguments.At(13)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(14), out float verticalDivider))
            {
                response = $"Invalid VerticalDivider '{arguments.At(14)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(15), out float maxAngle))
            {
                response = $"Invalid MaxAngle '{arguments.At(15)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(16), out float maxDistance))
            {
                response = $"Invalid MaxDistance '{arguments.At(16)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(17), out float kickForceMul))
            {
                response = $"Invalid KickForceMul '{arguments.At(17)}'. Expected a floating-point number.";
                return false;
            }
            if (!float.TryParse(arguments.At(18), out float kickForceUp))
            {
                response = $"Invalid KickForceUp '{arguments.At(18)}'. Expected a floating-point number.";
                return false;
            }
            
            if (!bool.TryParse(arguments.At(19), out bool checkOnlyDistance))
            {
                response = $"Invalid CheckOnlyDistance '{arguments.At(19)}'. Expected 'true' or 'false'.";
                return false;
            }
            
            if (!bool.TryParse(arguments.At(20), out bool destroyDoors))
            {
                response = $"Invalid DestroyDoors '{arguments.At(20)}'. Expected 'true' or 'false'.";
                return false;
            }
            
            if (!float.TryParse(arguments.At(21), out float destroyDoorMaxDistance))
            {
                response = $"Invalid DestroyMaxDistance '{arguments.At(21)}'. Expected a floating-point number.";
                return false;
            }

            Player target = Player.Get(playerId);
            Primitive ball = Primitive.Create(
                PrimitiveType.Sphere,
                PrimitiveFlags.Collidable | PrimitiveFlags.Visible,
                target.Position,
                Vector3.zero,
                size,
                false,
                Color.white);

            LightSourceToy ls = UnityEngine.Object.Instantiate<LightSourceToy>(PrefabHelper.GetPrefab<LightSourceToy>(PrefabType.LightSourceToy));
            
            ls.NetworkPosition = ball.Position;
            ls.transform.position = ball.Position;
            ls.NetworkLightIntensity = lightIntensity;
            ls.NetworkLightColor = Color.red;
            ls.NetworkLightRange = lightRange;
            ls.NetworkLightType = LightType.Point;
            ls.NetworkIsStatic = false;
            ls.NetworkShadowStrength = 1;
            ls.NetworkShadowType = LightShadows.None;
            ls.NetworkInnerSpotAngle = 1;
            ls.NetworkScale = Vector3.one;
            ls.transform.localScale = Vector3.one;
            ls.enabled = true;
            ls.NetworkRotation = Quaternion.identity;
            ls.transform.rotation = Quaternion.identity;
            ls.transform.parent = ball.AdminToyBase.transform;
            if (!UserSetting<bool>.Get<LightingVideoSetting>(LightingVideoSetting.RenderShadows))
            {
                ls.ShadowType = LightShadows.None;
            }
            
            NetworkServer.Spawn(ls.gameObject);
            NetworkServer.Spawn(ball.AdminToyBase.gameObject);
            ball.AdminToyBase.syncInterval = 0.06f;
            ls.syncInterval = 0.06f;
            ls.NetworkMovementSmoothing = 128;
            ball.AdminToyBase.NetworkMovementSmoothing = 128;
            Timing.CallDelayed(0.2f, () =>
            {
                Rigidbody rb = ball.AdminToyBase.gameObject.AddComponent<Rigidbody>();
                rb.mass = mass;
                rb.linearDamping = drag;
                rb.detectCollisions = true;
                rb.interpolation = interpolation;

                PhysicsMaterial material = new PhysicsMaterial("BouncyBall");
                material.bounciness      = bounciness;
                material.dynamicFriction = dynamicFric;
                material.staticFriction  = staticFric;
                material.bounceCombine   = bounceCombine;

                ball.AdminToyBase.gameObject.AddComponent<SphereCollider>().material = material;
                ball.AdminToyBase.gameObject.AddComponent<Ball>().Init(
                    ls, ball, changeSpeed, verticalDivider, maxAngle, maxDistance, kickForceMul, kickForceUp, checkOnlyDistance, destroyDoors, destroyDoorMaxDistance);
                
                BallRegistry.RegisterBall(ball.AdminToyBase.gameObject.GetComponent<Ball>());
            });

            response = "Bouncy ball successfully spawned!";
            return true;
        }
    }

}