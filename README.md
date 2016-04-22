# spess

Unmerged features of this branch:
<<<<<<< HEAD
+ Playable character movement with physics relative to the ship
+ Added wire prefab
	* Wires can be placed by the player
	* Wires can be created as either power or exhaust wires
	* Wires start with an origin object and connect to a source of the appropriate type
	* Wires can be placed at a source
	* Wires can be ended at a source or by pressing the finishWire key or by pressing the cancel key
+ Added power and exhaust source prefab
	* These serve no purpose so far except to end wires
+ Made prefabs for ship block room hitboxes

Outstanding issues:
- None
=======

	+ Ship movement
		* Ship moves using the arrow keys
		* Speed is adjustable in the ShipController script
		* Center of mass is calculated based on the locations of the attached blocks
	+ Blocks can be added to and removed from the ship using the mouse
		* When blocks are detached, they retain their trajectory
		
Outstanding issues:
	+ Removing the root block disables the ability to add new blocks
>>>>>>> 22d484e913d6dcc9edb288d85e70327de7c08e6d
