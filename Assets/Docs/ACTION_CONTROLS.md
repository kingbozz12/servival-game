# Approved action-control behavior

This is the approved gameplay rule for tools and melee weapons.

## Gathering tools
Axe and pickaxe are context tools.

- Axe action appears only when the player is close enough to a tree that can be harvested.
- Pickaxe action appears only when the player is close enough to a stone/ore node that can be mined.
- The action button shows the icon of the equipped tool.
- Walking away from the compatible resource hides the gather action.
- Using the wrong tool near a resource does not show the gather action.

Examples:
- Axe + tree -> show Chop button with axe icon.
- Axe + stone -> no gather button.
- Pickaxe + stone -> show Mine button with pickaxe icon.
- Pickaxe + tree -> no gather button.

## Melee weapons
Bat, road sign, machete, katana, golf club and future melee weapons use the combat action.

- The main attack button is available while a melee weapon is equipped.
- The button displays the icon of the exact equipped weapon.
- Swapping from bat to katana immediately swaps the attack-button icon.
- Attack behavior is independent from the gathering interaction.

## UI principle
Do not create permanent separate axe and pickaxe buttons.

The same context-action area changes according to what the player is carrying and what is nearby.

The system is intentionally data-driven so new melee weapons and gathering tools can be added without writing a new UI button for each item.
