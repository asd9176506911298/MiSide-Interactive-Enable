﻿using InteractiveEnable;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniverseLib.UI;

public class MainPanel : UniverseLib.UI.Panels.PanelBase
{
    public MainPanel(UIBase owner) : base(owner) { }

    public override string Name => "Interactive Enable Mod";
    public override int MinWidth => 600;
    public override int MinHeight => 300;
    public override Vector2 DefaultAnchorMin => new(0.25f, 0.25f);
    public override Vector2 DefaultAnchorMax => new(0.5f, 0.5f);
    public override bool CanDragAndResize => true;

    protected Text difficultText { get; private set; } = null!;
    protected Text musicText { get; private set; } = null!;

    protected override void ConstructPanelContent()
    {
        Text myText = UIFactory.CreateLabel(ContentRoot, "Features", "Features");
        UIFactory.SetLayoutElement(myText.gameObject, minWidth: 200, minHeight: 25);

        CreateInteractiveEnableToggle();
        CreateMiniGameToggle();
        CreateOpenDoorToggle();
        CreateTVGameButton();
        CreateDanceCarGameButton();
        CreateGlitchGameButton();


        CreateDifficultButton();
        CreateChangeMusicButton();
        CreateExitDanceButton();
        Owner.Enabled = false;
    }

    private void CreateExitDanceButton()
    {
        var exitDance = UIFactory.CreateButton(ContentRoot, "DanceGameExitButton", "Exit Dance Game", null);
        UIFactory.SetLayoutElement(exitDance.Component.gameObject, minHeight: 25, minWidth: 200);
        Text nameLabel = exitDance.Component.GetComponentInChildren<Text>();
        nameLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        nameLabel.alignment = TextAnchor.MiddleLeft;
        exitDance.OnClick += ExitDance;
    }

    private void ExitDance()
    {
        foreach (var x in GameObject.FindObjectsOfType<Location7_GameDance>())
        {
            x.eventFinishPlayer.Invoke();
            if (x.indexFinishMita >= 3)
            {
                GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Игры/Dance/Interactive DanceStart/Canvas").GetComponent<ObjectInteractive_CaseInfo>().dontDestroyAfter = true;
                GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Игры/Dance/Interactive DanceStart").GetComponent<ObjectInteractive>().active = true;
            }
            Plugin.Log.LogInfo($"DanceGame eventFinishPlayer");
        }
    }

    private void CreateChangeMusicButton()
    {
        var ChangeMusicRow = UIFactory.CreateHorizontalGroup(
          ContentRoot,
          "ChangeMusicRow",
          false,
          true,
          true,
          true,
          2,
          new Vector4(2, 2, 2, 2)
      );
        UIFactory.SetLayoutElement(
           ChangeMusicRow,
           minHeight: 25,
           minWidth: 200,
           flexibleHeight: 0,
           flexibleWidth: 9999
       );

        var prevButton = UIFactory.CreateButton(ChangeMusicRow, "perv", "perv", null);
        UIFactory.SetLayoutElement(prevButton.Component.gameObject, minHeight: 25, minWidth: 50);
        Text easyLabel = prevButton.Component.GetComponentInChildren<Text>();
        easyLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        easyLabel.alignment = TextAnchor.MiddleCenter;
        prevButton.OnClick += prevMusic;

        var nextButton = UIFactory.CreateButton(ChangeMusicRow, "next", "next", null);
        UIFactory.SetLayoutElement(nextButton.Component.gameObject, minHeight: 25, minWidth: 50);
        Text hardLabel = nextButton.Component.GetComponentInChildren<Text>();
        hardLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        hardLabel.alignment = TextAnchor.MiddleCenter;
        nextButton.OnClick += nextMusic;

        musicText = UIFactory.CreateLabel(ChangeMusicRow, "Music", "Music:");
        UIFactory.SetLayoutElement(difficultText.gameObject, minHeight: 25, minWidth: 200, flexibleWidth:100); 
    }

    private void prevMusic()
    {
        foreach (var x in GameObject.FindObjectsOfType<Location7_GameDance>())
        {
            if (x.musicIndexPlay > 0 && x.musicIndexPlay <= 2)
                x.musicIndexPlay -= 1;
            musicText.text = "Music: " + GlobalLanguage.GetString("Location 7", x.musicIndexPlay + 1);
            x.ChangeMusic();
            Plugin.Log.LogInfo($"musicName: {GlobalLanguage.GetString("Location 7", x.musicIndexPlay + 1)}");
        }
    }

    private void nextMusic()
    {
        foreach (var x in GameObject.FindObjectsOfType<Location7_GameDance>())
        {
            if (x.musicIndexPlay >= 0 && x.musicIndexPlay < 2)
                x.musicIndexPlay += 1;
            musicText.text = "Music: " + GlobalLanguage.GetString("Location 7", x.musicIndexPlay + 1);
            x.ChangeMusic();
            Plugin.Log.LogInfo($"musicName: {GlobalLanguage.GetString("Location 7", x.musicIndexPlay + 1)}");
        }
    }

