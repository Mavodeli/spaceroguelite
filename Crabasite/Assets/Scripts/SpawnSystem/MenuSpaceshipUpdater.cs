using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSpaceshipUpdater : MonoBehaviour
{
    DataPersistenceManager dpm;
    Sprite sprite;

    void Start()
    {
        dpm = GameObject.FindGameObjectWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        GameData existingGameData = dpm.getGameData(true);
        bool[] booleans = new bool[3];
        string[] ids = new string[]{"RepairWindshield", "RepairSpaceship", "InstallNewHyperdriveCore"};
        for(int i = 0; i < 3; i++){
            try{
                booleans[i] = !existingGameData.activeQuests[ids[i]];
            }
            catch(KeyNotFoundException){
                booleans[i] = false;
            }
        }
        string str = ConstructSpriteString.Spaceship(SceneManager.GetActiveScene().name, booleans[0], booleans[1], booleans[2]);
        sprite = Resources.Load<Sprite>(str);
        StartCoroutine(waitForCanvasDraw());
    }

    private IEnumerator waitForCanvasDraw(){
        yield return new WaitForEndOfFrame();
        gameObject.GetComponent<Image>().sprite = sprite;
    }
}
