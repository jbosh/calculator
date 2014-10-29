using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Drawing;

namespace Calculator
{
	public class TextBoxAdvanced : TextBox
	{
		public bool InterceptNextPaste { get; set; }
		public int CaretStart { get; set; }

		private Match CtrlClickMatch;
		private Tuple<int, int> CtrlClickLastSelection;

		public TextBoxAdvanced()
		{
			CaretStart = SelectionStart;
			KeyUp += TextBoxAdvanced_KeyUp;
			KeyDown += TextBoxAdvanced_KeyDown;
			MouseDown += TextBoxAdvanced_MouseDown;
			MouseMove += TextBoxAdvanced_MouseMove;
			TextChanged += TextBoxAdvanced_TextChanged;
			AllowDrop = true;
			DragOver += TextBoxAdvanced_DragOver;
			DragDrop += TextBoxAdvanced_DragDrop;

			undoStack = new List<UndoData>();
			undoStack.Add(new UndoData(0, ""));
			undoStackIndex = undoStack.Count;
		}

		private void TextBoxAdvanced_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Clicks < 2)
				CaretStart = SelectionStart;
			if (e.Clicks >= 2 || (ModifierKeys & Keys.Control) == Keys.Control)
			{
				var node = FindToken(CaretStart, Text);
				SelectionStart = node.Value.Index;
				SelectionLength = node.Value.Length;

				CtrlClickMatch = node.Value;
				CtrlClickLastSelection = new Tuple<int,int>(SelectionStart, SelectionLength);
				return;
			}
		}
		
		void TextBoxAdvanced_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (CtrlClickMatch != null && (ModifierKeys & Keys.Control) == Keys.Control)
				{
					var newSelection = new Tuple<int, int>(SelectionStart, SelectionLength);
					int index;
					if (newSelection.Item1 != CtrlClickLastSelection.Item1) //moving selection start
					{
						index = newSelection.Item1;
					}
					else //changing length only
					{
						index = newSelection.Item1 + newSelection.Item2;
					}

					var node = FindToken(index, Text);
					var match = node.Value;
					var start = Math.Min(CtrlClickMatch.Index, match.Index);
					var end = Math.Max(CtrlClickMatch.Index + CtrlClickMatch.Length, match.Index + match.Length);
					SelectionStart = start;
					SelectionLength = end - start;

					CtrlClickLastSelection = newSelection;
					return;
				}
			}
		}

		private void TextBoxAdvanced_KeyUp(object sender, KeyEventArgs e)
		{
			if (SelectionLength == 0)
				CaretStart = SelectionStart;
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
				case Keys.Control | Keys.A:
					SelectAll();
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
				case Keys.Control | Keys.OemCloseBrackets:
					FindMatchingBrace(false);
					return true;
				case Keys.Shift | Keys.Control | Keys.OemCloseBrackets:
					FindMatchingBrace(true);
					return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case 0x302: //WM_PASTE
					{
						if (!Clipboard.ContainsText())
							break;
						if (InterceptNextPaste)
						{
							InterceptNextPaste = false;
							break;
						}

						var restoreClipboard = Clipboard.GetText();
						if (Program.CopyPasteHelper)
						{
							var text = CopyHelpers.Process(Clipboard.GetText());
							if (string.IsNullOrEmpty(text)) //not gonna paste nothing
								break;

							Clipboard.SetText(text);
						}

						//Actually pump in the clipboard text because I don't want to implement paste
						base.WndProc(ref m);

						Clipboard.SetText(restoreClipboard);
					}
					break;
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
					{
						e.Handled = true;

						if (e.Shift)
						{
							if (CaretStart < SelectionStart + SelectionLength) //shrink
							{
								var node = FindToken(SelectionStart + SelectionLength, Text);
								var index = 0;
								if (node.Value.Index != SelectionStart + SelectionLength) //you're in the word
									index = node.Value.Index;
								else if (node.Previous != null) //move to previous word
									index = node.Previous.Value.Index;
								if (index < CaretStart)
									index = CaretStart;
								index -= SelectionStart + SelectionLength;
								SelectionLength += index;
							}
							else //grow
							{
								var node = FindToken(SelectionStart, Text);
								var index = 0;
								if (node.Value.Index != SelectionStart) //you're in the word
									index = node.Value.Index;
								else if (node.Previous != null) //move to previous word
									index = node.Previous.Value.Index;
								index -= SelectionStart;
								SelectionStart += index;
								SelectionLength -= index;
							}
						}
						else
						{
							var node = FindToken(SelectionStart, Text);
							var index = 0;
							if (node.Value.Index != SelectionStart) //you're in the word
								index = node.Value.Index;
							else if (node.Previous != null) //move to previous word
								index = node.Previous.Value.Index;
							SelectionStart = index;
						}
					}
					break;
				case Keys.Right:
					{
						e.Handled = true;
						if (e.Shift)
						{
							if (SelectionStart < CaretStart) //shrink
							{
								var node = FindToken(SelectionStart, Text);
								var index = Text.Length;
								if (node.Value.Index != SelectionStart) //you're in the word
									index = SelectionStart + SelectionLength;
								else if (node.Next != null) //move to next word
									index = node.Next.Value.Index;

								if (CaretStart < index)
									index = CaretStart;
								index -= SelectionStart;
								SelectionStart += index;
								if(SelectionStart + SelectionLength < Text.Length)
									SelectionLength -= index;
							}
							else //grow
							{
								var node = FindToken(SelectionStart + SelectionLength, Text);
								var index = Text.Length;
								if (node.Value.Index + node.Value.Length != SelectionStart + SelectionLength) //you're in the word
									index = node.Value.Index + node.Value.Length;
								else if (node.Next != null) //move to next word
									index = node.Next.Value.Index;

								index -= SelectionStart + SelectionLength;
								SelectionLength += index;
							}
						}
						else
						{
							var node = FindToken(SelectionStart, Text);
							var index = Text.Length;
							if (node.Value.Index + node.Value.Length != SelectionStart) //you're in the word
								index = node.Value.Index + node.Value.Length;
							else if (node.Next != null) //move to next word
								index = node.Next.Value.Index;

							SelectionStart = index;
						}
					}
					break;
			}
		}

		char[] ValidBraceMatchingCharacters = new[] { '[', ']', '(', ')', '{', '}' };
		private void FindMatchingBrace(bool shift)
		{
			var charLeft = '\0';
			var charRight = '\0';
			var caretLeft = SelectionStart - 1;
			var caretRight = SelectionStart;
			if (caretLeft >= 0 && Text.Length >= 1)
				charLeft = Text[caretLeft];
			if (caretRight < Text.Length)
				charRight = Text[caretRight];

			var c = '\0';
			var caret = 0;
			if (ValidBraceMatchingCharacters.Contains(charLeft))
			{
				c = charLeft;
				caret = caretLeft;
				var success = FindMatchingBrace(c, caret, shift);
				if (success)
					return;
			}
			if (ValidBraceMatchingCharacters.Contains(charRight))
			{
				c = charRight;
				caret = caretRight;
				FindMatchingBrace(c, caret, shift);
			}
		}
		private bool FindMatchingBrace(char c, int caret, bool shift)
		{
			if (c == 0)
				return false;

			var idx = Array.IndexOf(ValidBraceMatchingCharacters, c);
			var pushChar = ValidBraceMatchingCharacters[idx & ~1];
			var popChar = ValidBraceMatchingCharacters[idx | 1];

			var leftBrace = -1;
			var rightBrace = -1;
			var searchIndex = -1;
			var newCaretIdx = -1;
			var stack = new Stack<int>();
			for (var i = 0; i < Text.Length; i++)
			{
				if (Text[i] == pushChar)
				{
					if (i == caret)
					{
						searchIndex = stack.Count;
						leftBrace = i;
					}
					stack.Push(i);
				}
				else if (Text[i] == popChar)
				{
					if (i == caret)
					{
						if (stack.Count == 0)
							return false; //no matching brace
						newCaretIdx = stack.Pop();
						leftBrace = newCaretIdx;
						rightBrace = i;
						break;
					}
					if (stack.Count == 0)
						return false; //invalid set of characters
					stack.Pop();
					if (searchIndex == stack.Count)
					{
						newCaretIdx = i + 1;
						rightBrace = newCaretIdx;
						break;
					}
				}
			}

			if (newCaretIdx != -1)
			{
				if (shift)
				{
					SelectionStart = leftBrace;
					SelectionLength = rightBrace - leftBrace + 1;
				}
				else
				{
					SelectionLength = 0;
					SelectionStart = newCaretIdx;
				}
				return true;
			}
			return false;
		}

		//Order matters, first one wins
		private static Regex[] WordRegs = new[]
		{
			new Regex(@"0x[\d\.a-fA-F]+", RegexOptions.Compiled), //hex
			new Regex(@"0b[10]+", RegexOptions.Compiled), //binary
			new Regex(@"[\d\.]+[Ee][-\d\.]+", RegexOptions.Compiled), //scientific
			new Regex(@"[\d]*\.?[\d]+", RegexOptions.Compiled), //number
			new Regex(@"[\w][\w\d]+", RegexOptions.Compiled), //ids
			new Regex(@" +", RegexOptions.Compiled), //spaces
			new Regex(@".", RegexOptions.Compiled), //anything else
		};

		private static LinkedListNode<Match> FindToken(int index, string sentence)
		{
			var list = new LinkedList<Match>();
			var word = default(LinkedListNode<Match>);
			for (var i = 0; i < sentence.Length;)
			{
				foreach (var reg in WordRegs)
				{
					var match = reg.Match(sentence, i);
					if (match.Success && match.Index == i)
					{
						var node = list.AddLast(match);
						if (match.ContainsIndex(index))
							word = node;
						i += match.Length;
						break;
					}
				}
			}
			if (word == null)
			{
				if (index == 0)
					word = list.First;
				if (index == sentence.Length)
					word = list.Last;
				if(word == null)
					throw new Exception();
			}
			return word;
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

		int SetCaractPositionFromPoint(int x, int y)
		{
			var pt = PointToClient(new System.Drawing.Point(x, y));
			var idx = GetCharIndexFromPosition(pt);
			
			if (idx == Text.Length - 1)
			{
				var caretPoint = GetPositionFromCharIndex(idx);
				if (pt.X > caretPoint.X)
					idx += 1;
			}

			SelectionStart = idx;
			SelectionLength = 0;
			if (!Focused)
				Focus();
			return idx;
		}

		void TextBoxAdvanced_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text))
			{
				SetCaractPositionFromPoint(e.X, e.Y);
				e.Effect = DragDropEffects.Copy;
			}
			else if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (filenames.Length == 1)
				{
					e.Effect = DragDropEffects.Copy;
				}
			}
		}

		void TextBoxAdvanced_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.Text))
			{
				var str = e.Data.GetData(DataFormats.Text).ToString();
				var idx = SetCaractPositionFromPoint(e.X, e.Y);
				if (Program.CopyPasteHelper)
					str = CopyHelpers.Process(str);
				Text = Text.Insert(idx, str);
				SelectionStart = idx + str.Length;
			}
			else if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (filenames.Length == 1)
				{
					var calc = Parent as Windows.Calc;
					if (calc != null)
						calc.ReadFile(filenames[0]);
				}
			}
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
		private const int WM_SETREDRAW = 0x0b;

		/// <summary>
		/// When beginning changing fonts and such, this disables drawing.
		/// </summary>
		public void BeginUpdate()
		{
			SendMessage(Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
		}

		/// <summary>
		/// When finished changing fonts and such, this re-enables drawing. This will invalidate
		/// the control.
		/// </summary>
		public void EndUpdate()
		{
			SendMessage(this.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
			Invalidate();
		}
	}
}