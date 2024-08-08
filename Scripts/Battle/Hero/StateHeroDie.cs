using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class StateHeroDie : StateBase {
	[Export]
	public ActorHero _actor;

	public override string StateName => "Die";

	public override void OnEnter() {
		_actor.animatedSprite.Play("Die");
	}

	public override bool CanExit() {
		return false;
	}
}