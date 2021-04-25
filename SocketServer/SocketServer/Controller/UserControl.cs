using System;
using System.Collections.Generic;
using System.Text;
using SocketGameProtocol;
using SocketServer.DAO;
using System.Data.SqlClient;
using System.Security.Cryptography;
namespace SocketServer.Controller
{
    class UserControl
    {
        private readonly UserData userData;
        private readonly SongData songData;
        private readonly UsersSongData usersSongData;
        public UserControl()
        {
            //requestCode = RequestCode.User;
            userData = new UserData();
            songData = new SongData();
            usersSongData = new UsersSongData();
        }

        /// MD5 16位加密
        public static string GetMd5Str(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            //t2 = t2.ToLower();
            return t2;
        }
        // 注册
        public MainPack Signup(MainPack pack)
        {
            pack.Loginpack.Password = GetMd5Str(pack.Loginpack.Password);
            if (userData.Signup(pack)=="Succeed")
            {
                pack.Returncode = ReturnCode.Succeed;
                Console.WriteLine("注册成功");
            }
            else if(userData.Signup(pack) == "User Exists")
            {
                pack.Returncode = ReturnCode.UserExists;
                Console.WriteLine("用户已存在");
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }


        // 登录
        public MainPack Login(MainPack pack)
        {
            pack.Loginpack.Password = GetMd5Str(pack.Loginpack.Password);
            if (userData.Login(pack).Returncode == ReturnCode.Succeed)
            {
                Console.WriteLine("用户"+pack.User.Usrname+"登录成功");
                User user = userData.GetUser(pack.User.Userid);
                pack.User = user;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }

        //购买歌曲
        public MainPack Buysongs(MainPack pack)
        {
            User user = userData.GetUser(pack.User.Userid);
            Song song = songData.SerchSongByName(pack.Searchsongpack.SongName);
            if (!usersSongData.SongExist(pack.Searchsongpack.SongName, pack.User.Userid))
            {
                if (user.Goldcoins >= song.Price)
                {
                    if(user.Level >= song.Requirelevel)
                    {
                        if (userData.CostGoldCoins(user.Userid, song.Price))
                        {
                            usersSongData.AddSong(song.Name, user.Userid);    //添加到用户的歌曲资产
                            song.Downloads++;     //下载量加1
                            songData.UpdateSongData(song);
                            pack.Returncode = ReturnCode.Succeed;
                        }
                        else
                        {
                            pack.Returncode = ReturnCode.Fail;
                        }
                    }
                    else
                    {
                        pack.Returncode = ReturnCode.LevelNotEnough;
                    }
                }
                else
                {
                    pack.Returncode = ReturnCode.GoldcoinNotEnough;
                }
            }
            else
            {
                pack.Returncode = ReturnCode.SongExists;
            }
            
      
            return pack;
        }

        //在已有歌曲中搜索
        public MainPack SearchSongs(MainPack pack)
        {
            pack.Songs.Clear();
            pack.Userssong.Clear();
            if ((pack.Searchsongpack.SongName == "") && (pack.Searchsongpack.Author == ""))
            {
                pack.Authors.Add(songData.GetAllAuthors());
                //Console.WriteLine(pack.User.Userid);
                pack.Userssong.Add(usersSongData.GetAllSongs(pack.User.Userid));
                if (pack.Userssong != null && pack.Authors != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }

            }
            else if ((pack.Searchsongpack.SongName == "") && (pack.Searchsongpack.Author != ""))
            {
                pack.Userssong.Add(usersSongData.GetSongsByAuthor(pack.Searchsongpack.Author));
                if (pack.Userssong != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }
            }
            else if ((pack.Searchsongpack.SongName != "") && (pack.Searchsongpack.Author == ""))
            {
                pack.Userssong.Add(usersSongData.GetSongsByName(pack.Searchsongpack.SongName));
                if (pack.Userssong != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }
            }
            else if ((pack.Searchsongpack.SongName != "") && (pack.Searchsongpack.Author != ""))
            {
                pack.Userssong.Add(usersSongData.GetSongsByNameAndAuthor(pack.Searchsongpack.SongName, pack.Searchsongpack.Author));
                if (pack.Userssong != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }

            }
            return pack;
        }

    }
}
