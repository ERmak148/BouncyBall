using System;
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
    public class SpawnBallSimple : ICommand, IUsageProvider
    {
        public string Command { get; } = "spawnballsimple";

        public string[] Aliases { get; } = {};

        public string Description { get; } = "Spawn bouncy ball near your position";
        
        public bool SanitizeResponse { get; } = false;

        public string[] Usage { get; } = { };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("er.bouncyball"))
            {
                response = "You do not have permissions to use this command";
                return false;
            }

            Player ex = Player.Get(sender);
            Primitive ball = Primitive.Create(PrimitiveType.Sphere, PrimitiveFlags.Collidable | PrimitiveFlags.Visible, ex.Position, Vector3.zero, Vector3.one*0.7f, false, Color.white);
            LightSourceToy ls = UnityEngine.Object.Instantiate<LightSourceToy>(PrefabHelper.GetPrefab<LightSourceToy>(PrefabType.LightSourceToy));
            
            ls.NetworkPosition = ball.Position;
            ls.transform.position = ball.Position;
            ls.NetworkLightIntensity = 0.35f;
            ls.NetworkLightColor = Color.red;
            ls.NetworkLightRange = 1.6f;
            ls.NetworkLightType = LightType.Point;
            ls.NetworkIsStatic = false;
            ls.NetworkShadowStrength = 1;
            ls.NetworkShadowType = LightShadows.None;
            ls.NetworkInnerSpotAngle = 1;
            ls.NetworkScale = Vector3.one;
            ls.transform.localScale = Vector3.one;
            ls._light.enabled = true;
            ls._light.cullingMask = -1;
            ls.NetworkRotation = Quaternion.identity;
            ls.transform.rotation = Quaternion.identity;
            ls.transform.parent = ball.AdminToyBase.transform;
            if (!UserSetting<bool>.Get<LightingVideoSetting>(LightingVideoSetting.RenderShadows))
            {
                ls._light.shadows = LightShadows.None;
            }
            
            NetworkServer.Spawn(ls.gameObject);
            NetworkServer.Spawn(ball.AdminToyBase.gameObject);
            Timing.CallDelayed(0.2f, () =>
            {
                Rigidbody rb = ball.AdminToyBase.gameObject.AddComponent<Rigidbody>();
                rb.mass = 0.8f;
                rb.drag = 0.3f;
                rb.detectCollisions = true;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
        
                PhysicMaterial material = new PhysicMaterial("Bouncy");
                material.bounciness = 0.6f;
                material.dynamicFriction = 0.6f;
                material.staticFriction = 0.4f;
                material.bounceCombine = PhysicMaterialCombine.Maximum;
                ball.AdminToyBase.gameObject.AddComponent<SphereCollider>().material = material;

                ball.AdminToyBase.gameObject.AddComponent<Ball>().Init(ls, ball, 0.1f);
            });
            response = "Bouncy ball successfully spawned!";
            return true;
        }
    }
}