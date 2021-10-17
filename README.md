# Camera-Movement-By-Touch

## Table of contents
* Description
* Parameters
* Features

## Description
Script: PanPinchCameraMovement.cs 

Used for the camera movement in 2D space that is attached to any gameObject

## Parameters

* Camera To Move --> Camera component of the object that you want to move 

* X inferior limit --> the minim x value for the camera position [camera position is the center of the view]

* X superior limit --> the maximum x value for the camera position

* Y inferior limit 

* Y superior limit

* Minimum ortho size --> the minim ortho size of the camera that has the "Projection" in Orthographic mode

* Maximum ortho size --> the maximum ortho size



## Features

* Panning with one finger in any direction with finger remaining always under the touched object [also know as pixel perfect panning]
    
* Pinching with two or more fingers with zooming target in the middle point of the two fingers

* Supports panning during the pinching operation

* WorldSpace limits and orthographic limits

* Ignores any UI elements during touch operation
    
* Static bool MovingCamera with tolerance to know when camera is moving by touch

* Gizmo drawing in UNITY EDITOR to know the limits of the camera 