namespace Hertzole.UnityToolbox
{
	public interface IDamageable
	{
#if TOOLBOX_PHYSICS_2D || TOOLBOX_PHYSICS_3D
		void TakeDamage(int amount, HitData hitData = default, IDamager damager = null);
#else
		void TakeDamage(int amount, IDamager damager = null);
#endif
	}
}