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
	/// Address Service class
	/// </summary>
	public partial class AddressService : Service<Address, AddressDTO>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AddressService"/> class
		/// </summary>
		public AddressService() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AddressService"/> class
		/// </summary>
		public AddressService(IRepository<Address> repository) : base(repository)
		{
		}

		/// <summary>
		/// Creates a new model
		/// </summary>
		public override Address CreateNew()
		{
			return new Address();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<AddressDTO> QueryableDTO()
		{
			return this.Queryable().Select( m => new AddressDTO()
				{
					Raw = m.Raw,
					Street1 = m.Street1,
					Street2 = m.Street2,
					City = m.City,
					State = m.State,
					Country = m.Country,
					Zip = m.Zip,
					Latitude = m.Latitude,
					Longitude = m.Longitude,
					StandardizeAttempt = m.StandardizeAttempt,
					StandardizeService = m.StandardizeService,
					StandardizeResult = m.StandardizeResult,
					StandardizeDate = m.StandardizeDate,
					GeocodeAttempt = m.GeocodeAttempt,
					GeocodeService = m.GeocodeService,
					GeocodeResult = m.GeocodeResult,
					GeocodeDate = m.GeocodeDate,
					CreatedDateTime = m.CreatedDateTime,
					ModifiedDateTime = m.ModifiedDateTime,
					CreatedByPersonId = m.CreatedByPersonId,
					ModifiedByPersonId = m.ModifiedByPersonId,
					Id = m.Id,
					Guid = m.Guid,				});
		}
	}
}
