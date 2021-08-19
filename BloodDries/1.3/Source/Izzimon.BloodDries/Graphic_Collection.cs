using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Izzimon.BloodDries
{
	public abstract class Graphic_Collection : Graphic
	{
		// Token: 0x0600184D RID: 6221 RVA: 0x00091690 File Offset: 0x0008F890
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			Graphic[] array = this.subGraphics;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].TryInsertIntoAtlas(groupKey);
			}
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x000916BC File Offset: 0x0008F8BC
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			if (req.path.NullOrEmpty())
			{
				throw new ArgumentNullException("folderPath");
			}
			if (req.shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			List<Texture2D> list = (from x in ContentFinder<Texture2D>.GetAllInFolder(req.path)
									where !x.name.EndsWith(Graphic_Single.MaskSuffix)
									orderby x.name
									select x).ToList<Texture2D>();
			if (list.NullOrEmpty<Texture2D>())
			{
				Log.Error("Collection cannot init: No textures found at path " + req.path);
				this.subGraphics = new Graphic[]
				{
					BaseContent.BadGraphic
				};
				return;
			}
			this.subGraphics = new Graphic[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				string path = req.path + "/" + list[i].name;
				this.subGraphics[i] = GraphicDatabase.Get(typeof(Graphic_Single), path, req.shader, this.drawSize, this.color, this.colorTwo, this.data, req.shaderParameters, null);
			}
		}

		// Token: 0x040010AF RID: 4271
		protected Graphic[] subGraphics;
	}
}
