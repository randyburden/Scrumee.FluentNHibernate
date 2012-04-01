using FluentNHibernate.Mapping;
using Scrumee.Data.Entities;

namespace Scrumee.Data.Mappings.Fluent
{
    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            Id( x => x.Id );
            Map( x => x.Name );
            References( x => x.UserStory );
            References( x => x.User )
                .Cascade.SaveUpdate();
        }
    }
}
