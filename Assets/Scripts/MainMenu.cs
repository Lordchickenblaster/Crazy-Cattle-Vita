using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public EventTrigger[] buttons;
    public Text[] buttonTexts;
    public Color[] specialNormaColors;
    public Color hoverColor;
    public Color pressColor;
    public GameObject Map;
    public GameObject MMU;
    public GameObject MML;
    public GameObject TrophyRoom;
    public GameObject TrophyRoomBB;
    public GameObject Options;
    public GameObject Credits;
    public GameObject CreditsGB;
    public GameObject[] Maps;
    public GameObject[] Beaten;
    public GameObject ViewCredits;
    public Image TrophyRoomi;
    public Sprite[] TRSprites;
    int mbeat = 0;

    void DeactivateAll()
    {
        Map.SetActive(false);
        MMU.SetActive(false);
        MML.SetActive(false);
        TrophyRoom.SetActive(false);
        TrophyRoomBB.SetActive(false);
        Options.SetActive(false);
        Credits.SetActive(false);
        CreditsGB.SetActive(false);
    }

    public void ShowMainMenu()
    {
        DeactivateAll();
        MMU.SetActive(true);
        MML.SetActive(true);
    }

    public void ShowMap()
    {
        DeactivateAll();
        Map.SetActive(true);
    }

    public void ShowTrophyRoom()
    {
        DeactivateAll();
        TrophyRoom.SetActive(true);
        TrophyRoomBB.SetActive(true);
    }

    public void ShowCredits()
    {
        DeactivateAll();
        Credits.SetActive(true);
        CreditsGB.SetActive(true);
    }

    public void ShowOptions()
    {
        DeactivateAll();
        Options.SetActive(true);
    }

    void Start()
    {
        mbeat = PlayerPrefs.GetInt("MapsBeat", 0);
        ViewCredits.SetActive(mbeat == Maps.Length);
        TrophyRoomi.sprite = TRSprites[mbeat];
        for (int i = 0; i < mbeat; i++)
        {
            Beaten[i].SetActive(true);
        }
        for (int i = 0; i < mbeat+1; i++)
        {
            Maps[i].SetActive(true);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            AddEvent(buttons[i], EventTriggerType.PointerEnter, (data) => {
                buttonTexts[index].color = hoverColor;
            });

            AddEvent(buttons[i], EventTriggerType.PointerExit, (data) => {
                buttonTexts[index].color = specialNormaColors[index];
            });

            AddEvent(buttons[i], EventTriggerType.PointerDown, (data) => {
                buttonTexts[index].color = pressColor;
            });
        }
    }

    void AddEvent(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((data) => action.Invoke(data));
        trigger.triggers.Add(entry);
    }

    public void Ireland()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ireland");
    }

    public void Egypt()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Egypt");
    }

    public void Sweden()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sweden");
    }
}