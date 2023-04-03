using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using TMPro;

public class CommentarySystem : MonoBehaviour
{
    private static GameObject image;
    private static GameObject textField;
    private static TextMeshProUGUI textToBeDisplayed;
    private static Image imageToBeDisplayed;

    private static Sprite protagonistCommentSprite;
    private static Sprite aiCommentSprite;
    private static Sprite bcCommentSprite;

    private static bool alreadyShowingComment = false;
    private static Queue<string> stashedComments; // stores comments that are waiting to be displayed: #0 followed by an id will display a protagonist comment, #1 followed by an id will display an ai comment

    private GameObject soundController;

    // Typewriter effect
    private static bool isTypeWriting = false;
    private static string typeWriterText = "";
    private static int typeWriterCounter = 0;
    private const float AMOUNT_OF_SECONDS_UNTIL_NEXT_LETTER_APPEARS = 0.05f;
    private static float timeSinceLastLetterAppeared = 0f;

    void Start(){
        image = GameObject.Find("CommentBox");
        textField = GameObject.Find("CommentText");
        image.SetActive(false);
        textField.SetActive(false);

        soundController = GameObject.Find("Sounds");

        textToBeDisplayed = textField.GetComponent<TextMeshProUGUI>();
        imageToBeDisplayed = image.GetComponent<Image>();

        protagonistCommentSprite = Resources.Load<Sprite>("Sprites/TextBoxes/TextBoxProtagonistCommentNewWithArrow");
        aiCommentSprite = Resources.Load<Sprite>("Sprites/TextBoxes/TextBoxAiCommentWithArrow");
        bcCommentSprite = Resources.Load<Sprite>("Sprites/TextBoxes/TextBoxBCCommentWithArrow");

        stashedComments = new Queue<string>();

    }

    void Update(){
        // string str = "stashedComments: ";
        // string[] array = stashedComments.ToArray();
        // for(int i = 0; i < stashedComments.Count; i++){
        //     str += array[i];
        // }
        // if(str != "stashedComments: ") Debug.Log(str);

        if(alreadyShowingComment && isTypeWriting){
            if(Input.GetKeyDown("e")){
                isTypeWriting = false;
                textToBeDisplayed.text = typeWriterText;
            } else {
                if(timeSinceLastLetterAppeared >= AMOUNT_OF_SECONDS_UNTIL_NEXT_LETTER_APPEARS){
                    textToBeDisplayed.text += typeWriterText[typeWriterCounter];
                    soundController.SendMessage("playSound", new SoundParameter("TypewriterSound", this.gameObject, 0.75f, false));
                    typeWriterCounter++;
                    timeSinceLastLetterAppeared -= AMOUNT_OF_SECONDS_UNTIL_NEXT_LETTER_APPEARS;
                    if(textToBeDisplayed.text == typeWriterText){
                        isTypeWriting = false;
                    }
                } else {
                    timeSinceLastLetterAppeared += Time.deltaTime;
                }
            }
        } else if(alreadyShowingComment && !isTypeWriting){
            if(Input.GetKeyDown("e")){
                image.SetActive(false);
                textField.SetActive(false);
                alreadyShowingComment = false;
                if(stashedComments.Count > 0){
                    string nextComment = stashedComments.Dequeue();
                    if(nextComment.StartsWith("#0")){
                        // protagonist comment
                        displayProtagonistComment(nextComment.Substring(2));
                    } else if(nextComment.StartsWith("#1")){
                        // ai comment
                        displayAIComment(nextComment.Substring(2));
                    } else if(nextComment.StartsWith("#2")){
                        // bc comment
                        displayBCComment(nextComment.Substring(2));
                    } else {
                        // This one should never appear
                        Debug.LogWarning("The following comment hsould be displayed but can't, since it is neither an ai-comment nor a protagonist comment: [" + nextComment + "]");
                    }
                }
            }
        }
    }
    
    public static void displayProtagonistComment(string id){
        if(alreadyShowingComment){
            stashedComments.Enqueue("#0" + id);
            return;
        }
        imageToBeDisplayed.sprite = protagonistCommentSprite;
        displayComment(id);
    }

    public static void displayAIComment(string id){
        if(alreadyShowingComment){
            stashedComments.Enqueue("#1" + id);
            return;
        }
        imageToBeDisplayed.sprite = aiCommentSprite;
        displayComment(id);
    }

    public static void displayBCComment(string id){
        if(alreadyShowingComment){
            stashedComments.Enqueue("#2" + id);
            return;
        }
        imageToBeDisplayed.sprite = bcCommentSprite;
        displayComment(id);
    }

    private static void displayComment(string id){
        image.SetActive(true);
        textField.SetActive(true);
        alreadyShowingComment = true;

        isTypeWriting = true;
        typeWriterText = LoadFromFile(id);
        textToBeDisplayed.text = "" + typeWriterText[0];
        typeWriterCounter = 1;
        timeSinceLastLetterAppeared = 0f;
    }

    public static bool isShowingTextbox(){
        return alreadyShowingComment;
    }

    private static string LoadFromFile(string identifier)
    {
        // using Path.Combine because of different Paths of different OS's
        // string path = Path.Combine(Application.persistentDataPath, "english.json");
        string path = "Assets/Resources/english.json";
        string result = "DEFAULT COMMENT: should not appear!";
        if(File.Exists(path))
        {
            Dictionary<string, string> deserializedData = new Dictionary<string, string>();
            try
            {
                string jsonData = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonData = reader.ReadToEnd();
                    }
                }
                deserializedData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
                result = deserializedData[identifier];
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file:" + path + "\n" + e);
            }
        }
        return result;
    }
}

