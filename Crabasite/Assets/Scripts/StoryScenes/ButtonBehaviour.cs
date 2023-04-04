using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonBehaviour : MonoBehaviour
{
    int counter = 0;
    private string postEndingScene = "Credits";//"MainMenu";

    public void LoadStoryScene1()
    {
        SceneManager.LoadScene("StoryScene1");
    }

    public void LoadStoryScene2()
    {
        SceneManager.LoadScene("StoryScene2");
    }

    public void LoadColonyEnding()
    {
        SceneManager.LoadScene("ColonyEndingScene");
    }

    public void LoadSplitEnding()
    {
        SceneManager.LoadScene("SplitEndingScene");
    }

    public void LoadTakeEnding()
    {
        SceneManager.LoadScene("TakeEndingScene");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2 - abandoned spaceship");
    }

    public void TakeEndingEMail()
    {
        if (counter == 0)
        {
            TMP_Text text = GameObject.Find("EndingText").GetComponent<TMP_Text>();
            text.text = "'In accordance with the Imperial Criminal Justice Declaration and the Criminal Prosecution Regulations for Independent Contractors in Service of Imperial Frontier Colonies you have been found guilty and are therefore convicted of (1) theft of imperial property, (2) insufficient fulfillment of contract obligations towards imperial authorities and (3) murder in 1257 cases. All the offenses contained herein of which you were convicted have been added to your criminal record. You currently have at least one conviction or pending charge under imperial law that is not either sealed or expunged, therefore your status as an Imperial Citizen is hereby revoked with immediate effect. Since you are convicted of at least one criminal offense against His Imperial Highness, an arrest warrant as well as a bounty have been issued against you in all systems under expanded imperial juristiction. Since you no longer possess the status of Imperial Citizen, please return any and all items that are considered state property under imperial law in your possession. Failing to do so will result in a charge against you, the amount of which will be determined based on any and all such items in your possession at the time your citizenship was revoked. Please report to your Legal Supervisor to clear your outstanding charges and/or receive punishment for your convictions.'";
            counter += 1;
        }
        else if (counter >= 1)
        {
            SceneManager.LoadScene(postEndingScene);
        }
    }

    public void SplitEndingEMail()
    {
        if (counter == 0)
        {
            TMP_Text text = GameObject.Find("EndingText").GetComponent<TMP_Text>();
            text.text = "'For your service to His Imperial Highness in retrieving a cure for the parasite infection that plaques the imperial colony on Cryke 2H23, the Ministry of Medical Affairs has been granted permission and is most pleased to bestow the title of 'Honorary Colonial Subject' upon you. You have furthermore been granted permission to purchase residential property in the Imperial City. To claim the certificate signed by the High Secretary herself and the monetary compensation that come with your new civil status, please visit your Legal Supervisor at your convenience. Sincerely, Qarea, Chief Scientist on Cryke 2H23 on behalf of the Ministry of Medical Affairs. PS: I'm very sorry that on account of your physical condition we cannot meet in person and I hope that this message finds you in a state where you are still able to comprehend its meaning. Your wife passed away half an hour ago. The treatment sadly didn't do much to contain the parasite, the infection had already spread too far by the time you returned with the cure. According to her wishes, we will scatter her cremated remains outside of Cryke's atmosphere, as we did with those of your child. Since you do not have any relative eligible to inherit your possessions, they will be assimilated into colonial property upon your passing.'";
            counter += 1;
        }
        else if (counter >= 1)
        {
            SceneManager.LoadScene(postEndingScene);
        }
    }   

    public void ColonyEndingEMail()
    {
        if (counter == 0)
        {
            TMP_Text text = GameObject.Find("EndingText").GetComponent<TMP_Text>();
            text.text = "'Hey there! I just wanted to let you know that the medication they developed from the cure sample you brought back from the Asclepius is working great. Sasha responded really well to the treatment, they were among the first to receive a dose. By now they are as good as new and I'm on the mend as well. They are asking about you all the time. Yeah, I haven't told them yet. I know I shouldn't put it off like that, I... guess I still haven't completely come to terms with it myself. It feels so unreal y'know? Like, I always imagined the three of us would get through all of this together. That we would live our lives together. I suppose I haven't fully realised that... yeah, that that's it. You're gone, You have moved on without me. And the last thing you did was to save all of us... I'm not even sure I deserve that kind of sacrifice. Anyway, I don't want to end this message on such a heavy note :) I hope you're doing well, where ever you are right now. I hope this other place is nice and that you are happy. I think I should wrap it up for now so... if you can and if it's not too much to ask, wait for me. I wish for us to be reunited again someday, I really do. And if only to tell you about all the things happened in your absence, about how Sasha grew up, how they did in school, about the first partner they brought home and so on. But for now... so long!'";
            counter += 1;
        } else if (counter >= 1)
        {
            counter = 0;
            SceneManager.LoadScene(postEndingScene);
        }
    }
}
