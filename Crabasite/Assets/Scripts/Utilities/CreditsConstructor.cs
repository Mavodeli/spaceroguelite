using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class CreditsConstructor : MonoBehaviour
{
    private GameObject parent;
    private float scroll_speed;
    private float break_bound;
    private float between_tiles;//spacing
    private float all_tiles_height = 0;
    private bool tiles_height_was_updated = false;
    private TimerObject delay_timer;
    private float delay_time = 3;

    void Start()
    {
        parent = gameObject;

        //set scaling parameter according to screen size
        between_tiles = 50;
        break_bound = Screen.height+between_tiles*transform.childCount;
        scroll_speed = 0.00185f*Screen.height;

        gameObject.GetComponent<VerticalLayoutGroup>().spacing = between_tiles;

        string path = "Assets/Resources/credits.txt";
        if(File.Exists(path))
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    GameObject newTile = TMPTile(line);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error occured when trying to load credits data from file:" + path + "\n" + e);
            }
        }

        delay_timer = new TimerObject(gameObject.name+" delay_timer");
        delay_timer.start(delay_time);

        StartCoroutine(waitForCanvasDraw());
    }

    void FixedUpdate()
    {
        //update tiles height    
        if(!tiles_height_was_updated && all_tiles_height != 0){
            break_bound += all_tiles_height;
            tiles_height_was_updated = true;
        }

        if(delay_timer.runs()) return;
    
        if(parent.GetComponent<RectTransform>().position.y <= break_bound)
            parent.GetComponent<RectTransform>().position += new Vector3(0, scroll_speed, 0);
        else{
            delay_timer.setOnRunningOut(delegate(){SceneManager.LoadScene("MainMenu");});
            delay_timer.start(delay_time);
        }
    }

    private GameObject TMPTile(string content){
        GameObject go = new GameObject();
        go.transform.parent = parent.transform;
        go.name = "TMPTile";
        try{
            TMP_Text textComp = go.AddComponent<TextMeshProUGUI>();
            tileFormatter(textComp, content);
        }
        catch(System.ArgumentOutOfRangeException){//spacing tile
            go.name = "spacing "+go.name;
        }
        return go;
    }

    private void tileFormatter(TMP_Text tile, string content){
        //options set for all
        tile.color = new Color(1, 1, 1, .85f);
        tile.alignment = TextAlignmentOptions.Center;

        string format = content.Substring(0, 1);
        tile.text = content.Substring(2);

        if(format == "+"){//main header ('Credits')
            tile.fontSize = (40*Screen.height)/1080;
            tile.fontStyle = FontStyles.Bold;
            parent.GetComponent<RectTransform>().position -= new Vector3(0, tile.fontSize, 0);//dirty fix for main header being visible during start delay
        }
        else if(format == "#"){//header (job)
            tile.fontSize = (34*Screen.height)/1080;
            tile.fontStyle = FontStyles.Bold;
        }
        else if(format == "-"){//body (name)
            tile.fontSize = (26*Screen.height)/1080;
        }
        else{
            throw new System.NotSupportedException("The formatting prefix "+format+" is not a supported line prefix for a credits file! Currently only #, + and - are supported.");
        }
    }

    private IEnumerator waitForCanvasDraw(){
        yield return new WaitForEndOfFrame();
        all_tiles_height = (-1)*transform.GetChild(transform.childCount-1).GetComponent<RectTransform>().position.y;
        all_tiles_height += transform.GetChild(transform.childCount-1).GetComponent<TextMeshProUGUI>().fontSize;
    }
}