    private void CreateDifficultButton()
    {
        var difficultRow = UIFactory.CreateHorizontalGroup(
            ContentRoot,
            "DifficultRow",
            false,
            true,
            true,
            true,
            2,
            new Vector4(2, 2, 2, 2)
        );
        UIFactory.SetLayoutElement(
           difficultRow,
           minHeight: 25,
           minWidth: 200,
           flexibleHeight: 0,
           flexibleWidth: 999
       );

        var easyButton = UIFactory.CreateButton(difficultRow, "easy", "easy", null);
        UIFactory.SetLayoutElement(easyButton.Component.gameObject, minHeight: 25, minWidth: 50);
        Text easyLabel = easyButton.Component.GetComponentInChildren<Text>();
        easyLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        easyLabel.alignment = TextAnchor.MiddleCenter;
        easyButton.OnClick += FightEasy;

        var hardButton = UIFactory.CreateButton(difficultRow, "hard", "hard", null);
        UIFactory.SetLayoutElement(hardButton.Component.gameObject, minHeight: 25, minWidth: 50);
        Text hardLabel = hardButton.Component.GetComponentInChildren<Text>();
        hardLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        hardLabel.alignment = TextAnchor.MiddleCenter;
        hardButton.OnClick += FightHard;

        difficultText = UIFactory.CreateLabel(difficultRow, "Difficult", "Difficult:");
        UIFactory.SetLayoutElement(difficultText.gameObject, minHeight: 25, minWidth: 100);
    }

    private void FightEasy()
    {
        foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
        {
            x.enemyComplexity += 1;
            difficultText.text = "Difficult: " + x.enemyComplexity.ToString();
            Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
        }
    }

    private void FightHard()
    {
        foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
        {
            if (x.enemyComplexity > 0)
                x.enemyComplexity -= 1;
            difficultText.text = "Difficult: " + x.enemyComplexity.ToString();
            Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
        }
    }

    private void CreateTVGameButton()
    {
        var tpDance = UIFactory.CreateButton(ContentRoot, "DanceGameButton", "Teleport TV Game", null);
        UIFactory.SetLayoutElement(tpDance.Component.gameObject, minHeight: 25, minWidth: 200);
        Text nameLabel = tpDance.Component.GetComponentInChildren<Text>();
        nameLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        nameLabel.alignment = TextAnchor.MiddleLeft;
        tpDance.OnClick += tpTVClick;
    }


    void tpTVClick()
    {
        if (SceneManager.GetActiveScene().name != "Scene 4 - StartSecret")
        {
            if (SceneManager.GetActiveScene().name != "SceneMenu")
                SceneManager.LoadScene("Scene 4 - StartSecret", LoadSceneMode.Single);

            if (GameObject.Find("MenuGame") != null)
                GameObject.Find("MenuGame").GetComponent<Menu>().ButtonLoadScene("Scene 4 - StartSecret");

            return;
        }
        GameObject.Find("World/House/HouseGameNormal Tamagotchi/HouseGame Tamagotchi/House/Main").SetActive(true);
        GameObject.Find("World/Quests/Quest 1/Addon/Interactive Aihastion").GetComponent<ObjectInteractive>().active = true;
        GameObject.Find("World/Quests/Quest 1/Addon/Interactive Cards >>").GetComponent<ObjectInteractive>().active = true;
        GlobalTag.player.transform.position = new Vector3(0.68f, 0f, 0f);
    }

    private void CreateGlitchGameButton()
    {
        var tpGlitch = UIFactory.CreateButton(ContentRoot, "GlitchGameButton", "Teleport Glitch Game", null);
        UIFactory.SetLayoutElement(tpGlitch.Component.gameObject, minHeight: 25, minWidth: 200);
        Text nameLabel = tpGlitch.Component.GetComponentInChildren<Text>();
        nameLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        nameLabel.alignment = TextAnchor.MiddleLeft;
        tpGlitch.OnClick += tpGlitchClick;
    }

    void tpGlitchClick()
    {
        if (SceneManager.GetActiveScene().name != "Scene 19 - Glasses")
        {
            if (SceneManager.GetActiveScene().name != "SceneMenu")
                SceneManager.LoadScene("Scene 19 - Glasses", LoadSceneMode.Single);

            if (GameObject.Find("MenuGame") != null)
                GameObject.Find("MenuGame").GetComponent<Menu>().ButtonLoadScene("Scene 19 - Glasses");

            return;
        }

        GameObject.Find("World/Quests/Quest 2 Симулятор жизни").SetActive(true);
        GameObject.Find("World/Quests/Quest 2 Симулятор жизни/Glitches/GlitchGame 1").SetActive(true);
        GameObject.Find("World/Quests/Quest 2 Симулятор жизни/Glitches/GlitchGame 1/Glitch/GlitchSphere").GetComponent<ObjectInteractive>().active = true;
        GameObject.Find("World/Quests/Quest 2 Симулятор жизни/Glitches/GlitchGame 2").SetActive(true);
        GameObject.Find("World/Quests/Quest 2 Симулятор жизни/Glitches/GlitchGame 2/Glitch/GlitchSphere").GetComponent<ObjectInteractive>().active = true;
        GameObject.Find("World/Quests/Quest 2 Симулятор жизни/Glitches/GlitchGame 3").SetActive(true);
        GameObject.Find("World/Quests/Quest 2 Симулятор жизни/Glitches/GlitchGame 3/Glitch/GlitchSphere").GetComponent<ObjectInteractive>().active = true;
        GameObject.Find("World/Quests/Quest 2 Симулятор жизни/Glitches/GlitchGame 4").SetActive(true);
        GameObject.Find("World/Quests/Quest 2 Симулятор жизни/Glitches/GlitchGame 4/Glitch/GlitchSphere").GetComponent<ObjectInteractive>().active = true;

        //foreach(var x in GameObject.FindObjectsOfType<ObjectDoor>())
        //{
        //    x.Lock(false);
        //}
    }

