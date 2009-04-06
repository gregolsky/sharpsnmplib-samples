/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 12:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;

using Lextm.SharpSnmpLib.Mib;
using Microsoft.Practices.Unity;

namespace Lextm.SharpSnmpLib.Compiler
{
	internal class CompilerCore
	{
	    private readonly IList<string> _files = new List<string>();
	    private readonly BackgroundWorker worker = new BackgroundWorker();
	    private Parser _parser;
	    private Assembler _assembler;

	    public CompilerCore()
		{
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += backgroundWorker1_DoWork;
			worker.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
		}

	    public bool IsBusy
	    {
	        get { return worker.IsBusy; }
	    }

	    [Dependency]
	    public Parser Parser
	    {
	        get { return _parser; }
	        set { _parser = value; }
	    }

	    [Dependency]
	    public Assembler Assembler
	    {
            get { return _assembler; }
            set { _assembler = value; }
	    }

	    public event EventHandler<EventArgs> RunCompilerCompleted;

	    public event EventHandler<FileAddedEventArgs> FileAdded;

	    public void Compile(IEnumerable<string> files)
		{
			worker.RunWorkerAsync(files);
		}
		
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
		    IEnumerable<string> docs = (IEnumerable<string>)e.Argument;
		    IEnumerable<SharpMibException> errors;
		    CompileInternal(docs, out errors);
		    e.Result = errors;
		}

        private void CompileInternal(IEnumerable<string> docs, out IEnumerable<SharpMibException> errors)
	    {
	        IEnumerable<MibModule> modules = Parser.ParseToModules(docs, out errors);
	        Assembler.Assemble(modules);
	    }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TraceSource source = new TraceSource("Compiler");
            if (e.Result != null)
            {
                IEnumerable<SharpMibException> errors = (IEnumerable<SharpMibException>) e.Result;
                foreach (SharpMibException error in errors)
                {
                    source.TraceInformation(error.Message);
                }
            }

            if (e.Error != null)
            {
                source.TraceInformation(e.Error.Message);
            }

            source.Flush();
            source.Close();
            if (RunCompilerCompleted != null)
            {
                RunCompilerCompleted(this, EventArgs.Empty);
            }

            SystemSounds.Beep.Play();
        }

	    public void Add(string[] files)
	    {
            IList<string> filered = new List<string>();
            foreach (string file in files)
            {
                if (_files.Contains(file))
                {
                    continue;
                }

                _files.Add(file);
                filered.Add(file);
            }

	        if (FileAdded != null)
	        {
	            FileAdded(this, new FileAddedEventArgs(filered));
	        }
	    }

	    public void CompileAll()
	    {
	        Compile(_files);
	    }

	    public void Remove(string name)
	    {
	        _files.Remove(name);
	    }
	}
}