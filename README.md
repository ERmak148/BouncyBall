# BouncyBall
The BouncyBall EXILED plugin for SCP: Secret Laboratory that can create bouncy balls on the map.

Required minimum EXILED version: v9.6.0-beta3

## Commands:
For executing commands need permission "er.bouncyball"

spawnballsimple - spawn default bouncy ball at you position

spawnball - spawn customizable ball


### `spawnball` Command Arguments Description

This command spawns a glowing, bouncing sphere at another player’s location.

<details>
1. **PlayerId**  
   - The numeric ID of the player whose position the ball will appear at.  
   - *Example:* `2`  

2. **SizeX**  
   - How wide the ball is, left to right. Bigger numbers make a wider ball.  
   - *Example:* `1.5`  

3. **SizeY**  
   - How tall the ball is, bottom to top. Bigger numbers make a taller ball.  
   - *Example:* `1.5`  

4. **SizeZ**  
   - How deep the ball is, front to back. Bigger numbers make a deeper ball.  
   - *Example:* `1.5`  

5. **LightIntensity**  
   - How bright the ball’s glow is. Higher values make it shine more.  
   - *Example:* `0.35`  

6. **LightRange**  
   - How far the glow reaches around the ball. Larger values light up more area.  
   - *Example:* `1.6`  

7. **Mass**  
   - How heavy the ball feels when it bounces and rolls. Larger values make it heavier.  
   - *Example:* `0.8`  

8. **Drag**  
   - How quickly the ball slows down in the air. Higher values make it lose speed faster.  
   - *Example:* `0.3`  

9. **Interpolation**  
   - How the game smooths the ball’s motion between updates. Options:  
     - `None` – no smoothing  
     - `Interpolate` – smooth movement  
     - `Extrapolate` – predict fast movement  
   - *Example:* `Interpolate`  

10. **Bounciness**  
    - How springy the ball is when it hits something. Higher values make it bounce higher.  
    - *Example:* `0.6`  

11. **DynamicFriction**  
    - How much the ball resists sliding once it’s moving. Higher values slow sliding more.  
    - *Example:* `0.6`  

12. **StaticFriction**  
    - How much the ball resists starting to slide from a standstill. Higher values make it harder to push.  
    - *Example:* `0.4`  

13. **BounceCombine**  
    - How the ball’s springiness mixes with what it hits. Options:  
      - `Average` – take the average  
      - `Minimum` – use the lowest value  
      - `Multiply` – multiply both values  
      - `Maximum` – use the highest value  
    - *Example:* `Maximum`  

14. **ChangeSpeed**  
    - How fast the ball’s color shifts through the rainbow. Bigger numbers cycle faster.  
    - *Example:* `0.1`  

15. **VerticalDivider**  
    - Shapes how steeply you must look up or down to kick the ball. Larger numbers make the vertical view requirement more strict.  
    - *Example:* `2.13`  

16. **MaxAngle**  
    - How far to the side you can look and still kick the ball. Measured in degrees from straight ahead.  
    - *Example:* `34`  

17. **MaxDistance**  
    - How close you must be to the ball to kick it. Larger values let you kick from farther away.  
    - *Example:* `1.5`  

18. **KickForceMul**  
    - How much your kick strength is multiplied. Higher values send the ball flying faster.  
    - *Example:* `1.0`  

19. **KickForceUp**  
    - How much of your kick lifts the ball into the air. Higher values give it more vertical pop.  
    - *Example:* `2.7`  

20. **CheckOnlyDistance**  
    - If `true`, the ball ignores viewing angle and only checks if you’re close enough to kick.  
    - *Example:* `false`  

21. **DestroyDoors**  
    - If `true`, a fast-moving ball can break open doors it hits.  
    - *Example:* `false`  

22. **DestroyMaxDistance**  
    - How far ahead of the ball it can break doors. Larger values let it hit doors from farther away.  
    - *Example:* `0.5`  
</details>


### Example:
`spawnball 2 0.7 0.7 0.7 0.35 1.6 1.2 0.3 Interpolate 0.6 0.5 0.4 Maximum 0.1 2.13 34 1.5 1 2.7 false false 0.1`

## BouncyBall API Documentation for Developers

This API provides the functionality to spawn a customizable bouncy ball in the SCP: Secret Laboratory game using a variety of parameters. The `BouncyBallSpawner` class contains a method `SpawnBouncyBall` that allows you to spawn a bouncy ball with specific physics, lighting, and interaction settings.

---

### Method: `SpawnBouncyBall`

#### Description:
This method spawns a bouncy ball at a given position with customizable properties, including its physical behavior, light source settings, and player interaction parameters.

#### Parameters:
<details>
1. **`position`** (`Vector3`)
   - **Description**: The world position where the bouncy ball will be spawned.
   - **Example**: `new Vector3(10, 5, 10)`

2. **`scale`** (`Vector3`)
   - **Description**: The scale of the ball. This defines its size along the X, Y, and Z axes. 
   - **Example**: `new Vector3(2, 2, 2)`

