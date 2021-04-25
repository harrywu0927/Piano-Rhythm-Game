using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Button selectMusicBtn,personalDataBtn,challengeBtn,buyMusicBtn,logOutBtn;

    
    public Text welcomeBack, goldCoins, scores;

    void Start()
    {
        welcomeBack.text += Client.mainPack.User.Usrname;
        goldCoins.text += Client.mainPack.User.Goldcoins.ToString();
        scores.text += Client.mainPack.User.Scores.ToString();
        selectMusicBtn.onClick.AddListener(SelectMusicClick);
        personalDataBtn.onClick.AddListener(PersonalDataClick);
        challengeBtn.onClick.AddListener(ChallengeClick);
        buyMusicBtn.onClick.AddListener(BuyMusicClick);
        logOutBtn.onClick.AddListener(LogOutClick);
    }
    void SelectMusicClick()
    {
        SceneManager.LoadScene("SelectMusic");
    }
    void PersonalDataClick()
    {

    }
    void ChallengeClick()
    {
        SceneManager.LoadScene("ChallengeScene");
    }
    void BuyMusicClick()
    {
        SceneManager.LoadScene("MusicShop");
    }
    void LogOutClick()
    {
        Client.ClearPack();
        LoginPanel.isLogin = false;
        SceneManager.LoadScene("LoginScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
