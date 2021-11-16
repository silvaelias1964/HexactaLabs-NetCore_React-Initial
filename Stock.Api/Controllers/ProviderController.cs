﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Stock.Api.DTOs;
using Stock.AppService.Services;
using Stock.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/provider")]
    [ApiController]
    public class ProviderController : ControllerBase
    {

        private readonly ProviderService service;
        private readonly IMapper mapper;

        public ProviderController(ProviderService service, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }


        /// <summary>
        /// Get all providers.
        /// </summary>
        /// <returns>List of <see cref="ProviderDTO"/></returns>
        [HttpGet]
        public ActionResult<IEnumerable<ProviderDTO>> Get()
        {
            try
            {
                var result = service.GetAll();
                return mapper.Map<IEnumerable<ProviderDTO>>(result).ToList();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }        
        }

        /// <summary>
        /// Gets a provider by id.
        /// </summary>
        /// <param name="id">Provider id to return.</param>
        /// <returns>A <see cref="ProviderDTO"/></returns>
        [HttpGet("{id}")]
        public ActionResult<ProviderDTO> Get(string id)
        {
            return Ok(mapper.Map<ProviderDTO>(service.Get(id)));
        }

        /// <summary>
        /// Add a provider
        /// </summary>
        /// <param name="value">Provider information.</param>
        [HttpPost]
        public Provider Post([FromBody] ProviderDTO value)
        {
            TryValidateModel(value);
            var provider = service.Create(mapper.Map<Provider>(value));
            return mapper.Map<Provider>(provider);
        }

        /// <summary>
        /// Updates a provider.
        /// </summary>
        /// <param name="id">Provider id to edit.</param>
        /// <param name="value">Prodvider information.</param>
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] ProviderDTO value)
        {
            var provider = service.Get(id);
            TryValidateModel(value);
            try
            {
                mapper.Map<ProviderDTO, Provider>(value, provider);
                service.Update(provider);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;             
            }
        }

        /// <summary>
        /// Deletes a provider
        /// </summary>
        /// <param name="id">Provider id to delete.</param>
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var provider = service.Get(id);

            if (provider is null)
                return NotFound();

            service.Delete(provider);
            return Ok();
        }


    }
}
