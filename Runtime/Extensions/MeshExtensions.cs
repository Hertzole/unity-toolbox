using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public static class MeshExtensions
	{
		public static int GetSubMeshIndex(this Mesh mesh, int triangleIndex)
		{
			if (!mesh.isReadable)
			{
				Debug.LogError("You need to mark model's mesh as Read/Write Enabled in Import Settings.", mesh);
				return 0;
			}

			int triangleCounter = 0;
			for (int subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; subMeshIndex++)
			{
				int indexCount = mesh.GetSubMesh(subMeshIndex).indexCount;
				triangleCounter += indexCount / 3;
				if (triangleIndex < triangleCounter)
				{
					return subMeshIndex;
				}
			}

			Debug.LogError($"Failed to find triangle with index {triangleIndex} in mesh '{mesh.name}'. Total triangle count: {triangleCounter}", mesh);
			return 0;
		}
	}
}