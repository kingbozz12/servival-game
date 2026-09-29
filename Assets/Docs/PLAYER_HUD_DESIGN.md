# Approved player HUD

This HUD direction is approved as the current working reference. Final sizes, spacing and visual polish will be adjusted later against the real game scene, character model and mobile resolution.

## Information shown
- Character portrait
- Nickname
- Level
- Health
- Armor
- Food
- Water
- EXP progress

## Top-left player block
- Nickname is shown next to the portrait.
- Level is shown as a compact badge.
- Health uses an icon + bar + numeric value.
- Armor uses an icon + bar + numeric value.
- Food uses ONLY an icon + number. No food progress bar.
- Water uses ONLY an icon + number. No water progress bar.
- Do NOT show an EXP bar at the top.

Example:
- Nickname: Player5776
- Level: 12
- Health: 100 / 100
- Armor: 80 / 100
- Food: 76
- Water: 68

## Bottom-center level progress
- A thin EXP/level progress bar sits at the very bottom center.
- It may show the current level on the left and EXP progress on the right.
- It must stay visually light and unobtrusive.

Example:
- Ур. 12
- 320 / 1500

## Gameplay controls
- No permanent bottom quick-access item slots.
- No speaker button on the right side.
- No chat button on the right side.
- No prone / lie-down button.
- Contextual tool/melee action behavior follows ACTION_CONTROLS.md.
- Other movement/action buttons can be repositioned and refined later during real scene testing.

## Visual direction
- Dark translucent survival HUD.
- Compact enough for mobile landscape.
- Clean separation between stats.
- Health visually red.
- Armor visually cool blue.
- Food uses a warm food icon.
- Water uses a blue droplet icon.
- Bottom EXP bar uses a distinct accent so it does not compete with health/armor.
- Avoid unnecessary bars and unnecessary permanent buttons.

## Important
This is our own HUD direction. It may use familiar survival-game conventions, but layout, icon treatment, spacing, typography and final art should remain original.

The current mockup is a working design reference, not a final pixel-perfect layout. It will be refined later after the real scene, character, animations and device testing are in place.
