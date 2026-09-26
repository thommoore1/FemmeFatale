# Top-Down 2D Player Movement (Command Pattern + New Input System)

## How it fits together

```
Input System device  --callback-->  PlayerInputHandler  --enqueues-->  CommandInvoker
                                                                              |
                                                                       ProcessAll() each Update
                                                                              v
                                                                   IPlayerCommand.Execute(player)
                                                                              v
                                                                       PlayerController (receiver)
```

- **`PlayerController`** — the "receiver." Owns movement, dashing, and facing
  direction. Has no idea input exists; it just exposes `SetMoveInput`,
  `TryDash`, `Interact`.
- **`IPlayerCommand` + `MoveCommand` / `DashCommand` / `InteractCommand`** — each
  is a small object encapsulating "do this to the player." This is the
  seam that lets you later add replay, AI-driven input, or networked input
  without changing `PlayerController` at all.
- **`CommandInvoker`** — queues commands as they arrive and executes them
  once per frame, keeping a bounded history (handy for a debug overlay or
  future undo/replay features).
- **`PlayerInputHandler`** — the only class that touches the Input System.
  It implements the generated `PlayerControls.IPlayerActions` interface and
  turns each callback into a command.

## Setup steps in Unity

1. **Install the package**: Window → Package Manager → search "Input System"
   → Install. Unity will prompt to switch the active input handling to the
   new system (or "Both") — accept and let it restart.

2. **Import the action asset**: copy `PlayerControls.inputactions` into your
   project (e.g. `Assets/Input/`). Select it in the Project window and in
   the Inspector check **"Generate C# Class"**, then click Apply. This
   generates the `PlayerControls` class that `PlayerInputHandler` implements.
   - Action map: `Player`
   - Actions: `Move` (Vector2, WASD + arrows + left stick composite),
     `Dash` (button: Left Shift / gamepad South), `Interact` (button: E /
     gamepad West)
   - Feel free to open it in the Input Actions editor and add more bindings.

3. **Copy the scripts** from `Scripts/` into your project (keep the
   `Commands/` subfolder as-is).

4. **Set up the Player GameObject**:
   - Add a `Rigidbody2D` (Body Type: Dynamic, Gravity Scale will be forced
     to 0 in `Awake`), a `Collider2D`, and a `SpriteRenderer`.
   - Add `PlayerController` and `PlayerInputHandler`.
   - If you use `PlayerInput` elsewhere in your project, you don't need it
     here — `PlayerInputHandler` creates and owns its own `PlayerControls`
     instance directly.

5. Press Play. WASD/arrows/left-stick move the player, Left Shift/gamepad
   South dashes, E/gamepad West interacts (logs to console — wire it up to
   your interaction system in `PlayerController.Interact()`).

## Extending it

- **New action**: add it to the `.inputactions` asset, add a matching
  `On<Action>` method to `PlayerInputHandler` (the interface will require
  it once you regenerate the C# class), and add a new `IPlayerCommand`
  implementation.
- **Undo/replay**: `CommandInvoker.History` already gives you an ordered
  list of every command executed — record it, or feed it back through
  `ProcessAll` against a fresh player state.
- **Networked/AI input**: write another class that enqueues the same
  command types into a `CommandInvoker` — `PlayerController` needs no
  changes.
