namespace Hertzole.UnityToolbox
{
	public interface IHealable
	{
#if TOOLBOX_PHYSICS_2D || TOOLBOX_PHYSICS_3D
		void Heal(int amount, HitData hitData = default, IDamager healer = null);
#else
		void Heal(int amount, IDamager healer = null);
#endif
	}
}