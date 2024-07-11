using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class ElementHeroStatus : Control {
	[Export]
	public TextureProgressBar _hitsProgress;

	[Export]
	public TextureProgressBar _atbProgress;

	public void SetHits(int hits, int maxHits) {
		_hitsProgress.Value = hits / (double)maxHits;
	}

	public void SetAtb(double value) {
		_atbProgress.Value = value;
	}
}