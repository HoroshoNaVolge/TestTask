using Moq;
using TestTask.Application.Services;
using TestTask.Domain.Entities;
using TestTask.Application.DTOs;
using AutoMapper;
using TestTask.Domain.Interfaces;

namespace Tests
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IUchastokRepository> _uchastokRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _uchastokRepositoryMock = new Mock<IUchastokRepository>();
            _mapperMock = new Mock<IMapper>();

            _patientService = new PatientService(
                _patientRepositoryMock.Object,
                _uchastokRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetPatientsAsync_ReturnsSortedPatients()
        {
            var patients = new List<Patient>
        {
            new() { Id = 1, LastName = "Patient A", UchastokId = 1 },
            new() { Id = 2, LastName = "Patient B", UchastokId = 2 }
        };

            var patientDtos = new List<PatientListDto>
        {
            new() { Id = 1, UchastokNumber = "001" },
            new() { Id = 2, UchastokNumber = "002" }
        };

            _patientRepositoryMock.Setup(repo => repo.GetAllAsync(1, 10)).ReturnsAsync(patients);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PatientListDto>>(It.IsAny<IEnumerable<Patient>>()))
                       .Returns(patientDtos);

            var result = await _patientService.GetPatientsAsync(1, 10, "UchastokName");

            Assert.Equal(patientDtos[0].UchastokNumber, result.First().UchastokNumber);
            _patientRepositoryMock.Verify(repo => repo.GetAllAsync(1, 10), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<PatientListDto>>(patients), Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsPatientDto_WhenPatientExists()
        {
            var patient = new Patient { Id = 1, LastName = "Udavov", UchastokId = 1 };
            var patientDto = new PatientEditDto { Id = 1, UchastokId = 1 };

            _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(patient);
            _mapperMock.Setup(mapper => mapper.Map<PatientEditDto>(It.IsAny<Patient>())).Returns(patientDto);

            var result = await _patientService.GetPatientByIdAsync(1);

            Assert.Equal(patientDto.Id, result.Id);
            _patientRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<PatientEditDto>(patient), Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsNull_WhenPatientDoesNotExist()
        {
            _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Patient)null!);

            var result = await _patientService.GetPatientByIdAsync(1);

            Assert.Null(result);
            _patientRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreatePatientAsync_AddsPatientToRepository()
        {
            var patientDto = new PatientEditDto { UchastokId = 1 };
            var patient = new Patient { UchastokId = 1 };

            _mapperMock.Setup(mapper => mapper.Map<Patient>(It.IsAny<PatientEditDto>())).Returns(patient);
            _patientRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);

            await _patientService.CreatePatientAsync(patientDto);

            _mapperMock.Verify(mapper => mapper.Map<Patient>(patientDto), Times.Once);
            _patientRepositoryMock.Verify(repo => repo.AddAsync(patient), Times.Once);
        }

        [Fact]
        public async Task DeletePatientAsync_RemovesPatientFromRepository()
        {
            var existingPatient = new Patient { Id = 1, UchastokId = 1 };

            _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPatient);
            _patientRepositoryMock.Setup(repo => repo.DeleteAsync(existingPatient.Id)).Returns(Task.CompletedTask);

            await _patientService.DeletePatientAsync(1);

            _patientRepositoryMock.Verify(repo => repo.DeleteAsync(existingPatient.Id), Times.Once);
        }
    }
}