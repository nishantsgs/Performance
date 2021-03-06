﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using BasicApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicApi.Controllers
{
    [Authorize("pet-store-reader")]
    [Route("/pet")]
    public class PetController : ControllerBase
    {
        public PetController(BasicApiContext dbContext)
        {
            DbContext = dbContext;
        }

        public BasicApiContext DbContext { get; }

        [HttpGet("{id}", Name = "FindPetById")]
        public async Task<IActionResult> FindById(int id)
        {
            var pet = await DbContext.Pets
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (pet == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(pet);
        }

        [HttpGet("findByStatus")]
        public async Task<IActionResult> FindByStatus(string status)
        {
            var pet = await DbContext.Pets
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Status == status);
            if (pet == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(pet);
        }

        [HttpGet("findByTags")]
        public async Task<IActionResult> FindByTags(string[] tags)
        {
            var pet = await DbContext.Pets
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Tags.Any(t => tags.Contains(t.Name)));
            if (pet == null)
            {
                return new NotFoundResult();
            }

            return new ObjectResult(pet);
        }

        [Authorize("pet-store-writer")]
        [HttpPost]
        public async Task<IActionResult> AddPet([FromBody] Pet pet)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            DbContext.Pets.Add(pet);
            await DbContext.SaveChangesAsync();

            return new CreatedAtRouteResult("FindPetById", new { id = pet.Id }, pet);
        }

        [Authorize("pet-store-writer")]
        [HttpPut]
        public IActionResult EditPet(Pet pet)
        {
            throw new NotImplementedException();
        }

        [Authorize("pet-store-writer")]
        [HttpPost("{id}/uploadImage")]
        public IActionResult UploadImage(int id, IFormFile file)
        {
            throw new NotImplementedException();
        }

        [Authorize("pet-store-writer")]
        [HttpDelete("{id}")]
        public IActionResult DeletePet(int id)
        {
            throw new NotImplementedException();
        }
    }
}
