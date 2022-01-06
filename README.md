<!-- PROJECT LOGO -->

<br />
<p align="center">
  <h1 align="center">🎥 Camera Movement By Touch 👆</h1>

  <p align="center">
    README for Camera Movement
    <br />
    <a href="https://github.com/sergane13/Camera-Movement-By-Touch"><strong>Explore the docs »</strong></a>
    <p align="center">
    <img src="https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com)"/>
    <img src="https://img.shields.io/badge/Maintained%3F-yes-green" />
    <img src="https://img.shields.io/badge/Ask%20me%20-anything-orange" />
    </p>
    <p align="center"> 
      <a href="https://github.com/sergane13/Camera-Movement-By-Touch/issues">Report Bug</a>
      ·
      <a href="https://github.com/sergane13/Camera-Movement-By-Touch/issues">Request Feature</a>
    </p>   
  </p>
</p>


<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
        <li><a href="#How-to-use-it">How to use it</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

<br/>

<!-- ABOUT THE PROJECT -->
# 1. About The Project
<br/>

<img src="images/1.png" alt="Logo" width="1400">

<br>

## Description
<p>
  Out of the box sollution for managing movement of camera in your game and interaction with different gameObjects using the collider. 
</p>

### Features
 *  Panning with one finger in any direction with the finger remaining always under the touched object [also know as pixel perfect panning]

 * Panning has inertia

 * WorldSpace limits and orthographic limits

 * Gizmo drawing in UNITY EDITOR to know the limits of the camera

 * Visual feedback for achieving minimum ortho
    
 * Pinching with two or more fingers with zooming target in the middle point of the two fingers

 * Supports panning during the pinching operation

 * Ignores any UI elements during touch operation

 * Detects tap on GameObjects for opening screens or panels [UI canvas]
<br>

## Built With

* [Unity](https://unity.com/) in <img src=https://img.shields.io/badge/language-C%23-blue height = 14px/>
<!-- GETTING STARTED -->

## Working with

<p> 
  <img src="https://img.shields.io/badge/os-ios-lightgrey" height = 19px/>
  <img src="https://img.shields.io/badge/os-android-brightgreen" height = 20px/>
</p>


# Getting Started

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/sergane13/Camera-Movement-By-Touch
   ```
   
2. Open project in Unity
    ```sh
    Tested Unity version: 2021.2.0f1
    ```

3. Add scripts to any gameObject you want and assign the main camera to it
    <p>
      <img src="images/2.png" alt="Logo" width="700">
    </p>
 

### How to use it

  * The first script is `PanPinchCameracMovement.cs` that is used for managing the inputs from the user [touch inputs].
    * `cameraToMove`
        * The camera component that will be manipulated by the user
    * `limitXmin`
        * The minimum position of the camera field of view on X axis
    * `limitXmax`
        * The maximum position of the camera field of view on X axis 
    * `limitYmin`
        * The minimum position of the camera field of view on Y axis
    * `limitYmax`
        * The maximum position of the camera field of view on Y axis
    * `orthoMin`
        * The minum orhographic size of the camera
    * `orthoMax`
        * The maximum orhographic size of the camera
    * `interpolationStep` 
        * Sensitivity value for calculating the camera drag (inertia) after release of the finger.
        Reccomended value: 0.2
  * The second script is `TapOnGameObject.cs` that is responsible vor validating the "tap" on a gameobject veryfiyng that the finger has not left the initial touch position, taking in consideration a small sensitivity to get rid of false positives.
    * `sensitivity`
        * value for filtering touch.deltaposition to consider the touch as MOVING
        * Reccomended value: 0.2

### Disclaimer ⚠️

  * Both scripts are using the **old input system**  
  * Using the new input system will results in a lot of errors and incompatibilities, especialy with managing touch on UI elements

<!-- USAGE EXAMPLES -->

# Usage

#### After reasearching for quite a while, we could not find a good and sustainable sollution for camera movement for mobile aplications. In our case it was needed to build a 2d city builder game where user has to interact with the environment. It can be easily used in any 2d isometric games similar with `HayDay` or `Clash of Clans`. We tried to mimic the bahaviour that it is found in supercell games for camera movement, but adaptated for our usage.

* You can test the usage of the scripts in our game [Khyron Realm](https://khyron-realm.com/)
* You can also see the [Youtube](https://www.youtube.com/watch?v=uhusqjg41g8&t=78s&ab_channel=KhyronRealm) video


<!-- CONTRIBUTING -->
# Project structure

```bash
< PROJECT ROOT for Assets >
   |
   |--Editor
   |--Resources
      |-- first.png
      |-- second.png
   |--Scenes
      |--Test-Scenes.unity
   |--Scripts 
      |--PanPinchCameraMovement.cs     # Script for managing movement of camera
      |--TapOnGameObject.cs            # Script for managing touch of a gameObject
   |                          
  ************************************************************************
```

# Colaborators

* [Serban Chisca](https://github.com/sergane13)
* [Codrin Burlacu](https://github.com/whatever-jpg)


<!-- LICENSE -->
# License

Shield: [![CC BY 4.0][cc-by-shield]][cc-by]

This work is licensed under a
[Creative Commons Attribution 4.0 International License][cc-by].

[![CC BY 4.0][cc-by-image]][cc-by]

[cc-by]: http://creativecommons.org/licenses/by/4.0/
[cc-by-image]: https://i.creativecommons.org/l/by/4.0/88x31.png
[cc-by-shield]: https://img.shields.io/badge/License-CC%20BY%204.0-lightgrey.svg



Project Template adapted from [Othneil Drew](https://github.com/othneildrew) / [Best-README-Template](https://github.com/othneildrew/Best-README-Template).


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[product-screenshot]: images/screenshot.png
