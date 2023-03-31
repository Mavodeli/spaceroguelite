using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRoomHatchBehaviour : MonoBehaviour, IDataPersistence
{
    private DataPersistenceManager dpm;
    private Rigidbody2D rb;
    private bool loose;

    void Start()
    {
        dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(loose) detach();
    }

    private void detach(){
        rb.constraints = RigidbodyConstraints2D.None;
        loose = true;
        GameObject.FindGameObjectWithTag("QuestEventsContainer").SendMessage("InvokeEvent", "detachedEngineRoomHatch", SendMessageOptions.DontRequireReceiver);
    }

    public bool isLoose(){
        return loose;
    }

    public void LoadData(GameData data)
    {
        transform.localPosition = data.AS_EngineRoomPosition;
        transform.Rotate(data.AS_EngineRoomRotation, Space.World);
        transform.localScale = data.AS_EngineRoomScale;

        loose = data.AS_EngineRoomHatchLoose;
    }

    public void SaveData(ref GameData data)
    {
        data.AS_EngineRoomPosition = transform.localPosition;
        data.AS_EngineRoomRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        data.AS_EngineRoomScale = transform.localScale;

        data.AS_EngineRoomHatchLoose = loose;
    }
}
