using Godot;

namespace FinalDungeon.Battle;

public interface IAnimation {
	public void OnEnter();

	void OnProcess(double delta);

	void OnAnimationFinished(StringName name);
}