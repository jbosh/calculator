using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Updater
{
	class Program
	{
		static void Main(string[] args)
		{
			if(args.Length == 0)
				return;
			var path = Path.GetDirectoryName(Application.ExecutablePath);
			var dst = Path.Combine(path, "Calculator.exe");
			var src = args[0];
			if (!File.Exists(src))
				return;
			File.Copy(src, dst, true);
			File.Delete(src);
		}
	}
}
