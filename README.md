# Geographic-phenomena-visualizer
This is a Unity3D project for Oculus Quest for visualization of time passage supported by the paper DIEGETIC VISUALIZATION OF TIME IN VIRTUAL REALITY.
In such paper are exposed serveral elements of nature which helps the humans to perceive the time passage, the ones implemented in this
project are: nature growth (palm trees), day-light cycle (sun), ocean waves, birds and sound for the waves and birds, as well as a city growing in the background to understand the longer time scales.
# How to use it
To run and test this project is necessary to have Unity to build projects for Oculus Quest, the Oculus device is not necesarry since it's part of the project the XR Interaction Toolkit which provides a XR Devide Simulator to simulate the controllers of Oculus. 
In the main scene you will find a button panel with time speeds to see how the simulation behaves.
# About the program
This program starts simulating realtime with visible indicators as the birds, waves, their sound and the sun, as soon as you start speeding up the time, the indicators will show you such action. The chosen group of indicators are planed to show different time scales.
## Phases thresholds
The first limit is at x32 because at that speed the waves sound becames annoying, so it stops.
Then the birds get disabled at hours per second (x3600) because it get meaningless dots moving around.
After it the sun stops after x8600 which is the multiplier of days per second, we think that is the maximum the people can support without getting sick in a short period.
## Simulation phase
When the time is fast enough, the trees' growth became noticable, and when passed at least a year the background city also change its status for a bigger one.
After it the terrain starts changing step by step until some of the trees disappear because of the erosion.
## Implementation details
The object GameSystem is the main controller of the objects' behavior, it manage the growth of the city and terrain, and turns off the ocean sound and the birds.


The AnimalsController script is which manage the aparition of the whale's cadaver which is where the crows land so it looks like they are eating the cadaver, the cadaver also appears on predetermined points defined in the Unity Editor.


The TreeManager script is which gives the rhythm of grow to the trees and initial growth state as well as the dying due time of the trees. It change the local scale of every object with the tag of "Tree" in the application.


All the objects answers to the time given by the TimeInterface script which is who multiplies the time delta by the selected scale. 
## External packages
### Ocean crest 
This package was used for the ocean waves and with its time provider were possible to modify the speed of the waves.
### Living birds
This package contains several birds species with its animations and sounds.
### Darth Artisan 
With this package was possible to fill the beach scene with palms but also includes other tree models. 
