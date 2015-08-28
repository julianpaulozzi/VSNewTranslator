using System;

namespace NewTranslator.Core.Translation
{
    public class TranslationCacheKey : IEquatable<TranslationCacheKey>
    {
        public TranslationCacheKey(string name, string sourceLang, string destinationLang, string text)
        {
            Name = name.ToLowerInvariant();
            SourceLang = sourceLang.ToLowerInvariant();
            DestinationLang = destinationLang.ToLowerInvariant();
            Text = text.Trim();
        }

        public string Name { get; }
        public string SourceLang { get; }
        public string DestinationLang { get; }
        public string Text { get; }

        public bool Equals(TranslationCacheKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) && 
                   string.Equals(SourceLang, other.SourceLang, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(DestinationLang, other.DestinationLang, StringComparison.OrdinalIgnoreCase) && 
                   string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TranslationCacheKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (SourceLang?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (DestinationLang?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (Text?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public static bool operator ==(TranslationCacheKey left, TranslationCacheKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TranslationCacheKey left, TranslationCacheKey right)
        {
            return !Equals(left, right);
        }
    }
}