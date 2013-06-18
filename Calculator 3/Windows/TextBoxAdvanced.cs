using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

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
			TextChanged += TextBoxAdvanced_TextChanged;

			undoStack = new List<UndoData>();
			undoStack.Add(new UndoData(0, ""));
			undoStackIndex = undoStack.Count;
		}

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
			/*if (!e.Control)
				return;
			switch(e.KeyCode)
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
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Control | Keys.Back:
					SendKeys.SendWait("^+{LEFT}{BACKSPACE}");
					return true;
				case Keys.Control | Keys.Delete:
					SendKeys.SendWait("^+{RIGHT}{BACKSPACE}");
					return true;
				case Keys.Control | Keys.Z:
					if (undoStackIndex > 1)
					{
						undoStackIndex--;
						var data = undoStack[undoStackIndex - 1];
						Text = data.Value;
						SelectionStart = data.CaretStart;
					}
					return true;
				case Keys.Control | Keys.Y:
					if (undoStackIndex < undoStack.Count)
					{
						undoStackIndex++;
						var data = undoStack[undoStackIndex - 1];
						Text = data.Value;
						SelectionStart = data.CaretStart;
					}
					return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case 0x302: //WM_PASTE
					if (!Clipboard.ContainsText())
						break;
					if(Program.CopyPasteHelper)
						Clipboard.SetText(CopyHelpers.Process(Clipboard.GetText()));
					goto default;
				case 199: //EM_UNDO
					//intercept so that we can do it
					break;
				default:
					base.WndProc(ref m);
					break;
			}
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
							var index = FindPreviousWord(SelectionStart + SelectionLength, Text);
							if (index < CaretStart)
								index = CaretStart;
							index -= SelectionStart + SelectionLength;
							SelectionLength += index;
						}
						else
						{
							var index = FindPreviousWord(SelectionStart, Text);
							index -= SelectionStart;
							SelectionStart += index;
							SelectionLength -= index;
						}
					}
					else
					{
						var index = FindPreviousWord(SelectionStart, Text);
						SelectionStart = index;
					}
					break;
				case Keys.Right:
					e.Handled = true;
					if (e.Shift)
					{
						if (SelectionStart < CaretStart)
						{
							var index = FindNextWord(SelectionStart, Text);
							if (CaretStart < index)
								index = CaretStart;
							index -= SelectionStart;
							SelectionStart += index;
							SelectionLength -= index;
						}
						else
						{
							var index = FindNextWord(SelectionStart + SelectionLength, Text);
							index -= SelectionStart + SelectionLength;
							SelectionLength += index;
						}
					}
					else
					{
						var index = FindNextWord(SelectionStart, Text);
						SelectionStart = index;
					}
					break;
			}
		}

		enum WordType
		{
			Invalid,
			Numbers,
			Letters,
		}

		private static int FindPreviousWord(int index, string sentence)
		{
			//Int = 0, Letter = 1
			var type = WordType.Invalid;
			for (index--; index >= 0; index--)
			{
				//Letter
				if (sentence[index] >= '0' && sentence[index] <= '9' || sentence[index] == '.')
				{
					if (type == WordType.Invalid)
						type = WordType.Numbers;
					if (type != WordType.Numbers)
						return index + 1;
				}
				else if (sentence[index] >= 'A' && sentence[index] <= 'z')
				{
					if (type == WordType.Invalid)
						type = WordType.Letters;
					if (type != WordType.Letters)
						return index + 1;
				}
				else
				{
					if (type == WordType.Invalid)
						return index;
					return index + 1;
				}
			}
			return 0;
		}

		private static int FindNextWord(int index, string sentence)
		{
			//Int = 0, Letter = 1
			var type = WordType.Invalid;
			for (; index < sentence.Length; index++)
			{
				//Letter
				if (sentence[index] >= '0' && sentence[index] <= '9' || sentence[index] == '.')
				{
					if (type == WordType.Invalid)
						type = WordType.Numbers;
					if (type != WordType.Numbers)
						return index;
				}
				else if (sentence[index] >= 'A' && sentence[index] <= 'z')
				{
					if (type == WordType.Invalid)
						type = WordType.Letters;
					if (type != WordType.Letters)
						return index;
				}
				else
				{
					if (type == WordType.Invalid)
						return index + 1;
					return index;
				}
			}
			return index;
		}

		struct UndoData
		{
			public int CaretStart;
			public string Value;
			public UndoData(int caret, string value)
			{
				CaretStart = caret;
				Value = value;
			}
			public override string ToString()
			{
				return string.Format("{0}: {1}", CaretStart, Value);
			}
		}
		private const int MaxUndoRedoSteps = 128;
		private int undoStackIndex;
		private List<UndoData> undoStack;
		void TextBoxAdvanced_TextChanged(object sender, EventArgs e)
		{
			if (undoStackIndex <= undoStack.Count)
			{
				if (undoStack[undoStackIndex - 1].Value == Text) //we're un-doing or re-doing
					return;
				undoStack.RemoveRange(undoStackIndex, undoStack.Count - undoStackIndex);
			}
			if (undoStack.Count > MaxUndoRedoSteps)
				undoStack.RemoveAt(0);
			undoStack.Add(new UndoData(SelectionStart, Text));
			undoStackIndex = undoStack.Count;
		}
	}
}