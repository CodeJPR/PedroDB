using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedroEngine; 
internal class Database {
    public User Owner { get; set; }

	public List<Table> Tables { get; set; } = new();

	public Database(User owner) {
		Owner = owner;
	}
}
