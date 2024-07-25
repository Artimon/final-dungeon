using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class ElementHeroStatus : Control {
	[Export]
	public TextureProgressBar _hitsProgress;

	[Export]
	public Label _hitsLabel;

	[Export]
	public TextureProgressBar _atbProgress;

	public void SetHits(float hits, float maxHits) {
		_hitsProgress.Value = hits / (double)maxHits;

		var hitsDisplay = Mathf.FloorToInt(hits);
		var hitsText = Mathf.Max(1, hitsDisplay).ToString();

		_hitsLabel.Text = hitsText;
	}

	public void SetAtb(double value) {
		_atbProgress.Value = value;
	}
}