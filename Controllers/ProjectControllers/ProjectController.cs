﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XtramileBackend.Models.APIModels;
using XtramileBackend.Models.EntityModels;
using XtramileBackend.Services.ProjectService;

namespace XtramileBackend.Controllers.ProductControllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;

        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetProjectsAsync()
        {
            try
            {
                IEnumerable<TBL_PROJECT> productsData = await _projectServices.GetAllProjectsAsync();
                return Ok(productsData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting projects: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProjectAsync([FromBody] TBL_PROJECT project)
        {
            try
            {
                await _projectServices.AddProjectAsync(project);
                return Ok(project);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding a project: {ex.Message}");
            }
        }



        [HttpGet("getprojectcodes/{empId}")]
        public async Task<IActionResult> GetProjectCodesByEmpId(int empId)
        {
            try
            {
                IEnumerable<ProjectCodesViewModel> projectCodeData = await _projectServices.GetProjectCodesByEmployeeId(empId);
                return Ok(projectCodeData);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting pending requests: {ex.Message}");
            }
        }




        //To get project codes and corresponding project id.
        [HttpGet("projectcodes")]

        public async Task<IActionResult> GetProjectCodeandId()
        {
            try
            {
                IEnumerable<object> projectData = await _projectServices.GetProjectIdAndCode();
                return Ok(projectData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting projects: {ex.Message}");
            }
        }
    }
}
