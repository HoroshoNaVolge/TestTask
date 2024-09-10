using Moq;
using TestTask.Application.Services;
using TestTask.Application.DTOs;
using AutoMapper;
using TestTask.Application.Interfaces;
using TestTask.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using TestTask.Domain.Interfaces.Common;
using TestTask.Domain.Entities.Persons;
using TestTask.Domain.Interfaces.Persons;
using TestTask.Domain.Entities.Other;

namespace Tests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IPersonRepository<Doctor>> _doctorRepositoryMock;
        private readonly Mock<ICommonRepository<Cabinet>> _cabinetRepositoryMock;
        private readonly Mock<ICommonRepository<Specialization>> _specializationRepositoryMock;
        private readonly Mock<ICommonRepository<Uchastok>> _uchastokRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _doctorRepositoryMock = new Mock<IPersonRepository<Doctor>>();
            _cabinetRepositoryMock = new Mock<ICommonRepository<Cabinet>>();
            _specializationRepositoryMock = new Mock<ICommonRepository<Specialization>>();
            _uchastokRepositoryMock = new Mock<ICommonRepository<Uchastok>>();
            _mapperMock = new Mock<IMapper>();

            _doctorService = new DoctorService(
                _doctorRepositoryMock.Object,
                _cabinetRepositoryMock.Object,
                _specializationRepositoryMock.Object,
                _uchastokRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetDoctorsAsync_ReturnsSortedDoctors()
        {
            var doctors = new List<Doctor>
            {
            new() { Id = 1, FullName = "Doctor A", CabinetId = 1, SpecializationId = 1 },
            new() { Id = 2, FullName = "Doctor B", CabinetId = 2, SpecializationId = 2 }
        };

            var doctorDtos = new List<DoctorListDto>
        {
            new() { Id = 1, CabinetNumber = "101", SpecializationName = "Cardiologist" },
            new() { Id = 2, CabinetNumber = "102", SpecializationName = "Neurologist" }
        };

            _doctorRepositoryMock.Setup(repo => repo.GetAllAsync(1, 10, "CabinetNumber", CancellationToken.None)).ReturnsAsync(doctors);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<DoctorListDto>>(It.IsAny<IEnumerable<Doctor>>()))
                       .Returns(doctorDtos);

            var result = await _doctorService.GetAllAsync(1, 10, "CabinetNumber", CancellationToken.None);

            Assert.Equal(doctorDtos[0].CabinetNumber, result.First().CabinetNumber);
            _doctorRepositoryMock.Verify(repo => repo.GetAllAsync(1, 10, "CabinetNumber", CancellationToken.None), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<DoctorListDto>>(doctors), Times.Once);
        }
    }

    public class DoctorsControllerTests
    {
        private readonly Mock<IBaseService<DoctorListDto, DoctorEditDto, DoctorBaseDto, Doctor>> _doctorServiceMock;
        private readonly DoctorsController _controller;

        public DoctorsControllerTests()
        {
            _doctorServiceMock = new Mock<IBaseService<DoctorListDto, DoctorEditDto, DoctorBaseDto, Doctor>>();
            _controller = new DoctorsController(_doctorServiceMock.Object, logger: null!);
        }

        [Fact]
        public async Task GetDoctors_ReturnsOkResult_WithDoctorsList()
        {
            var doctorDtos = new List<DoctorListDto>
        {
            new() { Id = 1, CabinetNumber = "101", SpecializationName = "Cardiologist" },
            new() { Id = 2, CabinetNumber = "102", SpecializationName = "Neurologist" }
        };

            _doctorServiceMock.Setup(service => service.GetAllAsync(1, 10, "SpecializationName", CancellationToken.None)).ReturnsAsync(doctorDtos);

            var result = await _controller.GetAll(CancellationToken.None, 1, 10, "SpecializationName");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDoctors = Assert.IsType<List<DoctorListDto>>(okResult.Value);
            Assert.Equal(2, returnedDoctors.Count);
        }
    }
}
