using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuccubusWizard.Models;

namespace SuccubusWizard.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class IncubusController : ControllerBase
	{
		IncubusContext db;
		public IncubusController(IncubusContext context)
		{
			db = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<IncubusData>>> GetIncubusList()
		{
			return await db.IncubusList.ToListAsync();
		}

		[HttpGet("{MAC}")]
		public async Task<ActionResult<IncubusData>> Get(string MAC)
		{
			IncubusData incubus = await db.IncubusList.FirstOrDefaultAsync(x => x.MAC == MAC);
			if (incubus == null)
				return NotFound();
			return new ObjectResult(incubus);
		}

		[HttpGet]
		public async Task<ActionResult<string>> GetServerStatus()
		{
			IncubusData incubus = await db.IncubusList.FirstOrDefaultAsync(x => x.Id == 0);
			return Ok();
		}


		[HttpPost]
		public async Task<ActionResult<IncubusData>> ConnectIncubus(IncubusData incubus)
		{
			if (incubus == null)
				return BadRequest();

			IncubusData incubusFind = await db.IncubusList.FirstOrDefaultAsync(x => x.MAC == incubus.MAC);

			//Если он был подключён до этого, то пусть обновит данные о себе.
			if (incubusFind != null)
			{
				incubusFind.Data = incubus.Data;
				await db.SaveChangesAsync();
				return Ok(incubus);
			}
			else
			{
				db.IncubusList.Add(incubus);
				await db.SaveChangesAsync();
				return Ok(incubus);
			}
		}

		[HttpPost]
		public async Task<ActionResult<IncubusData>> DisconnectIncubus(IncubusData incubus)
		{
			if (incubus == null)
			{
				return BadRequest();
			}

			IncubusData FindIncubus = db.IncubusList.FirstOrDefault(i => i.MAC == incubus.MAC);
			if (FindIncubus == null)
			{
				return NotFound();
			}

			db.IncubusList.Remove(FindIncubus);
			await db.SaveChangesAsync();
			return Ok(FindIncubus);
		}

		[HttpPost]
		public async Task<ActionResult<IncubusData>> UpdateIncubus(IncubusData incubus)
		{
			if (incubus == null)
			{
				return BadRequest();
			}

			IncubusData FindIncubus = db.IncubusList.FirstOrDefault(x => x.MAC == incubus.MAC);
			if (FindIncubus == null)
			{
				return NotFound();
			}

			FindIncubus.Data = incubus.Data;

			await db.SaveChangesAsync();
			return new ObjectResult(FindIncubus);
		}
	}
}
