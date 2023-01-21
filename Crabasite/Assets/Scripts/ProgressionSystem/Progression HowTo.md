### What are all those classes?
- **ProgressionTracker**: Keeps track of the player's progression using a Dictionary of booleans. Those booleans are called flags. 
  - setFlag(id, value): Sets the value of the flag 'id' to 'value'. This is for setting flags (a.k.a. telling the PT that the player has 'unlocked' smth).
  - getFlag(id): Returns true if the given flag 'flag' is set to true. This is for getting flags (a.k.a. asking the PT if the player has 'unlocked' smth).

- **ProgressionTrigger**: Provides the gameObject it is attached to with an OnTriggerEnter. The behaviour of OnTriggerEnter can be defined using a delegate (a.k.a. lambda function). **The gameObjects holding these script have to be placed manually in the Unity Editor as they are different for each level. Add a Collider2D manually. The script will automatically attach the rigidbody but not the collider! See the checklist below for how to set them up.**
  - Setup(_function): Sets the function that should be executed OnTriggerEnter. (only void functions with no arguments are supported!)

- **Level_ProgressionScript**: This script contols the progression system for a specific level. It sets progression flags and/or checks if certain flags are set. It furthermore defines the behaviour of all the progression triggers in the scene. It should be used to trigger events like receiving a new mail, starting a BC dialogue, etc. (see DemoLevel_ProgressionScript.cs for examples on how this script could look like)

- **ProgressionDelegate**: provides child scripts with the delegate for triggering events via the ProgressionTrigger. **Level_ProgressionScripts must inherit from this class!**

#### ProgressionTrigger GameObject Checklist
- has the Gameobject a Collider2D?
- has the Gameobject a ProgressionTrigger script attached?
- has the Gameobject the "ProgressionTrigger" tag?
- did you set the collider bounds correctly?
- did you define the ProgressionTriggers' behaviour in the level's Level_ProgressionScript?