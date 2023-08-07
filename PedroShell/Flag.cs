using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedroDB.Shell; 
internal class Flag {

    private readonly string[] args;
    public Flag(string[] args) { 
        this.args = args;
    }

    public bool TryGetFlagValue(string id, out string value) {
        for(int i=0; i < args.Length; i++) {
            if (args[i] != id) {
                continue;
            }

            if (i + 1 >= args.Length) {
                value = "";
                return false;
            }

            value = args[i+1];
            return true;
        }
        value = "";
        return false;
    }
}
