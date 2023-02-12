using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;

    public void Setup(HealthSystem healthSystem){
        this.healthSystem = healthSystem;

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }
    // event for changing healtbarsize
    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e){
        transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(),1);
        if (transform.parent.tag == "HUD")
        {
            GameObject.FindGameObjectWithTag("HUD").transform.Find("PlayerHealthBar").GetComponent<Slider>().value = healthSystem.GetHealthPercent();
        }
    }
}
