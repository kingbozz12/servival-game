# Survival Game

Mobile isometric survival RPG inspired by the structure of games such as Last Day on Earth, while using original art, code, world and UI.

## Target
- Unity 6.6
- Android first
- Isometric/top-down camera
- Modular character customization and equipment
- Separate playable locations connected through a world map
- Persistent home base
- Resource gathering, combat, loot, crafting and building

## Current state
The repository now contains a functional prototype bootstrap.

On first opening the project in Unity, `PrototypeAutoBuilder` automatically creates:
- `CharacterCreator`
- `WorldMap`
- `HomeBase`
- `ForestLocation`

The first generated version uses primitive placeholder art so gameplay structure can be tested before final 3D characters, environment assets and UI art are ready.

Current flow:
`CharacterCreator -> WorldMap -> HomeBase / ForestLocation -> ExitZone -> WorldMap`

Movement is run-only: any joystick movement immediately uses the running locomotion state. WASD is available for editor testing.

If the generated scenes need to be rebuilt:
`Survival Game -> Rebuild Playable Prototype`

See `Assets/Docs` for the approved design rules.
