using FluentNHibernate.Mapping;
using Scrumee.Data.Entities;

namespace Scrumee.Data.Mappings.Fluent
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id( x => x.Id );
            Map( x => x.FirstName );
            Map( x => x.LastName );
        }
    }
}
