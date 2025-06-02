# Game Systems Documentation

## Overview
This project implements an advanced AI enemy system with sound detection, room-based spawning, and dynamic behavior management for Unity games. The system includes main enemies with microphone detection, secondary enemies with room constraints, player movement, camera controls, and interactive sound sources.

## 📁 File Structure

### Enemy System
- **`EnemyNavigation.cs`** - Main enemy AI with microphone detection and state machine
- **`EnemySecondary.cs`** - Room-constrained secondary enemies with limited lifetime
- **`RoomManager.cs`** - Manages room-based enemy spawning and sound detection

### Player System
- **`MCMovement.cs`** - Camera-relative player movement controller
- **`MoveCamera.cs`** - Mouse look camera system with cursor lock

### Interactive Elements
- **`GlobalSoundSource.cs`** - Sound sources and player-triggered traps

---

## 🤖 Enemy System Setup

### Main Enemy (EnemyNavigation)

#### Required Components
- **NavMeshAgent** - Automatically added by script
- **Animator** - For movement animations
- **AudioSource** - For microphone input (automatically configured)

#### Setup Steps
1. Create an empty GameObject for your main enemy
2. Attach the `EnemyNavigation` script
3. Set up the Animator with these parameters:
   - `IsWalking` (bool)
   - `IsRunning` (bool) 
   - `IsIdle` (bool)
   - `Attack` (trigger)

#### Configuration
```csharp
public List<Transform> wayPoint;           // Patrol waypoints
public float scoreImprovementThreshold;    // Sound target switching threshold
public float idleDuration;                 // Idle state duration
public float noiseThreshold;               // Minimum sound to investigate
public bool showDebug;                     // Enable debug logging
```

#### Waypoint Setup
1. Create empty GameObjects as waypoints
2. Position them where you want the enemy to patrol
3. Add them to the `wayPoint` list in order
4. Enemy will patrol in sequence automatically

### Secondary Enemy (EnemySecondary)

#### Required Components
- **NavMeshAgent** - Automatically added by script
- **Animator** - Same parameters as main enemy

#### Key Differences from Main Enemy
- **Room-constrained** - Only operates within assigned room
- **Limited lifetime** - Auto-despawns after set duration
- **Shared microphone** - Uses main enemy's microphone
- **Managed by RoomManager** - Spawned/configured automatically

#### Manual Configuration (if needed)
```csharp
secondaryEnemy.SetRoomManager(roomManager);
secondaryEnemy.SetAssignedRoom(room);
secondaryEnemy.SetLifetime(60f);
secondaryEnemy.SetWaypoints(roomWaypoints);
```

### Room Manager Setup

#### Configuration
```csharp
public EnemyNavigation mainEnemy;           // Main enemy reference
public GameObject secondaryEnemyPrefab;     // Secondary enemy prefab
public List<Room> rooms;                    // Room definitions
public float secondaryEnemyDuration = 60f;  // Secondary enemy lifetime
public float roomCooldownDuration = 90f;    // Room cooldown after despawn
public float autoSpawnTimer = 300f;         // Auto-spawn timer
public float mainEnemyDetectionRange = 20f; // Main enemy detection radius
```

#### Room Setup
1. In the Inspector, expand the `rooms` list
2. For each room:
   - Set `roomName` (for debugging)
   - Add waypoint Transforms to `waypoints` list
   - Leave other fields at default (managed by script)

#### Secondary Enemy Prefab Creation
1. Create a GameObject with `EnemySecondary` script
2. Add required components (NavMeshAgent, Animator)
3. Save as prefab
4. Assign to `secondaryEnemyPrefab` in RoomManager

---

## 🎮 Player System Setup

### Player Movement (MCMovement)

#### Setup
1. Attach to player GameObject
2. Assign `cameraTransform` in Inspector
3. Player speed is hardcoded to 5.0f (modify in script if needed)

#### Controls
- **WASD** or **Arrow Keys** - Movement
- Movement is relative to camera direction

### Camera System (MoveCamera)

#### Setup
1. Attach to camera GameObject
2. Adjust `speed` value for mouse sensitivity
3. Cursor automatically locks on start

