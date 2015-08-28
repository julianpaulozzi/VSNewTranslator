using System;
using Microsoft.VisualStudio.ComponentModelHost;

namespace NewTranslator
{
    internal static class ServicesHelper
    {
        public static TInterface GetServiceFromComponentModel<TInterface>(bool throwOnError = true) where TInterface : class
        {
            TInterface service = null;
            IComponentModel componentModel = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
            if (componentModel != null) service = componentModel.GetService<TInterface>();

            if (service == null && throwOnError)
                throw new InvalidOperationException("Unable to obtain service of type " + typeof(TInterface).Name);

            return service;
        }

        /// <summary>
        /// Get a service from the Sandcastle Help File Builder package
        /// </summary>
        /// <param name="throwOnError">True to throw an exception if the service cannot be obtained,
        /// false to return null.</param>
        /// <typeparam name="TInterface">The interface to obtain</typeparam>
        /// <typeparam name="TService">The service used to get the interface</typeparam>
        /// <returns>The service or null if it could not be obtained</returns>
        public static TInterface GetServiceFromPackage<TInterface, TService>(bool throwOnError = true)
            where TInterface : class
            where TService : class
        {
            IServiceProvider provider = NewTranslatorPackage.Instance;
            
            TInterface service = (provider == null) ? null : provider.GetService(typeof(TService)) as TInterface;

            if (service == null && throwOnError)
                throw new InvalidOperationException("Unable to obtain service of type " + typeof(TService).Name);

            return service;
        }
    }
}