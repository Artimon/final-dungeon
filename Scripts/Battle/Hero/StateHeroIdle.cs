using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class StateHeroIdle : StateBase {
	[Export]
	public AnimatedSprite2D _animatedSprite;

	public override string StateName => "Idle";

	public override void OnEnter() {
		_animatedSprite.Play("default");
	}
}