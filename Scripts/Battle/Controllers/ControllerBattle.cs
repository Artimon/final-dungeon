using FinalDungeon.Battle.UserInterface;
using Godot;

namespace FinalDungeon.Battle;

[GlobalClass]
public partial class ControllerBattle : Node {
	public static ControllerBattle instance;

	[Export]
	public ComponentActionMenu _actionMenu;

	public override void _EnterTree() {
		instance = this;
	}

	public bool TryBattleEnd() {
		if (!ControllerActors.instance.HasHeroes) {
			Defeat();
			return true;
		}

		if (!ControllerActors.instance.HasEnemies) {
			Victory();
			return true;
		}

		return false;
	}

	public void Victory() {
		GD.Print("Victory!");

		_actionMenu.Shutdown();
	}

	public void Defeat() {
		GD.Print("Defeat!");

		_actionMenu.Shutdown();
	}
}