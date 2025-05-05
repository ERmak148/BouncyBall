using System.Linq;
using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles.PlayableScps;
using UnityEngine;

namespace BouncyBall
{
    public class Ball : MonoBehaviour
    {
        private LightSourceToy ls;
        public LightSourceToy LightSource => ls;
        
        private Primitive ball;
        private float colorTime = 0;
        private Rigidbody rb;
        private readonly int doorLayerMask = LayerMask.GetMask("Door");
        
        private float colorChangeSpeed;
        private float verticaldivider = 2.13f;
        private float maxAngle = 34;
        private float maxDistance = 1.5f;
        private float kickForceMultiplier = 1f;
        private float kickForceUp = 2.7f;
        private bool checkOnlyDistance = false;
        private bool destroyDoors = false;
        private float destroyDoorMaxDistanse = 0.5f;
        
        public void Init(LightSourceToy lightSourceToy, Primitive primitive, float changespeed, float vd = 2.13f,
            float maxangle = 34, float maxdistance = 1.5f, float kickforcemul = 1f, float kickforceup = 2.7f,
            bool checkonlydistance = false, bool destroydoors = false, float destroydoormaxdistance = 0.5f)
        {
            ls = lightSourceToy;
            ball = primitive;
            colorChangeSpeed = changespeed;
            verticaldivider = vd;
            maxAngle = maxangle;
            maxDistance = maxdistance;
            kickForceMultiplier = kickforcemul;
            kickForceUp = kickforceup;
            checkOnlyDistance = checkonlydistance;
            destroyDoors = destroydoors;
            destroyDoorMaxDistanse = destroydoormaxdistance;
            
            rb = gameObject.GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            foreach (Player near in Player.List.Where(x => Vector3.Distance(x.Position, gameObject.transform.position) < maxDistance))
            {
                if (!checkOnlyDistance)
                {
                    Vector3 directionToTarget = gameObject.transform.position - near.Position;
                    directionToTarget.Normalize();
                
                    Vector3 localDirection = near.CameraTransform.InverseTransformDirection(directionToTarget);
                
                    float horizontalAngle = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;
                    float verticalAngle = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;
                
                    horizontalAngle = Mathf.Abs(horizontalAngle);
                    verticalAngle = Mathf.Abs(verticalAngle);
                    
                    if (horizontalAngle < maxAngle && verticalAngle < maxAngle/verticaldivider)
                    {
                        bool hasLineOfSight = Physics.Raycast(near.CameraTransform.position, 
                                                  directionToTarget, out RaycastHit hit, 
                                                  Vector3.Distance(near.Position, gameObject.transform.position), 
                                                  VisionInformation.VisionLayerMask) == false || 
                                              hit.transform == gameObject.transform;

                        if (hasLineOfSight)
                        {
                        
                            Vector3 kickDirection = near.CameraTransform.forward;
                        
                            float randomFactor = UnityEngine.Random.Range(0.8f, 1.2f);
                            Vector3 force = new Vector3(kickDirection.x * 8f * randomFactor * kickForceMultiplier, 
                                kickForceUp, 
                                kickDirection.z * 8f * randomFactor * kickForceMultiplier);

                            rb.AddForce(force, ForceMode.Impulse);
                        }
                    }
                }
                else
                {
                    Vector3 directionToTarget = gameObject.transform.position - near.Position;
                    directionToTarget.Normalize();
                    bool hasLineOfSight = Physics.Raycast(near.CameraTransform.position, 
                                              directionToTarget, out RaycastHit hit, 
                                              Vector3.Distance(near.Position, gameObject.transform.position), 
                                              VisionInformation.VisionLayerMask) == false || 
                                          hit.transform == gameObject.transform;

                    if (hasLineOfSight)
                    {
                    
                        Vector3 kickDirection = near.CameraTransform.forward;
                    
                        float randomFactor = UnityEngine.Random.Range(0.8f, 1.2f);
                        Vector3 force = new Vector3(kickDirection.x * 8f * randomFactor * kickForceMultiplier, 
                            kickForceUp, 
                            kickDirection.z * 8f * randomFactor * kickForceMultiplier);

                        rb.AddForce(force, ForceMode.Impulse);
                    }
                    
                }
            }

            if (destroyDoors)
            {
                if (rb.velocity.sqrMagnitude > 0.09f)
                {
                    Vector3 dir = rb.velocity.normalized;
                    if (Physics.Raycast(transform.position, dir, out RaycastHit hit, destroyDoorMaxDistanse, doorLayerMask))
                    {
                        if (hit.collider != null)
                        {
                            Door door = Door.Get(hit.collider.transform.GetComponentInParent<DoorVariant>());
                            if (door != null)
                            {
                                if (door.Base is IDamageableDoor dd)
                                {
                                    if (!dd.IsDestroyed)
                                    {
                                        dd.ServerDamage(ushort.MaxValue, DoorDamageType.Grenade);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            colorTime += Time.fixedDeltaTime;
            if (colorTime > 1000f)
                colorTime = 0f;
            
            float hue = Mathf.Repeat(colorTime * colorChangeSpeed, 1f);
            Color newColor = Color.HSVToRGB(hue, 1f, 1f);
            
            ls.NetworkLightColor = newColor;
            ball.Color = newColor;
        }
    }
}