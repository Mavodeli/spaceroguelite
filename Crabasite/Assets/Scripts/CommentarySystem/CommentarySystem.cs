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

    private static bool alreadyShowingComment = false;
    private static Queue<string> stashedComments; // stores comments that are waiting to be displayed: #0 followed by an id will display a protagonist comment, #1 followed by an id will display an ai comment

    // Typewriter effect
    private static bool isTypeWriting = false;
    private static string typeWriterText = "";
    private static int typeWriterCounter = 0;
    private const float AMOUNT_OF_SECONDS_UNTIL_NEXT_LETTER_APPEARS = 0.1f;
    private static float timeSinceLastLetterAppeared = 0f;

    void Start(){
        image = GameObject.Find("CommentBox");
        textField = GameObject.Find("CommentText");
        image.SetActive(false);
        textField.SetActive(false);

        textToBeDisplayed = textField.GetComponent<TextMeshProUGUI>();
        imageToBeDisplayed = image.GetComponent<Image>();

        protagonistCommentSprite = Resources.Load<Sprite>("Sprites/TextBoxes/TextBoxProtagonistComment");
        aiCommentSprite = Resources.Load<Sprite>("Sprites/TextBoxes/TextBoxAiComment");

        stashedComments = new Queue<string>();

    }

    void Update(){
        if(alreadyShowingComment && isTypeWriting){
            if(Input.GetKeyDown("c")){
                isTypeWriting = false;
                textToBeDisplayed.text = typeWriterText;
            } else {
                if(timeSinceLastLetterAppeared >= AMOUNT_OF_SECONDS_UNTIL_NEXT_LETTER_APPEARS){
                    textToBeDisplayed.text += typeWriterText[typeWriterCounter];
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
            if(Input.GetKeyDown("c")){
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

        //TODO: implement this
        //@Rico: the string obtained with LoadFromFile(id) should appear in a textbox ingame and not as a debug log ;)
        // Debug.Log(LoadFromFile(id));
    }

    public static void displayAIComment(string id){
        if(alreadyShowingComment){
            stashedComments.Enqueue("#1" + id);
        }
        imageToBeDisplayed.sprite = aiCommentSprite;
        displayComment(id);



        //TODO: implement this
        //@Rico: different text box
        // Debug.Log("Rogue AI: "+LoadFromFile(id));//the "Rogue AI: " prefix is for debugging only
    }

    private static void displayComment(string id){
        image.SetActive(true);
        textField.SetActive(true);
        alreadyShowingComment = true;

        isTypeWriting = true;
        typeWriterText = id; // TODO show actual text
        textToBeDisplayed.text = "" + typeWriterText[0];
        typeWriterCounter = 1;
        timeSinceLastLetterAppeared = 0f;
    }

    private static string LoadFromFile(string identifier)
    {
        // using Path.Combine because of different Paths of different OS's
        string path = Path.Combine(Application.persistentDataPath, "english.json");
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

