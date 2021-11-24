using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using Microsoft.CSharp;
using System.CodeDom.Compiler;
using UnityEngine;
//using Mono.CSharp;

public class compiler : MonoBehaviour
{
    public DirectoryInfo d;
    string folderpath = "";

    public Assembly compileScript(string source)
    {
        var provider = new CSharpCodeProvider();
        
        var param = new CompilerParameters();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            param.ReferencedAssemblies.Add(assembly.Location);
        }
        param.GenerateExecutable = false;
        param.GenerateInMemory = true;
        var result = provider.CompileAssemblyFromSource(param, source);

        if (result.Errors.Count > 0)
        {
            var msg = new StringBuilder();
            foreach (CompilerError error in result.Errors)
            {
                msg.AppendFormat("Error ({0}): {1}\n",
                    error.ErrorNumber, error.ErrorText);
            }
            throw new Exception(msg.ToString());
        }
        
        // Return the assembly
        return result.CompiledAssembly;
    }
    public void Init()
    {
        folderpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "OnTimeCompiler//scripts");
        if (!Directory.Exists(folderpath))
        {
            print("Creating mod folder");
            Directory.CreateDirectory(folderpath);
        }
        print("folder already exist");
        d = new DirectoryInfo(folderpath);
        foreach (var f in d.GetFiles("*.cs"))
        {
            print("compiling " + f.FullName);
            compile(f.FullName);
        }
        print("compiled first set of files");
        OnTimeCompiler.watcher wch = gameObject.AddComponent(typeof(OnTimeCompiler.watcher)) as OnTimeCompiler.watcher;
        wch.setup(folderpath, this);
        print("filewatcher setup");
    }

    public void compile(string filename)
    {
        string res = File.ReadAllText(filename);
        Assembly ass = compileScript(res);
        for (int i = 0; i < ass.GetTypes().Length; i++)
        {
            var typename = ass.GetTypes().GetValue(i).ToString();
            var method = ass.GetType(typename).GetMethod("Start");
            var del = (Action)System.Delegate.CreateDelegate(typeof(Action), method);
            del.Invoke();
           
        }

        //Type typo = ass.GetType("Program");
        //MethodInfo metho = typo.GetMethod("Main");
        //metho.Invoke(null, null);
    }

    void Start()
    {
        Init();
    }
   
}
//public class compilerNet : MonoBehaviour
//{
//    public DirectoryInfo d;
//    string folderpath = "";
//    StringWriter reportWriter;
//    CompilerSettings settings;
//    ConsoleReportPrinter printer;
//    CompilerContext compilerContext;
//    Evaluator evaluator ;
//    public void compile(string source)
//    {
//        var res = File.ReadAllText(source);
//        var comps = evaluator.Compile(res);

//        //evaluator.Run(res);
//        //evaluator.Run("testcharp m1 = new testcharp(); m1.Start();");
//        StringReader reader = new StringReader(res);
//        string line;
//        while ((line = reader.ReadLine()) != null)
//        {
//            object result;
//            bool result_set;
//            evaluator.Evaluate(line, out result, out result_set);
//            if (result_set)
//            {
//                Console.WriteLine(result);
//            }
//        };
//    }

//    void Start()
//    {
//        Init();
//    }
//    public void Init()
//    {
//        folderpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "OnTimeCompiler//scripts");
//        reportWriter = new StringWriter();
//        settings = new CompilerSettings();
//        printer = new ConsoleReportPrinter(reportWriter);
//        compilerContext = new CompilerContext(settings, printer);
//        evaluator = new Evaluator(compilerContext);

//        evaluator.ReferenceAssembly(Assembly.GetExecutingAssembly());
//        //foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
//        //{
//        //    settings.AssemblyReferences.Add(assembly.Location);
//        //    evaluator.ReferenceAssembly(assembly);

//        //}

//        if (!Directory.Exists(folderpath))
//        {
//            print("Creating mod folder");
//            Directory.CreateDirectory(folderpath);
//        }
//       print("folder already exist");
//        d = new DirectoryInfo(folderpath);
//        print("trying start");
//        foreach (var f in d.GetFiles("*.cs"))
//        {
//            print("compiling " + f.FullName);
//            compile(f.FullName);
//        }
//        print("compiled first set of files");
//        GameObject go = new GameObject();
//        OnTimeCompiler.watcher wch = go.AddComponent(typeof(OnTimeCompiler.watcher)) as OnTimeCompiler.watcher;
//       // OnTimeCompiler.watcher wch = new OnTimeCompiler.watcher();
//        wch.setupNet(folderpath, this);
//        print("filewatcher setup");
//    }
   
//}