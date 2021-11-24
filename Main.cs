using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;

//namespace OnTimeCompiler
//{


//	class MainProc
//    {
//		void lol()
//        {
//			Console.WriteLine("starting");
//			var reportWriter = new StringWriter();
//			var settings = new CompilerSettings();
//			var printer = new ConsoleReportPrinter(reportWriter);
//			var compilerContext = new CompilerContext(settings, printer);
//			var evaluator = new Evaluator(compilerContext);
//			evaluator.ReferenceAssembly(Assembly.GetExecutingAssembly());
//			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
//			{
//				settings.AssemblyReferences.Add(assembly.Location);
//				evaluator.ReferenceAssembly(assembly);

//			}

//			var folderpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "scripts");
//			var d = new DirectoryInfo(folderpath);
//			var count = 0;
//			foreach (var f in d.GetFiles("*.cs"))
//			{

//				settings.SourceFiles.Add(new SourceFile(f.Name, f.FullName, count));
//				var res = File.ReadAllText(f.FullName);
//				var comps = evaluator.Compile(res);

//				//evaluator.Run(res);
//				//evaluator.Run("testcharp m1 = new testcharp(); m1.Start();");
//				StringReader reader = new StringReader(res);
//				string line;
//				while ((line = reader.ReadLine()) != null)
//				{
//					object result;
//					bool result_set;
//					evaluator.Evaluate(line, out result, out result_set);
//					if (result_set)
//					{
//						Console.WriteLine(result);
//					}
//				};
//			}
//		}
		
//		static void Main()
//        {
//			compilerNet necom = new compilerNet();
//			necom.Init();
	

//			Console.Read();
//        }
//    }

//}

