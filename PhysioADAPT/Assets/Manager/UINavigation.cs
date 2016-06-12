using Assets.Manager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UINavigation : MonoBehaviour {

    public GameObject Intro, CloseBtn, Panel1, Panel2, Panel3, PanelOver;
    public GameObject Settings1, Settings2;
    public Toggle Panel1Toggle, Panel2Toggle;
    public Dropdown EnvironmentSelector;
    private float _timer = 3.0f;
    public static bool GameOver;
    public GameObject UDPprefab, ManagerPrefab;
    private bool _firstScreenSet;
    private int _environment = 1;
    public InputField PortReceive, PortSend, IPSend;
    public Text LocalIP;

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        var udp = GameObject.FindGameObjectWithTag("UDP");

        if (udp == null)
            Instantiate(UDPprefab);

        var manager = GameObject.FindGameObjectWithTag("Manager");

        if (manager == null)
            Instantiate(ManagerPrefab);

        _firstScreenSet = false;
    }

    
    private void Update()
    {
        if (!_firstScreenSet)
        {
            if (!GameOver)
                SetInterface(0);
            else
            {
                SetInterface(4);
                GameOver = false;
            }

            _firstScreenSet = true;
        }

        if (Intro.activeSelf)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                LocalIP.text = UDPReceiver.LocalIPAddress();
                SetInterface(1);
            }
        }

        Panel1Toggle.gameObject.GetComponentInChildren<Text>().text = Panel1Toggle.isOn ? "Disconnect" : "Connect";

        Panel2Toggle.gameObject.GetComponentInChildren<Text>().text = Panel2Toggle.isOn ? "Disconnect" : "Connect";
        
    }

    /// <summary>
    /// Sets interface screens according to given input
    /// </summary>
    /// <param name="step"></param>
    public void SetInterface(int step)
    {
        if (step == 0)
        {
            Intro.SetActive(true);
            Intro.SetActive(true);
            CloseBtn.SetActive(false);
            Panel1.SetActive(false);
            Panel2.SetActive(false);
            Panel3.SetActive(false);
            PanelOver.SetActive(false);
        }
        else if (step == 1)
        {
            Intro.SetActive(false);
            CloseBtn.SetActive(true);
            Panel1.SetActive(true);
            Settings1.SetActive(false);
            Panel2.SetActive(false);
            Panel3.SetActive(false);
            PanelOver.SetActive(false);
        }
        else if (step == 2)
        {
            Intro.SetActive(false);
            CloseBtn.SetActive(true);
            Panel1.SetActive(false);
            Panel2.SetActive(true);
            Settings2.SetActive(false);
            Panel3.SetActive(false);
            PanelOver.SetActive(false);
        }
        else if (step == 3)
        {
            Intro.SetActive(false);
            CloseBtn.SetActive(true);
            Panel1.SetActive(false);
            Panel2.SetActive(false);
            Panel3.SetActive(true);
            PanelOver.SetActive(false);
        }
        else if (step == 4)
        {
            Intro.SetActive(false);
            CloseBtn.SetActive(true);
            Panel1.SetActive(false);
            Panel2.SetActive(false);
            Panel3.SetActive(false);
            PanelOver.SetActive(true);
        }
    }

    public void Panel1Settings()
    {
        Settings1.SetActive(!Settings1.activeSelf);
    }

    public void Panel2Settings()
    {
        Settings2.SetActive(!Settings2.activeSelf);
    }

    public void SetEnvironment()
    {
        //_environment = EnvironmentSelector.value + 1;
    }

    public void LaunchEnvironment()
    {
        _environment = EnvironmentSelector.value + 1;
        DataManager.EnvironmentName = EnvironmentSelector.captionText.text;
        Debug.Log(DataManager.EnvironmentName);
        SceneManager.LoadScene(_environment);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReceiveConnection()
    {
        UDPReceiver.Port = PortReceive.text;
        UDPReceiver.Connected = !UDPReceiver.Connected;
    }

    public void SendConnection()
    {
        UDPSender.IP = IPSend.text;
        UDPSender.Port = PortSend.text;
        UDPSender.Connected = !UDPSender.Connected;
    }
}
