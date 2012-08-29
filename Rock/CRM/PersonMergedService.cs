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

using System;
using System.Linq;

using Rock.Data;

namespace Rock.CRM
{
	/// <summary>
	/// PersonMerged Service class
	/// </summary>
	public partial class PersonMergedService : Service<PersonMerged, PersonMergedDTO>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PersonMergedService"/> class
		/// </summary>
		public PersonMergedService() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PersonMergedService"/> class
		/// </summary>
		public PersonMergedService(IRepository<PersonMerged> repository) : base(repository)
		{
		}

		/// <summary>
		/// Creates a new model
		/// </summary>
		public override PersonMerged CreateNew()
		{
			return new PersonMerged();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<PersonMergedDTO> QueryableDTO()
		{
			return this.Queryable().Select( m => new PersonMergedDTO()
				{
					CurrentId = m.CurrentId,
					CurrentGuid = m.CurrentGuid,
					CreatedDateTime = m.CreatedDateTime,
					CreatedByPersonId = m.CreatedByPersonId,
					Id = m.Id,
					Guid = m.Guid,
		}
	}
}