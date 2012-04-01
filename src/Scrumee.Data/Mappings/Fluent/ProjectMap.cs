using FluentNHibernate.Mapping;
using Scrumee.Data.Entities;

namespace Scrumee.Data.Mappings.Fluent
{
    public class ProjectMap : ClassMap<Project>
    {
        public ProjectMap()
        {
            Id( x => x.Id );
            Map( x => x.Name );
            HasMany( x => x.UserStories )
                .Cascade.All()
                .Inverse();
        }
    }
}
