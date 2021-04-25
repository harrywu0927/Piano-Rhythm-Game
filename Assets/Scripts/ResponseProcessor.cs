using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    class ResponseProcessor
    {
        public void Login()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                Debug.Log("Login Success");
                LoginPanel.isLogin = true;
                Client.userid = Client.mainPack.User.Userid;
            }
            else
            {
                Debug.Log("Login Failed");
            }
        }
        public void Signup()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                Debug.Log("Signup Success");
                SignupPanel.signuped = true;
            }
            else if(Client.mainPack.Returncode == ReturnCode.UserExists)
            {
                Debug.Log("User is already existed");
            }
            else
            {
                Debug.Log("Server exception,Signup failed");
            }
        }
        public void SearchSongs()
        {
            if(Client.mainPack.Returncode == ReturnCode.Succeed && Client.mainPack.Requestcode == RequestCode.SongControl)
            {
                //Debug.Log(Client.mainPack.Requestcode);
                MusicShop.getSearchResult = true;
            }
            else if (Client.mainPack.Returncode == ReturnCode.Succeed && Client.mainPack.Requestcode == RequestCode.UserControl)
            {
                SelectMusic.getSearchResult = true;
                Debug.Log(Client.mainPack.Requestcode);
            }
            else
            {
                Debug.Log("search fail");
            }
        }
        public void Buysongs()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                Debug.Log("Buy success");
            }
            else if(Client.mainPack.Returncode == ReturnCode.GoldcoinNotEnough)
            {
                Debug.Log("Goldcoins not enough");
            }
        }
        public void GameSettlement()
        {
            if (Client.mainPack.Returncode == ReturnCode.Succeed)
            {
                GameResult.getResult = true;
                Debug.Log(Client.mainPack.Gameresultpack);
            }
            else
            {
                Debug.Log("Game settlement fail");
            }
        }
    }
}