3. **`lightIntensity`** (`float` - Default: `0.35f`)
   - **Description**: The intensity of the ball's light source. A higher value will make the light brighter.
   - **Example**: `0.7f`

4. **`lightRange`** (`float` - Default: `1.6f`)
   - **Description**: The range of the ball's light. A larger value will make the light cover a wider area.
   - **Example**: `5.0f`

5. **`mass`** (`float` - Default: `0.8f`)
   - **Description**: The mass of the ball, affecting how heavy it feels and how it interacts with forces like gravity.
   - **Example**: `1.2f`

6. **`drag`** (`float` - Default: `0.3f`)
   - **Description**: The drag (air resistance) affecting the ball's movement. A higher value will slow the ball down more quickly.
   - **Example**: `0.5f`

7. **`interpolation`** (`RigidbodyInterpolation` - Default: `RigidbodyInterpolation.Interpolate`)
   - **Description**: The interpolation mode for the Rigidbody, which controls how the ball's movement is smoothed. Options:
     - `None`: No smoothing.
     - `Interpolate`: Smooths the movement.
     - `Extrapolate`: Predicts the ball's position.
   - **Example**: `RigidbodyInterpolation.Interpolate`

8. **`bounciness`** (`float` - Default: `0.6f`)
   - **Description**: How bouncy the ball is. A higher value will make the ball bounce higher.
   - **Example**: `0.8f`

9. **`dynamicFriction`** (`float` - Default: `0.6f`)
   - **Description**: The amount of friction when the ball is moving. A higher value means the ball will slow down faster.
   - **Example**: `0.7f`

10. **`staticFriction`** (`float` - Default: `0.4f`)
    - **Description**: The friction when the ball is stationary. A higher value makes it harder to start moving.
    - **Example**: `0.5f`

11. **`bounceCombine`** (`PhysicMaterialCombine` - Default: `PhysicMaterialCombine.Maximum`)
    - **Description**: Determines how the bounciness is combined with the surface material. Options:
      - `Average`: Uses an average value.
      - `Minimum`: Uses the lower value.
      - `Multiply`: Multiplies both values.
      - `Maximum`: Uses the higher value.
    - **Example**: `PhysicMaterialCombine.Multiply`

12. **`colorChangeSpeed`** (`float` - Default: `0.1f`)
    - **Description**: The speed at which the ball's color changes over time.
    - **Example**: `0.2f`

13. **`screenVerticalDiv`** (`float` - Default: `2.13f`)
    - **Description**: A value used to adjust how the ball reacts based on the player's vertical angle of view.
    - **Example**: `2.5f`

14. **`maxLookAngle`** (`float` - Default: `34f`)
    - **Description**: The maximum angle of the player's view that will still allow interaction with the ball.
    - **Example**: `40f`

15. **`maxPlayerDistance`** (`float` - Default: `1.5f`)
    - **Description**: The maximum distance at which a player can interact with the ball.
    - **Example**: `2.0f`

16. **`kickForceMul`** (`float` - Default: `1f`)
    - **Description**: Multiplies the force applied when a player kicks the ball. A higher value makes the ball move faster when kicked.
    - **Example**: `1.5f`

17. **`kickForceUp`** (`float` - Default: `2.7f`)
    - **Description**: The upward force applied to the ball when it is kicked. A higher value makes the ball fly higher.
    - **Example**: `3.0f`

18. **`checkOnlyDistance`** (`bool` - Default: `false`)
    - **Description**: If set to `true`, the ball’s interaction with players will only be based on distance, without considering angle.
    - **Example**: `true`

19. **`destroyDoors`** (`bool` - Default: `false`)
    - **Description**: Whether or not the ball can destroy doors upon collision.
    - **Example**: `true`

20. **`destroyDoorMaxDistance`** (`float` - Default: `1f`)
    - **Description**: The maximum distance at which the ball can destroy doors when it collides with them.
    - **Example**: `2.0f`
</details>
---

### Example Usage:

```csharp
Vector3 spawnPosition = new Vector3(10, 5, 10);
Vector3 ballScale = new Vector3(2, 2, 2);

// Spawn a bouncy ball with default parameters
BouncyBallSpawner.SpawnBouncyBall(spawnPosition, ballScale);

// Spawn a bouncy ball with customized properties
BouncyBallSpawner.SpawnBouncyBall(
    spawnPosition,
    ballScale,
    lightIntensity: 0.8f,
    lightRange: 3.0f,
    mass: 1.0f,
    drag: 0.4f,
    interpolation: RigidbodyInterpolation.Extrapolate,
    bounciness: 0.9f,
    dynamicFriction: 0.7f,
    staticFriction: 0.5f,
    bounceCombine: PhysicMaterialCombine.Minimum,
    colorChangeSpeed: 0.15f,
    maxLookAngle: 45f,
    maxPlayerDistance: 2.0f,
    kickForceMul: 1.2f,
    kickForceUp: 3.0f,
    checkOnlyDistance: true,
    destroyDoors: true,
    destroyDoorMaxDistance: 2.0f
);
```
