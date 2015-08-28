using System.Collections.Generic;

namespace NewTranslator.Core.Translation
{
	public class DictionaryItem
	{
		public string Title { get; set; }
		public List<string> Terms { get; set; }

		public DictionaryItem()
		{
			Terms = new List<string>();
		}
	}	
}
