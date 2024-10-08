using AspNetBot.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetBot.EntitiesConfigs
{
    internal class BotUserConfig:IEntityTypeConfiguration<BotUser>
    {
        public void Configure(EntityTypeBuilder<BotUser> builder) 
        {
            builder.HasKey(x => x.Id);
        }
    }
}
