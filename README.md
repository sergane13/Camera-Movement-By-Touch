# Camera-Movement-By-Touch

### PanPinchCameraMovement.cs 

    Script used for the camera movement in 2D space
    Atach it to any gameobject

    Parameters

    1.Camera To Move --> Camera component of the object that you want to move 

    2.X inferior limit --> the minim x value for the camera position [camera position is the center of the view]

    3.X superior limit --> the maximum x value for the camera position

    4.Y inferior limit 

    5.Y superior limit

    6.Minimum ortho size --> the minim ortho size of the camera that has the "Projection" in Orthographic mode

    7.Maximum ortho size --> the maximum ortho size

### Features of the module

    1. Panning with one finger in any direction with finger remaining always under the touched object [also know as pixel perfect panning]
    
    2. Pinching with two or more fingers with zooming target in the middle point of the two fingers

    3. Supports panning during the pinching operation

    4. WorldSpace limits and orthographic limits

    5. Ignores any UI elements during touch operation