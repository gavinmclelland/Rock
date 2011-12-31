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
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace Rock.REST.Core
{
	/// <summary>
	/// REST WCF service for FieldTypes
	/// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("RouteName", "Core/FieldType")]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class FieldTypeService : IFieldTypeService, IService
    {
		/// <summary>
		/// Gets a FieldType object
		/// </summary>
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Core.DTO.FieldType Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Core.FieldTypeService FieldTypeService = new Rock.Core.FieldTypeService();
				Rock.Core.FieldType FieldType = FieldTypeService.Get( int.Parse( id ) );
				if ( FieldType.Authorized( "View", currentUser ) )
					return FieldType.DataTransferObject;
				else
					throw new WebFaultException<string>( "Not Authorized to View this FieldType", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Gets a FieldType object
		/// </summary>
		[WebGet( UriTemplate = "{id}/{apiKey}" )]
        public Rock.Core.DTO.FieldType ApiGet( string id, string apiKey )
        {
            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Core.FieldTypeService FieldTypeService = new Rock.Core.FieldTypeService();
					Rock.Core.FieldType FieldType = FieldTypeService.Get( int.Parse( id ) );
					if ( FieldType.Authorized( "View", user.Username ) )
						return FieldType.DataTransferObject;
					else
						throw new WebFaultException<string>( "Not Authorized to View this FieldType", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Updates a FieldType object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateFieldType( string id, Rock.Core.DTO.FieldType FieldType )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Core.FieldTypeService FieldTypeService = new Rock.Core.FieldTypeService();
				Rock.Core.FieldType existingFieldType = FieldTypeService.Get( int.Parse( id ) );
				if ( existingFieldType.Authorized( "Edit", currentUser ) )
				{
					uow.objectContext.Entry(existingFieldType).CurrentValues.SetValues(FieldType);
					
					if (existingFieldType.IsValid)
						FieldTypeService.Save( existingFieldType, currentUser.PersonId() );
					else
						throw new WebFaultException<string>( existingFieldType.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this FieldType", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Updates a FieldType object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}/{apiKey}" )]
        public void ApiUpdateFieldType( string id, string apiKey, Rock.Core.DTO.FieldType FieldType )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Core.FieldTypeService FieldTypeService = new Rock.Core.FieldTypeService();
					Rock.Core.FieldType existingFieldType = FieldTypeService.Get( int.Parse( id ) );
					if ( existingFieldType.Authorized( "Edit", user.Username ) )
					{
						uow.objectContext.Entry(existingFieldType).CurrentValues.SetValues(FieldType);
					
						if (existingFieldType.IsValid)
							FieldTypeService.Save( existingFieldType, user.PersonId );
						else
							throw new WebFaultException<string>( existingFieldType.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this FieldType", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Creates a new FieldType object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateFieldType( Rock.Core.DTO.FieldType FieldType )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Core.FieldTypeService FieldTypeService = new Rock.Core.FieldTypeService();
				Rock.Core.FieldType existingFieldType = new Rock.Core.FieldType();
				FieldTypeService.Add( existingFieldType, currentUser.PersonId() );
				uow.objectContext.Entry(existingFieldType).CurrentValues.SetValues(FieldType);

				if (existingFieldType.IsValid)
					FieldTypeService.Save( existingFieldType, currentUser.PersonId() );
				else
					throw new WebFaultException<string>( existingFieldType.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
            }
        }

		/// <summary>
		/// Creates a new FieldType object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "{apiKey}" )]
        public void ApiCreateFieldType( string apiKey, Rock.Core.DTO.FieldType FieldType )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Core.FieldTypeService FieldTypeService = new Rock.Core.FieldTypeService();
					Rock.Core.FieldType existingFieldType = new Rock.Core.FieldType();
					FieldTypeService.Add( existingFieldType, user.PersonId );
					uow.objectContext.Entry(existingFieldType).CurrentValues.SetValues(FieldType);

					if (existingFieldType.IsValid)
						FieldTypeService.Save( existingFieldType, user.PersonId );
					else
						throw new WebFaultException<string>( existingFieldType.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a FieldType object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteFieldType( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Core.FieldTypeService FieldTypeService = new Rock.Core.FieldTypeService();
				Rock.Core.FieldType FieldType = FieldTypeService.Get( int.Parse( id ) );
				if ( FieldType.Authorized( "Edit", currentUser ) )
				{
					FieldTypeService.Delete( FieldType, currentUser.PersonId() );
					FieldTypeService.Save( FieldType, currentUser.PersonId() );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this FieldType", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a FieldType object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}/{apiKey}" )]
        public void ApiDeleteFieldType( string id, string apiKey )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Core.FieldTypeService FieldTypeService = new Rock.Core.FieldTypeService();
					Rock.Core.FieldType FieldType = FieldTypeService.Get( int.Parse( id ) );
					if ( FieldType.Authorized( "Edit", user.Username ) )
					{
						FieldTypeService.Delete( FieldType, user.PersonId );
						FieldTypeService.Save( FieldType, user.PersonId );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this FieldType", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

    }
}
