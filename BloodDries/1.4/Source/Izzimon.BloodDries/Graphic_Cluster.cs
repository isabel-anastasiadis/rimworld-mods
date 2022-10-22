
using UnityEngine;
using Verse;

namespace Izzimon.BloodDries
{
    public class Graphic_Cluster: Verse.Graphic_Cluster
	{

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Logger.Debug($"GetColoredVersion called with color {newColor}");

			var coloredVersion = new Graphic_Cluster();
			var graphicRequest = new GraphicRequest()
			{
				path = this.path,
				maskPath = this.maskPath,
				color = newColor,
				colorTwo = newColorTwo,
				drawSize = this.drawSize,
				graphicData = this.data,
				shader = newShader
			};

			coloredVersion.Init(graphicRequest);

			return coloredVersion;
		}

	}
}
