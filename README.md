# FishHunt

## What it is

FishHunt is a short VR minigame built for Google Cardboard in Unity. You can look 
through the headset at a single fish swimming back and forth in front of you.
Once a second, the game automatically throws a spear toward wherever your
gaze/crosshair is aimed; if it connects with the fish you score a point. Each
round lasts 30 seconds, after which the game shows your score and restarts
automatically (or immediately if you pull the Cardboard trigger). The only
input the player has is aiming their head and everything else (throwing,
timing, scoring, respawning the fish) is handled by the game.

Code: [`FishHuntGame.cs`](Assets/Scripts/FishHuntGame.cs) (round/HUD/scoring
loop), [`FishTarget.cs`](Assets/Scripts/FishTarget.cs) (fish movement and
respawn), [`Spear.cs`](Assets/Scripts/Spear.cs) (projectile/hit detection).
Scene: [`Assets/Scenes/FishHunt.unity`](Assets/Scenes/FishHunt.unity).

## Why it's a game

FishHunt works for all six definitions from Juul:

1. **Fixed rules** - a spear fires every second toward the crosshair, a hit
   scores exactly one point, rounds run for a fixed 30 seconds. None of this
   is changeable mid-play; the code enforces it and the player can't modify it.
2. **Variable, quantifiable outcome** - the score at the end of a round can be
   anything from 0 up to (round length ÷ fire interval), and it's a plain
   integer.
3. **Valorization of outcomes** - a higher score is explicitly better; the HUD
   exists to show the score so the player can see how
   they did.
4. **Player effort** - skill is the difference between a miss and a hit is
   how well the player aims their head before each automatic throw, so skill
   directly determines the outcome.
5. **Player attached to outcome** - the end-of-round message ("Score: N")
   is the whole payoff of playing; you keep track of whether you beat your
   last round.
6. **Negotiable consequences** - nothing bad happens if you lose. It's a demo with     the option of no negative consequences

It also fits the simpler test from Schell's: 

A game is "a problem-solving activity, approached with a playful
attitude." FishHunt hands the player one repeated problem, "where will the
fish be when the next spear leaves?", and the player interacts with it purely
because it's fun to try to beat their own score, not because they're
obligated to.

## Sources / assets

- Fish and spear are primitive Unity geometry with custom materials
  (`Assets/Materials/Fish.mat`, `Assets/Materials/SpearMat.mat`) - no external
  3D models were used.
