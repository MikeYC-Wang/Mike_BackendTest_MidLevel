using System.ComponentModel.DataAnnotations;

namespace BackendExam.Api.Models
{
    public class MyOffice_ACPD
    {
        [Key]
        [StringLength(20)]
        public string ACPD_SID { get; set; } = string.Empty;

        [StringLength(60)]
        public string? ACPD_Cname { get; set; }

        [StringLength(40)]
        public string? ACPD_Ename { get; set; }

        [StringLength(40)]
        public string? ACPD_Sname { get; set; }

        [StringLength(60)]
        public string? ACPD_Email { get; set; }

        public byte? ACPD_Status { get; set; }

        public bool? ACPD_Stop { get; set; }

        [StringLength(60)]
        public string? ACPD_StopMemo { get; set; }

        [StringLength(30)]
        public string? ACPD_LoginID { get; set; }

        [StringLength(60)]
        public string? ACPD_LoginPWD { get; set; }

        [StringLength(600)]
        public string? ACPD_Memo { get; set; }

        public DateTime? ACPD_NowDateTime { get; set; }

        [StringLength(20)]
        public string? ACPD_NowID { get; set; }

        public DateTime? ACPD_UPDDateTime { get; set; }

        [StringLength(20)]
        public string? ACPD_UPDID { get; set; }
    }
}