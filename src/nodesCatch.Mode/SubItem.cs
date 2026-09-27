using System;

namespace nodesCatch.Mode;

[Serializable]
public class SubItem
{
	public string id { get; set; }

	public string remarks { get; set; }

	public string url { get; set; }

	public bool enabled { get; set; } = true;
}
