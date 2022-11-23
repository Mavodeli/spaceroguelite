using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttractTwo : Ultimate
{
    GameObject player;
    
    GameObject selectedObject1;
    GameObject selectedObject2;

    public float object1Speed;
    public float object2Speed;

    const float SPEED = 5;

    //selects two targets (currently always selects GameObject and GameObject2) and adds a script that to both of them, making them pull together.
    //needed to be split in two (AttractTwo and AttractTwoBehaviour) so the pulling of the objects happens separately
    //and we don't have to wait for them to collide to start the cooldown
    //TODO: make targets selectable.
    //TODO: make it do dmg
    public override void Use()
    {
        //select two targets
        selectedObject1 = GameObject.Find("GameObject");
        selectedObject2 = GameObject.Find("GameObject2");

        //attach the pull script
        AttractTwoBehaviour object1Script = selectedObject1.AddComponent<AttractTwoBehaviour>();
        AttractTwoBehaviour object2Script = selectedObject2.AddComponent<AttractTwoBehaviour>();

        //assign each other as the target to pull towards
        object1Script.target = selectedObject2;
        object2Script.target = selectedObject1;

        //assign the speed of the pull
        object1Script.speed = SPEED;
        object2Script.speed = SPEED;

        isActive = false;
    }
}
