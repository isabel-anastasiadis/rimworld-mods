using UnityEngine;
using Verse;

namespace Izzimon.BloodDries
{
    public class Graphic_Cluster: Verse.Graphic_Cluster
    {

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Cluster>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

	}
}
