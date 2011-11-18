//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace Rock.Api
{
    public class ServiceHelper
    {
        private CompositionContainer container;

        [ImportMany(typeof(IService))]
        IEnumerable<Lazy<IService, IServiceData>> services;

        public void AddRoutes( RouteCollection routes )
        {
            try
            {
                container.ComposeParts( this );

                var factory = new WebServiceHostFactory();

                foreach ( Lazy<IService, IServiceData> i in services )
                    routes.Add( new ServiceRoute( i.Metadata.RouteName, factory, i.Value.GetType() ) );
            }
            catch ( CompositionException ex )
            {
            }
		}

        public ServiceHelper(string extensionFolder)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add( new AssemblyCatalog( typeof( ServiceHelper ).Assembly ) );

            if ( Directory.Exists( extensionFolder ) )
                catalog.Catalogs.Add( new DirectoryCatalog( extensionFolder ) );

            container = new CompositionContainer( catalog );
        }
    }
}
