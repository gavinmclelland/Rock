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

using Rock.Data;

namespace Rock.Core
{
	/// <summary>
	/// Data Transfer Object for DefinedValue object
	/// </summary>
	public partial class DefinedValueDTO : DTO<DefinedValue>
	{

#pragma warning disable 1591
		public bool IsSystem { get; set; }
		public int DefinedTypeId { get; set; }
		public int Order { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
#pragma warning restore 1591

		/// <summary>
		/// Instantiates a new DTO object
		/// </summary>
		public DefinedValueDTO ()
		{
		}

		/// <summary>
		/// Instantiates a new DTO object from the model
		/// </summary>
		/// <param name="definedValue"></param>
		public DefinedValueDTO ( DefinedValue definedValue )
		{
			CopyFromModel( definedValue );
		}

		/// <summary>
		/// Copies the model property values to the DTO properties
		/// </summary>
		/// <param name="definedValue"></param>
		public override void CopyFromModel( DefinedValue definedValue )
		{
			this.IsSystem = definedValue.IsSystem;
			this.DefinedTypeId = definedValue.DefinedTypeId;
			this.Order = definedValue.Order;
			this.Name = definedValue.Name;
			this.Description = definedValue.Description;
			this.CreatedDateTime = definedValue.CreatedDateTime;
			this.ModifiedDateTime = definedValue.ModifiedDateTime;
			this.CreatedByPersonId = definedValue.CreatedByPersonId;
			this.ModifiedByPersonId = definedValue.ModifiedByPersonId;
			this.Id = definedValue.Id;
			this.Guid = definedValue.Guid;
		}

		/// <summary>
		/// Copies the DTO property values to the model properties
		/// </summary>
		/// <param name="definedValue"></param>
		public override void CopyToModel ( DefinedValue definedValue )
		{
			definedValue.IsSystem = this.IsSystem;
			definedValue.DefinedTypeId = this.DefinedTypeId;
			definedValue.Order = this.Order;
			definedValue.Name = this.Name;
			definedValue.Description = this.Description;
			definedValue.CreatedDateTime = this.CreatedDateTime;
			definedValue.ModifiedDateTime = this.ModifiedDateTime;
			definedValue.CreatedByPersonId = this.CreatedByPersonId;
			definedValue.ModifiedByPersonId = this.ModifiedByPersonId;
			definedValue.Id = this.Id;
			definedValue.Guid = this.Guid;
		}
	}
}