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
	/// Sites REST API
	/// </summary>
	public partial class SitesController : Rock.Rest.ApiController<Rock.CMS.Site, Rock.CMS.SiteDTO>
	{
		public SitesController() : base( new Rock.CMS.SiteService() ) { } 
	}
}