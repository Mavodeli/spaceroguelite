### What are all those objects?
- **ProgressionTracker**: Keeps track of the player's progression using a Dictionary of booleans. Those booleans are called flags. 
  - setFlag(id, value): Sets the value of the flag 'id' to 'value'. This is for setting flags (a.k.a. telling the PT that the player has 'unlocked' smth).
  - isTrueAt(flag): Returns true if the given flag 'flag' is set to true. This is for getting flags (a.k.a. asking the PT if the player has 'unlocked' smth).

- **ProgressionTrigger**: Provides the gameObject it is attached to with a OnTriggerEnter. The behaviour of OnTriggerEnter can be defined using a delegate (a.k.a. lambda function). **The gameObjects holding these script have to be placed manually in the Unity Editor as they are different for each level. Add a Collider2D manually. The script will automatically attach the rigidbody but not the collider! See the checklist below for how to set them up.**
  - Setup(_function): Sets the function that should be executed OnTriggerEnter. (only void functions with no arguments are supported!)

- **Level_ProgressionScript**: This script contols how the progression flags are to be set for the level. It furthermore defines the conditions under which what events (those can be arbitrary, common examples include: receiving a new mail, starting a BC dialogue, etc.) are triggered.

- **ProgressionDelegate**: provides child scripts with the delegate for triggering events via the ProgressionTrigger. **Level_ProgressionScripts must inherit from this class!**

#### ProgressionTrigger GameObject Checklist
- has the Gameobject a Collider2D?
- has the Gameobject a ProgressionTrigger script attached?
- did you set the collider bounds correctly?
- did you define the ProgressionTriggers' behaviour in the level's Level_ProgressionScript?