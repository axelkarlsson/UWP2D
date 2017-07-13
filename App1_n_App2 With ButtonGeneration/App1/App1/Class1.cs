using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class ProcessTracker
    {
        List<Process> plist = new List<Process>();
        public Dictionary<string, string> NameToPath;
        ProcessStartInfo p;
        public ProcessTracker(string name, string args)
        {
            p.FileName = NameToPath.TryGetValue(name);
            p.Arguments = args;
            Process p_ = new Process();
            p_.Start(p);
            plist.Add(p_);
        }
        public ProcessTracker() { }
    }
}
