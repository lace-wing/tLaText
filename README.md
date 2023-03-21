# tLaText
LaText for Terraria, tLaText, is a (WIP) text-editing library for Terraria mods run by tModLoader.

## Features
- [ ] What You Type Is What You See, WYTIWYS.
- [x] Cursor-based editing.
- [x] Multi-cursor support.
- [x] Vim-like commands.
- [x] Optional parameters for commands.
- [ ] Inline formatting tags.
- [ ] Fairly extendable.
- [ ] Yet, easy to use!

## Usage

### Commands
tLaText has extendable operations, where commands can be defined and be read from user inputs.
There can be optional or compulsory parameters, subject to commands.

Many of the commands are based on Vim, such as the <kbd>u</kbd><kbd>h</kbd><kbd>j</kbd><kbd>k</kbd> movements, insert and append.

### Cursors
Although Terraria itself does not support mobile cursor in its text-input system, LaText has an internal cursor system and can be displayed to users. Users can move and add cursor, also select pieces of texts.
Both the cursors and selected areas can be visually shown, and even colored using tLaText's Terraria integrated functions.

### Inline formatting tags
To easily emphasize or understate a text in Terraria without memorizing hex color codes, tLaText's inline formatting tags are more than usefull.
Inherited from Markdown syntax, \*, \_ and other tags surrounding texts will be automatically parsed into color tags while exported to Terraria-style texts.

### Raw & parsed text display
tLaText provides functions to parse raw texts into Terraria-style texts, then you can output them wherever you want.
