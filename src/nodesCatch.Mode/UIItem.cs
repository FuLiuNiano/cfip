using System;
using System.Collections.Generic;
using System.Drawing;

namespace nodesCatch.Mode;

[Serializable]
public class UIItem
{
	public Size mainSize { get; set; }

	public Dictionary<string, int> mainLvColWidth { get; set; }
}
