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
		public async Task<ActionResult<IEnumerable<Incubus>>> GetIncubusList()
		{
			return await db.IncubusList.ToListAsync();
		}

		[HttpGet("{MAC}")]
		public async Task<ActionResult<Incubus>> Get(string MAC)
		{
			Incubus incubus = await db.IncubusList.FirstOrDefaultAsync(x => x.MAC == MAC);
			if (incubus == null)
				return NotFound();
			return new ObjectResult(incubus);
		}

		[HttpGet]
		public async Task<ActionResult<string>> GetServerStatus()
		{
			Incubus incubus = await db.IncubusList.FirstOrDefaultAsync(x => x.Id == 0);
			return Ok(1);
		}


		[HttpPost]
		public async Task<ActionResult<Incubus>> ConnectIncubus(Incubus incubus)
		{
			if (incubus == null)
				return BadRequest();

			Incubus incubusFind = await db.IncubusList.FirstOrDefaultAsync(x => x.MAC == incubus.MAC);

			//Если он был подключён до этого, то пусть обновит данные о себе.
			if (incubusFind != null)
			{
				db.Update(incubus);
				await db.SaveChangesAsync();
				return Ok(incubus);
			}

			db.IncubusList.Add(incubus);
			await db.SaveChangesAsync();
			return Ok(incubus);

		}

		[HttpDelete]
		public async Task<ActionResult<Incubus>> DisconnectIncubus(Incubus incubus)
		{
			Incubus FindIncubus = db.IncubusList.FirstOrDefault(i => i.Name == incubus.Name & i.MAC == incubus.MAC);
			if (FindIncubus == null)
			{
				return NotFound();
			}
			db.IncubusList.Remove(FindIncubus);
			await db.SaveChangesAsync();
			return Ok(FindIncubus);
		}

		[HttpPut]
		public async Task<ActionResult<Incubus>> UpdateIncubus(Incubus incubus)
		{
			if (incubus == null)
			{
				return BadRequest();
			}
			if (!db.IncubusList.Any(x => x.MAC == incubus.MAC))
			{
				return NotFound();
			}

			db.Update(incubus);
			await db.SaveChangesAsync();
			return Ok(incubus);
		}
	}
}
