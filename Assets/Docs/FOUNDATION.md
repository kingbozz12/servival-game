# Core foundation

This first layer implements the data model for an LDOE-style mobile survival game.

## Character creation
- Male / female
- 3 skin-tone material slots
- 5 hairstyle slots (index 0 may be bald)
- 5 hair-color material slots
- Beard on/off
- Nickname
- JSON save using PlayerPrefs for prototype phase

## Modular equipment
Slots:
- Head
- Torso
- Jacket
- Legs
- Boots
- Gloves
- Backpack
- Weapon

Each equipped item can enable a separate model attached to the same character skeleton.

## World architecture
The game is intentionally NOT one seamless open world.

WorldMap -> choose location -> load Location Scene -> play -> walk into ExitZone -> return to WorldMap.

Persistent HomeBase will later save placed structures separately. Resource locations can later be procedurally populated from templates/seeds.

## Next milestones
1. Character creator UI
2. Mobile joystick/player controller
3. Isometric camera
4. Inventory/item definitions
5. Resource gathering
6. Zombie AI and combat
7. Home-base building/save system
