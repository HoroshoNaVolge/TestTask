using Moq;
using TestTask.Application.Services;
using TestTask.Application.DTOs;
using AutoMapper;
using TestTask.Domain.Interfaces.Common;
using TestTask.Domain.Entities.Persons;
using TestTask.Domain.Interfaces.Persons;
using TestTask.Domain.Entities.Other;

namespace Tests
{
    public class PatientServiceTests
    {
        private readonly Mock<IPersonRepository<Patient>> _patientRepositoryMock;
        private readonly Mock<ICommonRepository<Uchastok>> _uchastokRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPersonRepository<Patient>>();
            _uchastokRepositoryMock = new Mock<ICommonRepository<Uchastok>>();
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

            _patientRepositoryMock.Setup(repo => repo.GetAllAsync(1, 10, "UchastokNumber", CancellationToken.None)).ReturnsAsync(patients);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PatientListDto>>(It.IsAny<IEnumerable<Patient>>()))
                       .Returns(patientDtos);

            var result = await _patientService.GetAllAsync(1, 10, "UchastokNumber", CancellationToken.None);

            Assert.Equal(patientDtos[0].UchastokNumber, result.First().UchastokNumber);
            _patientRepositoryMock.Verify(repo => repo.GetAllAsync(1, 10, "UchastokNumber", CancellationToken.None), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<PatientListDto>>(patients), Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsPatientDto_WhenPatientExists()
        {
            var patient = new Patient { Id = 1, LastName = "Udavov", UchastokId = 1 };
            var patientDto = new PatientEditDto { Id = 1, UchastokId = 1 };

            _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(patient);
            _mapperMock.Setup(mapper => mapper.Map<PatientEditDto>(It.IsAny<Patient>())).Returns(patientDto);

            var result = await _patientService.GetByIdAsync(1);

            Assert.Equal(patientDto.Id, result.Id);
            _patientRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<PatientEditDto>(patient), Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsNull_WhenPatientDoesNotExist()
        {
            _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ThrowsAsync(new KeyNotFoundException());
            
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _patientService.GetByIdAsync(1));

            _patientRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreatePatientAsync_AddsPatientToRepository()
        {
            var patientDto = new PatientCreateDto { UchastokId = 1 };
            var patient = new Patient { UchastokId = 1 };
            var expectedId = 1;

            _uchastokRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _mapperMock.Setup(mapper => mapper.Map<Patient>(patientDto)).Returns(patient);
            _patientRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Patient>()))
                                                     .Callback<Patient>(p => p.Id = expectedId)
                                                     .ReturnsAsync(expectedId);


            var resultId = await _patientService.CreateAsync(patientDto);

            _uchastokRepositoryMock.Verify(repo => repo.ExistsAsync(patientDto.UchastokId.Value), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<Patient>(patientDto), Times.Once);
            _patientRepositoryMock.Verify(repo => repo.AddAsync(patient), Times.Once);

            Assert.Equal(expectedId, resultId);
        }

        [Fact]
        public async Task DeletePatientAsync_RemovesPatientFromRepository()
        {
            var existingPatient = new Patient { Id = 1, UchastokId = 1 };

            _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPatient);
            _patientRepositoryMock.Setup(repo => repo.DeleteAsync(existingPatient.Id)).Returns(Task.CompletedTask);

            await _patientService.DeleteAsync(1);

            _patientRepositoryMock.Verify(repo => repo.DeleteAsync(existingPatient.Id), Times.Once);
        }
    }
}