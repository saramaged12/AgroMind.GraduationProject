using AgroMind.GP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroMind.GP.Repository.Data.Configurations
{
    public class MessageConfigurations : IEntityTypeConfiguration<Messages>
    {
        public void Configure(EntityTypeBuilder<Messages> builder)
        {
            builder.HasOne(m => m.Sender)  
                .WithMany()  
                .HasForeignKey(m => m.SenderId)  
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Receiver)  
                .WithMany()  
                .HasForeignKey(m => m.ReceiverId)  
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
