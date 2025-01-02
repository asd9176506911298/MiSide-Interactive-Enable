using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

public class MainPanel : UniverseLib.UI.Panels.PanelBase
{
    public MainPanel(UIBase owner) : base(owner) { }

    public override string Name => "My Panel";
    public override int MinWidth => 100;
    public override int MinHeight => 200;
    public override Vector2 DefaultAnchorMin => new(0.25f, 0.25f);
    public override Vector2 DefaultAnchorMax => new(0.75f, 0.75f);
    public override bool CanDragAndResize => true;

    protected override void ConstructPanelContent()
    {
        Text myText = UIFactory.CreateLabel(ContentRoot, "myText", "Hello world");
        UIFactory.SetLayoutElement(myText.gameObject, minWidth: 200, minHeight: 25);

        Owner.Enabled = false;
    }


    protected override void OnClosePanelClicked()
    {
        Owner.Enabled = !Owner.Enabled;
    }
}