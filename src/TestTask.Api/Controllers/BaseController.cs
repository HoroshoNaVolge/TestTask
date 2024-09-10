﻿using Microsoft.AspNetCore.Mvc;
using TestTask.Application.Interfaces;
using TestTask.Application.Validation;

namespace TestTask.Api.Controllers
{
    [ApiController]
    public abstract class BaseController<TListDto, TEditDto, TCreateDto, TEntity> : ControllerBase
    {
        protected readonly IBaseService<TListDto, TEditDto, TCreateDto, TEntity> service;
        protected readonly ILogger logger;

        public BaseController(IBaseService<TListDto, TEditDto, TCreateDto, TEntity> service, ILogger logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TListDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "UchastokNumber")
        {
            var validationResult = ValidationHelper.ValidatePageParameters(pageNumber, pageSize);
            if (validationResult != null)
                return validationResult;

            var entities = await service.GetAllAsync(pageNumber, pageSize, sortBy, cancellationToken);
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEditDto>> GetById(int id)
        {
            return Ok(await service.GetByIdAsync(id));
        }

        [HttpPost]
        public virtual async Task<ActionResult> Create(TCreateDto createDto)
        {
            var createdId = await service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId });
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> Update(int id, TEditDto editDto)
        {
            await service.UpdateAsync(id, editDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);
            return Ok();
        }
    }
}