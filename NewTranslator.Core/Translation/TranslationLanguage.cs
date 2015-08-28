using System;
using System.Collections.Generic;

namespace NewTranslator.Core.Translation
{
	public class TranslationLanguage : IEquatable<TranslationLanguage>
	{
		public string Code { get; set; }
		public string Name { get; set; }

		public TranslationLanguage(string code, string name)
		{
			Code = code;
			Name = name;
		}

	    public bool Equals(TranslationLanguage other)
	    {
	        if (ReferenceEquals(null, other)) return false;
	        if (ReferenceEquals(this, other)) return true;
	        return string.Equals(Code, other.Code, StringComparison.OrdinalIgnoreCase);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return Equals((TranslationLanguage) obj);
	    }

	    public override int GetHashCode()
	    {
	        return (Code != null ? Code.GetHashCode() : 0);
	    }

	    public static bool operator ==(TranslationLanguage left, TranslationLanguage right)
	    {
	        return Equals(left, right);
	    }

	    public static bool operator !=(TranslationLanguage left, TranslationLanguage right)
	    {
	        return !Equals(left, right);
	    }
	}

    public class TranslationLanguageCollection : List<TranslationLanguage>
    {
        public TranslationLanguageCollection()
        {
        }
        
        public TranslationLanguageCollection(IEnumerable<TranslationLanguage> collection) : base(collection)
        {
        }

        public void Add(string code, string name)
        {
            Add(new TranslationLanguage(code, name));
        }
    }

    internal static class TranslationLanguageCollectionExtensions
    {
        internal static TranslationLanguageCollection Copy(
            this TranslationLanguageCollection translationLanguageCollection)
        {
            return new TranslationLanguageCollection(translationLanguageCollection);
        }
    }
}
