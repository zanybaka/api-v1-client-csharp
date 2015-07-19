using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Info.Blockchain.API
{
	public class NameValueCollection
	{
		private Dictionary<string, string> collection = new Dictionary<string, string>();

		public string this[string name]
		{
			get { return this.collection[name]; }
			set
			{
				if (this.collection.ContainsKey(value))
				{
					this.collection[value] += "," + value;
				}
				else
				{
					this.collection[value] = value;
				}
			}
		}

		public List<string> AllKeys
		{
			get
			{
				return this.collection.Keys.ToList();
			}
		}
		public int Count
		{
			get
			{
				return this.collection.Count;
			}
		}
	}
}
