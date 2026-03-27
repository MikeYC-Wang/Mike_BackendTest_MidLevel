using BackendExam.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BackendExam.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyOfficeAcpdController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MyOfficeAcpdController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/myofficeacpd
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MyOffice_ACPD>>> GetMyOffice_ACPD()
        {
            return await _context.MyOffice_ACPD.ToListAsync();
        }

        // GET: api/myofficeacpd/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MyOffice_ACPD>> GetMyOffice_ACPD(string id)
        {
            var myOffice_ACPD = await _context.MyOffice_ACPD.FindAsync(id);

            if (myOffice_ACPD == null)
            {
                return NotFound(); // 404
            }

            return myOffice_ACPD; // 200
        }

        // POST: api/myofficeacpd
        [HttpPost]
        public async Task<ActionResult<MyOffice_ACPD>> PostMyOffice_ACPD(MyOffice_ACPD myOffice_ACPD)
        {
            if (string.IsNullOrEmpty(myOffice_ACPD.ACPD_SID))
            {
                // 設定預存程序的 OUTPUT 參數
                var outSidParam = new SqlParameter
                {
                    ParameterName = "@ReturnSID",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 20,
                    Direction = System.Data.ParameterDirection.Output
                };

                // 傳入 TableName 並取得回傳的 SID
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC [dbo].[NEWSID] @TableName=@TableName, @ReturnSID=@ReturnSID OUTPUT",
                    new SqlParameter("@TableName", "MyOffice_ACPD"),
                    outSidParam
                );

                // 將資料庫產生的 SID 塞回 Model
                myOffice_ACPD.ACPD_SID = outSidParam.Value.ToString();
            }

            myOffice_ACPD.ACPD_NowDateTime = DateTime.Now;
            myOffice_ACPD.ACPD_UPDDateTime = DateTime.Now;

            _context.MyOffice_ACPD.Add(myOffice_ACPD);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMyOffice_ACPD), new { id = myOffice_ACPD.ACPD_SID }, myOffice_ACPD); // 201
        }

        // PUT: api/myofficeacpd/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMyOffice_ACPD(string id, MyOffice_ACPD myOffice_ACPD)
        {
            if (id != myOffice_ACPD.ACPD_SID)
            {
                return BadRequest(); // 400
            }

            myOffice_ACPD.ACPD_UPDDateTime = DateTime.Now;
            _context.Entry(myOffice_ACPD).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // 將錯誤寫入 Log
                var errorInfo = JsonSerializer.Serialize(new { Message = ex.Message, Id = id });
                await LogErrorAsync("PutMyOffice_ACPD", errorInfo);

                if (!MyOffice_ACPDExists(id))
                {
                    return NotFound(); // 404
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                var errorInfo = JsonSerializer.Serialize(new { Message = ex.Message, StackTrace = ex.StackTrace });
                await LogErrorAsync("PutMyOffice_ACPD_UnexpectedError", errorInfo);
                return StatusCode(500, "Internal server error");
            }

            return NoContent(); // 204
        }

        // DELETE: api/myofficeacpd/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMyOffice_ACPD(string id)
        {
            var myOffice_ACPD = await _context.MyOffice_ACPD.FindAsync(id);
            if (myOffice_ACPD == null)
            {
                return NotFound(); // 404
            }

            _context.MyOffice_ACPD.Remove(myOffice_ACPD);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        private bool MyOffice_ACPDExists(string id)
        {
            return _context.MyOffice_ACPD.Any(e => e.ACPD_SID == id);
        }

        private async Task LogErrorAsync(string programName, string actionJson)
        {
            var pReadId = new SqlParameter("@_InBox_ReadID", System.Data.SqlDbType.TinyInt) { Value = 0 };
            var pSpName = new SqlParameter("@_InBox_SPNAME", System.Data.SqlDbType.NVarChar, 120) { Value = "MyOfficeAcpdController" };
            var pGroupId = new SqlParameter("@_InBox_GroupID", System.Data.SqlDbType.UniqueIdentifier) { Value = Guid.NewGuid() };
            var pExProgram = new SqlParameter("@_InBox_ExProgram", System.Data.SqlDbType.NVarChar, 40) { Value = programName };
            var pActionJson = new SqlParameter("@_InBox_ActionJSON", System.Data.SqlDbType.NVarChar, -1) { Value = actionJson };
            var pOutValues = new SqlParameter("@_OutBox_ReturnValues", System.Data.SqlDbType.NVarChar, -1) { Direction = System.Data.ParameterDirection.Output };
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[usp_AddLog] @_InBox_ReadID=@_InBox_ReadID, @_InBox_SPNAME=@_InBox_SPNAME, @_InBox_GroupID=@_InBox_GroupID, @_InBox_ExProgram=@_InBox_ExProgram, @_InBox_ActionJSON=@_InBox_ActionJSON, @_OutBox_ReturnValues=@_OutBox_ReturnValues OUTPUT",
                pReadId, pSpName, pGroupId, pExProgram, pActionJson, pOutValues
            );
        }
    }
}