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

namespace Rock.CMS
{
	/// <summary>
	/// PageRoute Service class
	/// </summary>
	public partial class PageRouteService : Service<PageRoute, PageRouteDTO>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PageRouteService"/> class
		/// </summary>
		public PageRouteService() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PageRouteService"/> class
		/// </summary>
		public PageRouteService(IRepository<PageRoute> repository) : base(repository)
		{
		}

		/// <summary>
		/// Creates a new model
		/// </summary>
		public override PageRoute CreateNew()
		{
			return new PageRoute();
		}

		/// <summary>
		/// Query DTO objects
		/// </summary>
		/// <returns>A queryable list of DTO objects</returns>
		public override IQueryable<PageRouteDTO> QueryableDTO()
		{
			return this.Queryable().Select( m => new PageRouteDTO()
				{
					IsSystem = m.IsSystem,
					PageId = m.PageId,
					Route = m.Route,
					CreatedDateTime = m.CreatedDateTime,
					ModifiedDateTime = m.ModifiedDateTime,
					CreatedByPersonId = m.CreatedByPersonId,
					ModifiedByPersonId = m.ModifiedByPersonId,
					Id = m.Id,
					Guid = m.Guid,				});
		}
	}
}
