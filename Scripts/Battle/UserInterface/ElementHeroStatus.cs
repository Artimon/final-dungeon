using FinalDungeon.Battle.UserInterface;
using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class ElementHeroStatus : Control {
	[Export]
	public Label _nameLabel;

	[Export]
	public TextureProgressBar _hitsProgress;

	[Export]
	public Label _hitsLabel;

	[Export]
	public TextureProgressBar _atbProgress;

	[Export]
	public ComponentMana mana;

	public void ShowName(string name) {
		_nameLabel.Text = name;
	}

	public void SetHits(float hits, float maxHits) {
		_hitsProgress.Value = hits / (double)maxHits;

		var hitsDisplay = Mathf.CeilToInt(hits);
		var hitsText = Mathf.Max(0, hitsDisplay).ToString();

		_hitsLabel.Text = hitsText;
	}

	public void SetAtb(double value) {
		_atbProgress.Value = value;
	}
}