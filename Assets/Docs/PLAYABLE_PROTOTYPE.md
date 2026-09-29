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
The generated gameplay HUD is now the production UI foundation rather than a throwaway placeholder.

Included:
- Safe Area support for phone cutouts
- player portrait / nickname / level block
- health and armor bars with numeric values
- food and water as icon + number only
- thin EXP bar at the bottom center
- live minimap rendered by a dedicated top-down camera
- quest tracker
- top navigation
- mobile joystick
- contextual action cluster
- permanent Backpack and Craft buttons at the bottom-right
- Backpack button opens the inventory/equipment screen
- Craft button opens the crafting screen
- no item quick-slot strip
- no chat
- no speaker
- no prone
- no run button

## Important
The generated primitive art is temporary. It exists so gameplay can be tested before final characters, environments and UI art are ready.

To regenerate scenes later:
Survival Game -> Rebuild Playable Prototype
