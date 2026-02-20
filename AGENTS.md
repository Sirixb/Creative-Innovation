# AGENTS.md - Retro Dungeon Unity Project

## Project Overview
- **Engine**: Unity 2022.3.55 LTS
- **Type**: 2D Top-down Dungeon Crawler
- **Language**: C# (LangVersion 9.0)
- **Build Target**: WebGL

## Build Commands

### Building the Project
- **Unity Editor**: Open project in Unity Hub, press `Ctrl+B` to build
- **Command Line (Windows)**:
  ```
  "C:\Program Files\Unity\2022.3.55f1\Editor\Unity.exe" -buildTarget WebGL -quit -batchmode -projectPath "D:\Unity3D\4_Laboral\Retro Dungeon"
  ```
- **Build Output**: `Build/` directory

### Running Tests
- **All Tests**: `Window > General > Test Runner > Run All`
- **Single Test**: In Test Runner, right-click a specific test > `Run Selected`
- **Play Mode Tests**: Switch to Play mode in Test Runner, click `Run All`
- **Edit Mode Tests**: Switch to Edit mode in Test Runner, click `Run All`

## Code Style Guidelines

### Naming Conventions
- **Classes/Interfaces**: `PascalCase` (e.g., `PlayerController`, `IWeapon`)
- **Methods/Properties**: `PascalCase` (e.g., `GetComponent()`, `FacingLeft`)
- **Private Fields**: `_camelCase` with `_` prefix (e.g., `_rb`, `_movement`)
- **Serialized Fields**: `_camelCase` with `[SerializeField]` attribute
- **Constants**: `PascalCase` (e.g., `MaxHealth`)
- **Interfaces**: Prefix with `I` (e.g., `IWeapon`, `IDamager`, `IEnemyState`)
- **Enums**: `PascalCase` for enum and values

### File Organization
- **Scripts Location**: `Assets/Scripts/`
- **Directory Structure**: Group by feature (Player/, Enemies/, Consumable/, etc.)
- **Namespace**: Use meaningful namespaces (e.g., `Player`, `Enemies.AttackSelection`)
- **One class per file**: Each file should contain exactly one class/interface

### Formatting Rules
- **Indentation**: 4 spaces (Unity default)
- **Line Endings**: Platform default (LF on Unix, CRLF on Windows)
- **Braces**: Opening brace on same line (`{`)
- **Using Statements**: At top of file, alphabetically sorted
- **Regions**: Use `#region` for grouping related code (optional)

### Type Guidelines
- **Use `var`**: For local variables when type is obvious
- **Use `new()` with type inference: `var instance = new ClassName()`
- **Nullable**: Check for null before use; use null-conditional operators (`?.`)
- **Unity Types**: Use `Vector2`, `Vector3`, `Quaternion` for math
- **Collections**: Use `List<T>` for dynamic lists, arrays for fixed size

### Property Patterns
```csharp
// Exposed serialized field with property
[SerializeField] private float speed;
public float Speed { get => speed; set => speed = value; }

// Read-only property
public bool FacingLeft { get; private set; }
```

### Component Access
```csharp
// In Awake(), cache components
private void Awake()
{
    _rb = GetComponent<Rigidbody2D>();
    _animator = GetComponent<Animator>();
    _spriteRenderer = GetComponent<SpriteRenderer>();
}
```

### Error Handling
- **Null Checks**: Use null-conditional operators and explicit checks
- **Debug Logging**: Use `Debug.Log()`, `Debug.LogWarning()`, `Debug.LogError()`
- **Debug.DrawRay**: For visualization in Scene view
- **Don't suppress exceptions**: Let them propagate or handle explicitly

### Unity-Specific Patterns

#### State Machine Pattern (Enemies)
```csharp
public interface IEnemyState
{
    void Enter(EnemyState enemyState); 
    void Update(); 
    void Exit(); 
}
```

#### Factory Pattern
```csharp
public class CharacterFactory : MonoBehaviour
{
    public GameObject CreateCharacter(string id, Vector3 position);
}
```

#### Service Locator
```csharp
// Located at Assets/Scripts/Scene Managment/ServiceLocator/
ServiceLocator.Get<AudioController>().PlaySFX(clip);
```

### Event Patterns
```csharp
// Subscribe in OnEnable, unsubscribe in OnDisable
private void OnEnable()
{
    playerHealth.OnPlayerDie += DisableComponentsOnPlayerDie;
}

private void OnDisable()
{
    playerHealth.OnPlayerDie -= DisableComponentsOnPlayerDie;
}
```

### Animation
- Use `Animator.StringToHash()` for parameter hashes
- Cache hash values in `readonly` fields

### Physics
- Use `FixedUpdate()` for physics calculations
- Use `rb.MovePosition()` for character movement
- Use `Time.fixedDeltaTime` for velocity-based movement

## Project Structure
```
Assets/
├── Animations/       # Animation clips and controllers
├── Audio/            # Sound effects and music
├── Materials/        # Shader materials
├── Prefabs/          # Prefab GameObjects
├── Scenes/           # Unity scenes
├── Scripts/          # C# source code
│   ├── Player/       # Player-related scripts
│   ├── Enemies/      # Enemy AI and behaviors
│   ├── Consumable/   # Item system
│   ├── Destructible/ # Breakable objects
│   ├── Factory/      # Object creation
│   ├── Misc/         # Utilities
│   ├── PlayFab/      # PlayFab integration (auth, player data)
│   └── Scene Managment/ # Game flow, UI, services
├── Settings/         # Project settings
├── Sprites/          # 2D graphics
├── TextMesh Pro/     # UI text
└── Tilemap/          # Tile-based levels
```

## Key Packages
- `com.unity.cinemachine` (2.10.3) - Camera system
- `com.unity.test-framework` (1.1.33) - Unit testing
- `com.unity.render-pipelines.universal` (14.0.11) - URP rendering
- `com.unity.timeline` (1.7.6) - Cinematic sequences
- `com.playfab` (latest) - PlayFab SDK for Unity
