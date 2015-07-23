using System.Collections.Generic;
using System.Linq;

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
				if (this.collection.ContainsKey(name))
				{
					this.collection[name] += "," + value;
				}
				else
				{
					this.collection[name] = value;
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