    private void CreateDanceCarGameButton()
    {
        var tpDance = UIFactory.CreateButton(ContentRoot, "DanceGameButton", "Teleport Dance/Car Game", null);
        UIFactory.SetLayoutElement(tpDance.Component.gameObject, minHeight: 25, minWidth: 200);
        Text nameLabel = tpDance.Component.GetComponentInChildren<Text>();
        nameLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        nameLabel.alignment = TextAnchor.MiddleLeft;
        tpDance.OnClick += tpDanceCarClick;
    }


    void tpDanceCarClick()
    {
        if (SceneManager.GetActiveScene().name != "Scene 7 - Backrooms")
        {
            if(SceneManager.GetActiveScene().name != "SceneMenu")
                SceneManager.LoadScene("Scene 7 - Backrooms", LoadSceneMode.Single);

            if (GameObject.Find("MenuGame") != null)
                GameObject.Find("MenuGame").GetComponent<Menu>().ButtonLoadScene("Scene 7 - Backrooms");

            return;
        }
        GameObject quest4 = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой");
        if (quest4 != null) quest4.SetActive(true);

        GameObject quest4Interactive = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы");
        if (quest4Interactive != null) quest4Interactive.SetActive(true);

        GameObject quest4HouseMain = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/House/Main");
        if (quest4HouseMain != null) quest4HouseMain.SetActive(true);

        GameObject actMitaKepka = GameObject.Find("World/Acts/Mita Кепка");
        if (actMitaKepka != null) actMitaKepka.SetActive(true);

        GameObject interactiveDance = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы/Interactive Dance");
        if (interactiveDance != null)
        {
            ObjectInteractive danceObjectInteractive = interactiveDance.GetComponent<ObjectInteractive>();
            if (danceObjectInteractive != null) danceObjectInteractive.active = true;
        }

        GlobalTag.player.transform.position = new Vector3(-18.63f, 0f, -1.47f);

        GameObject interactiveDoor = GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы/Interactive Door");
        if (interactiveDoor != null)
        {
            ObjectInteractive doorObjectInteractive = interactiveDoor.GetComponent<ObjectInteractive>();
            if (doorObjectInteractive != null)
            {
                doorObjectInteractive.active = true;
                doorObjectInteractive.Click();
            }
        }

        GameObject.Find("World/House/Backrooms First/Game SpaceCar").SetActive(true);
        GameObject.Find("World/House/Backrooms First/Game SpaceCar").transform.position = new Vector3(-0.81f, 0f, -13.59f);
    }

    private void CreateOpenDoorToggle()
    {
        _ = UIFactory.CreateToggle(
            ContentRoot,
            "OpenDoorToggle",
            out Toggle toggle,
            out Text text
        );
        text.text = "OpenDoor";
        toggle.isOn = Plugin.isOpenDoor.Value;
        System.Action<bool> value = (value) =>
        {
            Plugin.isOpenDoor.Value = value;
        };
        toggle.onValueChanged.AddListener(
            value
        );
    }

    private void CreateInteractiveEnableToggle()
    {
        _ = UIFactory.CreateToggle(
            ContentRoot,
            "InteractiveEnableToggle",
            out Toggle toggle,  
            out Text text
        );
        text.text = "InteractiveEnable";
        toggle.isOn = Plugin.isInteractive.Value;
        System.Action<bool> value = (value) =>
        {
            Plugin.isInteractive.Value = value;
        };
        toggle.onValueChanged.AddListener(
            value
        );
    }

    private void CreateMiniGameToggle()
    {
        _ = UIFactory.CreateToggle(
            ContentRoot,
            "MiniGameToggle",
            out Toggle toggle,
            out Text text
        );
        text.text = "TV/Dance Game Infinite";
        toggle.isOn = Plugin.isMiniGame.Value;
        System.Action<bool> value = (value) =>
        {
            Plugin.isMiniGame.Value = value;
        };
        toggle.onValueChanged.AddListener(
            value
        );
    }

    protected override void OnClosePanelClicked()
    {
        Owner.Enabled = !Owner.Enabled;
    }
}