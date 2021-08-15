using RimWorld;
using UnityEngine;
using Verse;

namespace Izzimon.BloodDries
{
    public class Graphic_Cluster: Graphic_Collection
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001848 RID: 6216 RVA: 0x00091481 File Offset: 0x0008F681
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0009149D File Offset: 0x0008F69D
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Log.ErrorOnce("Graphic_Scatter cannot draw realtime.", 9432243);
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x000914B0 File Offset: 0x0008F6B0
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector3 a = thing.TrueCenter();
			Rand.PushState();
			Rand.Seed = thing.Position.GetHashCode();
			Filth filth = thing as Filth;
			int num;
			if (filth == null)
			{
				num = 3;
			}
			else
			{
				num = filth.thickness;
			}
			for (int i = 0; i < num; i++)
			{
				Material matSingle = this.MatSingle;
				Vector3 center = a + new Vector3(Rand.Range(-0.45f, 0.45f), 0f, Rand.Range(-0.45f, 0.45f));
				Vector2 size = new Vector2(Rand.Range(this.data.drawSize.x * 0.8f, this.data.drawSize.x * 1.2f), Rand.Range(this.data.drawSize.y * 0.8f, this.data.drawSize.y * 1.2f));
				float rot = (float)Rand.RangeInclusive(0, 360) + extraRotation;
				bool flipUv = Rand.Value < 0.5f;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(matSingle, thing.def.category.ToAtlasGroup(), flipUv, true, out matSingle, out uvs, out color);
				Printer_Plane.PrintPlane(layer, center, size, matSingle, rot, flipUv, uvs, new Color32[]
				{
					color,
					color,
					color,
					color
				}, 0.01f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x00091638 File Offset: 0x0008F838
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Scatter(subGraphic[0]=",
				this.subGraphics[0].ToString(),
				", count=",
				this.subGraphics.Length,
				")"
			});
		}

		// Token: 0x040010AB RID: 4267
		private const float PositionVariance = 0.45f;

		// Token: 0x040010AC RID: 4268
		private const float SizeVariance = 0.2f;

		// Token: 0x040010AD RID: 4269
		private const float SizeFactorMin = 0.8f;

		// Token: 0x040010AE RID: 4270
		private const float SizeFactorMax = 1.2f;
	

		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.Message(newColor.ToString());


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

			Log.Message(coloredVersion.subGraphics[0].color.ToString());

			return coloredVersion;
		}

	}
}
