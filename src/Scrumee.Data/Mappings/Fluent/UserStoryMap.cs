using FluentNHibernate.Mapping;
using Scrumee.Data.Entities;

namespace Scrumee.Data.Mappings.Fluent
{
    public class UserStoryMap : ClassMap<UserStory>
    {
        public UserStoryMap()
        {
            Id( x => x.Id );
            Map( x => x.Name );
            References( x => x.Project );
            HasMany( x => x.Tasks )
                .Cascade.All()
                .Inverse();
        }
    }
}
