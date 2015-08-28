using System;
using System.ComponentModel;

namespace NewTranslator.Core.Translation
{
    [Serializable]
    public class ClientCredential : IEquatable<ClientCredential>
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ClientId { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string ClientSecret { get; set; }
        
        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(ClientId) && !string.IsNullOrWhiteSpace(ClientSecret); }
        }
        
        public bool Equals(ClientCredential other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ClientId, other.ClientId, StringComparison.Ordinal) && string.Equals(ClientSecret, other.ClientSecret, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClientCredential) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ClientId != null ? ClientId.GetHashCode() : 0)*397) ^ (ClientSecret != null ? ClientSecret.GetHashCode() : 0);
            }
        }

        public static bool operator ==(ClientCredential left, ClientCredential right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ClientCredential left, ClientCredential right)
        {
            return !Equals(left, right);
        }
    }
}