using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    private ManaSystem manaSystem;

    public void Setup(ManaSystem manaSystem){
        this.manaSystem = manaSystem;

        manaSystem.OnManaChanged += ManaSystem_OnManaChanged;
    }
    // event for changing manabarsize
    private void ManaSystem_OnManaChanged(object sender, System.EventArgs e){
        transform.Find("Bar").localScale = new Vector3(manaSystem.GetManaPercent(),1);
    }
}
