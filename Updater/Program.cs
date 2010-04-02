using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Updater
{
	class Program
	{
		static void Main(string[] args)
		{
			if(args.Length != 2)
				return;
			Thread.Sleep(200);
			var dst = args[0];
			var src = args[1];
			foreach(var file in Directory.GetFiles(src))
			{
				var name = Path.GetFileName(file);
				File.Copy(file, Path.Combine(dst, name), true);
				File.Delete(file);
			}
			Directory.Delete(src);
		}
	}
}
