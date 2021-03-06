﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mp.Protractors.Test.Core.DependencyResolution;

namespace Mp.Protractors.Test.Controllers
{
    [Route("api/dependency-resolution")]
    public class DependencyResolutionController : ControllerBase
    {
        private readonly ICharDependencyContainer _dependencyContainer;

        public DependencyResolutionController(ICharDependencyContainer dependencyContainer)
        {
            _dependencyContainer = dependencyContainer;
        }

        [HttpGet("descriptors")]
        public IActionResult GetDescriptors()
        {
            var descriptors = _dependencyContainer.GetDescriptorsIncludingTransitiveDependencies();
            return Ok(descriptors);
        }

        [HttpPost("descriptors")]
        public IActionResult RegisterItem([FromBody] string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return BadRequest("Input must be provided");
            }

            try
            {
                var descriptor = Descriptor.Parse(input);
                _dependencyContainer.Register(descriptor);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var descriptors = _dependencyContainer.GetDescriptorsIncludingTransitiveDependencies();
            return Ok(descriptors);
        }
    }
}
