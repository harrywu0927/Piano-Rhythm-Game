using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;
using System.Net.Sockets;
using Assets.Scripts;
using UnityEngine.SceneManagement;
public class GameResult : MonoBehaviour
{
    public Text Perfect, Great, Good, Miss, Combo, FinalScore, Goldcoins, Exp;
    public Button Next, Retry;
    public static bool getResult;

    // Start is called before the first frame update
    void Start()
    {
        Next.onClick.AddListener(NextClick);
        Retry.onClick.AddListener(RetryClick);
        //getResult = false;
    }
    void NextClick()
    {
        SceneManager.LoadScene("SelectMusic");
    }
    void RetryClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (getResult==true)
        {
            InitResult();
            Debug.Log("gameresult init");
            getResult = false;
        }
    }
    void InitResult()
    {
        Combo.text = Client.mainPack.Gameresultpack.Combo.ToString();
        Perfect.text = Client.mainPack.Gameresultpack.Perfect.ToString();
        Great.text = Client.mainPack.Gameresultpack.Great.ToString();
        Good.text = Client.mainPack.Gameresultpack.Good.ToString();
        Miss.text = Client.mainPack.Gameresultpack.Miss.ToString();
        FinalScore.text = (Client.mainPack.Gameresultpack.Gamescore * 100).ToString() + "%";
        Goldcoins.text = Client.mainPack.Gameresultpack.Goldcoin.ToString();
        Exp.text = Client.mainPack.Gameresultpack.Experience.ToString();
    }
}
