To use the Car Spawner, simply add the car spawner object to the scene. Make sure the car_spawner object is completely underground.
The CarTest scene in the Assets/Scenes folder has an example.
The core scripts tied to this are: Vehicle.cs (script attached to each car) 
and CarSpawner.cs (game object that spawns the cars)

Vehicle.cs has OnCollisionEnter which is the collision handler. It is expecting that the ground plane
have tag Ground. start_moving and stop will start/stop the car (this is mainly for any additional collision behavior you want)

Note: 
Desired_move_direction must be set on the Car Spawner game object, acceptable values are: x,+x,z,-z
You can change the spawn frequency by changing the Spawn_interval of the Car Spawner game object
Car_lifetime dictates how long the car that is spawned will be moving (before automatically despawning)
When the car is stopped, it does not count toward the lifetime/time before car despawns.