#### Controls
- **Mouse Movement** - Look around
- Cursor is locked to center of screen

---

## 🔊 Sound System Setup

### GlobalSoundSource

#### Basic Sound Source Setup
1. Create GameObject at desired location
2. Attach `GlobalSoundSource` script
3. Configure properties:
   ```csharp
   public float volumeIntensity;    // Sound intensity (0.1-10)
   public bool isActive;            // Current state
   public KeyCode activationKey;    // Manual toggle key
   public Color sourceColor;        // Debug visualization color
   ```

#### Trap Setup
1. Enable `isTrap` checkbox
2. Add a **Collider** component set to **IsTrigger**
3. Configure trap settings:
   ```csharp
   public float trapDuration;       // How long trap stays active
   public float trapCooldown;       // Cooldown between activations
   ```

#### Trap Behavior
- **Player trigger only** - Enemies don't activate traps
- **Auto-deactivates** - After `trapDuration` seconds
- **Cooldown period** - Prevents immediate re-activation
- **Visual feedback** - Different colors for trap states

---

## 🎯 Enemy Behavior System

### State Machine
Both enemy types use the same 5-state system:

1. **Patrolling** - Moving between waypoints, listening for sounds
2. **Investigating** - Moving toward detected sound source
3. **Idle** - Waiting at investigation point, still listening
4. **Pursuing** - Death sentence mode (relentless player chase)
5. **Attacking** - Attack animation when catching player

### Sound Detection Priority
1. **GlobalSoundSources** (highest priority)
2. **Player microphone input** (when no GlobalSoundSources active)

### Death Sentence Mode
- Triggered when player detected while enemy is **Idle**
- Enemy switches to maximum speed pursuit
- Ignores all other sounds until player caught or attack completed

### Range-Based Detection
- **Main Enemy** - Uses `mainEnemyDetectionRange` from RoomManager
- **Secondary Enemy** - Room-constrained detection only
- **RoomManager** - Spawns secondary enemies for sounds outside main enemy range

---

## 🛠️ Setup Checklist

### Essential Setup
- [ ] NavMesh baked for your scene
- [ ] Main enemy with waypoints configured
- [ ] RoomManager with rooms and waypoints set up
- [ ] Secondary enemy prefab created
- [ ] Player with camera assigned
- [ ] Microphone permissions enabled in build settings

### Animation Setup
- [ ] Animator Controller created with required parameters
- [ ] Animation states connected properly
- [ ] Attack animation configured with proper timing

### Audio Setup
- [ ] Microphone device available
- [ ] Audio permissions granted
- [ ] GlobalSoundSources placed strategically

### Testing
- [ ] Waypoint connections work properly
- [ ] Enemy responds to microphone input
- [ ] Secondary enemies spawn in correct rooms
- [ ] Traps activate only for players
- [ ] Death sentence mode triggers correctly

---

## 🐛 Common Issues & Solutions

### Enemy Not Moving
- Check if NavMesh is baked
- Verify waypoints are on NavMesh
- Ensure NavMeshAgent is enabled

### No Microphone Detection
- Check microphone permissions
- Verify microphone is connected
- Only main enemy initializes microphone

### Secondary Enemies Not Spawning
- Verify RoomManager references are set
- Check if rooms are in cooldown
- Ensure main enemy detection range is correct

### Traps Not Working
- Verify Collider is set to **IsTrigger**
- Check if player has "Player" tag
- Ensure trap is not in cooldown

### Animation Issues
- Verify Animator parameters match script constants
- Check animation transitions are properly connected
- Ensure animations have proper loop settings

---

## 🎮 Gameplay Tips

### For Players
- Use GlobalSoundSources strategically to distract enemies
- Avoid making noise when enemies are idle (triggers death sentence)
- Traps can be used to lure enemies away from your path

### For Developers
- Adjust `scoreImprovementThreshold` to control enemy focus
- Modify detection ranges to balance difficulty
- Tune cooldown timers for desired enemy density
- Use debug flags to understand enemy behavior during testing