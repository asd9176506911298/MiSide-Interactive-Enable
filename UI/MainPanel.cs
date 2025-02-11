﻿using InteractiveEnable;
using InteractiveEnable.UI;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniverseLib.UI;


public class MainPanel : UniverseLib.UI.Panels.PanelBase
{
    public MainPanel(UIBase owner) : base(owner) { }

    public override string Name => "Interactive Enable Mod v1.0.3";
    public override int MinWidth => 300;
    public override int MinHeight => 500;
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
        CreateTVHintButton();
        CreateDifficultButton();

        CreateDanceCarGameButton();
        CreateChangeMusicButton();
        CreateExitDanceButton();

        CreateGlitchGameButton();

        CreateFPSGameButton();
        CreateInvincibleToggle();
        CreateOHKToggle();

        CreateMemeButton();

        CreateMuteToggle();

        CreateDesotryMemeButton();
        Owner.Enabled = false;
    }

    private void CreateDesotryMemeButton()
    {
        var DestoryMemeButton = UIFactory.CreateButton(ContentRoot, "DestoryMemeButton", "Destory Meme Object", null);
        UIFactory.SetLayoutElement(DestoryMemeButton.Component.gameObject, minHeight: 25, minWidth: 150);
        Text DestortMemeLabel = DestoryMemeButton.Component.GetComponentInChildren<Text>();
        DestortMemeLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        DestortMemeLabel.alignment = TextAnchor.MiddleLeft;
        DestoryMemeButton.OnClick += () => UIManager.DestroyAllMeme();
    }

    private void CreateMuteToggle()
    {
        _ = UIFactory.CreateToggle(
           ContentRoot,
           "MuteToggle",
           out Toggle toggle,
           out Text text
       );
        text.text = "Meme Mute";
        toggle.isOn = Plugin.Instance.isMute.Value;
        System.Action<bool> value = (value) =>
        {
            Plugin.Instance.isMute.Value = value;
        };
        toggle.onValueChanged.AddListener(
            value
        );
    }

    private void CreateMemeButton()
    {
        var memeRow = UIFactory.CreateHorizontalGroup(
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
           memeRow,
           minHeight: 25,
           minWidth: 200,
           flexibleHeight: 0,
           flexibleWidth: 999
       );

        var MaxwellButton = UIFactory.CreateButton(memeRow, "MaxwellButton", "Maxwell", null);
        UIFactory.SetLayoutElement(MaxwellButton.Component.gameObject, minHeight: 25, minWidth: 60);
        Text maxwellLabel = MaxwellButton.Component.GetComponentInChildren<Text>();
        maxwellLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        maxwellLabel.alignment = TextAnchor.MiddleCenter;
        MaxwellButton.OnClick += () => UIManager.CreateInteractiveObject("maxwell", "rotate");

        var FishButton = UIFactory.CreateButton(memeRow, "FishButton", "Fish", null);
        UIFactory.SetLayoutElement(FishButton.Component.gameObject, minHeight: 25, minWidth: 60);
        Text fishLabel = FishButton.Component.GetComponentInChildren<Text>();
        fishLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        fishLabel.alignment = TextAnchor.MiddleCenter;
        FishButton.OnClick += () => UIManager.CreateInteractiveObject("fish");

        var oiiaiButton = UIFactory.CreateButton(memeRow, "oiiaiButton", "oiiai", null);
        UIFactory.SetLayoutElement(oiiaiButton.Component.gameObject, minHeight: 25, minWidth: 60);
        Text oiiaiLabel = oiiaiButton.Component.GetComponentInChildren<Text>();
        oiiaiLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        oiiaiLabel.alignment = TextAnchor.MiddleCenter;
        oiiaiButton.OnClick += () => UIManager.CreateInteractiveObject("oii");

        var futureButton = UIFactory.CreateButton(memeRow, "FutureButton", "Future", null);
        UIFactory.SetLayoutElement(futureButton.Component.gameObject, minHeight: 25, minWidth: 60);
        Text futureLabel = futureButton.Component.GetComponentInChildren<Text>();
        futureLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        futureLabel.alignment = TextAnchor.MiddleCenter;
        futureButton.OnClick += () => UIManager.CreateInteractiveObject("future");

        var cowButton = UIFactory.CreateButton(memeRow, "CowButton", "Cow", null);
        UIFactory.SetLayoutElement(cowButton.Component.gameObject, minHeight: 25, minWidth: 60);
        Text cowLabel = cowButton.Component.GetComponentInChildren<Text>();
        cowLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        cowLabel.alignment = TextAnchor.MiddleCenter;
        cowButton.OnClick += () => UIManager.CreateInteractiveObject("cow");

        var caramelldansenButton = UIFactory.CreateButton(memeRow, "caramelldansenButton", "Caramelldansen", null);
        UIFactory.SetLayoutElement(caramelldansenButton.Component.gameObject, minHeight: 25, minWidth: 100);
        Text caramelldansenLabel = caramelldansenButton.Component.GetComponentInChildren<Text>();
        caramelldansenLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        caramelldansenLabel.alignment = TextAnchor.MiddleCenter;
        caramelldansenButton.OnClick += () => UIManager.CreateInteractiveObject("caramelldansen");

        var bocchiButton = UIFactory.CreateButton(memeRow, "BocchiButton", "bocchi", null);
        UIFactory.SetLayoutElement(bocchiButton.Component.gameObject, minHeight: 25, minWidth: 60);
        Text bocchiLabel = bocchiButton.Component.GetComponentInChildren<Text>();
        bocchiLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        bocchiLabel.alignment = TextAnchor.MiddleCenter;
        bocchiButton.OnClick += () => UIManager.CreateInteractiveObject("bocchi");
    }

    private void CreateTVHintButton()
    {
        var TVHintButton = UIFactory.CreateButton(ContentRoot, "TVHintButton", "TV Game Didn't Hint Key Click Me", null);
        UIFactory.SetLayoutElement(TVHintButton.Component.gameObject, minHeight: 25, minWidth: 300);
        Text nameLabel = TVHintButton.Component.GetComponentInChildren<Text>();
        nameLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        nameLabel.alignment = TextAnchor.MiddleLeft;
        TVHintButton.OnClick += ShowTVHint;
    }

    private void ShowTVHint()
    {
        foreach (var x in GameObject.FindObjectsOfType<MinigamesTelevisionController>())
        {
            x.KeysMenuActiveTrue();
            Plugin.Log.LogInfo($"KeysMenuActiveTrue");
        }
    }

    private void CreateInvincibleToggle()
    {
        _ = UIFactory.CreateToggle(
            ContentRoot,
            "InvincibleToggle",
            out Toggle toggle,
            out Text text
        );
        text.text = "FPS Player Invincible";
        toggle.isOn = Plugin.Instance.isInvincible.Value;
        System.Action<bool> value = (value) =>
        {
            Plugin.Instance.isInvincible.Value = value;
        };
        toggle.onValueChanged.AddListener(
            value
        );
    }

    private void CreateOHKToggle()
    {
         _ = UIFactory.CreateToggle(
            ContentRoot,
            "EnemyOHKToggle",
            out Toggle toggle,
            out Text text
        );
        text.text = "Enemy One Hit Kill";
        toggle.isOn = Plugin.Instance.isOHK.Value;
        System.Action<bool> value = (value) =>
        {
            Plugin.Instance.isOHK.Value = value;
        };
        toggle.onValueChanged.AddListener(
            value
        );
    }

    private void CreateFPSGameButton()
    {
        var FPSGame = UIFactory.CreateButton(ContentRoot, "FPSGameButton", "Teleport FPS Game", null);
        UIFactory.SetLayoutElement(FPSGame.Component.gameObject, minHeight: 25, minWidth: 300);
        Text nameLabel = FPSGame.Component.GetComponentInChildren<Text>();
        nameLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        nameLabel.alignment = TextAnchor.MiddleLeft;
        FPSGame.OnClick += tpFPSGame;
    }

    private void tpFPSGame()
    {
        if (SceneManager.GetActiveScene().name != "MinigameShooter")
        {
            if (SceneManager.GetActiveScene().name != "SceneMenu")
                SceneManager.LoadScene("MinigameShooter", LoadSceneMode.Single);

            if (GameObject.Find("MenuGame") != null)
                GameObject.Find("MenuGame").GetComponent<Menu>().ButtonLoadScene("MinigameShooter");

            return;
        }

        foreach (var obj in Resources.FindObjectsOfTypeAll(Il2CppSystem.Type.GetType("Shooter_Main")))
        {
            Plugin.Log.LogInfo($"Object: {obj}, Type: {obj.GetType()}");

            var shooterMain = obj.TryCast<Shooter_Main>();
            if (shooterMain != null)
            {
                // Access the associated GameObject and activate it
                shooterMain.gameObject.SetActive(true);
                Plugin.Log.LogInfo($"Activated GameObject: {shooterMain.gameObject.name}");
                continue;
            }

            var gameObject = obj.TryCast<UnityEngine.GameObject>();
            if (gameObject != null)
            {
                gameObject.SetActive(true);
                Plugin.Log.LogInfo($"Activated GameObject: {gameObject.name}");
                continue;
            }

            // Log unexpected object types
            Plugin.Log.LogInfo($"Unexpected object type: {obj.GetType()}");
        }



    }

    private void CreateExitDanceButton()
    {
        var exitDance = UIFactory.CreateButton(ContentRoot, "DanceGameExitButton", "Exit Dance Game", null);
        UIFactory.SetLayoutElement(exitDance.Component.gameObject, minHeight: 25, minWidth: 300);
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

        difficultText = UIFactory.CreateLabel(difficultRow, "Difficult", "Fight Difficult:");
        UIFactory.SetLayoutElement(difficultText.gameObject, minHeight: 25, minWidth: 100);
    }

    private void FightEasy()
    {
        foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
        {
            x.enemyComplexity += 1;
            difficultText.text = "Fight Difficult: " + x.enemyComplexity.ToString();
            Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
        }
    }

    private void FightHard()
    {
        foreach (var x in GameObject.FindObjectsOfType<Location4Fight>())
        {
            if (x.enemyComplexity > 0)
                x.enemyComplexity -= 1;
            difficultText.text = "Fight Difficult: " + x.enemyComplexity.ToString();
            Plugin.Log.LogInfo($"enemyComplexity: {x.enemyComplexity}");
        }
    }

    private void CreateTVGameButton()
    {
        var tpDance = UIFactory.CreateButton(ContentRoot, "DanceGameButton", "Teleport TV Game", null);
        UIFactory.SetLayoutElement(tpDance.Component.gameObject, minHeight: 25, minWidth: 300);
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
        UIFactory.SetLayoutElement(tpGlitch.Component.gameObject, minHeight: 25, minWidth: 300);
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
        var tpDance = UIFactory.CreateButton(ContentRoot, "DanceGameButton", "Teleport Dance/Car/Hammer Game", null);
        UIFactory.SetLayoutElement(tpDance.Component.gameObject, minHeight: 25, minWidth: 300);
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

        //GameObject.Find("World/House/Backrooms First/Game SpaceCar").SetActive(true);
        //GameObject.Find("World/House/Backrooms First/Game SpaceCar").transform.position = new Vector3(-0.81f, 0f, -13.59f);

        var carGame = UnityEngine.GameObject.Instantiate(GameObject.Find("World/House/Backrooms First/Game SpaceCar"));
        carGame.SetActive(true);
        carGame.transform.position = new Vector3(-0.81f, 0f, -13.59f);
        carGame.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        GameObject.Find("World/Quests/Quest4 - Проводим время с Кепкой/Интерактивы/Interactive ButtonHummer").GetComponent<ObjectInteractive>().active = true;
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
        toggle.isOn = Plugin.Instance.isOpenDoor.Value;
        System.Action<bool> value = (value) =>
        {
            Plugin.Instance.isOpenDoor.Value = value;
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
        toggle.isOn = Plugin.Instance.isInteractive.Value;
        System.Action<bool> value = (value) =>
        {
            Plugin.Instance.isInteractive.Value = value;
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
        toggle.isOn = Plugin.Instance.isMiniGame.Value;
        System.Action<bool> value = (value) =>
        {
            Plugin.Instance.isMiniGame.Value = value;
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