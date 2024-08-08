using Godot;

namespace FinalDungeon.Battle;

public abstract partial class StateBase : Node {
	[Export]
	public StateMachine _stateMachine;

	[Export]
	public bool isDefault;

	public abstract string StateName { get; }

	public override void _Ready() {
		_stateMachine.Add(this);
	}

	public virtual void OnEnter() { }
	public virtual void OnProcess(double deltaTime) { }
	public virtual void OnExit() { }

	public virtual bool CanExit() {
		return true;
	}
}