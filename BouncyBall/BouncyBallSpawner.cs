using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MEC;
using Mirror;
using UnityEngine;
using UserSettings;
using UserSettings.VideoSettings;

namespace BouncyBall
{
    public class BouncyBallSpawner
    {
        /// <summary>
        /// Spawns a bouncing ball with specified physics, lighting, and interaction parameters.
        /// </summary>
        /// <param name="position">The position where the ball will be created.</param>
        /// <param name="scale">The scale of the ball.</param>
        /// <param name="lightIntensity">The intensity of the ball's light source (default is 0.35f).</param>
        /// <param name="lightRange">The range of the ball's light (default is 1.6f).</param>
        /// <param name="mass">The mass of the ball (default is 0.8f).</param>
        /// <param name="drag">The drag coefficient (default is 0.3f).</param>
        /// <param name="interpolation">The Rigidbody interpolation mode (default is Interpolate).</param>
        /// <param name="bounciness">The bounciness of the physics material (default is 0.6f).</param>
        /// <param name="dynamicFriction">The dynamic friction of the physics material (default is 0.6f).</param>
        /// <param name="staticFriction">The static friction of the physics material (default is 0.4f).</param>
        /// <param name="bounceCombine">The bounce combination mode (default is Maximum).</param>
        /// <param name="colorChangeSpeed">The speed at which the ball's color changes (default is 0.1f).</param>
        /// <param name="screenVerticalDiv">Divider for calculating the vertical angle of view (default is 2.13f).</param>
        /// <param name="maxLookAngle">Maximum angle for player view interaction (default is 34 degrees).</param>
        /// <param name="maxPlayerDistance">Maximum player distance for interaction (default is 1.5f).</param>
        /// <param name="kickForceMul">Multiplier for player kick force (default is 1f).</param>
        /// <param name="kickForceUp">Vertical component of kick force (default is 2.7f).</param>
        /// <param name="checkOnlyDistance">Whether to check only the distance without angle consideration (default is false).</param>
        /// <param name="destroyDoors">Whether the ball can destroy doors on impact (default is false).</param>
        /// <param name="destroyDoorMaxDistance">Maximum distance to destroy a door (default is 1f).</param>
        public static void SpawnBouncyBall(Vector3 position, Vector3 scale, float lightIntensity = 0.35f, float lightRange = 1.6f,
            float mass = 0.8f, float drag = 0.3f, RigidbodyInterpolation interpolation = RigidbodyInterpolation.Interpolate, float bounciness = 0.6f, float dynamicFriction = 0.6f,
            float staticFriction = 0.4f, PhysicMaterialCombine bounceCombine = PhysicMaterialCombine.Maximum, float colorChangeSpeed = 0.1f, float screenVerticalDiv = 2.13f,
            float maxLookAngle = 34, float maxPlayerDistance = 1.5f, float kickForceMul = 1f, float kickForceUp = 2.7f, bool checkOnlyDistance = false,
            bool destroyDoors = false, float destroyDoorMaxDistance = 1f)
        {
            Primitive ball = Primitive.Create(
                PrimitiveType.Sphere,
                PrimitiveFlags.Collidable | PrimitiveFlags.Visible,
                position,
                Vector3.zero,
                scale,
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
            
            Timing.CallDelayed(0.05f, () =>
            {
                Rigidbody rb = ball.AdminToyBase.gameObject.AddComponent<Rigidbody>();
                rb.mass = mass;
                rb.drag = drag;
                rb.detectCollisions = true;
                rb.interpolation = interpolation;

                PhysicMaterial material = new PhysicMaterial("BouncyBall");
                material.bounciness      = bounciness;
                material.dynamicFriction = dynamicFriction;
                material.staticFriction  = staticFriction;
                material.bounceCombine   = bounceCombine;

                ball.AdminToyBase.gameObject.AddComponent<SphereCollider>().material = material;
                ball.AdminToyBase.gameObject.AddComponent<Ball>().Init(
                    ls, ball, colorChangeSpeed, screenVerticalDiv, maxLookAngle, maxPlayerDistance, kickForceMul, kickForceUp, checkOnlyDistance, destroyDoors, destroyDoorMaxDistance);
                
                BallRegistry.RegisterBall(ball.AdminToyBase.gameObject.GetComponent<Ball>());
            });
        }
    }
}