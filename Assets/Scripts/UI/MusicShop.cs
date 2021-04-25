using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class MusicShop : MonoBehaviour
{
    // Start is called before the first frame update
    public Button mItemPrefab;
    private Transform mContentTransform;
    public Dropdown selectAuthor;
    public InputField songName;
    public Button searchBtn, backBtn;
    public KeyCode keyCode;
    public static bool getSearchResult = false;
    //private Scrollbar mScrollbar;
    int lastindex;
    // Use this for initialization
    void Start()
    {
        mContentTransform = this.transform.Find("Scroll View/Viewport/Content");
        backBtn.onClick.AddListener(BackClick);
        searchBtn.onClick.AddListener(SearchClick);
        SearchClick();
        lastindex = -1;
        selectAuthor.onValueChanged.AddListener(DropdownListClick);
        //mScrollbar = this.transform.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        //mScrollbar.value = 1.0f;

    }

    void ShowItems(MainPack pack)
    {
        DestroySongList();
        for (int i = 0; i < pack.Songs.Count; i++)
        {
            Text[] texts = mItemPrefab.GetComponentsInChildren<Text>();
            texts[0].text = pack.Songs[i].Author;
            texts[1].text = "难度:" + pack.Songs[i].Difficulty.ToString();
            texts[2].text = "下载量:" + pack.Songs[i].Downloads.ToString();
            texts[3].text = "金币:" + pack.Songs[i].Price.ToString();
            texts[4].text = pack.Songs[i].Name;
            Button item = Instantiate(mItemPrefab, transform.position, transform.rotation);
            item.tag = "songlist";
            item.transform.SetParent(mContentTransform);
            item.onClick.AddListener(BuySong);
            item.name = pack.Songs[i].Name;
        }
    }
    void DestroySongList()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("songlist"))
        {
            Destroy(obj);
        }
    }
    void InitDropDownList()
    {
        List<string> authors = new List<string>();
        selectAuthor.ClearOptions();
        bool find = false;
        int index = 1;
        authors.Add("");
        for(int i = 0; i < Client.mainPack.Authors.Count; i++)
        {
            authors.Add(Client.mainPack.Authors[i]);
            if (Client.mainPack.Authors[i] == Client.mainPack.Searchsongpack.Author)
                find = true;
            if (!find)
                index++;

        }
        selectAuthor.AddOptions(authors);
        if(Client.mainPack.Searchsongpack.Author!="")
            selectAuthor.value = index;
    }
    void DropdownListClick(int index)
    {
        if (index != lastindex)
        {
            Client.mainPack.Actioncode = ActionCode.SearchSongs;
            Client.mainPack.Requestcode = RequestCode.SongControl;

            SearchSongPack searchSongPack = new SearchSongPack();
            searchSongPack.Author = selectAuthor.options[index].text;
            searchSongPack.SongName = songName.text;
            Client.mainPack.Searchsongpack = searchSongPack;
            //Client client = new Client();
            Debug.Log(index);
            lastindex = index;
            Client.Send(Client.mainPack);
        }
        
    }

    void BackClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void BuySong()
    {
        Client.mainPack.Actioncode = ActionCode.Buysongs;
        Client.mainPack.Requestcode = RequestCode.UserControl;
        var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        SearchSongPack searchSongPack = new SearchSongPack();
        searchSongPack.SongName = buttonSelf.name;
        Debug.Log(buttonSelf.name);
        Client.mainPack.Searchsongpack = searchSongPack;
        Client.Send(Client.mainPack);
    }
    void SearchClick()
    {
        Client.mainPack.Actioncode = ActionCode.SearchSongs;
        Client.mainPack.Requestcode = RequestCode.SongControl;
        
        SearchSongPack searchSongPack = new SearchSongPack();
        searchSongPack.Author = selectAuthor.options[selectAuthor.value].text;
        searchSongPack.SongName = songName.text;
        Client.mainPack.Searchsongpack = searchSongPack;
        //Client client = new Client();

        Client.Send(Client.mainPack);
        
    }
    private void Update()
    {
        if (Input.GetKeyUp(keyCode))
        {
            SearchClick();
        }
        if (getSearchResult)
        {
            ShowItems(Client.mainPack);
            getSearchResult = false;
            InitDropDownList();
            //Client.Close();
        }
    }
}
