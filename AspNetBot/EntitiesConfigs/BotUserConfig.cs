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
            builder.HasOne(x => x.Profession)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ProfessionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
