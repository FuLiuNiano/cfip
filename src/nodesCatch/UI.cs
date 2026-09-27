using System.Windows.Forms;

namespace nodesCatch;

internal class UI
{
	public static void Show(string msg)
	{
		MessageBox.Show(msg, "CFip 节点助手", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
	}

	public static void ShowWarning(string msg)
	{
		MessageBox.Show(msg, "CFip 节点助手", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
	}

	public static void ShowError(string msg)
	{
		MessageBox.Show(msg, "CFip 节点助手", MessageBoxButtons.OK, MessageBoxIcon.Hand);
	}

	public static DialogResult ShowYesNo(string msg)
	{
		return MessageBox.Show(msg, "CFip 节点助手", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
	}
}
