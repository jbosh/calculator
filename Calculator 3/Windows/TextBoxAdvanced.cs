using System.Windows.Forms;

namespace Calculator
{
	public class TextBoxAdvanced : TextBox
	{
		public TextBoxAdvanced()
		{
			CaretStart = SelectionStart;
			KeyUp += TextBoxAdvanced_KeyUp;
			KeyDown += TextBoxAdvanced_KeyDown;
			MouseDown += TextBoxAdvanced_MouseDown;
		}
		/// <summary>
		/// True if the cursor is on the left side of the selection.
		/// </summary>
		public int CaretStart { get; set; }
		private void TextBoxAdvanced_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Clicks < 2)
				CaretStart = SelectionStart;
			if (e.Clicks >= 2 || (ModifierKeys & Keys.Control) == Keys.Control)
			{
				SelectionStart = FindPreviousWord(CaretStart, Text);
				SelectionLength = FindNextWord(SelectionStart, Text) - SelectionStart;
				return;
			}
		}
		private void TextBoxAdvanced_KeyUp(object sender, KeyEventArgs e)
		{
			if (SelectionLength == 0)
				CaretStart = SelectionStart;
			Text = Text.Replace(new string((char) 127, 1), "");
			if (!e.Control)
				return;
			/*switch(e.KeyCode)
			{
				case Keys.C:
					Clipboard.SetText(SelectedText);
					e.Handled = true;
					break;
				case Keys.V:
					if (Clipboard.ContainsText())
					{
						int start = SelectionStart;
						Text = Text.Remove(start, SelectionLength);
						string clipboard = Clipboard.GetText();
						Text = Text.Insert(start, clipboard);
						SelectionStart = start + clipboard.Length;
						SelectionLength = 0;
						e.Handled = true;
					}
					break;
			}*/
		}
		private void TextBoxAdvanced_KeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Control)
				return;
			switch (e.KeyCode)
			{
				case Keys.Left:
					e.Handled = true;
					if (e.Shift)
					{
						if (CaretStart < SelectionStart + SelectionLength)
						{
							int index = FindPreviousWord(SelectionStart + SelectionLength, Text);
							if (index < CaretStart)
								index = CaretStart;
							index -= SelectionStart + SelectionLength;
							SelectionLength += index;
						}
						else
						{
							int index = FindPreviousWord(SelectionStart, Text);
							index -= SelectionStart;
							SelectionStart += index;
							SelectionLength -= index;
						}
					}
					else
					{
						int index = FindPreviousWord(SelectionStart, Text);
						SelectionStart = index;
					}
					break;
				case Keys.Right:
					e.Handled = true;
					if (e.Shift)
					{
						if (SelectionStart < CaretStart)
						{
							int index = FindNextWord(SelectionStart, Text);
							if (CaretStart < index)
								index = CaretStart;
							index -= SelectionStart;
							SelectionStart += index;
							SelectionLength -= index;
						}
						else
						{
							int index = FindNextWord(SelectionStart + SelectionLength, Text);
							index -= SelectionStart + SelectionLength;
							SelectionLength += index;
						}
					}
					else
					{
						int index = FindNextWord(SelectionStart, Text);
						SelectionStart = index;
					}
					break;
				case Keys.Back:
					e.Handled = true;
					if (SelectionLength > 0)
					{
						int start = SelectionStart;
						Text = Text.Remove(start, SelectionLength);
						SelectionStart = start;
					}
					else
					{
						int start = SelectionStart;
						int index = FindPreviousWord(start, Text);
						Text = Text.Remove(index, start - index);
						SelectionStart = index;
					}
					break;
				case Keys.Delete:
					e.Handled = true;
					if (SelectionLength > 0)
					{
						int start = SelectionStart;
						Text = Text.Remove(start, SelectionLength);
						SelectionStart = start;
					}
					else
					{
						int start = SelectionStart;
						int index = FindNextWord(start, Text);
						string cut = Text.Substring(start, index - start);
						Text = Text.Remove(start, index - start);
						SelectionStart = start;
						if (e.Shift)
							Clipboard.SetText(cut);
					}
					break;
			}
		}
		private static int FindPreviousWord(int index, string sentence)
		{
			//Int = 0, Letter = 1
			int type = -1;
			for (index--; index >= 0; index--)
			{
				//Letter
				if (sentence[index] >= '0' && sentence[index] <= '9' || sentence[index] == '.')
				{
					if (type == -1)
						type = 0;
					if (type != 0)
						return index + 1;
				}
				else if (sentence[index] >= 'A' && sentence[index] <= 'z')
				{
					if (type == -1)
						type = 1;
					if (type != 1)
						return index + 1;
				}
				else
				{
					if (type == -1)
						return index;
					return index + 1;
				}
			}
			return 0;
		}
		private static int FindNextWord(int index, string sentence)
		{
			//Int = 0, Letter = 1
			int type = -1;
			for (; index < sentence.Length; index++)
			{
				//Letter
				if (sentence[index] >= '0' && sentence[index] <= '9' || sentence[index] == '.')
				{
					if (type == -1)
						type = 0;
					if (type != 0)
						return index;
				}
				else if (sentence[index] >= 'A' && sentence[index] <= 'z')
				{
					if (type == -1)
						type = 1;
					if (type != 1)
						return index;
				}
				else
				{
					if (type == -1)
						return index + 1;
					return index;
				}
			}
			return index;
		}
	}
}