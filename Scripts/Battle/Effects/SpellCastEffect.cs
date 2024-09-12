using Godot;

namespace FinalDungeon.Battle.Effects;

[GlobalClass]
public partial class SpellCastEffect : Node2D {
	[Export]
	public GpuParticles2D _particles;

	public bool Playing {
		set {
			_particles.Emitting = value;
		}
	}
}