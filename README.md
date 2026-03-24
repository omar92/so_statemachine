# SO State Machine

A ScriptableObject-based State Machine for Unity that provides a clean, decoupled, and Inspector-friendly way to manage game states.

## Overview

SO State Machine uses Unity's ScriptableObject architecture to define and manage game states. By storing states as assets, it enables:

- **Decoupled state logic** – GameObjects react to state changes without tight coupling
- **Inspector-friendly setup** – Configure states and transitions directly in the Unity Editor
- **Event-driven transitions** – Enter/Exit callbacks via UnityEvents and a C# interface
- **Scene-safe state management** – Listener lists are automatically reset on scene loads

## Components

| Class | Type | Purpose |
|---|---|---|
| `GameStateSM` | `ScriptableObject` | Defines a single game state |
| `GameStateSMSO` | `ScriptableObject` | Variable container that tracks the active `GameStateSM` |
| `StateMachineSM` | `MonoBehaviour` | Drives state transitions at runtime |
| `GameStateListenerSM` | `MonoBehaviour` | Listens to state enter/exit events on a GameObject |
| `IStateListener` | Interface | Implement on any `MonoBehaviour` for code-driven state callbacks |

## Installation

1. Copy the contents of this repository into your Unity project under `Assets/` (e.g. `Assets/SOStateMachine/`).
2. The package depends on the [SO Variables](https://github.com/omar92/so_variables) framework (`VariableSO<T>`). Make sure it is also present in your project.

## Setup

### 1. Create Game States

In the **Project** window, right-click and select:

```
Create → SM → GameState
```

Create one asset per state (e.g. `MainMenu`, `Gameplay`, `Paused`). Optionally fill in the **Game State Description** field to document what the state represents.

### 2. Create a State Machine Variable

```
Create → SM → Variables → GameState
```

This `GameStateSMSO` asset acts as a shared variable that the state machine writes to and listeners read from.

### 3. Add the State Machine to the Scene

1. Create (or select) a `GameObject` in the scene.
2. Add the `StateMachineSM` component.
3. Assign the `GameStateSMSO` asset to **Current Game State**.
4. Assign the desired starting state to **Start State**.
5. *(Optional)* Wire up the **On Switch State** UnityEvent to run logic on every transition.

### 4. Assign the State Machine Reference to Each State

Each `GameStateSM` asset needs a reference to its owning `GameStateSMSO` so it can trigger transitions.

In the Inspector for every `GameStateSM` asset, set the **Statemachine** field to the same `GameStateSMSO` variable.

### 5. Add Listeners to GameObjects

Add the `GameStateListenerSM` component to any `GameObject` that should react to state changes.

- In the **States Listeners** list, add an entry for each state you want to respond to.
- Set the **Game State** field to the target `GameStateSM` asset.
- Wire up **On Enter** and **On Exit** UnityEvents.
- Enable **Listen When Disabled** if the listener should fire even while the GameObject is inactive.

## Triggering State Transitions

Call either method on a `GameStateSM` asset (e.g. from a UnityEvent button or script):

```csharp
// Switch to this state (ignored if already in this state)
gameState.Switch();

// Force-switch to this state (runs OnExit/OnEnter even if already active)
gameState.ForceSwitch();
```

## Implementing `IStateListener`

For code-driven state logic, implement the `IStateListener` interface on any `MonoBehaviour` that is a child (or self) of a `GameStateListenerSM`:

```csharp
using SO.SMachine;
using UnityEngine;

public class MyGameplayHandler : MonoBehaviour, IStateListener
{
    public void OnEnter(GameStateSM gameState)
    {
        Debug.Log($"Entered state: {gameState.name}");
        // enable gameplay systems, spawn enemies, etc.
    }

    public void OnExit(GameStateSM gameState)
    {
        Debug.Log($"Exited state: {gameState.name}");
        // clean up, pause timers, etc.
    }
}
```

`IStateListener` callbacks are invoked at the end of the frame after `UnityEvent` callbacks.

## API Reference

### `GameStateSM` (ScriptableObject)

| Member | Description |
|---|---|
| `GameStateDescription` | Editor-only description of what this state does |
| `statemachine` | The `GameStateSMSO` this state belongs to |
| `Switch()` | Requests a transition to this state (no-op if already current) |
| `ForceSwitch()` | Forces a transition to this state regardless of the current state |
| `RegisterListener(gameStateListener)` | Registers an event-based listener |
| `UnregisterListener(gameStateListener)` | Unregisters an event-based listener |

### `StateMachineSM` (MonoBehaviour)

| Field | Description |
|---|---|
| `CurrentGameState` | The `GameStateSMSO` variable that holds the active state |
| `startState` | The `GameStateSM` to activate when the component enables |
| `onSwitchState` | UnityEvent fired on every state transition |

### `GameStateListenerSM` (MonoBehaviour)

| Field | Description |
|---|---|
| `Init` | UnityEvent fired once on first enable |
| `StatesListeners` | List of `gameStateListener` entries to handle |

### `gameStateListener` (Serializable)

| Field | Description |
|---|---|
| `GameState` | The state asset to listen to |
| `OnEnter` | UnityEvent invoked when the state becomes active |
| `OnExit` | UnityEvent invoked when the state becomes inactive |
| `listenWhenDisabled` | If `true`, stays registered while the GameObject is disabled |
| `GameStateBehaviour` | Editor-only description of this object's role in the state |

### `IStateListener` (Interface)

| Method | Description |
|---|---|
| `OnEnter(GameStateSM gameState)` | Called at end-of-frame when the state is entered |
| `OnExit(GameStateSM gameState)` | Called immediately when the state is exited |

## License

This project is provided as-is. See the repository owner for licensing details.
