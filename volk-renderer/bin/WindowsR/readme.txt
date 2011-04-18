To use: simple run the .exe, right now it will only render the default scene included and 
save the image to "render.jpg" in this directory. 

The information that pops up in the console window is a bit weird:
	- first two "True" lines are printed, that is just to show
		that the textures are loaded fine
	- next there are three lines of (a,b,c) vectors, these are
		the camera axes
	- after the program is done rendering it will print statistics
		about the render, such as number of rays shot, shadow rays
		calculated, etc. Also the time it took to render the scene
		will pop up.
	- if you run the program by double clicking the exe the window 
		might close before you get to see these lines, don't worry
		about it, the statistics aren't very interesting yet.