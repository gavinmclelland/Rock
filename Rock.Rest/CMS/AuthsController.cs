//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using Rock.CMS;

namespace Rock.Rest.CMS
{
	/// <summary>
	/// Auths REST API
	/// </summary>
	public partial class AuthsController : Rock.Rest.ApiController<Rock.CMS.Auth, Rock.CMS.AuthDTO>
	{
		public AuthsController() : base( new Rock.CMS.AuthService() ) { } 
	}
}