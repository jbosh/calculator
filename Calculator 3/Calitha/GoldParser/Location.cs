namespace Calitha.GoldParser
{
	/// <summary>
	/// The Location defines positional information of the input that is being parsed.
	/// </summary>
	public class Location
	{
		/// <summary>
		/// Creates a new Location object.
		/// </summary>
		/// <param name="position">Zero based position.</param>
		/// <param name="lineNr">Zero based line number.</param>
		/// <param name="columnNr">Zero based column number.</param>
		public Location(int position, int lineNr, int columnNr)
		{
			Init(position, lineNr, columnNr);
		}

		/// <summary>
		/// Creates a new Location object.
		/// </summary>
		/// <param name="location">Positional information will be copied from this object.</param>
		public Location(Location location)
		{
			Init(location.Position, location.LineNr, location.ColumnNr);
		}

		private void Init(int position, int lineNr, int columnNr)
		{
			Position = position;
			LineNr = lineNr;
			ColumnNr = columnNr;
		}

		public Location Clone()
		{
			return new Location(this);
		}

		/// <summary>
		/// Converts the location to a string. Line number and column number will be
		/// incremented by one.
		/// </summary>
		/// <returns>The output string.</returns>
		public override string ToString()
		{
			return "(pos: " + (Position + 0) + ", ln: " + (LineNr + 1) + ", col: " + (ColumnNr + 1) + ")";
		}

		/// <summary>
		/// Signals that the input has encountered an end-of-line.
		/// </summary>
		public void NextLine()
		{
			Position++;
			LineNr++;
			ColumnNr = 0;
		}

		/// <summary>
		/// Signals that the input has advanced one character (which was not an end-of-line.
		/// </summary>
		public void NextColumn()
		{
			Position++;
			ColumnNr++;
		}

		/// <summary>
		/// The zero-based position.
		/// </summary>
		public int Position { get; private set; }

		/// <summary>
		/// The zero-based line number.
		/// </summary>
		public int LineNr { get; private set; }

		/// <summary>
		/// The zero-based column number.
		/// </summary>
		public int ColumnNr { get; private set; }
	}
}