using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Entities;

namespace TestTask.Infrastructure.Data
{
    internal class UchastokDbConfiguration : IEntityTypeConfiguration<Uchastok>
    {
        public void Configure(EntityTypeBuilder<Uchastok> builder)
        {
            builder.HasData(
                  new Uchastok { Id = 1, Number = "Тестовый участок №1" },
                  new Uchastok{ Id = 2, Number = "Тестовый участок №2" });
        }
    }

    internal class CabinetDbConfiguration : IEntityTypeConfiguration<Cabinet>
    {
        public void Configure(EntityTypeBuilder<Cabinet> builder)
        {
            builder.HasData(
                  new Cabinet { Id = 1, Number = "Тестовый кабинет №1" },
                  new Cabinet { Id = 2, Number = "Тестовый кабинет №2" });
        }
    }

    internal class SpecializationDbConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.HasData(
                  new Specialization { Id = 1, Name = "Тестовый специализация №1" },
                  new Specialization { Id = 2, Name = "Тестовая специализация №2" });
        }
    }
}
