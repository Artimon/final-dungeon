using Godot;

namespace FinalDungeon.Battle.Effects;

public partial class EffectDamageNumber : Node2D {
	[Export]
	public PixelPerfectNumber _pixelPerfectNumber;

	[Export]
	public AnimationPlayer _animationPlayer;

	public override void _Ready() {
		_animationPlayer.Play("FlyOff");
		_animationPlayer.AnimationFinished += _OnAnimationFinished;
	}

	public void SetNumber(int damage) {
		_pixelPerfectNumber.SetNumber(damage);
	}

	public void _OnAnimationFinished(StringName _) {
		this.Remove();
	}
}