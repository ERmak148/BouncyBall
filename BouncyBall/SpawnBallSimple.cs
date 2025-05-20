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
using Player = Exiled.API.Features.Player;

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
            try
            {
                if (!sender.CheckPermission("er.bouncyball"))
                {
                    response = "You do not have permissions to use this command";
                    return false;
                }

                Player ex = Player.Get(sender);
                Primitive ball = Primitive.Create(PrimitiveType.Sphere,
                    PrimitiveFlags.Collidable | PrimitiveFlags.Visible, ex.Position, Vector3.zero, Vector3.one * 0.7f,
                    false, Color.white);
                NetworkServer.Spawn(ball.AdminToyBase.gameObject);


                LightSourceToy ls =
                    UnityEngine.Object.Instantiate<LightSourceToy>(
                        PrefabHelper.GetPrefab<LightSourceToy>(PrefabType.LightSourceToy));
                
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
                ls.enabled = true;
                ls.NetworkRotation = Quaternion.identity;
                ls.transform.rotation = Quaternion.identity;
                ls.transform.parent = ball.AdminToyBase.transform;
                if (!UserSetting<bool>.Get<LightingVideoSetting>(LightingVideoSetting.RenderShadows))
                {
                    ls.ShadowType = LightShadows.None;
                }

                NetworkServer.Spawn(ls.gameObject);
                ball.AdminToyBase.syncInterval = 0.06f;
                ls.syncInterval = 0.06f;
                ls.NetworkMovementSmoothing = 128;
                ball.AdminToyBase.NetworkMovementSmoothing = 128;
                Timing.CallDelayed(0.2f, () =>
                {
                    try
                    {
                        Rigidbody rb = ball.AdminToyBase.gameObject.AddComponent<Rigidbody>();
                        rb.mass = 0.8f;
                        rb.linearDamping = 0.3f;
                        rb.detectCollisions = true;
                        rb.interpolation = RigidbodyInterpolation.Interpolate;
                        
                        PhysicsMaterial material = new PhysicsMaterial("Bouncy");
                        material.bounciness = 0.6f;
                        material.dynamicFriction = 0.6f;
                        material.staticFriction = 0.4f;
                        material.bounceCombine = PhysicsMaterialCombine.Maximum;
                        ball.AdminToyBase.gameObject.AddComponent<SphereCollider>().material = material;
                        
                        ball.AdminToyBase.gameObject.AddComponent<Ball>().Init(ls, ball, 0.1f);
                        
                        BallRegistry.RegisterBall(ball.AdminToyBase.gameObject.GetComponent<Ball>());
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                    }
                });
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            response = "Bouncy ball successfully spawned!";
            return true;
        }
    }
}