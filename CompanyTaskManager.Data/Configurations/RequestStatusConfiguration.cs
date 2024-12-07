

using CompanyTaskManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyTaskManager.Data.Configurations;

public class RequestStatusConfiguration : IEntityTypeConfiguration<RequestStatus>
{
    public void Configure(EntityTypeBuilder<RequestStatus> builder)
    {
        builder.HasData(
            new RequestStatus
            {
                Id = 1,
                Name = "Pending"
            },
            new RequestStatus
            {
                Id = 2,
                Name = "Approved"
            },
            new RequestStatus
            {
                Id = 3,
                Name = "Declined"
            },
            new RequestStatus
            {
                Id = 4,
                Name = "Canceled"
            }
            );
    }
}
