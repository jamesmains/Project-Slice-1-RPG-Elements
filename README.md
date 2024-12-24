# Project Slice 1 RPG Elements
 This project slice showcases RPG elements such as stats, leveling, inventory management, custom animation system, and dynamic environments.

 ## Stats
 The stats system is loosely based off of RuneScape in that you have a multitude of stats that make up your character without rigid leveling system.
 Each stat can be leveled up by interacting with objects that require similar stats. Currently the only implementation of this is via combat and the 'Slashing' skill with the sword.
 Every Entity can have a set of stats. In this project the 'Scary Bush WITH DEFENCE' entity has a Defence stat of 1 which reducing incoming damage by 1.

 ## Leveling
 Each stat can track how much experience it's gained and once it's reached the cap for the next level transfers any remaining exp to that new level. Pretty standard stuff.
 Currently the Level is also the stats value. This was more due to simplicity rather than any specific design choice.

 ## Inventory Management
 The player has two main windows for Inventory (there is a toolbar but it is disabled). In the main window is where any item the player gets will appear, and the equipment window which
 requires items of specific types before slotting them in. The equipment window is for any equipment the player can wear and it affects their stats based on the equipment's stats.
 Loosely tied to this window is the Stat Breakdown window which simply displays all the player stats in conjunction with the equipment worn.

 ## Animation System
 The custom animation system was created to be flexible but allowing predefined animations. Implementing new animations is fairly easy but does force a certain workflow with Aseprite.
 There are some cons to this system and I won't say it's "better" than the Unity animation system but it is quicker to implement animations once set up. The biggest draw back is the fact that it
 relies on so many gameobjects living under the animation controller as well as not seamlessly transitioning all the time to new animations.

 ## Dynamic Environments
 The tiles and sprites around the game scene can change based on the current season. There is no automatic implementation of this, just a button on the season controller. There is a bit of legwork
 to get tiles to work with it but overall it is pretty fun to use and change the entire scene with a click of a button. There are also listeners that just send events for things like gameobjects or
 particle system. For instance one house starts their fireplace one season earlier than their neighbor.
