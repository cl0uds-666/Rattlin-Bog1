# Rattlin' Bog (Unity WebGL party memory game)

A tiny hotseat memory game for local turns. Players click cards in order from the fixed chain and submit each round.

## Fixed chain used by the game
This is hardcoded exactly as requested:

`bog, hole, seed, tree, branch, twig, leaf, nest, egg, bird, feather, flea, leg, boot, lace`

## Scene setup (single scene)

1. Create a new scene (for example `Main`).
2. Add a `Canvas` + `EventSystem`.
3. On Canvas:
   - Add `Canvas Scaler`
   - Set **UI Scale Mode** = `Scale With Screen Size`
   - Suggested **Reference Resolution** = `1080 x 1920`
   - Suggested **Screen Match Mode** = `Match Width Or Height`, match around `0.7` for mobile-first feel.
4. Add a root `Panel` stretched full screen.
5. Add a `Vertical Layout Group` + `Content Size Fitter (Vertical Fit: Preferred Size)` to the panel content container.
6. Add these UI elements in vertical order:
   - Title (`TextMeshProUGUI`): `Rattlin' Bog`
   - Round text (`TextMeshProUGUI`)
   - Instruction text (`TextMeshProUGUI`)
   - Selected preview text (`TextMeshProUGUI`)
   - Result text (`TextMeshProUGUI`)
   - Card area object with `Grid Layout Group` (2-4 columns depending on width)
   - Buttons row (`Horizontal Layout Group`) with `Submit` and `Clear` buttons
   - `Restart / Play Again` button
7. Make button heights touch-friendly (at least ~90 px at 1080x1920 reference).
8. Use TMP text on all labels/buttons for readability.

## Prefab setup for cards

Create a `WordCardButton` prefab:

1. Create a `Button` object.
2. Put a `TextMeshProUGUI` child inside it for the word label.
3. Add the `WordCardButton` script to the button root.
4. Assign:
   - `Word Label` = TMP child text
   - `Button` = same button component on root
5. Save as prefab (for example `WordCardButton.prefab`).

## Manager setup

1. Create an empty object: `GameManager`.
2. Add `RattlinBogGameManager` script.
3. Assign references in inspector:
   - `Round Text`
   - `Instruction Text`
   - `Selection Preview Text`
   - `Result Text`
   - `Card Container` (the grid transform)
   - `Card Prefab` (`WordCardButton.prefab`)
   - `Submit Button`
   - `Clear Button`
   - `Restart Button`
4. Optional: keep `Shuffle Cards On New Game` enabled for replay variety.

## Behavior implemented

- All cards visible all the time.
- Round starts at 1 (`bog` only), then grows by one each successful round.
- Selection cannot exceed current round requirement.
- Submit compares player sequence to first `N` entries of the fixed chain.
- Clear resets current selection.
- Wrong answer ends game and shows highest successful round.
- Restart starts a new game.
- Already-selected cards are disabled until Clear/next round.
- Mouse/touch only via Unity UI buttons (works desktop + mobile browser).

## WebGL build notes (itch.io)

- Build target: **WebGL**.
- Keep default Unity UI interactions (click/tap compatible).
- Avoid keyboard-only controls (this setup uses no keyboard input).
- Keep scene lightweight: one scene, simple UI, no unsupported plugins.

## Scripts in this repo

- `Assets/Scripts/RattlinBogGameManager.cs`
- `Assets/Scripts/WordCardButton.cs`
