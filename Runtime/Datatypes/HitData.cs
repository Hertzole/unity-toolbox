#if TOOLBOX_PHYSICS_2D || TOOLBOX_PHYSICS_3D
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public readonly struct HitData
	{
#if TOOLBOX_PHYSICS_3D
		public readonly RaycastHit? hit3D;

		public HitData(RaycastHit hit)
		{
			hit3D = hit;
			hit2D = null;
		}
#endif
#if TOOLBOX_PHYSICS_2D
		public readonly RaycastHit2D? hit2D;

		public HitData(RaycastHit2D hit)
		{
			hit3D = null;
			hit2D = hit;
		}
#endif
	}
}
#endif