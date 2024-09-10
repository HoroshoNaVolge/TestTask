using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Entities.Other;

namespace TestTask.Infrastructure.Data
{
    internal class UchastokDbConfiguration : IEntityTypeConfiguration<Uchastok>
    {
        public void Configure(EntityTypeBuilder<Uchastok> builder)
        {
            builder.HasData(
                  new Uchastok { Id = 1, Number = "1А" },
                  new Uchastok { Id = 2, Number = "1Б" },
                  new Uchastok { Id = 3, Number = "2А" },
                  new Uchastok { Id = 4, Number = "2Б" });
        }
    }

    internal class CabinetDbConfiguration : IEntityTypeConfiguration<Cabinet>
    {
        public void Configure(EntityTypeBuilder<Cabinet> builder)
        {
            builder.HasData(
                  new Cabinet { Id = 1, Number = "101" },
                  new Cabinet { Id = 2, Number = "204" },
                  new Cabinet { Id = 3, Number = "212" },
                  new Cabinet { Id = 4, Number = "302" });
        }
    }

    internal class SpecializationDbConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.HasData(
                  new Specialization { Id = 1, Name = "Терапевт" },
                  new Specialization { Id = 2, Name = "Хирург" },
                  new Specialization { Id = 3, Name = "Окулист" },
                  new Specialization { Id = 4, Name = "Педиатр" });
        }
    }
}
