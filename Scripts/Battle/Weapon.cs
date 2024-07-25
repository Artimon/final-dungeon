using Godot;

namespace FinalDungeon.Battle;

public partial class Weapon : Node2D {
	[Export]
	public AnimatedSprite2D _effectSprite;

	[Export]
	public AudioStreamPlayer _hitAudio;

	public override void _EnterTree() {
		Visible = false;

		_effectSprite.AnimationFinished += () => {
			Visible = false;
		};
	}

	public void Play() {
		Visible = true;
		_effectSprite.Play("default");
		_hitAudio.Play();
	}
}