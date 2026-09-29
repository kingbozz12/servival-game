# Playable prototype generation

This repository now contains code that automatically creates a basic playable Unity prototype the first time the project is opened.

Generated scenes:
- CharacterCreator
- WorldMap
- HomeBase
- ForestLocation

## Current playable flow
CharacterCreator -> WorldMap -> HomeBase / ForestLocation -> green ExitZone -> WorldMap

## Character creator prototype
The UI already represents:
- male / female
- 3 skin tones
- 5 hairstyles
- 5 hair colors
- beard on/off
- nickname

Final 3D character assets will replace the placeholder capsule later.

## Movement
Normal locomotion is run-only.
Any joystick input makes the character run.
There is no walk button/state and no separate run button.

For editor testing, WASD also drives the same run movement.

## Gameplay HUD
The generated HUD is only a functional placeholder.
Approved rules remain:
- nickname
- level
- health bar + number
- armor bar + number
- food number only
- water number only
- no top EXP bar
- thin EXP bar bottom-center
- no quick slots
- no chat
- no speaker
- no prone
- no run button

## Important
The generated primitive art is temporary. It exists so gameplay can be tested before final characters, environments and UI art are ready.

To regenerate scenes later:
Survival Game -> Rebuild Playable Prototype
