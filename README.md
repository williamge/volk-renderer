Volk 3D Renderer
==============

Introduction
-----------

Volk is a simple 3d renderer, started off as a naive raytracer with plans to support more advanced features such as photon mapping, radiosity and MLT.

Raytracing
----------

Features so far:

* ray casting
* Phong shading
* multiple depth reflections and refractions
* textures
* naive anti-aliasing
* threading (slow and might be broken)
* camera settings (fov and position only so far)
* image information stored as floating point values for improved precision/manipulation

Shapes:

* sphere
* plane
* triangle
* quadrilateral

Lights:

* point light
* area lights


Pictures
-----------
The Cornell Box with soft shadows from an area light.
![cornell box in volk](https://github.com/pjpe/volk-renderer/raw/master/dev%20pics/version%200.3/softshadows.jpg "The Cornell box in volk without GI or radiosity")

The Cornell Box again but with a point light lighting the scene.
![volk](https://github.com/pjpe/volk-renderer/raw/master/dev%20pics/version%200.3/cornellbasic.jpg "The Cornell box in Volk, no global illumination or radiosity")



A scene with lots of reflective surfaces.
![volk](https://github.com/pjpe/volk-renderer/raw/master/dev%20pics/version%200.3/deepreflections.jpg "Deep Reflections")


![volk](https://github.com/pjpe/volk-renderer/raw/master/dev%20pics/version%200.3/messing%20with%20fov/testspheretexture2.jpg "Basic scene with textures")

![volk](https://github.com/pjpe/volk-renderer/raw/master/dev%20pics/version%200.3/test3.jpg "different lighting values")


TODO
----------

Add:

* reduce the number of new() calls
* figure out what the hell is wrong with threading
