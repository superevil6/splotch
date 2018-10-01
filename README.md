# splotch

A color mixing game:

Initial Push: Created the basis for the ball object, the player's next color object, and the ability to click a ball and have it mix with the player's next color to create a new color.

This is a Unity Project, if you want to try it out, you have to download Unity.

To do next:

- Object pool balls, create a maximum amount that should repopulate if it falls below.
- Implement a combo system.
- Implement Scoring.

Basic Premise:
You have a grid of balls with various different colors. Colors mix as they would in real life. 
If you add blue to yellow, it becomes green. Add blue to white, it becomes white. Add blue to green... Well, it becomes brown. 
Anything + Brown = Black. Black balls can not be removed once they're on the board. 
The more same colors you connect together, the more points you get. White balls cannot be connected. 
Primary colors are worth 5 points * amount, secondary colors are worth 15 points * amount, brown are worth 1 point * amount. 
When combo'd the points also increase. I haven't decided by how much yet. 
