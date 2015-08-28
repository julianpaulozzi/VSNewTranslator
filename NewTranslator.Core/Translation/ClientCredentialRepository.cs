using System;
using System.Collections.Generic;

namespace NewTranslator.Core.Translation
{
    internal class ClientCredentialRepository
    {
        private static readonly Lazy<ClientCredentialRepository> _instance = 
            new Lazy<ClientCredentialRepository>(() => new ClientCredentialRepository());

        private readonly Dictionary<string, ClientCredential> _credentials = 
            new Dictionary<string, ClientCredential>();

        private ClientCredentialRepository()
        {
        }

        internal static ClientCredentialRepository Current
        {
            get { return _instance.Value; }
        }

        internal bool TryAddOrUpdateCredential(string traslatorName, ClientCredential clientCredential)
        {
            if (string.IsNullOrEmpty(traslatorName) || clientCredential == null || !clientCredential.IsValid)
                return false;

            if (_credentials.ContainsKey(traslatorName))
                _credentials[traslatorName] = clientCredential;
            else
                _credentials.Add(traslatorName, clientCredential);

            return true;
        }

        internal ClientCredential GetCredential(string traslatorName)
        {
            return _credentials.ContainsKey(traslatorName) ? _credentials[traslatorName] : null;
        }
    }
}