# 🪞 Echo Twin — Setup Guide (for total Unity beginners)

**The pitch:** Every move you make is recorded. When the round timer runs out, an
**Echo Twin** — a ghost that replays your last run exactly — joins you. Yellow
plates only respond to **you**, blue plates only respond to your **echo**. Latch
every plate to win the level.

It's a co-op game where your partner is *your own past self*. That one sentence
is your interview hook.

---

## How the game works (rules)

- Move with **WASD** or **arrow keys**.
- Each **round** lasts ~8 seconds (a timer is shown at the top).
- Round 1: no ghost yet. Walk onto the spot where a **blue** plate is (so your
  future echo will press it) and press any **yellow** plates on the way.
- Round 2: your echo replays round 1 while you play a fresh run.
- Pressed plates **stay pressed** (they turn green) — so every round you make
  progress, even if you "fail".
- When ALL plates are green → level complete → next level automatically.

---

# Part 0 — Install Unity (one time, ~30 min)

1. Download **Unity Hub** from https://unity.com/download and install it.
2. Open Unity Hub → **Installs** → **Install Editor** → pick the latest
   **Unity 6 LTS** (any version starting with "6000"). You don't need any of
   the checkboxes on the next screen; just click Install.
3. Open Unity Hub → **Projects** → **New project**:
   - Template: **Universal 2D** (or "2D" if that's what you see)
   - Project name: `EchoTwin`
   - Click **Create project**.

The big panel in the middle is the **Scene** view, the left panel is the
**Hierarchy** (list of objects in your level), and the bottom is the **Project**
window (your files). The right side is the **Inspector** (settings for whatever
you have selected).

---

# Part 1 — One-time project setting

1. Menu: **Edit → Project Settings → Player**.
2. Under **Other Settings**, find **Active Input Handling** and set it to
   **Input Manager (Old)**.
3. Unity asks to restart the editor — click **Apply/Restart**.

> Why: the scripts use Unity's classic input system (`Input.GetAxisRaw`). This
> setting makes sure it's active.

---

# Part 2 — Add the scripts

1. In Unity's **Project** window (bottom), open the `Assets` folder.
2. Right-click → **Create → Folder** → name it `Scripts`.
3. Drag the 5 `.cs` files from your computer's
   `Documents/EchoTwin/Scripts` folder into that `Assets/Scripts` folder.
4. Click one script — if you see red errors in the **Console** tab (next to
   Project window), re-check Part 1.

---

# Part 3 — Build Level 1 (step by step)

> **Menu names differ slightly between Unity versions:** sprites are at
> *GameObject → 2D Object → Sprites → Square* in Unity 6, or
> *GameObject → 2D Object → Square* in older versions.

### 3.1 Camera
1. Click **Main Camera** in the Hierarchy.
2. In the Inspector set: Position `(0, 0, -10)`, and on the Camera component
   set **Size** to `5.5`, **Background** to a dark color (e.g. `16162A`).

### 3.2 Player
1. Hierarchy → right-click → **2D Object → Sprites → Square**. Rename it `Player`.
2. Inspector: Position `(0, -3, 0)`, Scale `(0.6, 0.6, 1)`.
3. Sprite Renderer → **Color** → yellow (e.g. `FFD53D`).
4. **Add Component** (button at the bottom of the Inspector) → search `Rigidbody 2D`:
   - **Gravity Scale** = `0`
   - **Freeze Rotation Z** = ✅ ticked
5. **Add Component** → `Box Collider 2D` (leave everything default).
6. At the very top of the Inspector, open the **Tag** dropdown (says "Untagged")
   → select **Player** (it's a built-in tag).
7. **Add Component** → type `Player Controller` → add your script.

### 3.3 Ghost
1. Hierarchy → right-click → **2D Object → Sprites → Circle**. Rename it `Ghost`.
2. Inspector: Position `(0, -3, 0)`, Scale `(0.6, 0.6, 1)`.
3. Sprite Renderer → Color: light blue with **alpha ~150** (e.g. R `56`, G `140`,
   B `255`, A `150`) so it looks ghostly.
4. **Add Component** → `Rigidbody 2D` → set **Body Type** = `Kinematic`.
5. **Add Component** → `Circle Collider 2D` → tick **Is Trigger**.
6. Tag dropdown → **Ghost**. (Not built in: click Tag dropdown → **Add Tag…** →
   press the **+** → name it `Ghost` → then select it on the Ghost object.)
7. **Add Component** → `Ghost Replay` script.
8. **Uncheck the big checkbox** at the top-left of the Inspector (next to the
   name) to disable the Ghost for now. The game enables it automatically.

### 3.4 Walls
Create 4 squares (same way as Player) and make them look like a border. For each:
grey color, and **Add Component → Box Collider 2D** (not a trigger).

| Object | Position | Scale |
|---|---|---|
| Wall Top    | `(0, 4.9, 0)`   | `(18, 0.4, 1)` |
| Wall Bottom | `(0, -4.9, 0)`  | `(18, 0.4, 1)` |
| Wall Left   | `(-9, 0, 0)`    | `(0.4, 10, 1)` |
| Wall Right  | `(9, 0, 0)`     | `(0.4, 10, 1)` |

### 3.5 UI text (timer + messages)
1. Hierarchy → right-click → **UI → Text - TextMeshPro**.
   If a popup asks to **Import TMP Essentials** → click it (one time only).
2. Rename it `TimerText`. Set:
   - Anchor Presets (the square icon in the Rect Transform): hold **Alt** and
     click the **top-center** preset.
   - Pos Y: `-40`, Pos X: `0`
   - Font Size: `34`, Alignment: center, Color: white, Text: `Time: 8`
3. Select TimerText, press **Ctrl+D** (Cmd+D on Mac) to duplicate. Rename the
   copy `MessageText`. Set Pos Y: `-100`, Font Size: `26`, Color: light yellow.
   Clear its Text (leave empty).

### 3.6 GameManager
1. Hierarchy → right-click → **Create Empty** → rename `GameManager`.
2. **Add Component** → `Game Manager` script.
3. Drag objects from the Hierarchy into the script's fields:
   - **Player** ← drag the Player object
   - **Ghost** ← drag the Ghost object
   - **Timer Text** ← drag TimerText
   - **Message Text** ← drag MessageText
   - **Next Level Name** = `Level2`

### 3.7 Plates (do these last, so you can drag the GameManager in)
Each plate is a square with a trigger collider and the Plate script.

Create `Plate Yellow 1`:
1. **2D Object → Sprites → Square**, rename `Plate Yellow 1`.
2. Position `(5, 2, 0)`, Scale `(0.7, 0.7, 1)`, Color yellow (`FFD53D`).
3. **Add Component** → `Box Collider 2D` → tick **Is Trigger**.
4. **Add Component** → `Plate` script.
5. Tick **Is Player Plate** ✅ and drag the **GameManager** object into its
   GameManager field.

Create `Plate Blue 1` the same way, but:
- Position `(-5, 2, 0)`, Color blue (`388CFF`)
- **Is Player Plate** ❌ unticked
- Drag GameManager into the field.

### 3.8 Save the scene
**File → Save As…** → save into `Assets/Scenes` as `Level1`.
(The default scene `SampleScene` can be deleted later.)

---

# Part 4 — Level layouts (copy these coordinates)

Each level needs the same skeleton (Player, Ghost, 4 walls, Canvas, GameManager)
— the fastest way is **File → Save As… Level2**, then move/recreate objects.

### Level 1 — "Learn the loop" (Round time: 8)
| Object | Position | Notes |
|---|---|---|
| Player (spawn) | `(0, -3)` | |
| Plate Blue 1   | `(-5, 2)` | ghost plate |
| Plate Yellow 1 | `(5, 2)`  | player plate |

**Solution:** Round 1: walk left onto the blue plate spot, then right onto the
yellow plate. Round 2: your echo presses blue → win.

### Level 2 — "Two halves" (Round time: 8)
| Object | Position | Notes |
|---|---|---|
| Player (spawn) | `(0, -3.5)` | |
| Wall (vertical) | `(0, 1)` | Scale `(0.4, 7, 1)` — gap at the bottom |
| Plate Blue 1   | `(-6, 2.5)` | ghost |
| Plate Yellow 1 | `(-2.5, 0.5)` | player |
| Plate Blue 2   | `(6, 2.5)` | ghost |
| Plate Yellow 2 | `(2.5, 0.5)` | player |

**Solution:** R1: left half (blue1 → yellow1). R2: right half (yellow2) while
echo does left. R3: echo presses blue2 → win.

### Level 3 — "The crossing" (Round time: 10)
| Object | Position | Notes |
|---|---|---|
| Player (spawn) | `(0, -3.5)` | |
| Wall (vertical) | `(0, 1)` | Scale `(0.4, 7, 1)` |
| Wall (block in the bottom gap) | `(0, -3.9)` | Scale `(1, 0.8, 1)` |
| Plate Blue 1   | `(-6, 2.5)` | ghost |
| Plate Yellow 1 | `(-3, 0.5)` | player |
| Plate Yellow 3 | `(-6, -2)` | player |
| Plate Blue 2   | `(6, 2.5)` | ghost |
| Plate Yellow 2 | `(3, 0.5)` | player |
| Plate Yellow 4 | `(6, -2)` | player |

In the GameManager Inspector, set **Round Time** to `10` for this level.

**Solution:** R1: left side (blue1, yellow1, yellow3). R2: right side
(yellow2, yellow4). R3: echo presses blue2 → win.

---

# Part 5 — The Win scene

1. **File → New Scene** → save as `Win` in `Assets/Scenes`.
2. **UI → Text - TextMeshPro**, set Font Size `60`, center it: Anchor Presets →
   hold Alt, click **middle-center**. Text: `You Win! 🪞`
3. Add a second TMP text below it: `You synchronized with yourself perfectly.` Font Size `24`.
4. Hierarchy → **UI → Button - TextMeshPro**. Position it below the text.
   Set its label text to `Play Again`.
5. Hierarchy → **Create Empty** → rename `SceneButtons` → **Add Component** →
   `Scene Buttons` script.
6. Select the **Button** → in the Inspector find **On Click ()** → press **+** →
   drag the `SceneButtons` object into the slot → dropdown →
   **SceneButtons → LoadFirstLevel ()**.
7. Set the camera background to something celebratory. Save.

---

# Part 6 — Register the scenes (required!)

`SceneManager.LoadScene` only works with scenes added to the build list.

1. **File → Build Profiles** (older Unity: **File → Build Settings**).
2. Open each scene (double-click it in `Assets/Scenes`) and click
   **Add Open Scenes** — OR drag the scene files from the Project window into
   the scene list.
3. Make sure the order is: `Level1`, `Level2`, `Level3`, `Win`.
   (Drag to reorder if needed.)
4. You don't need to actually build — just press **Play** in the editor.

**Now press Play.** 🎮

---

# Part 7 — How to talk about this in your interview

**The pitch (memorize this):**
> "Echo Twin is a single-player co-op puzzle game. Your movement is recorded
> each round, and a ghost replays it while you play the next round. Some
> switches only respond to you, some only to your past self — so you must
> collaborate with a recording of your own decisions."

**What each script does (so you can explain the code):**

| Script | Job | Key concept |
|---|---|---|
| `PlayerController` | Reads WASD, sets the Rigidbody2D velocity | Physics movement, input |
| `GameManager` | Round timer, records positions into a `List<Vector2>`, hands the list to the ghost, checks win | Game loop, state |
| `GhostReplay` | Walks through the recorded list one physics step per frame | Playback = same speed as recording |
| `Plate` | Latches when the right tag enters its trigger | Events, tags, triggers |
| `SceneButtons` | UI button reloads level 1 | Scene management |

**Good "if I had more time" answers:**
- Multiple echoes (keep the last 3 recordings) for deeper puzzles
- Undo/rewind, sound effects, animations, gamepad support
- A level editor so players can create puzzles
- Visual: echo trail, screen shake, plate press animation

---

# Part 8 — Troubleshooting

| Problem | Fix |
|---|---|
| `NullReferenceException` when playing | You forgot to drag something into the GameManager or Plate fields. Check every field is filled. |
| Pressing keys does nothing | Part 1 — set Active Input Handling to Input Manager (Old). |
| `Scene couldn't be loaded` error | Part 6 — add all scenes to Build Profiles. |
| Warning about `.velocity` being obsolete | Harmless. Or replace `velocity` with `linearVelocity` in the scripts (Unity 6 name). |
| Player falls off screen | Walls need a Box Collider 2D (Part 3.4), player needs Rigidbody2D + collider. |
| Ghost doesn't press blue plates | Ghost's collider must have **Is Trigger** ticked, tag must be exactly `Ghost`, and its Rigidbody2D must be **Kinematic**. |
| Plates never latch | Plate's GameManager field must be filled, and collider **Is Trigger** ticked. |

Have fun — and good luck with the interview! 🍀
